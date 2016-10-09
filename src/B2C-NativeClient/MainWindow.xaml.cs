using Microsoft.Identity.Client;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace B2C_NativeClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields
        public PublicClientApplication pca;
        public string CurrentUserId;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            EditProfileButton.Visibility = Visibility.Collapsed;
            SignOutButton.Visibility = Visibility.Collapsed;
            UserPreferencesPanel.Visibility = Visibility.Collapsed;
            CreateUserTabItem.IsEnabled = false;
            DeleteUserTabItem.IsEnabled = false;

            pca = new PublicClientApplication(Globals.clientId) { };
        }

        #region Events
        /// <summary>
        /// To SignUp the user using Azure B2C SignUp policy
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void SignUp(object sender = null, RoutedEventArgs args = null)
        {
            AuthenticationResult result = null;
            try
            {
                // Use the app's clientId here as the scope parameter, indicating that
                // you want a token to the your app's backend web API (represented by
                // the cloud hosted task API).  Use the UiOptions.ForceLogin flag to
                // indicate to MSAL that it should show a sign-up UI no matter what.
                result = await pca.AcquireTokenAsync(new string[] { Globals.clientId },
                        string.Empty, UiOptions.ForceLogin, null, null, Globals.authority,
                        Globals.signUpPolicy);

                // Upon success, indicate in the app that the user is signed in.
                SignUpButton.Visibility = Visibility.Collapsed;
                SignInButton.Visibility = Visibility.Collapsed;
                ResetPasswordButton.Visibility = Visibility.Collapsed;
                EditProfileButton.Visibility = Visibility.Visible;
                SignInAndSignUpButton.Visibility = Visibility.Collapsed;
                SignOutButton.Visibility = Visibility.Visible;
                UserPreferencesPanel.Visibility = Visibility.Visible;
                CreateUserTabItem.IsEnabled = true;
                DeleteUserTabItem.IsEnabled = true;

                // When the request completes successfully, you can get user 
                // information from the AuthenticationResult
                LoadData(result);
            }

            // Handle any exeptions that occurred during execution of the policy.
            catch (MsalException ex)
            {
                if (ex.ErrorCode != "authentication_canceled")
                {
                    // An unexpected error occurred.
                    string message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        message += "Inner Exception : " + ex.InnerException.Message;
                    }
                    //TODO: Log this message
                    MessageBox.Show(message);
                }

                return;
            }
        }

        /// <summary>
        /// To SignIn the user using Azure B2C SignIn policy
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void SignIn(object sender = null, RoutedEventArgs args = null)
        {
            AuthenticationResult result = null;
            try
            {
                result = await pca.AcquireTokenAsync(new string[] { Globals.clientId },
                    string.Empty, UiOptions.ForceLogin, null, null, Globals.authority,
                    Globals.signInPolicy);

                SignInButton.Visibility = Visibility.Collapsed;
                SignUpButton.Visibility = Visibility.Collapsed;
                EditProfileButton.Visibility = Visibility.Visible;
                ResetPasswordButton.Visibility = Visibility.Collapsed;
                SignInAndSignUpButton.Visibility = Visibility.Collapsed;
                SignOutButton.Visibility = Visibility.Visible;
                UserPreferencesPanel.Visibility = Visibility.Visible;
                CreateUserTabItem.IsEnabled = true;
                DeleteUserTabItem.IsEnabled = true;

                // When the request completes successfully, you can get user 
                // information from the AuthenticationResult                
                LoadData(result);
            }
            catch (MsalException ex)
            {
                if (ex.ErrorCode != "authentication_canceled")
                {
                    // An unexpected error occurred.
                    string message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        message += "Inner Exception : " + ex.InnerException.Message;
                    }

                    MessageBox.Show(message);
                }

                return;
            }
        }

        /// <summary>
        /// To SignIn and Signup the user using Azure B2C SignIn and SignUp policy
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void SignInAndSignUp(object sender = null, RoutedEventArgs args = null)
        {
            AuthenticationResult result = null;
            try
            {
                result = await pca.AcquireTokenAsync(new string[] { Globals.clientId },
                    string.Empty, UiOptions.ForceLogin, null, null, Globals.authority,
                    Globals.signInAndSignUpPolicy);

                SignInButton.Visibility = Visibility.Collapsed;
                SignUpButton.Visibility = Visibility.Collapsed;
                EditProfileButton.Visibility = Visibility.Visible;
                ResetPasswordButton.Visibility = Visibility.Collapsed;
                SignInAndSignUpButton.Visibility = Visibility.Collapsed;
                SignOutButton.Visibility = Visibility.Visible;
                UserPreferencesPanel.Visibility = Visibility.Visible;
                CreateUserTabItem.IsEnabled = true;
                DeleteUserTabItem.IsEnabled = true;

                // When the request completes successfully, you can get user 
                // information from the AuthenticationResult                            
                LoadData(result);
            }
            catch (MsalException ex)
            {
                if (ex.ErrorCode != "authentication_canceled")
                {
                    // An unexpected error occurred.
                    string message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        message += "Inner Exception : " + ex.InnerException.Message;
                    }

                    MessageBox.Show(message);
                }

                return;
            }
        }

        /// <summary>
        /// Edit User profile using Azure B2C Edit Profile Policy
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void EditProfile(object sender = null, RoutedEventArgs args = null)
        {
            AuthenticationResult result = null;
            try
            {
                result = await pca.AcquireTokenAsync(new string[] { Globals.clientId },
                    string.Empty, UiOptions.ForceLogin, null, null, Globals.authority,
                    Globals.editProfilePolicy);

                // When the request completes successfully, you can get user 
                // information from the AuthenticationResult
                LoadData(result);
            }
            catch (MsalException ex)
            {
                if (ex.ErrorCode != "authentication_canceled")
                {
                    // An unexpected error occurred.
                    string message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        message += "Inner Exception : " + ex.InnerException.Message;
                    }

                    MessageBox.Show(message);
                }

                return;
            }
        }

        /// <summary>
        /// Reset Local Account of the User to reset password using Azure B2C reset password policy
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void RestPassword(object sender = null, RoutedEventArgs args = null)
        {
            AuthenticationResult result = null;
            try
            {
                result = await pca.AcquireTokenAsync(new string[] { Globals.clientId },
                    string.Empty, UiOptions.ForceLogin, null, null, Globals.authority,
                    Globals.passwordResetPolicy);

                SignInButton.Visibility = Visibility.Collapsed;
                SignUpButton.Visibility = Visibility.Collapsed;
                EditProfileButton.Visibility = Visibility.Visible;
                ResetPasswordButton.Visibility = Visibility.Collapsed;
                SignInAndSignUpButton.Visibility = Visibility.Collapsed;
                SignOutButton.Visibility = Visibility.Visible;
                UserPreferencesPanel.Visibility = Visibility.Visible;
                CreateUserTabItem.IsEnabled = true;
                DeleteUserTabItem.IsEnabled = true;

                // When the request completes successfully, you can get user 
                // information from the AuthenticationResult
                LoadData(result);
            }
            catch (MsalException ex)
            {
                if (ex.ErrorCode != "authentication_canceled")
                {
                    // An unexpected error occurred.
                    string message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        message += "Inner Exception : " + ex.InnerException.Message;
                    }

                    MessageBox.Show(message);
                }

                return;
            }
        }

        /// <summary>
        /// Represents an event that is raised when the sign-out operation is complete.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SignOut(object sender, RoutedEventArgs e)
        {
            // Clear any remnants of the user's session.
            pca.UserTokenCache.Clear(Globals.clientId);

            // This is a helper method that clears browser cookies in the browser control that MSAL uses, it is not part of MSAL.
            ClearCookies();

            SignInButton.Visibility = Visibility.Visible;
            SignUpButton.Visibility = Visibility.Visible;
            EditProfileButton.Visibility = Visibility.Collapsed;
            ResetPasswordButton.Visibility = Visibility.Visible;
            SignInAndSignUpButton.Visibility = Visibility.Visible;
            SignOutButton.Visibility = Visibility.Collapsed;
            UserPreferencesPanel.Visibility = Visibility.Collapsed;
            CreateUserTabItem.IsEnabled = false;
            DeleteUserTabItem.IsEnabled = false;

            ClearData();

            return;
        }

        /// <summary>
        /// Update the Default prefered tab using Graph API
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void UpdateUserPreference(object sender = null, RoutedEventArgs args = null)
        {
            try
            {
                if (TabsComboBox.SelectedIndex != 0)
                {
                    string accessToken = await GetAccessToken();
                    var response = await SetTabPreference(Globals.API_UPDATE_FEED_PREFERENCE, accessToken, new GraphObjectModel()
                    {
                        UserJsonData = JsonConvert.SerializeObject(new
                        {
                            extension_0250e000b26943759c571e4d89f8cc90_DefaultFeedTab = TabsComboBox.SelectedItem as string
                        }),
                        UserId = CurrentUserId
                    });

                    if (string.IsNullOrEmpty(response))
                    {
                        MessageBox.Show("Preferences has been saved. Please logout and login to reflect the changes");
                    }
                }
                else
                {
                    MessageBox.Show("Please choose a value form the combobox");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        #endregion

        #region Graph API Methods
        private async void CreateUser(object sender = null, RoutedEventArgs args = null)
        {
            try
            {
                GraphUserModel newUser = new GraphUserModel();
                newUser.AccountEnabled = true;
                newUser.SignInNames.Add(new SignInName()
                {
                    Type = "emailAddress",
                    Value = CreateUserEmailTextBox.Text
                });
                newUser.CreationType = "LocalAccount";
                newUser.DisplayName = CreateUserDisplayName.Text;
                newUser.MailNickname = CreateUserNickName.Text;

                newUser.PasswordProfile.Password = CreateUserPassword.Text;
                newUser.PasswordProfile.ForceChangePasswordNextLogin = false;

                newUser.PasswordPolicies = "DisablePasswordExpiration";

                string accessToken = await GetAccessToken();
                var response = Service.Execute<string>(Globals.API_CREATE_USER, Method.POST, accessToken, new GraphObjectModel()
                {
                    UserJsonData = JsonConvert.SerializeObject(newUser),
                    UserId = CurrentUserId //optional no need to set
                });

                if (!string.IsNullOrEmpty(response))
                {
                    MessageBox.Show(response);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }
        private async void DeleteUser(object sender = null, RoutedEventArgs args = null)
        {
            try
            {
                if (string.IsNullOrEmpty(DeleteUserIdTextBox.Text))
                {
                    MessageBox.Show("Please enter userid to delete");
                    return;
                }
                if (DeleteUserIdTextBox.Text == CurrentUserId)
                {
                    MessageBox.Show("You can not enter current userid");
                    return;
                }
                string accessToken = await GetAccessToken();
                var response = Service.Execute<string>(Globals.API_DELETE_USER, Method.POST, accessToken, new GraphObjectModel()
                {
                    UserId = DeleteUserIdTextBox.Text
                });

                if (!string.IsNullOrEmpty(response))
                {
                    MessageBox.Show(response);
                }
                else
                {
                    MessageBox.Show("User deleted successfully!.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }
        #endregion

        #region Helper Methods
        private void ClearCookies()
        {
            const int INTERNET_OPTION_END_BROWSER_SESSION = 42;
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
        }
        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);

        private async Task<string> GetAccessToken()
        {
            // Here we want to check for a cached token, independent of whatever policy was used to acquire it.
            TokenCacheItem tci = pca.UserTokenCache.ReadItems(Globals.clientId).Where(i => i.Scope.Contains(Globals.clientId) && !string.IsNullOrEmpty(i.Policy)).FirstOrDefault();
            string existingPolicy = tci == null ? null : tci.Policy;

            // Use AcquireTokenSilent to indicate that MSAL should throw an exception if a token cannot be acquired
            AuthenticationResult result = await pca.AcquireTokenSilentAsync(new string[] { Globals.clientId }, string.Empty, Globals.authority, existingPolicy, false);

            return result.Token;
        }
        #endregion

        #region Data Methods
        private async void LoadData(AuthenticationResult result)
        {
            try
            {                
                UsernameLabel.Content = result.User.Name;
                LoadComboBox();
                LoadClaims(result.IdToken);                              
                LoadTabsData();
            }
            catch (Exception ex)
            {
                // An unexpected error occurred.
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += "Inner Exception : " + ex.InnerException.Message;
                }

                MessageBox.Show(message);
            }
        }
        private async void LoadTabsData()
        {
            string accessToken = await GetAccessToken();
            //Sport Feeds
            var sportFeeds = await GetFeeds(Globals.API_SPORT_FEED, accessToken);
            SportFeedsListView.ItemsSource = sportFeeds;

            //Movie Feeds
            var movieFeeds = await GetFeeds(Globals.API_MOVIES_FEED, accessToken);
            MovieFeedsListView.ItemsSource = movieFeeds;

            //Latest News Feeds
            var topNewsFeeds = await GetFeeds(Globals.API_TOP_NEWS_FEED, accessToken);            
            TopNewsListView.ItemsSource = topNewsFeeds;

            //Technology News Feeds
            var technologyFeeds = await GetFeeds(Globals.API_TECHNOLOGY_FEED, accessToken);
            TechnologyNewsListView.ItemsSource = technologyFeeds;

            //Business News Feeds
            var businessFeeds = await GetFeeds(Globals.API_BUSINESS_FEED, accessToken);
            BusinessNewsListView.ItemsSource = businessFeeds;
        }
        private async void LoadClaims(string id_token)
        {
            var jwtToken = new JwtSecurityToken(id_token);
            Dictionary<string, string> claimsDictionary = new Dictionary<string, string>();

            if (jwtToken != null && jwtToken.Claims != null)
            {
                foreach (var claim in jwtToken.Claims)
                {
                    claimsDictionary.Add(claim.Type, claim.Value);
                    if (claim.Type == "oid")
                    {
                        CurrentUserId = claim.Value;
                        UserIdTextBox.Text = claim.Value;
                    }
                    if(claim.Type == "extension_DefaultFeedTab")
                    {
                        SetComboBoxValue(claim.Value);
                    }
                }
            }
            ClaimsListView.ItemsSource = claimsDictionary;
        }
        private async void LoadComboBox()
        {
            List<string> data = new List<string>();
            data.Add("--Choose Default Tab--");
            data.Add("Sports");
            data.Add("Movies");
            data.Add("Top News");
            data.Add("Tech News");
            data.Add("Business News");

            TabsComboBox.ItemsSource = data;
            TabsComboBox.SelectedIndex = 0;
        }
        private async void ClearData()
        {
            //TODO: clear data on the page
            UsernameLabel.Content = "Please Login";

            for (int i = 0; i < FeedTabs.Items.Count - 2; i++)
            {
                FeedTabs.SelectedIndex = i;
                FeedTabs.UpdateLayout();
            }

            System.Threading.Thread.Sleep(250);
            FeedTabs.SelectedIndex = 0;
        }
        private async void SetComboBoxValue(string value)
        {
            TabsComboBox.SelectedValue = value;
            //Highlight the default tab
            FeedTabs.SelectedIndex = TabsComboBox.SelectedIndex;            
        }
        #endregion

        #region API Methods
        private async Task<List<Feed>> GetFeeds(string apiURL, string accessToken)
        {
            var feeds = Service.Execute<List<Feed>>(apiURL, Method.GET, accessToken);

            return feeds;
        }
        private async Task<string> SetTabPreference(string apiURL, string accessToken, GraphObjectModel graphObject)
        {
            var response = Service.Execute<string>(apiURL, Method.POST, accessToken, graphObject);

            return response;
        }
        #endregion
    }
}
