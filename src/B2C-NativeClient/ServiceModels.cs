using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2C_NativeClient
{
    public class Feed
    {
        public string Subject { get; set; }
        public string Summary { get; set; }
        public DateTimeOffset PublishedDate { get; set; }
        public List<string> ImageLinks { get; set; }
        public string ImageLink
        {
            get
            {
                if(ImageLinks!=null && ImageLinks.Count > 0)
                {
                    return ImageLinks[0];
                }
                return null;
            }
        }
    }
}
