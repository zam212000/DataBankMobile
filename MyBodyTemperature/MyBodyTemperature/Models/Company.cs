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
        [Ignore]
        public ImageSource ImageProperty { get; set; }
    }
}
