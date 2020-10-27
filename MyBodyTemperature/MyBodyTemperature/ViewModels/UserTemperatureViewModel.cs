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
            AddTemperatureCommand = new DelegateCommand(OnTemperatureCommandExecuted);
        }

        public DelegateCommand AddTemperatureCommand { get; }

        private string _temperature = string.Empty;
        public string Temperature
        {
            get => _temperature;
            set
            {
                SetProperty(ref _temperature, value);
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

        private async void OnTemperatureCommandExecuted()
        {
            try
            {
                //var userProfile = new UserProfile();
                //userProfile.Temperature = double.Parse(Temperature);
                //userProfile.TemperatureDate = DateTime.Now;
                //userProfile.UserId = CurrentUserProfile.UserId;

                CurrentUserProfile.Temperature = double.Parse(Temperature);
                CurrentUserProfile.TemperatureDate = DateTime.Now;
                var result = await _dbService.UpdateItemAsync(CurrentUserProfile);

                var historyTemp = new UserTemperature
                {
                    Temperature = CurrentUserProfile.Temperature,
                    TemperatureDate = CurrentUserProfile.TemperatureDate,
                    UserId = CurrentUserProfile.UserId
                };

                await _dbService.InsertUserTemperatureAsync(historyTemp);

                await _pageDialogService.DisplayAlertAsync("Success", "Succcessfully added user temperature", "Ok");

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
