using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyBodyTemperature.Services
{
    public interface IValidationService
    {
        Task<bool> EmailValidAsync(string emailAddress);

        Task<bool> PasswordValidAsync(string password);

        Task<bool> SouthAfricanIDValidAsync(string idNumber);

        Task<bool> NetworkReachableAsync(string host);

        Task<bool> NetworkConnectedAsync();
    }
}
