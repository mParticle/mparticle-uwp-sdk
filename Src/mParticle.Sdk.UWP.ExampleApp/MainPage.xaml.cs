using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace mParticle.Sdk.UWP.ExampleApp
{
    /// <summary>
    /// Example usage of the mParticle UWP SDK
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            //CurrentUser may be null if no user has been identified yet
            DisplayUser(MParticle.Instance.Identity.CurrentUser);

            //listen for a user update
            MParticle.Instance.Identity.IdentityStateChange += Identity_IdentityStateChange;
        }

        private void Identity_IdentityStateChange(object sender, IdentityStateChangeEventArgs e)
        {
            DisplayUser(e.User);
        }

        private void SetUserAttributeButton_Click(object sender, RoutedEventArgs e)
        {
            var userAttributeKey = (FindName("userAttributeKey") as TextBox).Text;
            var userAttributeValue = (FindName("userAttributeValue") as TextBox).Text;
            MParticle.Instance.Identity.CurrentUser?.UserAttribute(userAttributeKey, userAttributeValue);
            DisplayUser(MParticle.Instance.Identity.CurrentUser);
        }

        private void OnIdentityButton_Click(object sender, RoutedEventArgs e)
        {
            string content = ((Button)sender).Content as string;
            var customerId = (FindName("customerIdInput") as TextBox).Text;
            var email = (FindName("emailInput") as TextBox).Text;
            var identityRequest = IdentityApiRequest.EmptyUser()
                .CustomerId(customerId)
                .Email(email)
                .Build();
            Task<IdentityApiResult> task = null;
            switch (content)
            {
                case "Identify":
                    task = MParticle.Instance.Identity.IdentifyAsync(identityRequest);
                    break;
                case "Login":
                    task = MParticle.Instance.Identity.LoginAsync(identityRequest);
                    break;
                case "Logout":
                    task = MParticle.Instance.Identity.LogoutAsync(identityRequest);
                    break;
                case "Modify":
                    task = MParticle.Instance.Identity.CurrentUser?.ModifyAsync(identityRequest);
                    break;
            }

            App.HandleIdentityTaskAsync(task);
        }

        private void DisplayUser(MParticleUser user)
        {
            if (user == null)
                return;

            var textBlock = (FindName("currentUserText") as TextBlock);
            string text = "MPID: " + user.Mpid.ToString() + "\n";
            string email = "";
            if (user.UserIdentities.ContainsKey(Core.Dto.Events.UserIdentityType.Email))
            {
                email = user.UserIdentities[Core.Dto.Events.UserIdentityType.Email];
            }

            text += "Email: " + email + "\n";
            string customerId = "";
            if (user.UserIdentities.ContainsKey(Core.Dto.Events.UserIdentityType.CustomerId))
            {
                customerId = user.UserIdentities[Core.Dto.Events.UserIdentityType.CustomerId];
            }

            text += "Customer ID: " + customerId + "\n";
            text += "User attributes:\n";
            text += string.Join("\n", user.UserAttributes);
            textBlock.Text = text;
        }
    }
}