using System;
using System.Collections.Generic;
using System.Text;

namespace MyBodyTemperature.Models
{
    public class UserTemperature
    {
        public int UserId { get; set; }
        public double Temperature { get; set; }
        public DateTime TemperatureDate { get; set; }
    }
}
