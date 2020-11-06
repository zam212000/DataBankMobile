using MyBodyTemperature.Helpers;
using MyBodyTemperature.Models;
using MyBodyTemperature.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyBodyTemperature.ViewModels
{
    public class EmployeesViewModel : BaseViewModel
    {
        private readonly IDbService _dbService;
        public DelegateCommand ItemAddedCommand { get; set; }
        public DelegateCommand TextChangedCommand { get; }

        public DelegateCommand<UserProfile> ItemSelectedCommand => new DelegateCommand<UserProfile>(OnItemSelectedCommand);
        public EmployeesViewModel(INavigationService navigationService, IDbService dbService) : base(navigationService)
        {
            ItemAddedCommand = new DelegateCommand(AddNewItem);
            TextChangedCommand = new DelegateCommand(TextChanged);
            _dbService = dbService;
        }

        public DelegateCommand NextProfileCommand { get; }

        private Models.Company _companyProfile;
        public Models.Company CompanyProfile
        {
            get => _companyProfile;
            set
            {
                SetProperty(ref _companyProfile, value);
            }
        }

        private string _emailAddress = string.Empty;
        public string EmailAddress
        {
            get => _emailAddress;
            set
            {
                SetProperty(ref _emailAddress, value);
            }
        }

        private ImageSource _companyImageProperty;

        public ImageSource CompanyImageProperty
        {
            get { return _companyImageProperty; }
            set { SetProperty(ref _companyImageProperty, value); }
        }

        private UserProfile _selectedItem;
        public UserProfile SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetProperty(ref _selectedItem, value);
                IsItemSelected = !Equals(_selectedItem, null);
            }
        }

        private bool _isItemSelected;
        public bool IsItemSelected
        {
            get { return _isItemSelected; }
            set { SetProperty(ref _isItemSelected, value); }
        }

        private string _searchKeyword;
        public string SearchKeyword
        {
            get { return _searchKeyword; }
            set { SetProperty(ref _searchKeyword, value); }
        }


        private async void OnItemSelectedCommand(UserProfile userProfile)
        {
            Settings.CurrentUserId = userProfile.UserId;
            var param = new NavigationParameters();
            param.Add("UserProfileParam", userProfile);
            await NavigationService.NavigateAsync("EmployeeDetailPage", param);
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                SelectedItem = null;
                CompanyProfile = Settings.CurrentCompany;

                if (CompanyProfile.ImageContent != null)
                {

                    CompanyImageProperty = ImageSource.FromStream(() => new MemoryStream(CompanyProfile.ImageContent));
                }

                else
                {
                    CompanyImageProperty = ImageSource.FromFile("companyIcon.png");
                }


                await LoadAllItems();
            }
            catch (Exception ex)
            {
                // await _dialogService.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        }

        private async Task LoadAllItems()
        {
            var res = await _dbService.GetItemsAsync(CompanyProfile.CompanyID);

            if (!Equals(res, null))
            {
                UserProfiles = new ObservableCollection<UserProfile>(res);

                foreach (var item in UserProfiles)
                {
                    item.FullName = $"{item.FirstNames} {item.Surname}";
                    string dateString = string.Empty;
                    if (item.TemperatureDate.Day == DateTime.Now.Day)
                    {
                        dateString = $"Today {item.TemperatureDate.ToShortTimeString()}";
                    }
                    else if (item.TemperatureDate.Day == DateTime.Now.Day - 1)
                    {
                        dateString = $"Yesterday {item.TemperatureDate.ToShortTimeString()}";
                    }
                    else
                    {
                        dateString = item.TemperatureDate.ToString("yyyy/MM/dd");
                    }

                    item.CovidMetadata = new CovidMetadata
                    {
                        HighFever = item.Temperature > 37.5 ? "Yes" : "No",
                        Temperature = $"{item.Temperature }°C",
                        TemperatureDate = dateString
                    };

                    if (item.ImageContent != null)
                    {
                        item.ImageProperty = ImageSource.FromStream(() => new MemoryStream(item.ImageContent));
                    }
                    else
                    {
                        item.ImageProperty = ImageSource.FromFile("defaultpic.png");
                    }
                }

                OriginalItems = UserProfiles;
            }
        }

        public async void TextChanged()
        {
            try
            {
                if (SearchKeyword?.Length > 0)
                {
                    var items = OriginalItems.Where(x =>
                            x.FullName.ToLower().Contains(SearchKeyword.ToLower()) ||
                             x.PhoneNumber.ToLower().Contains(SearchKeyword.ToLower()) ||
                              x.IDNumber.ToLower().Contains(SearchKeyword.ToLower()) ||
                              x.Temperature.ToString().Contains(SearchKeyword.ToLower())
                            );


                    UserProfiles = new ObservableCollection<UserProfile>(items);
                }

                else if (SearchKeyword?.Length == 0)
                {
                    await LoadAllItems();
                }
            }
            catch
            {

            }
        }

        private async void AddNewItem()
        {
            await NavigationService.NavigateAsync("CreateProfilePage");
        }

        private ObservableCollection<UserProfile> _userProfiles;
        public ObservableCollection<UserProfile> UserProfiles
        {
            get { return _userProfiles; }
            set { SetProperty(ref _userProfiles, value); }
        }

        public ObservableCollection<UserProfile> OriginalItems;

    }
}
