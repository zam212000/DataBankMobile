using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyBodyTemperature.Services.RemoteService
{
    public interface IRemoteDataService
    {
        Task<bool> AuthenticateUserAsync();
        Task<string> SendSmsAsync(string message, string cellNumber);
    }
}
