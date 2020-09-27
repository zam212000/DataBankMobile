using MyBodyTemperature.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyBodyTemperature.Converters
{
    public class MenuItemTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var menuItemType = (MenuItemType)value;

            var platform = Device.RuntimePlatform == Device.UWP;

            switch (menuItemType)
            {
                case MenuItemType.Home:
                    return platform ? "Assets/ic_home.png" : "ic_home.png";
                case MenuItemType.OurEmployees:
                    return platform ? "Assets/illustratrion_bot.png" : "illustratrion_bot.png";
                case MenuItemType.OurVisitors:
                    return platform ? "Assets/ic_fitness_centre.png" : "ic_fitness_centre.png";
                case MenuItemType.Suggestions:
                    return platform ? "Assets/ic_beach.png" : "ic_beach.png";
                case MenuItemType.Logout:
                    return platform ? "Assets/ic_logout.png" : "ic_logout.png";
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}