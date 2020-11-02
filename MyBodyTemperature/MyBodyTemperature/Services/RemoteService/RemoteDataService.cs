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
using System.Net.Http.Headers;
using System.Net;
using SkiaSharp;

namespace MyBodyTemperature.Services.RemoteService
{
    public class RemoteDataService : BaseHttpService, IRemoteDataService
    {
        readonly Uri baseUri = new Uri(Constants.BaseURL);
        readonly Uri baseSmsUri = new Uri(Constants.BaseSmsURL);

        public async Task<bool> AuthenticateUserAsync()
        {
            try
            {
                var uri = new Uri(baseSmsUri, "v1/Authentication");

                var apiKey = "e00612f2-f932-49fd-acaf-6e31d26ad980";
                var apiSecret = "B8hYjvxlZ6yLBGgJAjVgA+NGrj+hHKga";
                var accountApiCredentials = $"{apiKey}:{apiSecret}";

                // Our endpoint expects this string to be Base64 encoded, to that end:
                var plainTextBytes = Encoding.UTF8.GetBytes(accountApiCredentials);
                var base64Credentials = Convert.ToBase64String(plainTextBytes);

                Client.DefaultRequestHeaders.Clear();

                Client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Client.DefaultRequestHeaders.Add("Authorization", $"Basic {base64Credentials}");
                //Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);

                HttpResponseMessage response = await Client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var jsonData = (JObject)JsonConvert.DeserializeObject(result);

                    var token = jsonData["token"].Value<string>();
                    AccountDetailsStore.Instance.Token = token;

                    return true;
                }

            }
            catch (Exception ex)
            {
                AnalyticsService.TrackError(ex, new Dictionary<string, string>
                {
                    { "Method", "RemoteDataService.AuthenticateUserAsync()" }
                });

            }

            return false;
        }


        public async Task<string> SendSmsAsync(string message, string cellNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(AccountDetailsStore.Instance.Token))
                {
                   var authenticatedToken = await AuthenticateUserAsync();
                    if(!authenticatedToken)
                    {
                        return string.Empty;
                    }
                }

                Uri uri = new Uri(baseSmsUri, "/v1/bulkmessages");

                Client.DefaultRequestHeaders.Clear();
                Client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", AccountDetailsStore.Instance.Token));

                //TODO
                Random generator = new Random();
                var generateTokenNumbers = generator.Next(0, 999999).ToString("D6");
                var msgs = new
                {
                    Messages = new[]
                  {
                       new
                           {
                             content = $"Ngena Access App: use OTP {generateTokenNumbers} to complete your company registration",
                             destination = cellNumber
                           }
                }
                };

                var content = new StringContent(JsonConvert.SerializeObject(msgs), Encoding.UTF8, "application/json");
                var response = await Client.PostAsync(uri, content);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return generateTokenNumbers;
                }

            }
            catch (Exception ex)
            {
                AnalyticsService.TrackError(ex, new Dictionary<string, string>
                {
                    { "Method", "RemoteDataService.SendSmsAsync()" }
                });

            }

            return string.Empty;
        }

        public async Task<bool> SendAnySmsAsync(string message, string cellNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(AccountDetailsStore.Instance.Token))
                {
                    var authenticatedToken = await AuthenticateUserAsync();
                    if (!authenticatedToken)
                    {
                        return false;
                    }
                }

                Uri uri = new Uri(baseSmsUri, "/v1/bulkmessages");

                Client.DefaultRequestHeaders.Clear();
                Client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", AccountDetailsStore.Instance.Token));

                var msgs = new
                {
                    Messages = new[]
                  {
                       new
                           {
                             content = message,
                             destination = cellNumber
                           }
                }
                };

                var content = new StringContent(JsonConvert.SerializeObject(msgs), Encoding.UTF8, "application/json");
                var response = await Client.PostAsync(uri, content);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                AnalyticsService.TrackError(ex, new Dictionary<string, string>
                {
                    { "Method", "RemoteDataService.SendSmsAsync()" }
                });

            }

            return false;
        }

    }
}
