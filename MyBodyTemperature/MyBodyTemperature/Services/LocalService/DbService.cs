using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MyBodyTemperature.Models;


namespace MyBodyTemperature.Services
{
    public class DbService : IDbService
    {
        bool initialized;
        readonly SQLiteAsyncConnection database;

        public DbService()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CovidAccessDB.db3");
            database = new SQLiteAsyncConnection(dbPath);
            Initialize();
        }

        private void Initialize()
        {
            try
            {
                if (!initialized)
                {
                    database.CreateTableAsync<Models.UserProfile>().Wait();
                    database.CreateTableAsync<UserTemperature>().Wait();
                    database.CreateTableAsync<Company>().Wait();
                    initialized = true;
                }
            }
            catch (Exception)
            {
                initialized = false;
            }
        }

        public Task<List<Models.UserProfile>> GetItemsAsync()
        {
            return database.Table<Models.UserProfile>().ToListAsync();
        }

        public Task<Models.UserProfile> GetItemAsync(int id)
        {
            return database.Table<Models.UserProfile>().Where(i => i.UserId == id).FirstOrDefaultAsync();
        }

        public async Task<int> InsertItemAsync(Models.UserProfile item)
        {
            var result = await database.InsertAsync(item);
            return result;

        }

        public Task<int> UpdateItemAsync(Models.UserProfile item)
        {
            return database.UpdateAsync(item);
        }

        public Task<int> DeleteItemAsync(Models.UserProfile item)
        {
            return database.DeleteAsync(item);
        }

        public Task<int> InsertUserTemperatureAsync(UserTemperature item)
        {
            return database.InsertAsync(item);
        }

        public Task<int> UpdateUserTemperatureAsync(UserTemperature item)
        {
            return database.UpdateAsync(item);
        }

        public Task<int> DeleteUserTemperatureAsync(UserTemperature item)
        {
            return database.DeleteAsync(item);
        }

        public Task<List<UserTemperature>> GetUserTemperatureItemsAsync(int userId)
        {
            return database.Table<UserTemperature>().Where(i => i.UserId == userId).ToListAsync();
        }

        public async Task<int> AddNewCompanyAsync(Company item)
        {
            return await database.InsertAsync(item);
        }
        public async Task<int> UpdateCompanyAsync(Company item)
        {
            return await database.UpdateAsync(item);
        }
        public async Task<int> DeleteCompanyAsync(Company item)
        {
            return await database.DeleteAsync(item);
        }
    }
}
