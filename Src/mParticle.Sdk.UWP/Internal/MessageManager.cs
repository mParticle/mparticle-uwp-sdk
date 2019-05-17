using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using mParticle.Sdk.Core;
using mParticle.Sdk.Core.Dto.Events;

namespace mParticle.Sdk.UWP
{
    internal class MessageManager
    {
        private readonly EventsApiClient eventsApiManager;
        private readonly PersistenceManager persistenceManager;
        private readonly AppInfo appInfo;
        private readonly DeviceInfo deviceInfo;
        private const long ThrottleTimeMillis = 2 * 60 * 60 * 1000;
        private readonly IList<SdkMessage> pendingMessages = new List<SdkMessage>();

        public MessageManager(EventsApiClient eventsApiManager, PersistenceManager persistenceManager)
        {
            this.eventsApiManager = eventsApiManager;
            this.eventsApiManager.EventsApiUploadUnsuccessful += EventsApiManager_EventsApiUnsuccessfulRequest;
            this.eventsApiManager.EventsApiUploadFailure += EventsApiManager_EventsApiUploadFailure;
            this.persistenceManager = persistenceManager;
            this.appInfo = ApplicationInfoBuilder.Build();
            this.deviceInfo = DeviceInfoBuilder.Build();
        }

        public bool Enabled { get; set; }

        public void LogMessage(SdkMessage message)
        {
            if (Enabled)
            {
                if (MParticle.Instance.Identity.CurrentUser != null)
                {
                    eventsApiManager.EnqueueMessage(CreateBatch(new SdkMessage[] { message }));
                }
                else
                {
                    lock (pendingMessages)
                    {
                        pendingMessages.Add(message);
                    }
                }
            }
        }

        private RequestHeaderSdkMessage CreateBatch(SdkMessage[] messages)
        {
            var batch = new RequestHeaderSdkMessage()
            {
                Messages = messages,
                Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                AppInfo = appInfo,
                DeviceInfo = deviceInfo,
                ClientMpId = MParticle.Instance.Identity.CurrentUser?.Mpid,
                DeviceApplicationStamp = Guid.Parse(persistenceManager.DeviceApplicationStamp),
                Debug = MParticle.Instance.Options.DevelopmentMode,
                SdkVersion = MParticle.SdkVersion
            };
            var user = MParticle.Instance.Identity.CurrentUser;
            if (user != null)
            {
                batch.UserIdentities = persistenceManager.UserIdentities(user.Mpid).ToArray();
                batch.UserAttributes = persistenceManager.UserAttributes(user.Mpid);
            }
            return batch;
        }

        private void EventsApiManager_EventsApiUnsuccessfulRequest(object sender, UnsuccessfulApiRequestEventArgs e)
        {
            var statusCode = e.HttpStatusCode;
            if (statusCode >= 500 || statusCode == 429)
            {
                eventsApiManager.EnqueueMessage(e.RequestHeaderSdkMessage);
                if (statusCode == (int)HttpStatusCode.ServiceUnavailable || statusCode == 429)
                {
                    eventsApiManager.NextPermittedRequestTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + ThrottleTimeMillis;
                }
            }
        }

        private void EventsApiManager_EventsApiUploadFailure(object sender, EventsApiFailureEventArgs e)
        {
            eventsApiManager.EnqueueMessage(e.RequestHeaderSdkMessage);
        }

        internal void LogPending(MParticleUser user)
        {
            lock (pendingMessages)
            {
                if (pendingMessages.Count > 0)
                {
                    eventsApiManager.EnqueueMessage(CreateBatch(pendingMessages.ToArray()));
                    pendingMessages.Clear();
                }
            }
        }
    }
}