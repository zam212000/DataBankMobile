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

        public Task<List<Models.UserProfile>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<Models.UserProfile>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
            // SQLite does not have a separate Boolean storage class. 
            // Instead, Boolean values are stored as integers 0 (false) and 1 (true).
        }

        public Task<List<Models.UserProfile>> GetItemsDoneAsync()
        {
            // SQL
            return database.QueryAsync<Models.UserProfile>("SELECT * FROM [UserProfile] WHERE [Done] = 1");
        }

        public Task<Models.UserProfile> GetItemAsync(int id)
        {
            // not used?
            return database.Table<Models.UserProfile>().Where(i => i.UserId == id).FirstOrDefaultAsync();
        }

        public Task<int> InsertItemAsync(Models.UserProfile item)
        {
            return database.InsertAsync(item);
        }

        public Task<int> UpdateItemAsync(Models.UserProfile item)
        {
            return database.UpdateAsync(item);
        }

        public Task<int> DeleteItemAsync(Models.UserProfile item)
        {
            return database.DeleteAsync(item);
        }
    }
}
