using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using mParticle.Sdk.Core.Dto.Events;
using Newtonsoft.Json;

namespace mParticle.Sdk.Core
{
    public class EventsApiClient
    {
        private readonly Queue<RequestHeaderSdkMessage> messages;
        private readonly IList<Task> dispatchingTasks;
        private readonly Uri eventsEndpointUri;
        private readonly string apiKey;
        private readonly string apiSecret;
        private Timer uploadTimer;
        private TimeSpan uploadInterval;

        /// <summary>
        /// Provides notification that a <see cref="RequestHeaderSdkMessage"/> has been been successfully sent.
        /// </summary>
        public event EventHandler<EventsApiSuccessEventArgs> EventsApiUploadSuccess;


        /// <summary>
        /// Provides notification that a <see cref="RequestHeaderSdkMessage"/> completed but had a non-2xx status code.
        /// </summary>
        public event EventHandler<UnsuccessfulApiRequestEventArgs> EventsApiUploadUnsuccessful;

        /// <summary>
        /// Provides notification that a <see cref="RequestHeaderSdkMessage"/> failed to upload.
        /// </summary>
        public event EventHandler<EventsApiFailureEventArgs> EventsApiUploadFailure;

        private const String EventUrlFormat = "https://nativesdks.mparticle.com/v2/{0}/events";

        /// <summary>
        /// Instantiates a new instance of <see cref="EventsApiClient"/>.
        /// </summary>
        public EventsApiClient(String key, String secret)
        {
            dispatchingTasks = new List<Task>();
            messages = new Queue<RequestHeaderSdkMessage>();
            apiKey = key;
            apiSecret = secret;
            eventsEndpointUri = new Uri(String.Format(EventUrlFormat, apiKey));
            this.UploadInterval = TimeSpan.FromSeconds(10);
            this.MaximumQueueSize = 100;
        }

        /// <summary>
        /// Gets or sets the user agent request header
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Gets or sets the maximum message queue size
        /// </summary>
        public int MaximumQueueSize { get; set; }

        /// <summary>
        /// Gets or sets the logging interface
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Gets or sets the frequency at which messages should be sent to the service. Default is immediate.
        /// </summary>
        /// <remarks>Setting to TimeSpan.Zero will cause the upload to get sent immediately.</remarks>
        public TimeSpan UploadInterval
        {
            get { return uploadInterval; }
            set
            {
                if (uploadInterval != value)
                {
                    uploadInterval = value;
                    if (uploadTimer != null)
                    {
                        uploadTimer.Dispose();
                        uploadTimer = null;
                    }
                    if (uploadInterval > TimeSpan.Zero)
                    {
                        uploadTimer = new Timer(On_Upload_Interval, null, UploadInterval, UploadInterval);
                    }
                }
            }
        }

        public long NextPermittedRequestTime { get; set; }

        /// <summary>
        /// Empties the queue of <see cref="RequestHeaderSdkMessage"/>s waiting to be dispatched.
        /// </summary>
        /// <remarks>If a <see cref="RequestHeaderSdkMessage"/> is actively beeing sent, this will not abort the request.</remarks>
        public void Clear()
        {
            lock (messages)
            {
                messages.Clear();
            }
        }

        /// <summary>
        /// Dispatches all messages in the queue.
        /// </summary>
        /// <returns>Returns once all items that were in the queue at the time the method was called have finished being sent.</returns>
        public async Task DispatchAsync()
        {
            long now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (now < NextPermittedRequestTime)
            {
                return;
            }
            Task allDispatchingTasks = null;
            lock (dispatchingTasks)
            {
                if (dispatchingTasks.Any())
                {
                    allDispatchingTasks = Task.WhenAll(dispatchingTasks);
                }
            }
            if (allDispatchingTasks != null)
            {
                await allDispatchingTasks;
            }

            RequestHeaderSdkMessage[] uploadsToSend;
            lock (messages)
            {
                uploadsToSend = messages.OrderBy(u => u.Timestamp).ToArray();
                messages.Clear();
            }
            if (uploadsToSend.Any())
            {
                await RunDispatchingTask(UploadQueuedMessages(uploadsToSend));
            }
        }

        public void EnqueueMessage(RequestHeaderSdkMessage message)
        {
            lock (messages)
            {
                messages.Enqueue(message);
                while (messages.Count > MaximumQueueSize)
                {
                    messages.Dequeue();
                }
            }
        }

        /// <summary>
        /// Suspends operations and flushes the queue.
        /// </summary>
        /// <remarks>Call <see cref="Resume"/> when returning from a suspended state to resume operations.</remarks>
        /// <returns>Operation returns when all <see cref="RequestHeaderSdkMessage"/>s have been flushed.</returns>
        public async Task SuspendAsync()
        {
            await DispatchAsync(); // flush all pending messages in the queue

            // shut down the timer if enabled
            if (uploadTimer != null)
            {
                uploadTimer.Dispose();
                uploadTimer = null;
            }
        }

        public void Resume()
        {
             uploadTimer = new Timer(On_Upload_Interval, null, UploadInterval, UploadInterval);
        }

        async void On_Upload_Interval(object sender)
        {
            await DispatchAsync();
        }

        async Task RunDispatchingTask(Task newDispatchingTask)
        {
            lock (dispatchingTasks)
            {
                dispatchingTasks.Add(newDispatchingTask);
            }
            try
            {
                await newDispatchingTask;
            }
            finally
            {
                lock (dispatchingTasks)
                {
                    dispatchingTasks.Remove(newDispatchingTask);
                }
            }
        }

