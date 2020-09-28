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
        Task<List<Models.UserProfile>> GetItemsNotDoneAsync();
        Task<List<Models.UserProfile>> GetItemsDoneAsync();
        Task<Models.UserProfile> GetItemAsync(int id);
        Task<int> InsertItemAsync(Models.UserProfile item);
        Task<int> UpdateItemAsync(Models.UserProfile item);
        Task<int> DeleteItemAsync(Models.UserProfile item);
    }
}
