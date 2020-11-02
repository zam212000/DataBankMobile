using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;


namespace MyBodyTemperature.Services
{
    public class ValidationService : IValidationService
    {

        const string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                  @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";


        public async Task<bool> NetworkConnectedAsync()
        {
            return await Task.FromResult(Connectivity.NetworkAccess == NetworkAccess.Internet);
        }

        public async Task<bool> NetworkReachableAsync(string host)
        {
            return await Task.FromResult(false);
            //return await CrossConnectivity.Current.IsRemoteReachable(host);
        }

        public async Task<bool> EmailValidAsync(string emailAddress)
        {
            return await Task.FromResult(Regex.IsMatch(emailAddress?.Trim(), emailRegex, RegexOptions.IgnoreCase));
        }

        public async Task<bool> SouthAfricanIDValidAsync(string idNumber)
        {
            Int64 numberIdNumber;
            bool result = false;
            if (Int64.TryParse(idNumber, out numberIdNumber) && idNumber.Length == 13 && ValidateDateFromIDNumber(idNumber))
            {

                var digits = new int[13];
                for (int i = 0; i < 13; i++)
                {
                    digits[i] = int.Parse(idNumber.Substring(i, 1));
                }
                int control1 = digits.Where((v, i) => i % 2 == 0 && i < 12).Sum();
                string second = string.Empty;
                digits.Where((v, i) => i % 2 != 0 && i < 12).ToList().ForEach(v =>
                      second += v.ToString());
                var string2 = (int.Parse(second) * 2).ToString();
                int control2 = 0;
                for (int i = 0; i < string2.Length; i++)
                {
                    control2 += int.Parse(string2.Substring(i, 1));
                }
                var control = (10 - ((control1 + control2) % 10)) % 10;
                if (digits[12] == control)
                {
                    result = true;
                }
            }

            return await Task.FromResult(result);
        }

        private bool ValidateDateFromIDNumber(string idNumber)
        {
            bool validDate = false;

            DateTime dt;
            if (DateTime.TryParseExact(idNumber.Substring(0, 6)
                                        , "yyMMdd"
                                        , CultureInfo.InvariantCulture
                                        , DateTimeStyles.None
                                        , out dt))
            {
                validDate = true;
            }
            else
            {
                validDate = false;
            }

            return validDate;
        }

        public async Task<bool> PasswordValidAsync(string password)
        {
            return await Task.FromResult(password.Length >= 8 &&
                        password.Any(char.IsDigit) &&
                        password.Any(char.IsLetter) &&
                        password.Any(char.IsUpper) &&
                        password.Any(char.IsPunctuation) &&
                        password.Any(char.IsLower));
        }

    }
}
