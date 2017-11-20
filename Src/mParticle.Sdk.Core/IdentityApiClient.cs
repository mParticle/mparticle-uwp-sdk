using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using mParticle.Sdk.Core.Dto.Identity;
using Newtonsoft.Json;

namespace mParticle.Sdk.Core
{
    public class IdentityApiClient
    {
        private readonly string apiKey;
        private readonly string apiSecret;

        private const string IdentityUrlFormat = "https://identity.mparticle.com/v1/{0}";
        private const string IdentityPathIdentify = "identify";
        private const string IdentityPathLogin = "login";
        private const string IdentityPathLogout = "logout";
        private const string IdentityPathModify = "{0}/modify";

        private IdentityApiClient() { }

        public IdentityApiClient(String key, String secret)
        {
            apiKey = key;
            apiSecret = secret;
        }

        public string UserAgent { get; set; }

        public ILogger Logger { get; set; }

        public async Task<Object> Identify(IdentityRequest identityRequest)
        {
            Uri identifyUri = new Uri(String.Format(IdentityUrlFormat, IdentityPathIdentify));
            return await SendIdentityRequest(identityRequest, identifyUri);
        }

        public async Task<Object> Login(IdentityRequest identityRequest)
        {
            Uri identifyUri = new Uri(String.Format(IdentityUrlFormat, IdentityPathLogin));
            return await SendIdentityRequest(identityRequest, identifyUri);
        }

        public async Task<Object> Logout(IdentityRequest identityRequest)
        {
            Uri identifyUri = new Uri(String.Format(IdentityUrlFormat, IdentityPathLogout));
            return await SendIdentityRequest(identityRequest, identifyUri);
        }

        public async Task<Object> Modify(long mpid, ModifyRequest modifyRequest)
        {
            Uri modifyUri = new Uri(String.Format(IdentityUrlFormat, String.Format(IdentityPathModify, mpid.ToString())));
            return await SendIdentityRequest(modifyRequest, modifyUri);
        }

        private async Task<Object> SendIdentityRequest(IdentityBaseRequest identityRequest, Uri identityUri)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await SendIdentityRequestAsync(identityRequest, identityUri, httpClient);
                    if (response.IsSuccessStatusCode)
                    {
                        var stringResult = response.Content.ReadAsStringAsync().Result;
                        this.Logger?.Log(new LogEntry(LoggingEventType.Debug, "Identity Api Request Success:\n" + stringResult));
                        return JsonConvert.DeserializeObject<IdentityResponse>(stringResult) ?? new IdentityResponse();
                    }
                    else
                    {

                        this.Logger?.Log(new LogEntry(LoggingEventType.Debug, "Identity Api Request failed:\n" + response.ToString()));
                        var stringResult = response.Content.ReadAsStringAsync().Result;
                        var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(stringResult);
                        return errorResponse ?? new ErrorResponse() { StatusCode = (int)response.StatusCode };
                    }
                }
                catch (Exception ex)
                {
                    this.Logger?.Log(new LogEntry(LoggingEventType.Debug, "Identity Api Request failed:\n" + ex.Message));
                    return new ErrorResponse() { StatusCode = -1 };
                }
            }
        }

        private async Task<HttpResponseMessage> SendIdentityRequestAsync(IdentityBaseRequest identityRequest, Uri identityUri, HttpClient httpClient)
        {
            using (var httpRequest = CreateHttpRequest(identityRequest, identityUri))
            {
                return await httpClient.SendAsync(httpRequest);
            }
        }

        private HttpRequestMessage CreateHttpRequest(IdentityBaseRequest identityRequest, Uri identityUri)
        {
            String identityJson = JsonConvert.SerializeObject(identityRequest);
            this.Logger?.Log(new LogEntry(LoggingEventType.Debug, "Performing identity request:\n" + identityJson));
            var request = new HttpRequestMessage()
            {
                RequestUri = identityUri,
                Method = HttpMethod.Post,
                Content = new StringContent(identityJson, Encoding.UTF8, "application/json")
            };

            if (!string.IsNullOrEmpty(UserAgent))
            {
                request.Headers.UserAgent.ParseAdd(UserAgent);
            }
            request.Headers.Add("x-mp-key", apiKey);

            EventsApiClient.AddSignature(request, identityJson, identityUri.PathAndQuery, apiSecret);

            return request;
        }
    }
}
