﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyBodyTemperature.Models
{
    public class UserProfile
    {
        public UserProfile()
        {
            new CovidMetadata
            {
                Temperature = "37c",
                TemperatureDate = "2020/03/20"
            };
        }
        [PrimaryKey]
        [AutoIncrement]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstNames { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }

        public string FullName { get; set; } 
        public string AvatarUrl { get; set; }
        [Ignore]
        public ImageSource ImageProperty { get; set; }
        public byte[] ImageContent { get; set; }
        public bool PhoneNumberConfirmed { get; set; } 
        public string Token { get; set; }
        public string Password { get; set; }
        public bool IsRegistered { get; set; }
        public int VerificationMethod { get; set; } = 1;
        [Ignore]
        public CovidMetadata CovidMetadata { get; set; }
    }
}
