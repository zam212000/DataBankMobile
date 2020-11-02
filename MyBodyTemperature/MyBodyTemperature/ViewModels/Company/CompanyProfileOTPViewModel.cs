using Acr.UserDialogs;
using MyBodyTemperature.Models;
using MyBodyTemperature.Services;
using MyBodyTemperature.Services.RemoteService;
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
        private readonly IRemoteDataService _remoteDataService;
        public CompanyProfileOTPViewModel(INavigationService navigationService, IDbService dbService, IPageDialogService dialogService, IRemoteDataService remoteDataService) : base(navigationService)
        {
            _dbService = dbService;
            _pageDialogService = dialogService;
            _remoteDataService = remoteDataService;
            NextProfileCommand = new DelegateCommand(OnNextProfileCommandExecuted, () => false);
            CancelCommand = new DelegateCommand(OnCancelCommandExecuted);
            ResendTokenCommand = new DelegateCommand(OnResendTokenCommandExecuted);
        }

        public DelegateCommand NextProfileCommand { get; }
        public DelegateCommand ResendTokenCommand { get; }
        public DelegateCommand CancelCommand { get; }

        private async void OnCancelCommandExecuted()
        {
            await NavigationService.GoBackAsync();
        }

        private string _oTPNumber = string.Empty;
        public string OTPNumber
        {
            get => _oTPNumber;
            set
            {
                SetProperty(ref _oTPNumber, value);
            }
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


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters != null && parameters.ContainsKey("CompanyProfile"))
            {
                CompanyProfile = parameters["CompanyProfile"] as Models.Company;

            }
        }

        private async void OnResendTokenCommandExecuted()
        {
            try
            {
                var smsSend = await _remoteDataService.SendSmsAsync("", CompanyProfile.PhoneNumber);
                if (string.IsNullOrEmpty(smsSend))
                {
                    //TODO - this might be security breach...
                    await _pageDialogService.DisplayAlertAsync("Unsuccessful", "Failed to send OTP, please ensure phone number provided is correct", "Ok");
                    return;
                }

                CompanyProfile.Token = smsSend;
                await _dbService.UpdateCompanyAsync(CompanyProfile);
            }

            catch
            {

            }
        }

        private async void OnNextProfileCommandExecuted()
        {
            try
            {
                if (OTPNumber.Equals(CompanyProfile.Token))
                {
                    var param = new NavigationParameters();
                    param.Add("CompanyProfile", CompanyProfile);

                    await NavigationService.NavigateAsync("CompanyProfilePasswordPage", param);
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync("Incorrect token", "Incorrect token provided.", "Ok");
                }
            }

            catch
            {

            }
        }
    }
}