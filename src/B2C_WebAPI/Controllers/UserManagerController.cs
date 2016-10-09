using B2C_WebAPI.GraphAPI;
using B2C_WebAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace B2C_WebAPI.Controllers
{
    //[Authorize]
    [RoutePrefix("api/UserManager")]
    public class UserManagerController : ApiController
    {
        private static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        private static string clientId = ConfigurationManager.AppSettings["b2cGraph:ClientId"];
        private static string clientSecret = ConfigurationManager.AppSettings["b2cGraph:ClientSecret"];

        [Route("UpdateFeedPreference")]
        [HttpPost]
        public async Task<string> UpdateFeedPreference(GraphObject graphObject)
        {
            var client = new B2CGraphClient(clientId, clientSecret, tenant);
            var response = await client.UpdateUser(graphObject.UserId, graphObject.UserJsonData);

            return response;
        }

        [Route("CreateUser")]
        [HttpPost]
        public async Task<string> CreateUser(GraphObject graphObject)
        {
            var client = new B2CGraphClient(clientId, clientSecret, tenant);
            var response = await client.CreateUser(graphObject.UserJsonData);

            return response;
        }

        [Route("DeleteUser")]
        [HttpPost]
        public async Task<string> DeleteUser(GraphObject graphObject)
        {
            var client = new B2CGraphClient(clientId, clientSecret, tenant);
            var response = await client.DeleteUser(graphObject.UserId);

            return response;
        }
    }
}
