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
    public class UpdateUserProfileViewModel : BaseViewModel
    {
        private readonly IDbService _dbService;
        private readonly IPageDialogService _pageDialogService;
        public UpdateUserProfileViewModel(INavigationService navigationService, IDbService dbService, IPageDialogService dialogService) : base(navigationService)
        {
            _dbService = dbService;
            _pageDialogService = dialogService;
            UpdateUserCommand = new DelegateCommand(OnUpdateUserCommandExecuted, () => false);
            CancelCommand = new DelegateCommand(OnCancelCommandExecuted);
        }

        public DelegateCommand UpdateUserCommand { get; }
        public DelegateCommand CancelCommand { get; }



        private UserProfile _userProfile;
        public UserProfile CurrentUserProfile
        {
            get => _userProfile;
            set
            {
                SetProperty(ref _userProfile, value);
            }
        }

        private async void OnCancelCommandExecuted()
        {
            await NavigationService.GoBackAsync();
        }
        private async void OnUpdateUserCommandExecuted()
        {
            try
            {
                var result = await _dbService.UpdateItemAsync(CurrentUserProfile);
                await _pageDialogService.DisplayAlertAsync("Success", "Succcessfully updated user", "Ok");

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
