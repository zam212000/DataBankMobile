using MyBodyTemperature.Services.UserProfile;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using MyBodyTemperature.Models;
using MyBodyTemperature.Helpers;

namespace MyBodyTemperature.ViewModels
{
    public class LogInViewModel : BaseViewModel
    {
        private readonly ILoginApiDataService _loginApiDataService;
        public LogInViewModel(INavigationService navigationService, ILoginApiDataService loginApiDataService) : base(navigationService)
        {
            _loginApiDataService = loginApiDataService;
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
                // IsBusy = true;
                var userProfile = new Models.UserProfile
                {
                    EmailAddress = "test@test.com",
                    Surname = "Test",
                    FirstNames = "Test Test",
                    UserName = "Kwazi Lamula",
                    UserId = 10,
                };

                Settings.User = userProfile;

                await NavigationService.NavigateAsync("/MainTabbedPage");

                //var authUser = await _loginApiDataService.AuthenticateUserAsync(UserName, Password);
                //if (!authUser)
                //{

                //}
            }
            catch
            {

            }

            finally
            {
                //   IsBusy = false;
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
