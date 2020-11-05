using Acr.UserDialogs;
using MyBodyTemperature.Models;
using MyBodyTemperature.Services;
using MyBodyTemperature.Services.EncryptionService;
using MyBodyTemperature.Services.RemoteService;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace MyBodyTemperature.ViewModels.Company
{
    public class CompanyProfilePasswordViewModel : BaseViewModel
    {
        private readonly IDbService _dbService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IRemoteDataService _remoteDataService;
        public CompanyProfilePasswordViewModel(INavigationService navigationService, IDbService dbService, IPageDialogService dialogService, IRemoteDataService remoteDataService) : base(navigationService)
        {
            _dbService = dbService;
            _pageDialogService = dialogService;
            _remoteDataService = remoteDataService;
            NextProfileCommand = new DelegateCommand(OnNextProfileCommandExecuted, () => false);
            CancelCommand = new DelegateCommand(OnCancelCommandExecuted);
        }

        public DelegateCommand NextProfileCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand TakePhotoCommand { get; }

        private string _username = string.Empty;
        public string Username
        {
            get => _username;
            set
            {
                SetProperty(ref _username, value);
            }
        }

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

        private Models.Company _companyProfile;
        public Models.Company CompanyProfile
        {
            get => _companyProfile;
            set
            {
                SetProperty(ref _companyProfile, value);
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
                IsBusy = true;
                if (Username.Length < 5)
                {
                    await _pageDialogService.DisplayAlertAsync("Create profile", "A minimum of 6 characters is required for username", "Ok");
                    return;
                }

                if (await _dbService.CompanyUserNameExists(Username))
                {
                    await _pageDialogService.DisplayAlertAsync("Create profile", "Username already taken, please choose a different username", "Ok");
                    return;
                }

                if (!Password.Equals(ConfirmPassword))
                {
                    await _pageDialogService.DisplayAlertAsync("Create profile", "The password is not matching", "Ok");
                    return;
                }

                if (Password.Length < 6)
                {
                    await _pageDialogService.DisplayAlertAsync("Create profile", "A minimum of 6 characters is required for password", "Ok");
                    return;
                }

                CompanyProfile.Password = AesEncryptionService.Encrypt(Password);
                var companyAccount = RandomDigits(10);
                CompanyProfile.CompanyRegNumber = companyAccount;
                CompanyProfile.Username = Username;
                CompanyProfile.PhoneNumberConfirmed = true;

                await _dbService.UpdateCompanyAsync(CompanyProfile);

                var msg = $"Ngena Access App: Your Company has been successfully created. Your Company account is {companyAccount}. Your Username is {Username} and Password is {ConfirmPassword}";
                //SEND MESSAGE
                await _remoteDataService.SendAnySmsAsync(msg, CompanyProfile.PhoneNumber);
                await _pageDialogService.DisplayAlertAsync("Create profile", msg, "Ok");

                await NavigationService.NavigateAsync("LogInPage");
            }

            catch
            {
                //TODO
            }

            finally
            {
                IsBusy = false;
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters != null && parameters.ContainsKey("CompanyProfile"))
            {
                CompanyProfile = parameters["CompanyProfile"] as Models.Company;

            }
        }

        public string RandomDigits(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }
    }
}
