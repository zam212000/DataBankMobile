using MyBodyTemperature.Models;
using MyBodyTemperature.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            DefaultEmployeeStatusCollection();
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
                    StatusEmployee = EmployeeStatusList.LastOrDefault();
                }
                else
                {
                    AccessGrantedEnabled = false;
                    AccessGranted = true;
                    StatusEmployee = EmployeeStatusList.FirstOrDefault();
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

                CurrentUserProfile.StatusID = StatusEmployee.ID;
                CurrentUserProfile.AccessGranted = StatusEmployee.ID == 1;

                var result = await _dbService.UpdateItemAsync(CurrentUserProfile);

                var historyTemp = new UserTemperature
                {
                    Temperature = CurrentUserProfile.Temperature,
                    TemperatureDate = CurrentUserProfile.TemperatureDate,
                    UserId = CurrentUserProfile.UserId
                };

                await _dbService.InsertUserTemperatureAsync(historyTemp);

                await _pageDialogService.DisplayAlertAsync("Success", "Succcessfully added employee temperature", "Ok");

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
