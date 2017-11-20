using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mParticle.Sdk.Core.Dto.Events;
using Windows.ApplicationModel;

namespace mParticle.Sdk.UWP
{
    [TestClass]
    public class MParticleUserTests
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
        public void TestGetUserIdentities()
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
            var user = new MParticleUser(5, persistenceManager);
            var identities = user.UserIdentities;
            Assert.AreEqual(1, identities.Count);
            Assert.AreEqual("foo identity", identities[UserIdentityType.Twitter]);
        }

        [TestMethod]
        public void TestGetUserIdentitiesNoIdentities()
        {
            PersistenceManager persistenceManager = new PersistenceManager(MParticleOptions.Builder("foo", "bar").Build());
            persistenceManager.Initialize(Package.Current.Id.Version);
            var user = new MParticleUser(5, persistenceManager);
            var identities = user.UserIdentities;
            Assert.AreEqual(0, identities.Count);
        }

        [TestMethod]
        public void TestSetUserAttributes()
        {
            PersistenceManager persistenceManager = new PersistenceManager(MParticleOptions.Builder("foo", "bar").Build());
            persistenceManager.Initialize(Package.Current.Id.Version);
            var user = new MParticleUser(5, persistenceManager);
            user.UserAttribute("foo attribute key", "foo value");
            Assert.AreEqual("foo value", user.UserAttributes["foo attribute key"]);

        }

        [TestMethod]
        public void TestSetUserAttributeList()
        {
            PersistenceManager persistenceManager = new PersistenceManager(MParticleOptions.Builder("foo", "bar").Build());
            persistenceManager.Initialize(Package.Current.Id.Version);
            var user = new MParticleUser(5, persistenceManager);
            var attributeValueList = new List<string>();
            attributeValueList.Add("foo item 1");
            user.UserAttribute("foo attribute list key", attributeValueList);
            Assert.AreEqual("foo item 1", ((IList<string>)user.UserAttributes["foo attribute list key"])[0]);

        }

        [TestMethod]
        public void TestSetUserAttributeTag()
        {
            PersistenceManager persistenceManager = new PersistenceManager(MParticleOptions.Builder("foo", "bar").Build());
            persistenceManager.Initialize(Package.Current.Id.Version);
            var user = new MParticleUser(5, persistenceManager);
            user.UserTag("foo user attribute tag");
            Assert.AreEqual(1, user.UserAttributes.Count);
            Assert.IsTrue(user.UserAttributes.ContainsKey("foo user attribute tag"));
        }

        [TestMethod]
        public void TestRemoveUserAttribute()
        {
            PersistenceManager persistenceManager = new PersistenceManager(MParticleOptions.Builder("foo", "bar").Build());
            persistenceManager.Initialize(Package.Current.Id.Version);
            var user = new MParticleUser(5, persistenceManager);
            user.UserTag("foo user attribute tag 1");
            user.UserTag("foo user attribute tag 2");
            Assert.AreEqual(2, user.UserAttributes.Count);
            user.RemoveUserAttribute("foo user attribute tag 1");
            Assert.AreEqual(1, user.UserAttributes.Count);
        }
    }
}