using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2C_NativeClient
{
    public static class Service
    {
        public static T Execute<T>(string endPoint, Method method, string token, object data = null, bool IsGraphRequest = false)
        {
            var client = new RestClient(endPoint);
            var request = new RestRequest(method);

            request.AddHeader("Authorization", string.Format("bearer {0}", token));
            request.RequestFormat = DataFormat.Json;

            if (data != null)
            {
                request.AddBody(data);
            }
            
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = JsonConvert.DeserializeObject<T>(response.Content);
                return result;
            }
            else
            {   
                throw new Exception(response.Content);
            }
        }
    }
}
