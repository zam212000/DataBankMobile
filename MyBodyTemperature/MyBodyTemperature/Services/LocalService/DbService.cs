﻿using SQLite;
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
                    database.CreateTableAsync<Event>().Wait();
                    initialized = true;
                }
            }
            catch (Exception)
            {
                initialized = false;
            }
        }

        public Task<List<Models.UserProfile>> GetItemsAsync(int companyID)
        {
            return database.Table<Models.UserProfile>().Where(y => y.CompanyID == companyID).OrderByDescending(x => x.TemperatureDate).ToListAsync();
        }

        public async Task<Models.UserProfile> GetItemAsync(int id)
        {
            return await database.Table<Models.UserProfile>().Where(i => i.UserId == id).FirstOrDefaultAsync();
        }

        public Task<Models.UserProfile> GetItemByUniqueDescriptionAsync(string phoneNumber, string idNumber, string employeeNumber)
        {
            return database.Table<Models.UserProfile>().Where(i =>
                        i.PhoneNumber == phoneNumber ||
                        i.IDNumber == idNumber ||
                        i.EmployeeNumber == employeeNumber).FirstOrDefaultAsync();
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

        public async Task<List<UserTemperature>> GetUserTemperatureItemsAsync(int userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            if (endDate == null)
            {
                return await database.Table<UserTemperature>().Where(i =>
                   i.UserId == userId
                   ).OrderBy(x => x.TemperatureDate).Take(7).ToListAsync();
            }

            else
            {

                var result = await database.Table<UserTemperature>().Where(i =>
                       i.UserId == userId &&
                       i.TemperatureDate >= startDate &&
                       i.TemperatureDate <= endDate
                       ).OrderByDescending(x => x.Id).Take(7).ToListAsync();

                return result;
            }
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

        public async Task<Company> GetCompanyByID(int id)
        {
            return await database.Table<Company>().FirstOrDefaultAsync(i => i.CompanyID == id);
        }
        public async Task<Company> GetCompanyByName(string name)
        {
            return await database.Table<Company>().FirstOrDefaultAsync(i => i.CompanyName.ToLower() == name.ToLower());
        }

        public async Task<Company> GetCompanyByUsername(string username)
        {
            return await database.Table<Company>().FirstOrDefaultAsync(i => i.Username.ToLower() == username.ToLower());
        }

        public async Task<bool> CompanyUserNameExists(string username)
        {
            return (await database.Table<Company>().FirstOrDefaultAsync(i => i.Username.ToLower() == username.ToLower()) != null);
        }

        public async Task<List<Event>> GetEventsAsync(int companyID)
        {
            return await database.Table<Event>().Where(y => y.CompanyID == companyID).OrderByDescending(x => x.StartDate).ToListAsync();
        }
        public async Task<int> AddNewEventAsync(Event item)
        {
            return await database.InsertAsync(item);
        }
        public async Task<int> UpdateEventAsync(Event item)
        {
            return await database.UpdateAsync(item);
        }
        public async Task<int> DeleteEventAsync(Event item)
        {
            return await database.DeleteAsync(item);
        }


    }
}
