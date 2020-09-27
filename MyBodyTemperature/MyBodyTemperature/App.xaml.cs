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

            //SERVICES
            containerRegistry.Register<IAnalyticsService, AppCenterAnalyticsService>();
            containerRegistry.Register<ILoginApiDataService, LoginApiDataService>();

        }

    }
}
