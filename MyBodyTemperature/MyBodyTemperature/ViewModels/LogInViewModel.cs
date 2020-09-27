using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBodyTemperature.ViewModels
{
    public class LogInViewModel : BaseViewModel
    {
        public LogInViewModel(INavigationService navigationService)
          : base(navigationService)
        {
            Title = "Log In";
        }

        private string _randomTemp;
        public string RandomTemp
        {
            get => _randomTemp;
            set
            {
                SetProperty(ref _randomTemp, value);
            }
        }


    }
}
