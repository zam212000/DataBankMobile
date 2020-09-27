using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBodyTemperature.ViewModels
{
    public class CreateProfileViewModel : BaseViewModel
    {
        public CreateProfileViewModel(INavigationService navigationService) : base(navigationService)
        {
            NextProfileCommand = new DelegateCommand(OnNextProfileCommandExecuted);
        }

        public DelegateCommand NextProfileCommand { get; }

        private string _emailAddress = string.Empty;
        public string EmailAddress
        {
            get => _emailAddress;
            set
            {
                SetProperty(ref _emailAddress, value);
            }
        }

        private string _firstName = string.Empty;
        public string FirstName
        {
            get => _firstName;
            set
            {
                SetProperty(ref _firstName, value);
            }
        }

        private string _lastName = string.Empty;
        public string LastName
        {
            get => _lastName;
            set
            {
                SetProperty(ref _lastName, value);
            }
        }

        private string _cellPhoneNumber = string.Empty;
        public string CellPhoneNumber
        {
            get => _cellPhoneNumber;
            set
            {
                SetProperty(ref _cellPhoneNumber, value);
            }
        }

        private async void OnNextProfileCommandExecuted()
        {
            try
            {
                await NavigationService.NavigateAsync("CreateProfilePasswordPage");
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
