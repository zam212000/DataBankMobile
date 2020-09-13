using Prism;
using Prism.Ioc;
using MyBodyTemperature.ViewModels;
using MyBodyTemperature.Views;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Xamarin.Forms;
using MyBodyTemperature.Forms;
using ZXing.Net.Mobile.Forms;
using System.Collections.Generic;
using System;
using BarcodeScanner;
using Rg.Plugins.Popup.Services;
using Rg.Plugins.Popup.Contracts;

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
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.RegisterForNavigation<HomePage>();
			containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
			containerRegistry.RegisterForNavigation<ResultOverviewPage, ResultOverviewViewModel>();

			containerRegistry.Register<IBarcodeScannerService, ContentPageBarcodeScannerService>();


		}

		public void UITestBackdoorScan(string param)
		{
			var expectedFormat = ZXing.BarcodeFormat.QR_CODE;
			Enum.TryParse(param, out expectedFormat);
			var opts = new ZXing.Mobile.MobileBarcodeScanningOptions
			{
				PossibleFormats = new List<ZXing.BarcodeFormat> { expectedFormat }
			};

			System.Diagnostics.Debug.WriteLine("Scanning " + expectedFormat);

			var scanPage = new ZXingScannerPage(opts);
			scanPage.OnScanResult += (result) =>
			{
				scanPage.IsScanning = false;

				Device.BeginInvokeOnMainThread(() =>
				{
					var format = result?.BarcodeFormat.ToString() ?? string.Empty;
					var value = result?.Text ?? string.Empty;

					MainPage.Navigation.PopAsync();
					MainPage.DisplayAlert("Barcode Result", format + "|" + value, "OK");
				});
			};

			MainPage.Navigation.PushAsync(scanPage);
		}
	}
}