        async Task UploadQueuedMessages(IEnumerable<RequestHeaderSdkMessage> uploads)
        {
            using (var httpClient = new HttpClient())
            {
                foreach (var upload in uploads)
                {
                    await Upload(upload, httpClient);
                }
            }
        }

        async Task Upload(RequestHeaderSdkMessage upload, HttpClient httpClient)
        {
            try
            {
                using (var response = await SendMessageAsync(upload, httpClient))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        await OnMessageSentAsync(upload, response);
                    }
                    else
                    {
                        OnUnsuccessfulApiRequest(upload, response);
                    }
                }
            }
            catch (Exception ex)
            {
                OnApiFailure(upload, ex);
            }
        }

        async Task<HttpResponseMessage> SendMessageAsync(RequestHeaderSdkMessage message, HttpClient httpClient)
        {
            using (var httpRequest = CreateHttpRequest(message))
            {
                return await httpClient.SendAsync(httpRequest);
            }
        }

        void OnUnsuccessfulApiRequest(RequestHeaderSdkMessage upload, HttpResponseMessage response)
        {
            this.Logger?.Log(new LogEntry(LoggingEventType.Debug, "Bad Api Request:\n" + response.StatusCode));
            EventsApiUploadUnsuccessful?.Invoke(this, new UnsuccessfulApiRequestEventArgs(upload, (int)response.StatusCode));
        }

        void OnApiFailure(RequestHeaderSdkMessage upload, Exception exception)
        {
            this.Logger?.Log(new LogEntry(LoggingEventType.Debug, "Failed Api Request:\n" + exception.Message));
            EventsApiUploadFailure?.Invoke(this, new EventsApiFailureEventArgs(upload, exception));
        }

        async Task OnMessageSentAsync(RequestHeaderSdkMessage upload, HttpResponseMessage response)
        { 
            this.Logger?.Log(new LogEntry(LoggingEventType.Debug, "Upload succeeded: " + response.StatusCode));
            EventsApiUploadSuccess?.Invoke(this, new EventsApiSuccessEventArgs(upload, await response.Content.ReadAsStringAsync()));
        }

        private HttpRequestMessage CreateHttpRequest(RequestHeaderSdkMessage batchRequest)
        {
            String message = JsonConvert.SerializeObject(batchRequest, new JsonSerializerSettings() {NullValueHandling = NullValueHandling.Ignore });
            this.Logger?.Log(new LogEntry(LoggingEventType.Debug, "Performing upload:\n" + message));
            var request = new HttpRequestMessage()
            {
                RequestUri = eventsEndpointUri,
                Method = HttpMethod.Post,
                Content = new StringContent(message, Encoding.UTF8, "application/json")
            };

            if (!string.IsNullOrEmpty(UserAgent))
            {
                request.Headers.UserAgent.ParseAdd(UserAgent);
            }

            AddSignature(request, message, eventsEndpointUri.PathAndQuery, apiSecret);

            return request;
        }

        internal static void AddSignature(HttpRequestMessage request, string message, string path, string apiSecret)
        {
            string method = request.Method.ToString();
            string dateHeader = DateTime.UtcNow.ToString("R");
            string signature = method + "\n" + dateHeader + "\n" + path + message;

            request.Headers.Add("Date", dateHeader);

            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(apiSecret)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(signature));

                request.Headers.Add("x-mp-signature",
                    BitConverter.ToString(hash, 0, hash.Length).Replace("-", String.Empty).ToLower());
            }

        }

    }

    /// <summary>
    /// Supplies additional information when <see cref="RequestHeaderSdkMessage"/>s fail to send.
    /// </summary>
    public sealed class EventsApiFailureEventArgs : EventArgs
    {
        internal EventsApiFailureEventArgs(RequestHeaderSdkMessage upload, Exception error)
        {
            Error = error;
            RequestHeaderSdkMessage = upload;
        }

        /// <summary>
        /// Gets the <see cref="Exception"/> thrown when the failure occurred.
        /// </summary>
        public Exception Error { get; private set; }

        /// <summary>
        /// Gets the <see cref="RequestHeaderSdkMessage"/> associated with the event.
        /// </summary>
        public RequestHeaderSdkMessage RequestHeaderSdkMessage { get; private set; }
    }

    /// <summary>
    /// Supplies additional information when <see cref="RequestHeaderSdkMessage"/>s are successfully sent.
    /// </summary>
    public sealed class EventsApiSuccessEventArgs : EventArgs
    {
        internal EventsApiSuccessEventArgs(RequestHeaderSdkMessage upload, string response)
        {
            Response = response;
            RequestHeaderSdkMessage = upload;
        }

        /// <summary>
        /// Gets the response text.
        /// </summary>
        public string Response { get; private set; }

        /// <summary>
        /// Gets the <see cref="RequestHeaderSdkMessage"/> associated with the event.
        /// </summary>
        public RequestHeaderSdkMessage RequestHeaderSdkMessage { get; private set; }
    }

    /// <summary>
    /// Supplies additional information when <see cref="RequestHeaderSdkMessage"/>s fail due to non-2xx status.
    /// </summary>
    public sealed class UnsuccessfulApiRequestEventArgs : EventArgs
    {
        internal UnsuccessfulApiRequestEventArgs(RequestHeaderSdkMessage upload, int statusCode)
        {
            HttpStatusCode = statusCode;
            RequestHeaderSdkMessage = upload;
        }

        /// <summary>
        /// Gets the HTTP status code
        /// </summary>
        public int HttpStatusCode { get; private set; }

        /// <summary>
        /// Gets the <see cref="RequestHeaderSdkMessage"/> associated with the event.
        /// </summary>
        public RequestHeaderSdkMessage RequestHeaderSdkMessage { get; private set; }
    }
}
