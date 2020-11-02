using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MyBodyTemperature.Helpers;
using MyBodyTemperature.Models;
using MyBodyTemperature.Services.AnalyticsService;
using Newtonsoft.Json;

namespace MyBodyTemperature.Services
{
    public class BaseHttpService
    {
        protected IAnalyticsService AnalyticsService { get; private set; }
        protected HttpClient Client { get; private set; }
        protected string Token { get; }

        public BaseHttpService()
        {
            //Token = AccountDetailsStore.Instance.Token;
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            Client = new HttpClient(handler);

           // Client = new HttpClient();


           // Client.MaxResponseContentBufferSize = 256000;
        }

        protected async Task<StatusMessage> GetResponseResultAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var contentResult = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<StatusMessage>(contentResult);

                return result ?? new StatusMessage { Success = true };
            }

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var contentResult = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<APIResultResponse>(contentResult);

                var modelStates = result?.ModelState?.Values?.FirstOrDefault();
                return new StatusMessage { Success = false, Message = modelStates == null ? result?.Message :  string.Join(",", modelStates?.Select(x => x.ToString())) };
            }

            return new StatusMessage { Success = false };
        }

    }
}
