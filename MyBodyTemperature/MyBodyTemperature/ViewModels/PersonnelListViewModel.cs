using MyBodyTemperature.Models;
using MyBodyTemperature.Services;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;

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

            //ImageSource streamImage = ImageSource.FromStream(() =>
            //{
            //    // Before returning you must convert a image to stream
            //    var stream = file.GetStream();
            //    file.Dispose();
            //    return stream;
            //});

            //// The allocation works but no image is displayed
            //Image = streamImage;
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

        private ImageSource _imageProperty;

        public ImageSource ImageProperty
        {
            get { return _imageProperty; }
            set { SetProperty(ref _imageProperty, value); }
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


        private async void AddNewItem()
        {
            await NavigationService.NavigateAsync("CreateProfilePage");
        }

        public Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
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