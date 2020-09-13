using MyBodyTemperature.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace MyBodyTemperature.Forms
{
    public class HomePage : ContentPage
    {
        ZXingScannerPage scanPage;
        Button buttonScanDefaultOverlay;
        private Stopwatch StopWatch = new Stopwatch();

        public HomePage() : base()
        {
            //buttonScanDefaultOverlay = new Button
            //{
            //    Text = "Scan with Default Overlay",
            //    AutomationId = "scanWithDefaultOverlay",
            //};
            //buttonScanDefaultOverlay.Clicked += async delegate
            //{
            //LoadContentInfo();
            //};


            var stack = new StackLayout();
            //stack.Children.Add(buttonScanDefaultOverlay);

            Content = stack;
        }

        private async void LoadContentInfo()
        {
            scanPage = new ZXingScannerPage();
            await Navigation.PushAsync(scanPage);

            Task.Delay(new TimeSpan(0, 0, 10)).Wait();

            await Navigation.PopAsync();

            var customContinuousScanPage = new ResultOverviewPage();
            await Navigation.PushAsync(customContinuousScanPage);

            //Device.StartTimer(new TimeSpan(0, 0, 10), ()  =>
            //{

            //    if (StopWatch.IsRunning)
            //    {
            //        Device.BeginInvokeOnMainThread(async () =>
            //        {
            //            await Navigation.PopAsync();
            //            // await DisplayAlert("Scanned Barcode", "My result", "OK");
            //            var customContinuousScanPage = new ResultOverviewPage();
            //            await Navigation.PushAsync(customContinuousScanPage);
            //        });
            //        StopWatch.Stop();
            //    }

            //    return true;
            //});

            //await Navigation.PushAsync(scanPage);
        }

        public async Task DummyCamera()
        {
            scanPage = new ZXingScannerPage();
            scanPage.AutoFocus();

            Device.StartTimer(new TimeSpan(0, 0, 10), () =>
                  {
                      if (StopWatch.IsRunning)
                      {
                          scanPage.IsScanning = false;

                          Device.BeginInvokeOnMainThread(async () =>
                          {
                              await Navigation.PopAsync();
                              // return await Task.FromResult(true);
                              var customContinuousScanPage = new ResultOverviewPage();
                              await Navigation.PushAsync(customContinuousScanPage);

                          });

                          StopWatch.Stop();
                      }

                      return true;
                  });

            await Navigation.PushAsync(scanPage);

            //return await Task.FromResult(true);
        }
    }
}
