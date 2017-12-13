using Microsoft.VisualStudio.TestTools.UnitTesting;
using mParticle.Sdk.Core.Dto.Identity;
using Windows.ApplicationModel;

namespace mParticle.Sdk.UWP
{
    [TestClass]
    public class IdentityApiTests
    {

        [TestInitialize]
        public void SetupTests()
        {
            var manager = new PersistenceManager(MParticleOptions.Builder("foo","bar").Build());
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
        public void TestGenerateDeviceApplicationStamp()
        {
            var manager = new PersistenceManager(MParticleOptions.Builder("foo", "bar").Build());
            manager.Initialize(new PackageVersion());
            Assert.IsNull(manager.DeviceApplicationStamp);
            var identityApi = new IdentityApi("foo", "bar", manager);
            var das = identityApi.GenerateDasIfNeeded();
            Assert.IsTrue(!string.IsNullOrEmpty(das));
            var das2 = identityApi.GenerateDasIfNeeded();
            Assert.AreEqual(das, das2);
            Assert.AreEqual(das, manager.DeviceApplicationStamp);
        }

        [TestMethod]
        public void TestAddDeviceIdentities()
        {
            var manager = new PersistenceManager(MParticleOptions.Builder("foo", "bar").Build());
            manager.Initialize(new PackageVersion());
            var identityApi = new IdentityApi("foo", "bar", manager);
            var identities = new Identities();
            Assert.AreEqual(0, identities.Count);
            identityApi.AddDeviceIdentities(identities);
            Assert.IsNotNull(identities[IdentityType.DeviceApplicationStamp]);
            Assert.AreEqual(identityApi.GenerateDasIfNeeded(), identities[IdentityType.DeviceApplicationStamp]);
            Assert.IsNotNull(identities[IdentityType.MicrosoftAdvertisingId]);
            Assert.AreEqual(DeviceInfoBuilder.QueryAdvertisingId(), identities[IdentityType.MicrosoftAdvertisingId]);
            Assert.IsNotNull(identities[IdentityType.MicrosoftPublisherId]);
            Assert.AreEqual(DeviceInfoBuilder.QueryPublisherId(), identities[IdentityType.MicrosoftPublisherId]);
        }

        [TestMethod]
        public void TestAddUserIdentities()
        {
            var manager = new PersistenceManager(MParticleOptions.Builder("foo", "bar").Build());
            manager.Initialize(new PackageVersion());
            var identityApi = new IdentityApi("foo", "bar", manager);
            var identities = new Identities();
            Assert.AreEqual(0, identities.Count);
            var apiRequest = IdentityApiRequest.EmptyUser()
                .CustomerId("foo customer id")
                .Email("foo email")
                .UserIdentity(Core.Dto.Events.UserIdentityType.Google, "foo google")
                .Build();
            IdentityApi.AddUserIdentities(identities, apiRequest);
            Assert.AreEqual(identities[IdentityType.CustomerId], "foo customer id");
            Assert.AreEqual(identities[IdentityType.Email], "foo email");
            Assert.AreEqual(identities[IdentityType.Google], "foo google");
        }
    }
}