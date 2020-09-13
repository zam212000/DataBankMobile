using BarcodeScanner;
using MyBodyTemperature.Forms;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Navigation.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace MyBodyTemperature.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private IBarcodeScannerService _barcodeScanner { get; }

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            ReadTemperatureCommand = new DelegateCommand(OnReadTemperatureCommandExecuted);
            Title = "Body Temperature";
        }

        public DelegateCommand ReadTemperatureCommand { get; }

        private Stopwatch StopWatchTemp = new Stopwatch();
        TimeSpan SessionDuration = TimeSpan.FromSeconds(10);


        private async void OnReadTemperatureCommandExecuted()
        {
            try
            {

                await new ContentPageBarcodeScannerService().ReadBarcodeResultAsync();
                await NavigationService.NavigateAsync("ResultOverviewPage");
                /*
                StopWatchTemp.Reset();
                Device.StartTimer(new TimeSpan(0, 0, 1), () =>
                {
                    Task.Run(async () =>
                    {
                       
                        if (!StopWatchTemp.IsRunning && StopWatchTemp.Elapsed.Seconds == 0)
                        {
                            StopWatchTemp.Restart();
                            await new ContentPageBarcodeScannerService().ReadBarcodeResultAsync();
                        }

                        else if (StopWatchTemp.IsRunning && StopWatchTemp.Elapsed.Seconds >= SessionDuration.Seconds)
                        {
                            StopWatchTemp.Stop();
                            await NavigationService.NavigateAsync("ResultOverviewPage");
                        }

                    });

                    return true;
                });
                */
            }
            catch (Exception e)
            {

            }
        }
    }
}
