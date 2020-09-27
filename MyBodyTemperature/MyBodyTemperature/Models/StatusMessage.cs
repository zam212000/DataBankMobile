using System;
using System.Collections.Generic;
using System.Text;

namespace MyBodyTemperature.Models
{
    public class StatusMessage
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class APIResultResponse
    {
        public string Message { get; set; }

        public Dictionary<string, string[]> ModelState { get; set; }
    }
}
