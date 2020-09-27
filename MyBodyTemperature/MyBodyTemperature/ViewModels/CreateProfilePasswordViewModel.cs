using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBodyTemperature.ViewModels
{
    public class CreateProfilePasswordViewModel : BaseViewModel
    {
        public CreateProfilePasswordViewModel(INavigationService navigationService) : base(navigationService)
        {
            ConfirmPasswordCommand = new DelegateCommand(OnConfirmPasswordCommandExecuted, () => false);
        }

        public DelegateCommand ConfirmPasswordCommand { get; }
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


        private void OnConfirmPasswordCommandExecuted()
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
                //IsBusy = false;
            }

        }
    }
}
