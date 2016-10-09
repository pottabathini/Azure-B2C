using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2C_WebAPI.Models
{
    public class Feed
    {
        public string Subject { get; set; }
        public string Summary { get; set; }
        public DateTimeOffset PublishedDate { get; set; }
        public List<string> ImageLinks { get; set; }
    }
}