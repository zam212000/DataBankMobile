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
            UpdateProfileCommand = new DelegateCommand(OnNextProfileCommandExecuted);
            TakePhotoCommand = new DelegateCommand(OnPhotoTakenCommandExecuted);
            AddTemperatureCommand = new DelegateCommand(OnAddTemperatureCommandExecuted);
            //EntryData = GetItems();
        }

        public event EventHandler IsActiveChanged;
        public DelegateCommand UpdateProfileCommand { get; }
        public DelegateCommand AddTemperatureCommand { get; }
        
        public DelegateCommand TakePhotoCommand { get; }

        public Chart ChartBar => new BarChart() { Entries = EntryData, BackgroundColor = SKColors.White };

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


        private ObservableCollection<CovidMetadata> _historyCovidMetadata;
        public ObservableCollection<CovidMetadata> HistoryCovidMetadata
        {
            get { return _historyCovidMetadata; }
            set { SetProperty(ref _historyCovidMetadata, value); }
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
                    dateString = "Today";
                }
                else if (CurrentUserProfile.TemperatureDate.Day == DateTime.Now.Day - 1)
                {
                    dateString = "Yesterday";
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

                EntryData = GetItems();
            }
        }


        protected virtual void RaiseIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }

        public Microcharts.Entry[] GetItems(bool forceRefresh = false)
        {
            return GetItemData(forceRefresh).Result;
        }

        public async Task<Microcharts.Entry[]> GetItemsAsync(bool forceRefresh = false)
        {
            return await GetItemData(forceRefresh);
        }

        private async Task<Microcharts.Entry[]> GetItemData(bool forceRefresh)
        {

            var userTempHistory = await _dbService.GetUserTemperatureItemsAsync(CurrentUserProfile.UserId);
            var items = new List<Microcharts.Entry>();

            foreach (var item in userTempHistory)
            {
                var color = item.Temperature > 37.5 ? RedColor : GreenColor;
                items.Add(new Microcharts.Entry((float)item.Temperature) { Color = color, Label = item.TemperatureDate.ToString("MMMM dd"), ValueLabel = $"{item.Temperature}°C", });
            }

            //var items = new[]
            //{
            //    new Microcharts.Entry(34) { Color = GreenColor, Label = "12 Oct", ValueLabel = "34°C", },
            //    new Microcharts.Entry(35) { Color = GreenColor, Label = "13 Oct", ValueLabel = "35°C" },
            //    new Microcharts.Entry(38) { Color = RedColor, Label = "14 Oct", ValueLabel = "38°C" },
            //    new Microcharts.Entry(39) { Color = RedColor, Label = "15 Oct", ValueLabel = "39°C" },
            //    new Microcharts.Entry(34) { Color = GreenColor, Label = "16 Oct", ValueLabel = "34°C", },
            //    new Microcharts.Entry(40) { Color = RedColor, Label = "17 Oct", ValueLabel = "40°C" },
            //    new Microcharts.Entry(35) { Color = GreenColor, Label = "18 Oct", ValueLabel = "35°C" },
            //    new Microcharts.Entry(36) { Color = GreenColor, Label = "19 Oct", ValueLabel = "36°C" },
            //    new Microcharts.Entry(38) { Color = RedColor, Label = "20 Oct", ValueLabel = "38°C", },
            //    new Microcharts.Entry(35) { Color = GreenColor, Label = "21 Octc", ValueLabel = "35°C" },
            //    new Microcharts.Entry(34) { Color = GreenColor, Label = "22 Oct", ValueLabel = "34°C" },
            //    new Microcharts.Entry(39) { Color = RedColor, Label = "23 Oct", ValueLabel = "39°C" }
            //};

            return await Task.FromResult(items.ToArray());
        }

        private static readonly SKColor GreenColor = SKColor.Parse("#006400");
        private static readonly SKColor RedColor = SKColor.Parse("#B22222");

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

    }
}
