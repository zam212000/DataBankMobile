using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBodyTemperature.ViewModels
{
    public class ResultOverviewViewModel : BaseViewModel
    {
        public ResultOverviewViewModel(INavigationService navigationService)
          : base(navigationService)
        {
            Title = "Body Temperature Result";
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

        public  void RandomizedTemp()
        {
            Random random = new Random();
            var result = random.NextDouble() * (34 - 38) + 34;
            RandomTemp = $"Your result is { result.ToString()}";
        }
    }
}
