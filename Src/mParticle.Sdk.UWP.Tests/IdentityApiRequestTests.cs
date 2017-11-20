using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mParticle.Sdk.UWP
{
    [TestClass]
    public class IdentityApiRequestTests
    {

        [TestMethod]
        public void TestEmptyBuild()
        {
            var requestBuilder = IdentityApiRequest.EmptyUser();
            var request = requestBuilder.Build();
            Assert.AreEqual(0, request.UserIdentities.Count);
        }

        [TestMethod]
        public void TestSetIdentities()
        {
            var requestBuilder = IdentityApiRequest.EmptyUser();
            var request = requestBuilder
                .Email("foo email")
                .CustomerId("foo customer id")
                .UserIdentity(Core.Dto.Events.UserIdentityType.Facebook, "foo facebook")
                .Build();
            Assert.AreEqual(3, request.UserIdentities.Count);
            Assert.AreEqual("foo email", request.UserIdentities[Core.Dto.Events.UserIdentityType.Email]);
            Assert.AreEqual("foo customer id", request.UserIdentities[Core.Dto.Events.UserIdentityType.CustomerId]);
            Assert.AreEqual("foo facebook", request.UserIdentities[Core.Dto.Events.UserIdentityType.Facebook]);
        }

        [TestMethod]
        public void TestSetUserAliasHandler()
        {
            var requestBuilder = IdentityApiRequest.EmptyUser();
            var request = requestBuilder
                .UserAliasDelegate((oldUser, newUser) => { }).Build();
            Assert.IsNotNull(request.UserAliasDelegate);
        }
    }
}