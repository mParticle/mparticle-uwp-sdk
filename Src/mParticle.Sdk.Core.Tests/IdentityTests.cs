using Microsoft.VisualStudio.TestTools.UnitTesting;
using mParticle.Sdk.Core.Dto.Identity;
using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Tests
{
    [TestClass]
    public class IdentityTests
    {
        [TestMethod]
        public void TestIdentityRequestSerialization()
        {
            var request = new IdentityRequest();
            request.ClientSdk = new ClientSdk();
            request.ClientSdk.Platform = Platform.Xbox;
            request.ClientSdk.SdkVendor = "foo vendor";
            request.ClientSdk.SdkVersion = "foo version";
            request.Context = "foo context";
            request.Environment = Dto.Identity.Environment.Production;
            request.KnownIdentities = new Identities();
            request.KnownIdentities.Add(IdentityType.MicrosoftPublisherId, "foo ad id");
            request.RequestId = "foo request id";
            request.RequestTimestampMs = 1234;
            request.SourceRequestId = "foo source request id";

            var serialized = JsonConvert.SerializeObject(request);
            var deserializedRequest = JsonConvert.DeserializeObject<IdentityRequest>(serialized);
            Assert.AreEqual(request.ClientSdk.Platform, deserializedRequest.ClientSdk.Platform);
            Assert.AreEqual(request.ClientSdk.SdkVendor, deserializedRequest.ClientSdk.SdkVendor);
            Assert.AreEqual(request.ClientSdk.SdkVersion, deserializedRequest.ClientSdk.SdkVersion);
            Assert.AreEqual(request.Context, deserializedRequest.Context);
            Assert.AreEqual(request.Environment, deserializedRequest.Environment);
            Assert.AreEqual(request.KnownIdentities[IdentityType.MicrosoftPublisherId], deserializedRequest.KnownIdentities[IdentityType.MicrosoftPublisherId]);
            Assert.AreEqual(request.RequestId, deserializedRequest.RequestId);
            Assert.AreEqual(request.RequestTimestampMs, deserializedRequest.RequestTimestampMs);
            Assert.AreEqual(request.SourceRequestId, deserializedRequest.SourceRequestId);
        }
    }
}