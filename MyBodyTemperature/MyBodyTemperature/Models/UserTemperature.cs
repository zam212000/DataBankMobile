using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBodyTemperature.Models
{
    public class UserTemperature
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Temperature { get; set; }
        public DateTime TemperatureDate { get; set; }
    }
}
