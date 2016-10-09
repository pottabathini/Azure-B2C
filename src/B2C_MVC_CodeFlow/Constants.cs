namespace MvcHybrid
{
    public class Constants
    {
        public const string BaseAddress = "https://login.microsoftonline.com/tenantazureb2c.onmicrosoft.com/v2.0";

        public const string AuthorizeEndpoint = BaseAddress + "/connect/authorize";
        public const string LogoutEndpoint = BaseAddress + "/connect/endsession";
        public const string TokenEndpoint = "https://login.microsoftonline.com/tenantazureb2c.onmicrosoft.com/oauth2/v2.0/token?p=b2c_1_sign_in";
        public const string TokenEndPointSignInPolicy = "https://login.microsoftonline.com/tenantazureb2c.onmicrosoft.com/oauth2/v2.0/token?p=b2c_1_sign_in";
        public const string UserInfoEndpoint = BaseAddress + "/connect/userinfo";
        public const string IdentityTokenValidationEndpoint = BaseAddress + "/connect/identitytokenvalidation";
        public const string TokenRevocationEndpoint = BaseAddress + "/connect/revocation";
        public const string IntrospectionEndpoint = BaseAddress + "/connect/introspect";

        public const string ClientId = "2048095b-a953-4150-9ab4-5a2f6334d99f";

        public const string AspNetWebApiSampleApi = "http://localhost:63071/";
        //public const string AspNetWebApiSampleApi = "http://localhost:5000/";
    }
}