using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2C_WebAPI
{
    public partial class Startup
    {
        // The OWIN middleware will invoke this method when the app starts
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}