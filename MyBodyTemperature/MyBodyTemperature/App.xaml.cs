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
using MyBodyTemperature.ViewModels.Company;
using MyBodyTemperature.Views.Company;
using MyBodyTemperature.Services.RemoteService;

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
            Device.SetFlags(new string[] { "Expander_Experimental" });
            await NavigationService.NavigateAsync("NavigationPage/LogInPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LogInPage, LogInViewModel>();
            containerRegistry.RegisterForNavigation<CreateProfilePage, CreateProfileViewModel>();
            containerRegistry.RegisterForNavigation<CreateProfilePasswordPage, CreateProfilePasswordViewModel>();

            containerRegistry.RegisterForNavigation<MainTabbedPage, TabbedViewModel>();
            containerRegistry.RegisterForNavigation<MessagesPage, MessagesViewModel>();
            containerRegistry.RegisterForNavigation<VisitorsPage, VisitorsViewModel>();
            containerRegistry.RegisterForNavigation<EmployeesPage, EmployeesViewModel>();
            containerRegistry.RegisterForNavigation<EmployeeDetailPage, EmployeeDetailViewModel>();
            containerRegistry.RegisterForNavigation<UserTemperaturePage, UserTemperatureViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<UpdateUserProfilePage, UpdateUserProfileViewModel>();

            containerRegistry.RegisterForNavigation<SettingsPage, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<UpdateUserProfilePage, UpdateUserProfileViewModel>();

            containerRegistry.RegisterForNavigation<CompanyProfileEditPage, CompanyProfileViewModel>();
            containerRegistry.RegisterForNavigation<CompanyProfilePage, CompanyProfileViewModel>();
            containerRegistry.RegisterForNavigation<CompanyProfileOTPPage, CompanyProfileOTPViewModel>();
            containerRegistry.RegisterForNavigation<CompanyProfilePasswordPage, CompanyProfilePasswordViewModel>();

            //SERVICES
            containerRegistry.Register<IAnalyticsService, AppCenterAnalyticsService>();
            containerRegistry.Register<ILoginApiDataService, LoginApiDataService>();
            containerRegistry.Register<IDbService, DbService>();
            containerRegistry.Register<IRemoteDataService, RemoteDataService>();
            containerRegistry.Register<IValidationService, ValidationService>();

            


        }

    }
}
