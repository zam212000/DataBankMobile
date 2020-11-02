using MyBodyTemperature.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json;
using MyBodyTemperature.Helpers;
using Newtonsoft.Json.Linq;
using MyBodyTemperature.Services.AnalyticsService;

namespace MyBodyTemperature.Services.UserProfile
{
    public class LoginApiDataService : BaseHttpService, ILoginApiDataService
    {
        readonly Uri baseUri = new Uri(Constants.BaseURL);

        public LoginApiDataService(IAnalyticsService analyticsService) 
        {
        }

        public async Task<bool> AuthenticateUserAsync(string username, string password)
        {
            try
            {
                var uri = new Uri(baseUri, "oauth2/token");

                var postBody = new Dictionary<string, string>()
                {
                    { "grant_type", "password" },
                    { "scope", "mobile" },
                    { "client_id", "android" },
                    { "idnumber", username  },
                    { "password", password },

                };

                var content = new FormUrlEncodedContent(postBody);

                var response = await Client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var jsonData = (JObject)JsonConvert.DeserializeObject(result);

                    var token = jsonData["access_token"].Value<string>();
                    WebApiHostAccountDetailsStore.Instance.WebApiHostToken = token;

                    return true;
                }

            }
            catch (Exception ex)
            {
                AnalyticsService.TrackError(ex, new Dictionary<string, string>
                {
                    { "Method", "LoginApiDataService.AuthenticateUserAsync()" }
                });

            }

            return false;
        }
    }
}
