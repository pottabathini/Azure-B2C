using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2C_NativeClient
{
    public class GraphObjectModel
    {
        public string UserId { get; set; }
        public string UserJsonData { get; set; }
    }    
    public class GraphUserModel
    {
        public GraphUserModel()
        {
            SignInNames = new List<SignInName>();
            PasswordProfile = new PasswordProfile();
        }

        [JsonProperty(PropertyName = "accountEnabled")]
        public bool AccountEnabled { get; set; }

        [JsonProperty(PropertyName = "signInNames")]
        public List<SignInName> SignInNames { get; set; }

        [JsonProperty(PropertyName = "creationType")]
        public string CreationType { get; set; }

        [JsonProperty(PropertyName = "displayName")]
        public string DisplayName { get; set; }

        [JsonProperty(PropertyName = "mailNickname")]
        public string MailNickname { get; set; }

        [JsonProperty(PropertyName = "passwordProfile")]
        public PasswordProfile PasswordProfile { get; set; }

        [JsonProperty(PropertyName = "passwordPolicies")]
        public string PasswordPolicies { get; set; }
    }

    public class SignInName
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
    public class PasswordProfile
    {
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "forceChangePasswordNextLogin")]
        public bool ForceChangePasswordNextLogin { get; set; }
    }
    public class GraphException
    {
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }
        public string StackTrace { get; set; }
    }
}