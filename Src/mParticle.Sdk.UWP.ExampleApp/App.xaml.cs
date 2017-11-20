using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace mParticle.Sdk.UWP.ExampleApp
{
    /// <summary>
    /// Example UWP Application integrating the mParticle UWP SDK
    /// </summary>
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs launchArgs)
        {
            
            // Create an Identity Request:
            // The SDK will automatically make an Identify() request during initialization,
            // if you know identities of the current-user, you should provide them.
            // Otherwise, the SDK will use the Identities of the most recent user.
            var identifyRequest = IdentityApiRequest.EmptyUser()
                .CustomerId("foo")
                .Email("bar")
                .Build();

            // Create an MParticleOptions object:
            // You must at least provide an mParticle workspace key and secret
            MParticleOptions options =
                MParticleOptions.Builder(apiKey: "REPLACE ME", apiSecret: "REPLACE ME")
                .IdentifyRequest(identifyRequest)
                .LaunchArgs(launchArgs)
                .Logger(new ExampleConsoleLogger())
                .Build();

            // Initialize the mParticle SDK:
            // You must do this prior to calling MParticle.Instance
            var task = MParticle.StartAsync(options);
            HandleIdentityTaskAsync(task);

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                Window.Current.Content = rootFrame;
            }

            if (launchArgs.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(MainPage), launchArgs.Arguments);
                }
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Example async handling of Identity requests.
        /// </summary>
        /// <param name="task"></param>
        public static async void HandleIdentityTaskAsync(Task<IdentityApiResult> task)
        {
            var result = await task;
            if (result.Successful)
            {
                var user = result.User;
                Debug.Write("Identity Example App, successful identity call for user: " + user.Mpid.ToString());
            }
            else
            {
                string errorString = JsonConvert.SerializeObject(result.Error);
                Debug.Write("Identity Example App, error: " + errorString + "\n");
                switch (result.Error.StatusCode)
                {
                    case IdentityApi.Unauthorized:
                    //Unauthorized: this indicates a bad workspace API key and / or secret.
                    case IdentityApi.BadRequest:
                    //Bad request: inspect the error response and modify as necessary.
                    case IdentityApi.ServerError:
                    //Server error: perform an exponential backoff and retry.
                    case IdentityApi.ThrottleError:
                    //Throttle error: this indicates that your mParticle workspace has exceeded its provisioned
                    //Identity API throughput. Perform an exponential backoff and retry.
                    case IdentityApi.UnknownError:
                    //Unknown error: this indicates that the device has no network connectivity. 
                    //Retry when the device reconnects.
                    default:
                        break;
                }
            }
        }
    }
}
