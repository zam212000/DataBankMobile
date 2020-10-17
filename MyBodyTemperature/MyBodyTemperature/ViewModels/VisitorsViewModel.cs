using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBodyTemperature.ViewModels
{
    public class VisitorsViewModel : BaseViewModel
    {
        private readonly IPageDialogService _pageDialogService;
        public VisitorsViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {

        }

    }
}
