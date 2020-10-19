using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBodyTemperature.ViewModels
{
    public class TabbedViewModel : BaseViewModel
    {
        private readonly IPageDialogService _pageDialogService;

        public DelegateCommand ItemAddedCommand { get; set; }
        public TabbedViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            _pageDialogService = dialogService;
            ItemAddedCommand = new DelegateCommand(AddNewItem);
        }

        private async void AddNewItem()
        {
            await NavigationService.NavigateAsync("CreateProfilePage");
        }

    }
}
