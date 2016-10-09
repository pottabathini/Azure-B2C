using B2C_WebAPI.Common;
using B2C_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;

namespace B2C_WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Feed")]
    public class FeedController : ApiController
    {
        [Route("SportFeeds")]
        [HttpGet]
        public List<Feed> GetSportFeeds()
        {
            XmlReader reader = XmlReader.Create(Globals.SPORTS_FEED_URL);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();
            //process and return feeds
            return ProcessFeedData(feed.Items);
        }

        [Route("MovieFeeds")]
        [HttpGet]
        public List<Feed> GetLatestMoviesFeeds()
        {
            XmlReader reader = XmlReader.Create(Globals.MOVIE_FEED_URL);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();
            //process and return feeds
            return ProcessFeedData(feed.Items);
        }

        [Route("TopNewsFeeds")]
        [HttpGet]
        public List<Feed> GetTopNewsFeeds()
        {
            XmlReader reader = XmlReader.Create(Globals.LATEST_NEWS_FEED_URL);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();
            //process and return feeds
            return ProcessFeedData(feed.Items);
        }

        [Route("TechnologyFeeds")]
        [HttpGet]
        public List<Feed> GetTechnologyFeeds()
        {
            XmlReader reader = XmlReader.Create(Globals.TECHNOLOGY_FEED_URL);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();
            //process and return feeds
            return ProcessFeedData(feed.Items);
        }

        [Route("BusinessNewsFeeds")]
        [HttpGet]
        public List<Feed> GetBusinessFeeds()
        {
            XmlReader reader = XmlReader.Create(Globals.BUSINESS_NEWS_FEED_URL);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();
            //process and return feeds
            return ProcessFeedData(feed.Items);
        }

        #region Private Methods
        private List<Feed> ProcessFeedData(IEnumerable<SyndicationItem> items)
        {
            List<Feed> feeds = new List<Feed>();

            foreach (SyndicationItem item in items)
            {
                var feed = new Feed()
                {
                    Subject = item.Title.Text,
                    Summary = item.Summary.Text,
                    PublishedDate = item.PublishDate,
                    ImageLinks = new List<string>()
                };
                if (item.ElementExtensions != null)
                {
                    foreach (SyndicationElementExtension extension in item.ElementExtensions)
                    {
                        XElement element = extension.GetObject<XElement>();

                        if (element.HasAttributes)
                        {
                            foreach (var attribute in element.Attributes())
                            {
                                string value = attribute.Value.ToLower();
                                if ((value.StartsWith("http://") || value.StartsWith("https://")) && (value.EndsWith(".jpg") || value.EndsWith(".png") || value.EndsWith(".gif")))
                                {
                                    feed.ImageLinks.Add(value);
                                }
                            }
                        }
                    }
                }
                feeds.Add(feed);
            }

            return feeds;
        }
        #endregion
    }
}
