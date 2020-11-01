using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyBodyTemperature.Models
{
    public class Company
    {
        [PrimaryKey]
        [AutoIncrement]
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddresss { get; set; }
        public string CompanyEmail { get; set; }
        public string AvatarUrl { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public bool IsRegistered { get; set; }
        public int VerificationMethod { get; set; } = 1;

        public byte[] ImageContent { get; set; }
        [Ignore]
        public ImageSource ImageProperty { get; set; }

    }
}
