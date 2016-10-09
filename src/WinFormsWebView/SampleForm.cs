using IdentityModel.OidcClient;
using IdentityModel.OidcClient.WebView.WinForms;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace WinForms
{
    public partial class SampleForm : Form
    {
        private OidcClient _oidcClient;
        private HttpClient _apiClient;

        public SampleForm()
        {
            InitializeComponent();

            var authority = "https://login.microsoftonline.com/tenantazureb2c.onmicrosoft.com/v2.0";

            var options = new OidcClientOptions(
                authority,
                "2048095b-a953-4150-9ab4-5a2f6334d99f", 
                "not-used",
                "openid 2048095b-a953-4150-9ab4-5a2f6334d99f offline_access",
                "urn:ietf:wg:oauth:2.0:oob", 
                new WinFormsWebView());
            options.UseFormPost = true;

            _oidcClient = new OidcClient(options);  
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            AccessTokenDisplay.Clear();
            OtherDataDisplay.Clear();
           
            var result = await _oidcClient.LoginAsync(Silent.Checked, new { p = "b2c_1_sign_in" });

            if (result.Success)
            {
                AccessTokenDisplay.Text = result.AccessToken;

                var sb = new StringBuilder(128);
                foreach (var claim in result.Claims)
                {
                    sb.AppendLine($"{claim.Type}: {claim.Value}");
                }

                if (!string.IsNullOrWhiteSpace(result.RefreshToken))
                {
                    sb.AppendLine($"refresh token: {result.RefreshToken}");
                }

                OtherDataDisplay.Text = sb.ToString();

                _apiClient = new HttpClient(result.Handler);
                _apiClient.BaseAddress = new Uri("https://demo.identityserver.io/api/");
            }
            else
            {
                MessageBox.Show(this, result.Error, "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void LogoutButton_Click(object sender, EventArgs e)
        {
            await _oidcClient.LogoutAsync(trySilent: Silent.Checked);
            AccessTokenDisplay.Clear();
            OtherDataDisplay.Clear();
        }

        private async void CallApiButton_Click(object sender, EventArgs e)
        {
            if (_apiClient == null)
            {
                return;
            }

            var result = await _apiClient.GetAsync("test");
            if (result.IsSuccessStatusCode)
            {
                OtherDataDisplay.Text = JArray.Parse(await result.Content.ReadAsStringAsync()).ToString();
            }
            else
            {
                OtherDataDisplay.Text = result.ReasonPhrase;
            }
        }
    }
}