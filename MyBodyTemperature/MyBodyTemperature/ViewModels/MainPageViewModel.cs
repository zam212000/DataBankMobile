using MyBodyTemperature.Helpers;
using MyBodyTemperature.Models;
using MyBodyTemperature.ViewModels;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBodyTemperature.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        const string skype = "Skype";

        ObservableCollection<MenuItem> menuItems;

        public MainPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            MenuItems = new ObservableCollection<MenuItem>();

            InitMenuItems();
        }

        public string UserName => Settings.User?.UserName;

        public string UserAvatar => Settings.User?.AvatarUrl;

        public ObservableCollection<MenuItem> MenuItems { get; set; }

        public DelegateCommand<MenuItem> MenuItemSelectedCommand => new DelegateCommand<MenuItem>(OnSelectMenuItem);


        void InitMenuItems()
        {
            MenuItems.Add(new MenuItem
            {
                Title = "Home",
                MenuItemType = MenuItemType.Home,
                //PageName = nameof(HomePage),
                IsEnabled = true
            });

            MenuItems.Add(new MenuItem
            {
                Title = "Our Visitors",
                MenuItemType = MenuItemType.OurVisitors,
                //PageName = nameof(BookingPage),
                IsEnabled = true
            });

            MenuItems.Add(new MenuItem
            {
                Title = "Our Employees",
                MenuItemType = MenuItemType.OurEmployees,
                IsEnabled = false
                //PageName = nameof(MyBookingsPage),
                // IsEnabled = AppSettings.HasBooking
            });

            MenuItems.Add(new MenuItem
            {
                Title = "Suggestions",
                MenuItemType = MenuItemType.Suggestions,
                //PageName = nameof(SuggestionsPage),
                //  ViewModelType = typeof(SuggestionsViewModel),
                IsEnabled = true
            });

            MenuItems.Add(new MenuItem
            {
                Title = "Logout",
                MenuItemType = MenuItemType.Logout,
                // PageName = nameof(LogInPage),
                IsEnabled = true,
                AfterNavigationAction = RemoveUserCredentials
            });
        }

        private async void OnSelectMenuItem(MenuItem item)
        {
            //if (item.IsEnabled && item.ViewModelType != null)
            //{
            //    item.AfterNavigationAction?.Invoke();
            //    await NavigationService.NavigateToAsync(item.ViewModelType, item);
            //}
        }


        async void RemoveUserCredentials()
        {
            Settings.RemoveUserData();
            await NavigationService.NavigateAsync("/LoginPage");
            // return authenticationService.LogoutAsync();
        }

        void OnBookingRequested()
        {

        }

        void OnCheckoutRequested(object args) => SetMenuItemStatus(MenuItemType.OurEmployees, false);

        void SetMenuItemStatus(MenuItemType type, bool enabled)
        {
            var menuItem = MenuItems.FirstOrDefault(m => m.MenuItemType == type);

            if (menuItem != null)
            {
                menuItem.IsEnabled = enabled;
            }
        }

        async Task OpenBotAsync()
        {
            await Task.Delay(100);

            var bots = new[] { skype };

            try
            {

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"OpenBot: {ex}");
            }
        }
    }
}
