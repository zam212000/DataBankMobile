using Prism;
using Prism.Ioc;
using MyBodyTemperature.ViewModels;
using MyBodyTemperature.Views;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Xamarin.Forms;
using System.Collections.Generic;
using System;
using Rg.Plugins.Popup.Services;
using Rg.Plugins.Popup.Contracts;
using MyBodyTemperature.Services.UserProfile;
using MyBodyTemperature.Services.AnalyticsService;
using MyBodyTemperature.Services;

namespace MyBodyTemperature
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/LogInPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LogInPage, LogInViewModel>();
            containerRegistry.RegisterForNavigation<CreateProfilePage, CreateProfileViewModel>();
            containerRegistry.RegisterForNavigation<CreateProfilePasswordPage, CreateProfilePasswordViewModel>();
            containerRegistry.RegisterForNavigation<PersonnelList, PersonnelListViewModel>();

            containerRegistry.RegisterForNavigation<MainTabbedPage, TabbedViewModel>();
            containerRegistry.RegisterForNavigation<MessagesPage, MessagesViewModel>();
            containerRegistry.RegisterForNavigation<VisitorsPage, VisitorsViewModel>();
            containerRegistry.RegisterForNavigation<EmployeesPage, EmployeesViewModel>();
            containerRegistry.RegisterForNavigation<EmployeeDetailPage, EmployeeDetailViewModel>();
            containerRegistry.RegisterForNavigation<UserTemperaturePage, UserTemperatureViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<UpdateUserProfilePage, UpdateUserProfileViewModel>();




            //containerRegistry.RegisterForNavigation<VisitorsPage, VisitorsViewModel>();
            //containerRegistry.RegisterForNavigation<VisitorsPage, VisitorsViewModel>();


            //SERVICES
            containerRegistry.Register<IAnalyticsService, AppCenterAnalyticsService>();
            containerRegistry.Register<ILoginApiDataService, LoginApiDataService>();
            containerRegistry.Register<IDbService, DbService>();

        }

    }
}
