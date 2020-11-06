using MyBodyTemperature.Models;
using MyBodyTemperature.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBodyTemperature.ViewModels
{
    public class UserTemperatureViewModel : BaseViewModel
    {
        private readonly IDbService _dbService;
        private readonly IPageDialogService _pageDialogService;
        public UserTemperatureViewModel(INavigationService navigationService, IDbService dbService, IPageDialogService dialogService) : base(navigationService)
        {
            _dbService = dbService;
            _pageDialogService = dialogService;
            AddTemperatureCommand = new DelegateCommand(OnTemperatureCommandExecuted, () => false);
            CancelCommand = new DelegateCommand(OnCancelCommandExecuted);
        }

        public DelegateCommand AddTemperatureCommand { get; }
        public DelegateCommand CancelCommand { get; }

        private string _temperature = string.Empty;
        public string Temperature
        {
            get => _temperature;
            set
            {
                SetProperty(ref _temperature, value);
                double.TryParse(value, out var temp);
                if (temp > 37.5)
                {
                    AccessGranted = false;
                    AccessGrantedEnabled = true;
                }
                else
                {
                    AccessGrantedEnabled = false;
                    AccessGranted = true;
                }
            }
        }

        private UserProfile _userProfile;
        public UserProfile CurrentUserProfile
        {
            get => _userProfile;
            set
            {
                SetProperty(ref _userProfile, value);
            }
        }


        private bool _accessGranted = true;
        public bool AccessGranted
        {
            get => _accessGranted;
            set
            {
                SetProperty(ref _accessGranted, value);
            }
        }

        private bool _accessGrantedEnabled = false;
        public bool AccessGrantedEnabled
        {
            get => _accessGrantedEnabled;
            set
            {
                SetProperty(ref _accessGrantedEnabled, value);
            }
        }

        private async void OnCancelCommandExecuted()
        {
            await NavigationService.GoBackAsync();
        }
        private async void OnTemperatureCommandExecuted()
        {
            try
            {
                if (!double.TryParse(Temperature, out var temp))
                {
                    await _pageDialogService.DisplayAlertAsync("Invalid Temperature", "Invalid Temperature entered", "Ok");
                    return;
                }

                if (temp > 45 || temp < 30)
                {
                    await _pageDialogService.DisplayAlertAsync("Invalid Temperature", "Invalid Temperature entered", "Ok");
                    return;
                }
                CurrentUserProfile.Temperature = double.Parse(Temperature);
                CurrentUserProfile.TemperatureDate = DateTime.Now;
                CurrentUserProfile.AccessGranted = AccessGranted;

                var result = await _dbService.UpdateItemAsync(CurrentUserProfile);

                var historyTemp = new UserTemperature
                {
                    Temperature = CurrentUserProfile.Temperature,
                    TemperatureDate = CurrentUserProfile.TemperatureDate,
                    UserId = CurrentUserProfile.UserId
                };

                await _dbService.InsertUserTemperatureAsync(historyTemp);

                await _pageDialogService.DisplayAlertAsync("Success", "Succcessfully added user temperature", "Ok");

                OnCancelCommandExecuted();

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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters != null && parameters.ContainsKey("UserProfileParam"))
            {
                CurrentUserProfile = parameters["UserProfileParam"] as UserProfile;

            }
        }

    }

}
