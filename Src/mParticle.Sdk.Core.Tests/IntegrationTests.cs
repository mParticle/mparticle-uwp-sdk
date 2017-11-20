using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mParticle.Sdk.Core.Dto.Events;
using mParticle.Sdk.Core.Dto.Identity;

namespace mParticle.Sdk.Core.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        const string ApiKey = "REPLACE ME";
        const string ApiSecret = "REPLACE ME";

        [TestMethod]
        public void TestUploadSuccess()
        {
            EventsApiClient apiManager = new EventsApiClient(ApiKey, ApiSecret);
            RequestHeaderSdkMessage message = new RequestHeaderSdkMessage();
            message.DeviceInfo = new DeviceInfo();
            message.ClientMpId = 123;
            message.AppInfo = new AppInfo();
            apiManager.EventsApiUploadSuccess += (request, response) => {  Assert.IsFalse(response == null); };
            apiManager.EventsApiUploadFailure += (request, exception) => { Assert.Fail("Failed:" + exception.Error); };
            apiManager.EnqueueMessage(message);
            apiManager.DispatchAsync().Wait();
        }

        [TestMethod]
        public void TestBadApiKey()
        {
            EventsApiClient apiManager = new EventsApiClient("foo bad key", "foo bad secret");
            RequestHeaderSdkMessage message = new RequestHeaderSdkMessage();
            message.DeviceInfo = new DeviceInfo();
            message.ClientMpId = 123;
            message.AppInfo = new AppInfo();
            apiManager.EventsApiUploadSuccess += (request, successargs) => { Assert.Fail(); };
            apiManager.EventsApiUploadFailure += (request, failureargs) => { Assert.Fail(); };
            apiManager.EventsApiUploadUnsuccessful += (request, badrequestargs) => {
                Assert.IsNotNull(request);
                Assert.AreEqual(400, badrequestargs.HttpStatusCode);
            };
            apiManager.EnqueueMessage(message);
            apiManager.DispatchAsync().Wait();
        }

        [TestMethod]
        public void TestIdentifyRequest()
        {
            var request = new IdentityRequest();
            request.ClientSdk = new ClientSdk();
            request.ClientSdk.Platform = Platform.Xbox;
            request.ClientSdk.SdkVendor = "foo vendor";
            request.ClientSdk.SdkVersion = "foo version";
            request.Context = "foo context";
            request.Environment = Dto.Identity.Environment.Development;
            request.KnownIdentities = new Identities();
            request.KnownIdentities.Add(IdentityType.DeviceApplicationStamp, Guid.NewGuid().ToString());
            request.RequestId = "foo request id";
            request.RequestTimestampMs = 1234;
            request.SourceRequestId = "foo source request id";

            var client = new IdentityApiClient(ApiKey, ApiSecret);
            var task = client.Identify(request);
            task.Wait();
            var response = task.Result;
            Assert.IsNotNull(response);
            Assert.AreEqual(typeof(IdentityResponse), response.GetType());
            Assert.IsNotNull(((IdentityResponse)response).Mpid);
        }

        [TestMethod]
        public void TestBadIdentifyRequest()
        {
            var request = new IdentityRequest();
            request.ClientSdk = new ClientSdk();
            request.ClientSdk.Platform = Platform.Xbox;
            request.ClientSdk.SdkVendor = "foo vendor";
            request.ClientSdk.SdkVersion = "foo version";
            request.Context = "foo context";
            request.Environment = Dto.Identity.Environment.Production;
            request.KnownIdentities = new Identities();
            request.RequestId = "foo request id";
            request.RequestTimestampMs = 1234;
            request.SourceRequestId = "foo source request id";

            var client = new IdentityApiClient(ApiKey, ApiSecret);
            var task = client.Identify(request);
            task.Wait();
            var response = task.Result;
            Assert.IsNotNull(response);
            Assert.AreEqual(typeof(ErrorResponse), response.GetType());
            Assert.AreEqual(((ErrorResponse)response).StatusCode, 400);
        }
    }
}