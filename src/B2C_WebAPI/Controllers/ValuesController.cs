using B2C_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Web.Http;
using System.Xml;

namespace B2C_WebAPI.Controllers
{
    //[Authorize]
    public class ValuesController : ApiController
    {
        // GET api/values        
        public IEnumerable<string> Get()
        {
            string url = "http://www.espn.com/espn/rss/news";
            XmlReader reader = XmlReader.Create(url);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();

            List<Feed> feeds = new List<Feed>();

            foreach (SyndicationItem item in feed.Items)
            {
                feeds.Add(new Feed() {
                    Subject = item.Title.Text,
                    Summary = item.Summary.Text,
                    PublishedDate = item.PublishDate
                });
            }

            return new string[] { "value1", "value2", "values3" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
