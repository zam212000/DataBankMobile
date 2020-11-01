using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MyBodyTemperature.Models;

namespace MyBodyTemperature.Services
{
    public interface IDbService
    {
        Task<List<Models.UserProfile>> GetItemsAsync();
        Task<Models.UserProfile> GetItemAsync(int id);
        Task<int> InsertItemAsync(Models.UserProfile item);
        Task<int> UpdateItemAsync(Models.UserProfile item);
        Task<int> DeleteItemAsync(Models.UserProfile item);

        Task<int> InsertUserTemperatureAsync(UserTemperature item);
        Task<int> UpdateUserTemperatureAsync(UserTemperature item);
        Task<int> DeleteUserTemperatureAsync(UserTemperature item);
        Task<List<UserTemperature>> GetUserTemperatureItemsAsync(int userId);

        Task<int> AddNewCompanyAsync(Company item);
        Task<int> UpdateCompanyAsync(Company item);
        Task<int> DeleteCompanyAsync(Company item);
    }
}
