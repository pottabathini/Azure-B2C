using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2C_NativeClient
{
    public static class Globals
    {
        #region B2C Properties
        public static string aadInstance = "https://login.microsoftonline.com/";
        public static string redirectUri = "urn:ietf:wg:oauth:2.0:oob";
        public static string tenant = "tenantazureb2c.onmicrosoft.com";
        public static string clientId = "0bcf1588-48a7-4ffd-82ef-5a5d2ad770c5";
        public static string signInPolicy = "B2C_1_sign_in_wpf";
        public static string signUpPolicy = "B2C_1_sign_up_wpf";
        public static string editProfilePolicy = "B2C_1_edit_profile_wpf";
        public static string passwordResetPolicy = "B2C_1_password_reset_wpf";
        public static string signInAndSignUpPolicy = "B2C_1_sign_in_sign_up_wpf";

        //public static string clientId = "2048095b-a953-4150-9ab4-5a2f6334d99f";
        //public static string signInPolicy = "B2C_1_sign_in";
        //public static string signUpPolicy = "B2C_1_sign_up";

        public static string authority = string.Concat(aadInstance, tenant);
        #endregion

        #region Service Properties
        public static string Service_API_URL = "http://localhost:63071/api/";
        public static string API_SPORT_FEED = Service_API_URL + "feed/Sportfeeds";
        public static string API_MOVIES_FEED = Service_API_URL + "feed/MovieFeeds";
        public static string API_TOP_NEWS_FEED = Service_API_URL + "feed/TopNewsFeeds";
        public static string API_TECHNOLOGY_FEED = Service_API_URL + "feed/TechnologyFeeds";
        public static string API_BUSINESS_FEED = Service_API_URL + "feed/BusinessNewsFeeds";

        public static string API_UPDATE_FEED_PREFERENCE = Service_API_URL + "UserManager/UpdateFeedPreference";
        public static string API_CREATE_USER = Service_API_URL + "UserManager/CreateUser";
        public static string API_DELETE_USER = Service_API_URL + "UserManager/DeleteUser";
        #endregion
    }
}