using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MyBodyTemperature.Models;
using Plugin.Settings;
using Plugin.Settings.Abstractions;


namespace MyBodyTemperature.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        private const string ApiAuthTokenKey = "apitoken_key";
        private static readonly string AuthTokenDefault = string.Empty;
        private const string IdLatitude = "latitude";
        private const string IdLongitude = "longitude";
        private static readonly double FakeLatitudeDefault = 0;
        private static readonly double FakeLongitudeDefault = 0;

        public static double Latitude
        {
            get => AppSettings.GetValueOrDefault(IdLatitude, FakeLatitudeDefault);
            set => AppSettings.AddOrUpdateValue(IdLatitude, value);
        }

        public static double Longitude
        {
            get => AppSettings.GetValueOrDefault(IdLongitude, FakeLongitudeDefault);
            set => AppSettings.AddOrUpdateValue(IdLongitude, value);
        }

        public static string IECWebApiAuthToken
        {
            get => AppSettings.GetValueOrDefault(ApiAuthTokenKey, AuthTokenDefault);
            set => AppSettings.AddOrUpdateValue(ApiAuthTokenKey, value);
        }

        public static UserProfile User
        {
            get => PreferencesHelpers.Get(nameof(User), default(UserProfile));
            set => PreferencesHelpers.Set(nameof(User), value);
        }

        public static void RemoveUserData() => User = null;
    }
}
    