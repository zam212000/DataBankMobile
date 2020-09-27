
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

namespace MyBodyTemperature.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Body Temperature";
        }

        public DelegateCommand ReadTemperatureCommand { get; }

    }
}
