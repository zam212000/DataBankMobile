using MyBodyTemperature.Models;
using MyBodyTemperature.Services;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MyBodyTemperature.ViewModels
{
    public class PersonnelListViewModel : BaseViewModel
    {
        private readonly IDbService _dbService;
        public DelegateCommand ItemAddedCommand { get; set; }

        public DelegateCommand<UserProfile> ItemSelectedCommand => new DelegateCommand<UserProfile>(OnItemSelectedCommand);
        public PersonnelListViewModel(INavigationService navigationService, IDbService dbService) : base(navigationService)
        {
            ItemAddedCommand = new DelegateCommand(AddNewItem);
            _dbService = dbService;
            //NextProfileCommand = new DelegateCommand(OnNextProfileCommandExecuted);
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
        private async void OnItemSelectedCommand(UserProfile senditem)
        {
            var p = new NavigationParameters();
            p.Add("item", senditem);

            await NavigationService.NavigateAsync("TodoItemPage", p);
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                SelectedItem = null;

                var res = await _dbService.GetItemsAsync();

                if (!Equals(res, null))
                {
                    UserProfiles = new ObservableCollection<UserProfile>(res);
                }
            }
            catch (Exception ex)
            {
               // await _dialogService.DisplayAlertAsync("Error", ex.Message, "OK");
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

        public ObservableCollection<UserProfile> userProfiles;

    }

}