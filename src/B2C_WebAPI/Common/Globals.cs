using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2C_WebAPI.Common
{
    public static class Globals
    {
        public static string SPORTS_FEED_URL = "http://www.espn.com/espn/rss/news";
        public static string MOVIE_FEED_URL = "http://www.comingsoon.net/feed";
        public static string LATEST_NEWS_FEED_URL = "http://rss.cnn.com/rss/cnn_us.rss";
        public static string TECHNOLOGY_FEED_URL = "http://rss.cnn.com/rss/cnn_tech.rss";
        public static string BUSINESS_NEWS_FEED_URL = "http://rss.cnn.com/rss/money_latest.rss";

        #region Graph API Constants
        public static string aadInstance = "https://login.microsoftonline.com/";
        public static string aadGraphResourceId = "https://graph.windows.net/";
        public static string aadGraphEndpoint = "https://graph.windows.net/";
        public static string aadGraphSuffix = "";
        public static string aadGraphVersion = "api-version=1.6";
        #endregion
    }
}