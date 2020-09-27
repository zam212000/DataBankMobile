using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyBodyTemperature.Services.UserProfile
{
    public interface ILoginApiDataService
    {
        Task<bool> AuthenticateUserAsync(string idNumber, string password);
    }
}
