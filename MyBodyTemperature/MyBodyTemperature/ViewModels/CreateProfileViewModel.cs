using Acr.UserDialogs;
using MyBodyTemperature.Helpers;
using MyBodyTemperature.Models;
using MyBodyTemperature.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyBodyTemperature.ViewModels
{
    public class CreateProfileViewModel : BaseViewModel
    {
        private readonly IDbService _dbService;
        private readonly IPageDialogService _pageDialogService;
        public CreateProfileViewModel(INavigationService navigationService, IDbService dbService, IPageDialogService dialogService) : base(navigationService)
        {
            _dbService = dbService;
            _pageDialogService = dialogService;
            NextProfileCommand = new DelegateCommand(OnNextProfileCommandExecuted, () => false);
            TakePhotoCommand = new DelegateCommand(OnPhotoTakenCommandExecuted, () => false);
            CancelCommand = new DelegateCommand(OnCancelCommandExecuted);
            SwitchSelectedCommand = new DelegateCommand<string>(OnSwitchItemSelected);
        }

        public DelegateCommand NextProfileCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand TakePhotoCommand { get; }

        public DelegateCommand<string> SwitchSelectedCommand { get; private set; }


        private string _firstName = string.Empty;
        public string FirstName
        {
            get => _firstName;
            set
            {
                SetProperty(ref _firstName, value);
            }
        }

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

        private string _cellPhoneNumber = string.Empty;
        public string CellPhoneNumber
        {
            get => _cellPhoneNumber;
            set
            {
                SetProperty(ref _cellPhoneNumber, value);
            }
        }
        private string _idNumber = string.Empty;
        public string IDNumber
        {
            get => _idNumber;
            set
            {
                SetProperty(ref _idNumber, value);
            }
        }

        private string _employeeNumber = string.Empty;
        public string EmployeeNumber
        {
            get => _employeeNumber;
            set
            {
                SetProperty(ref _employeeNumber, value);
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


        private string _imageUrl = string.Empty;
        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                SetProperty(ref _imageUrl, value);
            }
        }

        private string _photoName = string.Empty;
        public string PhotoName
        {
            get => _photoName;
            set
            {
                SetProperty(ref _photoName, value);
            }
        }

        private byte[] _imageContent;
        public byte[] ImageContent
        {
            get => _imageContent;
            set
            {
                SetProperty(ref _imageContent, value);
            }
        }

        private async void OnCancelCommandExecuted()
        {
            await NavigationService.GoBackAsync();
        }

        private async void OnSwitchItemSelected(string value)
        {

        }

        private async void OnPhotoTakenCommandExecuted()
        {
            var mediaFile = await MediaService.GetMediaFileFromCamera(FirstName).ConfigureAwait(false);

            if (mediaFile is null)
            {
                await _pageDialogService.DisplayAlertAsync("Photo failed", "Failed to take the photo", "Ok");
            }
            else
            {
                ImageUrl = mediaFile.Path;
                ImageContent = GetImageBytes(mediaFile.GetStream());
            }
        }

        private async void OnNextProfileCommandExecuted()
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

                var userProfile = new UserProfile();
                userProfile.PhoneNumber = CellPhoneNumber;
                userProfile.FirstNames = FirstName;
                userProfile.ImageContent = ImageContent;
                userProfile.AvatarUrl = ImageUrl;
                userProfile.CompanyID = Settings.CurrentCompany.CompanyID;
                userProfile.Temperature = double.Parse(Temperature);
                userProfile.TemperatureDate = DateTime.Now;
                userProfile.IDNumber = IDNumber;
                userProfile.EmployeeNumber = EmployeeNumber;
                userProfile.AccessGranted = AccessGranted;

                var _userId = await _dbService.InsertItemAsync(userProfile);
                if (_userId > 0)
                {
                    var historyTemp = new UserTemperature
                    {
                        Temperature = userProfile.Temperature,
                        TemperatureDate = userProfile.TemperatureDate,
                        UserId = userProfile.UserId
                    };
                    var tempResult = await _dbService.InsertUserTemperatureAsync(historyTemp);
                }

                await _pageDialogService.DisplayAlertAsync("Success", "Succcessfully added the user", "Ok");
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

    }
}
