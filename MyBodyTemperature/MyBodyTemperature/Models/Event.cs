using System;
using System.Collections.Generic;
using System.Text;

namespace MyBodyTemperature.Models
{
    public class Event
    {
        public int EventID { get; set; }
        public int CompanyID { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Active { get; set; } = true;

    }
}
