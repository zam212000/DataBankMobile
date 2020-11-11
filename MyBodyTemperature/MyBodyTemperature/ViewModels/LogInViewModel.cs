using MyBodyTemperature.Services.UserProfile;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using MyBodyTemperature.Models;
using MyBodyTemperature.Helpers;
using MyBodyTemperature.Services.RemoteService;
using MyBodyTemperature.Services;
using Prism.Services;
using MyBodyTemperature.Services.EncryptionService;

namespace MyBodyTemperature.ViewModels
{
    public class LogInViewModel : BaseViewModel
    {
        private readonly IDbService _dbService;
        private readonly ILoginApiDataService _loginApiDataService;
        private readonly IRemoteDataService _remoteDataService;
        private readonly IPageDialogService _pageDialogService;
        public LogInViewModel(INavigationService navigationService, ILoginApiDataService loginApiDataService,
            IRemoteDataService remoteDataService, IDbService dbService, IPageDialogService pageDialogService)
            : base(navigationService)
        {
            _loginApiDataService = loginApiDataService;
            _remoteDataService = remoteDataService;
            _pageDialogService = pageDialogService;
            _dbService = dbService;

            LogInCommand = new DelegateCommand(OnSignInCommandExecuted);
            SignUpCommand = new DelegateCommand(OnSignUpCommandExecuted);
            ForgotPasswordCommand = new DelegateCommand(OnForgotPasswordCommandExecuted);
        }

        public DelegateCommand LogInCommand { get; }
        public DelegateCommand ForgotPasswordCommand { get; }
        public DelegateCommand SignUpCommand { get; }

        private string _username = string.Empty;
        public string UserName
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

        private async void OnSignInCommandExecuted()
        {
            try
            {
                IsBusy = true;
                var company = await _dbService.GetCompanyByUsername(UserName);
                if (company != null)
                {
                    if (!company.PhoneNumberConfirmed)
                    {
                        await _pageDialogService.DisplayAlertAsync("Login", "Company registration is incomplete. Please go to Signup to finalise registration", "Ok");
                        return;
                    }

                    if (AesEncryptionService.Decrypt(company.Password) == Password)
                    {
                        Settings.CurrentCompany = company;
                        await NavigationService.NavigateAsync("/MainTabbedPage");
                    }
                    else
                    {
                        await _pageDialogService.DisplayAlertAsync("Login", "Username or password incorrect", "Ok");
                    }
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync("Login", "Username or password incorrect", "Ok");
                }

                //var company = await _dbService.GetCompanyByID(1);
                //Settings.CurrentCompany = company;
                //await NavigationService.NavigateAsync("/MainTabbedPage");
            }
            catch (Exception e)
            {

            }

            finally
            {
                IsBusy = false;
            }

        }

        private void OnForgotPasswordCommandExecuted()
        {
            try
            {

            }
            catch (Exception e)
            {
                //LOG ERROR
            }

            finally
            {
                // IsBusy = false;
            }

        }


        private async void OnSignUpCommandExecuted()
        {
            try
            {
                await NavigationService.NavigateAsync("CompanyProfilePage");
            }
            catch (Exception e)
            {
                //LOG ERROR
            }

            finally
            {
                // IsBusy = false;
            }

        }
    }
}
