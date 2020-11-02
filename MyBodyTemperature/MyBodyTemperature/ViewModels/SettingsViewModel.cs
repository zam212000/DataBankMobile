using MyBodyTemperature.Helpers;
using MyBodyTemperature.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBodyTemperature.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly IDbService _dbService;
        private readonly IPageDialogService _pageDialogService;
        public SettingsViewModel(INavigationService navigationService, IDbService dbService, IPageDialogService dialogService) : base(navigationService)
        {
            _dbService = dbService;
            _pageDialogService = dialogService;
            CancelCommand = new DelegateCommand(OnCancelCommandExecuted);
            CompanyProfile = Settings.CurrentCompany;
        }

        public DelegateCommand AddTemperatureCommand { get; }
        public DelegateCommand CancelCommand { get; }

        private async void OnCancelCommandExecuted()
        {
            await NavigationService.GoBackAsync();
        }

        private Models.Company _companyProfile;
        public Models.Company CompanyProfile
        {
            get => _companyProfile;
            set
            {
                SetProperty(ref _companyProfile, value);
            }
        }

    }
}
