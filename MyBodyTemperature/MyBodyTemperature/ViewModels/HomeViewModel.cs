using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBodyTemperature.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";

        }
    
    }
}
