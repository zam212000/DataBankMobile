using MyBodyTemperature.Models;
using MyBodyTemperature.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;

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
            UpdateUserCommand = new DelegateCommand(OnUpdateUserCommandExecuted);
            CancelCommand = new DelegateCommand(OnCancelCommandExecuted);
            ImageProperty = ImageSource.FromFile("defaultpic.png");
            DefaultEmployeeStatusCollection();
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

        private ImageSource _imageProperty;
        public ImageSource ImageProperty
        {
            get { return _imageProperty; }
            set { SetProperty(ref _imageProperty, value); }
        }

        public ObservableCollection<EmployeeStatus> _employeeStatusList;
        public ObservableCollection<EmployeeStatus> EmployeeStatusList
        {
            get => _employeeStatusList;
            set
            {
                _employeeStatusList = value;
                RaisePropertyChanged();
            }
        }


        private EmployeeStatus _employeeStatus;
        public EmployeeStatus StatusEmployee
        {
            get => _employeeStatus;
            set
            {
                SetProperty(ref _employeeStatus, value);
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
                CurrentUserProfile.StatusID = StatusEmployee.ID;
                if (string.IsNullOrEmpty(CurrentUserProfile.FirstNames))
                {
                    await _pageDialogService.DisplayAlertAsync("Warning", "Employee name is required", "Ok");
                    return;
                }
                if (string.IsNullOrEmpty(CurrentUserProfile.PhoneNumber))
                {
                    await _pageDialogService.DisplayAlertAsync("Warning", "Employee phone number is required", "Ok");
                    return;
                }

                var result = await _dbService.UpdateItemAsync(CurrentUserProfile);
                await _pageDialogService.DisplayAlertAsync("Success", "Succcessfully updated employee details", "Ok");

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

                if (CurrentUserProfile.ImageContent != null)
                {

                    ImageProperty = ImageSource.FromStream(() => new MemoryStream(CurrentUserProfile.ImageContent));
                }
                else
                {
                    ImageProperty = ImageSource.FromFile("defaultpic.png");
                }

                if (CurrentUserProfile.StatusID == 0 || CurrentUserProfile.StatusID == 1)
                {
                    StatusEmployee = EmployeeStatusList.FirstOrDefault();
                }
                else if (CurrentUserProfile.StatusID == 2)
                {
                    StatusEmployee = EmployeeStatusList.LastOrDefault();
                }

            }
        }

        public void DefaultEmployeeStatusCollection()
        {
            EmployeeStatusList = new ObservableCollection<EmployeeStatus>()
            {
               new EmployeeStatus{ ID = 1,   Description= "Access Granted" },
               new EmployeeStatus{ ID = 2,   Description= "Isolation" }
            };


        }
    }
}
