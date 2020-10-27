using Microcharts;
using MyBodyTemperature.Models;
using MyBodyTemperature.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp.Views.Forms;
using SkiaSharp;
using Xamarin.Forms;
using MyBodyTemperature.Helpers;

namespace MyBodyTemperature.ViewModels
{
    public class EmployeeDetailViewModel : BaseViewModel
    {
        private readonly IDbService _dbService;
        private readonly IPageDialogService _pageDialogService;
        public EmployeeDetailViewModel(INavigationService navigationService, IDbService dbService, IPageDialogService dialogService) : base(navigationService)
        {
            _dbService = dbService;
            _pageDialogService = dialogService;
            TakePhotoCommand = new DelegateCommand(OnPhotoTakenCommandExecuted);
            AddTemperatureCommand = new DelegateCommand(OnAddTemperatureCommandExecuted);
            RemoveUserCommand = new DelegateCommand(OnRemoveUserCommandExecuted);
            UpdateUserCommand = new DelegateCommand(OnUpdateUserCommandExecuted);
        }

        public event EventHandler IsActiveChanged;
        public DelegateCommand UpdateProfileCommand { get; }
        public DelegateCommand AddTemperatureCommand { get; }
        public DelegateCommand RemoveUserCommand { get; }
        public DelegateCommand UpdateUserCommand { get; }

        public DelegateCommand TakePhotoCommand { get; }

        public Chart ChartBar => new BarChart() { Entries = EntryDataCollection, BackgroundColor = SKColors.White };

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

        private UserProfile _userProfile;
        public UserProfile CurrentUserProfile
        {
            get => _userProfile;
            set
            {
                SetProperty(ref _userProfile, value);
            }
        }


        private Microcharts.Entry[] _entryData;
        public Microcharts.Entry[] EntryData
        {
            get => _entryData;
            set
            {
                _entryData = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Microcharts.Entry> _entryDataCollection;
        public ObservableCollection<Microcharts.Entry> EntryDataCollection
        {
            get { return _entryDataCollection; }
            set { SetProperty(ref _entryDataCollection, value); }
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
                CurrentUserProfile.ImageContent = this.ImageContent;
            }
        }

        private async void OnAddTemperatureCommandExecuted()
        {
            var param = new NavigationParameters();
            param.Add("UserProfileParam", CurrentUserProfile);
            await NavigationService.NavigateAsync("UserTemperaturePage", param, true, true);
        }

        private async void OnUpdateUserCommandExecuted()
        {
            var param = new NavigationParameters();
            param.Add("UserProfileParam", CurrentUserProfile);
            await NavigationService.NavigateAsync("UpdateUserProfilePage", param, true, true);
        }

        
        private async void OnRemoveUserCommandExecuted()
        {
            var confirm = await _pageDialogService.DisplayAlertAsync("Delete User", "Are you sure you want to permanently delete this user", "Yes", "Cancel");
            if (confirm)
            {
                await _dbService.DeleteItemAsync(CurrentUserProfile);
                await NavigationService.NavigateAsync("EmployeesPage");
            }
        }


        private async void OnNextProfileCommandExecuted()
        {
            try
            {
                var userProfile = new UserProfile();
                userProfile.PhoneNumber = CellPhoneNumber;
                userProfile.FirstNames = FirstName;
                userProfile.ImageContent = ImageContent;
                userProfile.AvatarUrl = ImageUrl;
                userProfile.Temperature = double.Parse(Temperature);
                userProfile.TemperatureDate = DateTime.Now;
                userProfile.UserId = CurrentUserProfile.UserId;
                var result = await _dbService.UpdateItemAsync(userProfile);

                //TODO - Update user temp

                await _pageDialogService.DisplayAlertAsync("Success", "Succcessfully added the user", "Ok");

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

                CurrentUserProfile.FullName = $"{CurrentUserProfile.FirstNames} {CurrentUserProfile.Surname}";
                string dateString = string.Empty;
                if (CurrentUserProfile.TemperatureDate.Day == DateTime.Now.Day)
                {
                    dateString = $"Today {CurrentUserProfile.TemperatureDate.ToShortTimeString()}";
                }
                else if (CurrentUserProfile.TemperatureDate.Day == DateTime.Now.Day - 1)
                {
                    dateString = $"Yesterday {CurrentUserProfile.TemperatureDate.ToShortTimeString()}";
                }
                else
                {
                    dateString = CurrentUserProfile.TemperatureDate.ToString("yyyy/MM/dd");
                }

                CurrentUserProfile.CovidMetadata = new CovidMetadata
                {
                    HighFever = CurrentUserProfile.Temperature > 37.5 ? "Yes" : "No",
                    Temperature = $"{CurrentUserProfile.Temperature }°C",
                    TemperatureDate = dateString
                };

                CurrentUserProfile.ImageProperty = ImageSource.FromStream(() => new MemoryStream(CurrentUserProfile.ImageContent));

                // await AssignChartEntries();
            }
        }


        protected virtual void RaiseIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task<Microcharts.Entry[]> GetChartEntriesData()
        {
            //CurrentUserProfile.UserId
            var userTempHistory = await _dbService.GetUserTemperatureItemsAsync(Settings.CurrentUserId);
            var items = new List<Microcharts.Entry>();

            foreach (var item in userTempHistory)
            {
                var color = item.Temperature > 37.5 ? RedColor : GreenColor;
                items.Add(new Microcharts.Entry((float)item.Temperature) { Color = color, Label = item.TemperatureDate.ToString("MMMM dd"), ValueLabel = $"{item.Temperature}°C", });
            }

            // Always assign date to latest...
            if (userTempHistory.Any() && CurrentUserProfile != null)
            {
                var latestRecord = userTempHistory.OrderByDescending(x => x.TemperatureDate)?.FirstOrDefault();
                if (latestRecord != null)
                {
                    CurrentUserProfile.Temperature = latestRecord.Temperature;
                    CurrentUserProfile.TemperatureDate = latestRecord.TemperatureDate;

                    string dateString = string.Empty;
                    if (CurrentUserProfile.TemperatureDate.Day == DateTime.Now.Day)
                    {
                        dateString = $"Today {CurrentUserProfile.TemperatureDate.ToShortTimeString()}";
                    }
                    else if (CurrentUserProfile.TemperatureDate.Day == DateTime.Now.Day - 1)
                    {
                        dateString = $"Yesterday {CurrentUserProfile.TemperatureDate.ToShortTimeString()}";
                    }
                    else
                    {
                        dateString = CurrentUserProfile.TemperatureDate.ToString("yyyy/MM/dd");
                    }

                    CurrentUserProfile.CovidMetadata = new CovidMetadata
                    {
                        HighFever = CurrentUserProfile.Temperature > 37.5 ? "Yes" : "No",
                        Temperature = $"{CurrentUserProfile.Temperature }°C",
                        TemperatureDate = dateString
                    };
                }
            }
            return items.ToArray();
        }


        private async void AssignData()
        {

            var userTempHistory = await _dbService.GetUserTemperatureItemsAsync(1);
            var items = new List<Microcharts.Entry>();

            foreach (var item in userTempHistory)
            {
                var color = item.Temperature > 37.5 ? RedColor : GreenColor;
                items.Add(new Microcharts.Entry((float)item.Temperature) { Color = color, Label = item.TemperatureDate.ToString("MMMM dd"), ValueLabel = $"{item.Temperature}°C", });
            }

            EntryData = items.ToArray();
        }

        private static readonly SKColor GreenColor = SKColor.Parse("#006400");
        private static readonly SKColor RedColor = SKColor.Parse("#B22222");

    }
}
