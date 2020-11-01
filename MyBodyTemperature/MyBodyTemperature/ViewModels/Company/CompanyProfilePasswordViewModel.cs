using Acr.UserDialogs;
using MyBodyTemperature.Models;
using MyBodyTemperature.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using Xamarin.Forms;

namespace MyBodyTemperature.ViewModels.Company
{
    public class CompanyProfilePasswordViewModel: BaseViewModel
    {
        private readonly IDbService _dbService;
        private readonly IPageDialogService _pageDialogService;
        public CompanyProfilePasswordViewModel(INavigationService navigationService, IDbService dbService, IPageDialogService dialogService) : base(navigationService)
        {
            _dbService = dbService;
            _pageDialogService = dialogService;
            NextProfileCommand = new DelegateCommand(OnNextProfileCommandExecuted, () => false);
            CancelCommand = new DelegateCommand(OnCancelCommandExecuted);
        }

        public DelegateCommand NextProfileCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand TakePhotoCommand { get; }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
            }
        }

        private string _confirmPassword = string.Empty;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                SetProperty(ref _confirmPassword, value);
            }
        }

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
