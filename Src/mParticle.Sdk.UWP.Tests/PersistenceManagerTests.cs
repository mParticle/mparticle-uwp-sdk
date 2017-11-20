using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mParticle.Sdk.Core.Dto.Events;
using Windows.ApplicationModel;

namespace mParticle.Sdk.UWP
{
    [TestClass]
    public class PersistenceManagerTests
    {

        [TestInitialize]
        public void SetupTests()
        {
            var manager = new PersistenceManager(MParticleOptions.Builder("foo", "bar").Build());
            manager.Initialize(new PackageVersion());
            manager.Clear();
        }

        [TestCleanup]
        public void CleanupTests()
        {
            var manager = new PersistenceManager(MParticleOptions.Builder("foo", "bar").Build());
            manager.Initialize(new PackageVersion());
            manager.Clear();
        }

        [TestMethod]
        public void TestIsFirstRun()
        {
            PersistenceManager persistenceManager = new PersistenceManager(MParticleOptions.Builder("foo", "bar").Build());
            persistenceManager.Initialize(Package.Current.Id.Version);
            Assert.AreEqual(true, persistenceManager.IsFirstRun);
            persistenceManager.Initialize(Package.Current.Id.Version);
            Assert.AreEqual(false, persistenceManager.IsFirstRun);
        }

        [TestMethod]
        public void TestDeviceApplicationStamp()
        {
            PersistenceManager persistenceManager = new PersistenceManager(MParticleOptions.Builder("foo", "bar").Build());
            persistenceManager.Initialize(Package.Current.Id.Version);
            Assert.IsNull(persistenceManager.DeviceApplicationStamp);
            persistenceManager.DeviceApplicationStamp = "foo das";
            Assert.AreEqual("foo das", persistenceManager.DeviceApplicationStamp);
        }

        [TestMethod]
        public void TestNoUserIdentities()
        {
            PersistenceManager persistenceManager = new PersistenceManager(MParticleOptions.Builder("foo", "bar").Build());
            persistenceManager.Initialize(Package.Current.Id.Version);
            var identities = persistenceManager.UserIdentities(1);
            Assert.IsNotNull(identities);
            Assert.AreEqual(0, identities.Count);
        }


        [TestMethod]
        public void TestUpdateUserIdentities()
        {
            PersistenceManager persistenceManager = new PersistenceManager(MParticleOptions.Builder("foo", "bar").Build());
            persistenceManager.Initialize(Package.Current.Id.Version);
            IList<UserIdentity> userIdentities = new List<UserIdentity>();
            UserIdentity identity = new UserIdentity();
            identity.DateFirstSet = 123;
            identity.Identity = "foo identity";
            identity.IsFirstSeen = true;
            identity.Name = UserIdentityType.Twitter;
            userIdentities.Add(identity);
            persistenceManager.SetUserIdentities(5, userIdentities);
            var identities = persistenceManager.UserIdentities(5);
            Assert.AreEqual(1, identities.Count);
            Assert.AreEqual(123, identities[0].DateFirstSet);
            Assert.AreEqual("foo identity", identities[0].Identity);
            Assert.AreEqual(true, identities[0].IsFirstSeen);
            Assert.AreEqual(UserIdentityType.Twitter, identities[0].Name);

        }

        [TestMethod]
        public void TestClearUserIdentities()
        {
            PersistenceManager persistenceManager = new PersistenceManager(MParticleOptions.Builder("foo", "bar").Build());
            persistenceManager.Initialize(Package.Current.Id.Version);
            IList<UserIdentity> userIdentities = new List<UserIdentity>();
            UserIdentity identity = new UserIdentity();
            identity.DateFirstSet = 123;
            identity.Identity = "foo identity";
            identity.IsFirstSeen = true;
            identity.Name = UserIdentityType.Twitter;
            userIdentities.Add(identity);
            persistenceManager.SetUserIdentities(5, userIdentities);
            var identities = persistenceManager.UserIdentities(5);
            Assert.AreEqual(1, identities.Count);

            persistenceManager.SetUserIdentities(5, null);
            identities = persistenceManager.UserIdentities(5);
            Assert.AreEqual(0, identities.Count);
        }


        [TestMethod]
        public void TestIsUpgrade()
        {
            PersistenceManager persistenceManager = new PersistenceManager(MParticleOptions.Builder("foo", "bar").Build());
            PackageVersion version1 = new PackageVersion() { Major = 1 };
            PackageVersion version2 = new PackageVersion() { Major = 2 };

            persistenceManager.Initialize(version1);
            Assert.AreEqual(false, persistenceManager.IsUpgrade);
            persistenceManager.Initialize(version2);
            Assert.AreEqual(true, persistenceManager.IsUpgrade);
            persistenceManager.Initialize(version2);
            Assert.AreEqual(false, persistenceManager.IsUpgrade);
        }

        [TestMethod]
        public void TestSessionStorage()
        {
            PersistenceManager persistenceManager = new PersistenceManager(MParticleOptions.Builder("foo", "bar").Build());
            persistenceManager.Initialize(new PackageVersion());
            Session fooSession = new Session();
            fooSession.AddMpid(new MParticleUser(4, persistenceManager));
            persistenceManager.LastSession = fooSession;
            Session fooSession2 = persistenceManager.LastSession;
            Assert.AreEqual(fooSession.Id, fooSession2.Id);
            Assert.AreEqual(fooSession.StartTimeMillis, fooSession2.StartTimeMillis);
            CollectionAssert.AreEqual((List<long>)fooSession.Mpids, (List<long>)fooSession2.Mpids);

            Session fooSession3 = new Session();
            fooSession3.LastEventTimeMillis = 123;
            fooSession3.AddMpid(new MParticleUser(10, persistenceManager));
            fooSession3.BackgroundTime = 456;
            persistenceManager.LastSession = fooSession3;

            Session fooSession4 = persistenceManager.LastSession;
            Assert.AreEqual(fooSession3.Id, fooSession4.Id);
            Assert.AreEqual(fooSession3.StartTimeMillis, fooSession4.StartTimeMillis);
            Assert.AreEqual(fooSession3.LastEventTimeMillis, fooSession4.LastEventTimeMillis);
            Assert.AreEqual(fooSession3.BackgroundTime, fooSession4.BackgroundTime);
            CollectionAssert.AreEqual((List<long>)fooSession3.Mpids, (List<long>)fooSession4.Mpids);
        }
    }
}