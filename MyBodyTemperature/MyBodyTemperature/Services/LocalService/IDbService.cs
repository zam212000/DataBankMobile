using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MyBodyTemperature.Models;

namespace MyBodyTemperature.Services
{
    public interface IDbService
    {
        Task<List<Models.UserProfile>> GetItemsAsync(int companyID);
        Task<Models.UserProfile> GetItemAsync(int id);
        Task<int> InsertItemAsync(Models.UserProfile item);
        Task<int> UpdateItemAsync(Models.UserProfile item);
        Task<int> DeleteItemAsync(Models.UserProfile item);

        Task<int> InsertUserTemperatureAsync(UserTemperature item);
        Task<int> UpdateUserTemperatureAsync(UserTemperature item);
        Task<int> DeleteUserTemperatureAsync(UserTemperature item);
        Task<List<UserTemperature>> GetUserTemperatureItemsAsync(int userId);

        Task<Company> GetCompanyByID(int id);
        Task<Company> GetCompanyByName(string name);
        Task<Company> GetCompanyByUsername(string username);
        Task<int> AddNewCompanyAsync(Company item);
        Task<int> UpdateCompanyAsync(Company item);
        Task<int> DeleteCompanyAsync(Company item);

        Task<bool> CompanyUserNameExists(string username);

        Task<List<Event>> GetEventsAsync(int companyID);
        Task<int> AddNewEventAsync(Event item);
        Task<int> UpdateEventAsync(Event item);
        Task<int> DeleteEventAsync(Event item);
    }
}
