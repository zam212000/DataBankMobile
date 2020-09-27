using System;
using System.Collections.Generic;
using System.Text;

namespace MyBodyTemperature.Helpers
{
    public sealed class AccountDetailsStore
    {
        private static readonly AccountDetailsStore instance = new AccountDetailsStore();

        private AccountDetailsStore() { }

        public static AccountDetailsStore Instance
        {
            get
            {
                return instance;
            }
        }

        public string Token { get; set; }
    }

    public sealed class WebApiHostAccountDetailsStore
    {
        private static readonly WebApiHostAccountDetailsStore instance = new WebApiHostAccountDetailsStore();

        private WebApiHostAccountDetailsStore() { }

        public static WebApiHostAccountDetailsStore Instance
        {
            get
            {
                return instance;
            }
        }

        public string WebApiHostToken { get; set; }
    }
}
