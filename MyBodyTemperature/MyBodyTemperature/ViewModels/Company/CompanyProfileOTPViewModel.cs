using Acr.UserDialogs;
using MyBodyTemperature.Models;
using MyBodyTemperature.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
namespace MyBodyTemperature.ViewModels.Company
{
    public class CompanyProfileOTPViewModel : BaseViewModel
    {
        private readonly IDbService _dbService;
        private readonly IPageDialogService _pageDialogService;
        public CompanyProfileOTPViewModel(INavigationService navigationService, IDbService dbService, IPageDialogService dialogService) : base(navigationService)
        {
            _dbService = dbService;
            _pageDialogService = dialogService;
            NextProfileCommand = new DelegateCommand(OnNextProfileCommandExecuted, () => false);
            CancelCommand = new DelegateCommand(OnCancelCommandExecuted);
        }

        public DelegateCommand NextProfileCommand { get; }
        public DelegateCommand CancelCommand { get; }

        private async void OnCancelCommandExecuted()
        {
            await NavigationService.GoBackAsync();
        }

        private async void OnNextProfileCommandExecuted()
        {
            try
            {
                await NavigationService.NavigateAsync("CompanyProfilePasswordPage");
            }

            catch
            {

            }
        }
    }
}