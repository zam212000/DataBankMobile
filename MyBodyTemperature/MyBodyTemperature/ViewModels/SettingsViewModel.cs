using MyBodyTemperature.Helpers;
using MyBodyTemperature.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyBodyTemperature.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly IDbService _dbService;
        private readonly IPageDialogService _pageDialogService;
        public SettingsViewModel(INavigationService navigationService, IDbService dbService, IPageDialogService dialogService) : base(navigationService)
        {
            _dbService = dbService;
            _pageDialogService = dialogService;
            CancelCommand = new DelegateCommand(OnCancelCommandExecuted);
            UpdateCompanyCommand = new DelegateCommand(OnUpdateCompanyCommandExecuted);
            IsExpanded = true;
        }

        public DelegateCommand AddTemperatureCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand UpdateCompanyCommand { get; }

        private async void OnCancelCommandExecuted()
        {
            await NavigationService.GoBackAsync();
        }

        private Models.Company _companyProfile;
        public Models.Company CompanyProfile
        {
            get => _companyProfile;
            set
            {
                SetProperty(ref _companyProfile, value);
            }
        }

        private ImageSource _companyImageProperty;

        public ImageSource CompanyImageProperty
        {
            get { return _companyImageProperty; }
            set { SetProperty(ref _companyImageProperty, value); }
        }

        private ObservableCollection<Models.Event> _events;
        public ObservableCollection<Models.Event> Events
        {
            get { return _events; }
            set { SetProperty(ref _events, value); }
        }

        public bool IsExpanded { get; set; }
        public Point ShadowOffset { get; set; } = new Point(20, 20);

        public async void OnUpdateCompanyCommandExecuted()
        {
            if (string.IsNullOrEmpty(CompanyProfile.CompanyName))
            {
                await _pageDialogService.DisplayAlertAsync("warning", "Company name is required", "Ok");
                return;
            }

            else if (string.IsNullOrEmpty(CompanyProfile.PhoneNumber))
            {
                await _pageDialogService.DisplayAlertAsync("warning", "Company phone number is required", "Ok");
                return;
            }

            else if (string.IsNullOrEmpty(CompanyProfile.CompanyEmail))
            {
                await _pageDialogService.DisplayAlertAsync("warning", "Company email address is required", "Ok");
                return;
            }

            var result = await _dbService.UpdateCompanyAsync(CompanyProfile);
            if (result > 0)
            {
                await _pageDialogService.DisplayAlertAsync("Success", "Succcessfully updated company details", "Ok");
            }
        }

        private async Task LoadEventItems()
        {
            var res = await _dbService.GetEventsAsync(CompanyProfile.CompanyID);

            if (!Equals(res, null))
            {
                Events = new ObservableCollection<Models.Event>(res);

                //foreach (var item in Events)
                //{
                    
                //}
            }
        }

        private async void OnItemSelectedCommand(Agenda ItemeSelected)
        {
            //Settings.CurrentUserId = userProfile.UserId;
            //var param = new NavigationParameters();
            //param.Add("UserProfileParam", userProfile);
            //await NavigationService.NavigateAsync("EmployeeDetailPage", param);
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                CompanyProfile = Settings.CurrentCompany;

                if (CompanyProfile.ImageContent != null)
                {

                    CompanyImageProperty = ImageSource.FromStream(() => new MemoryStream(CompanyProfile.ImageContent));
                }

                else
                {
                    CompanyImageProperty = ImageSource.FromFile("companyIcon.png");
                }

                await LoadEventItems();
            }
            catch
            {

            }
        }


        public ObservableCollection<Agenda> MyAgenda { get => GetAgenda(); }

        private ObservableCollection<Agenda> GetAgenda()
        {
            return new ObservableCollection<Agenda>
            {
                new Agenda { Topic = "Test 1 Event", Duration = "07:30 UTC - 11:30 UTC", Color = "#B96CBD", Date = new DateTime(2020, 3, 23),
                    Speakers = new ObservableCollection<Speaker>{ new Speaker { Name = "Maddy Leger", Time = "07:30" }, new Speaker { Name = "David Ortinau", Time = "08:30" }, new Speaker { Name = "Gerald Versluis", Time = "10:30" } } },

                new Agenda { Topic = "Test 2 Event", Duration = "07:30 UTC - 11:30 UTC", Color = "#49A24D", Date = new DateTime(2020, 3, 24),
                    Speakers = new ObservableCollection<Speaker>{ new Speaker { Name = "Maddy Leger", Time = "07:30" }, new Speaker { Name = "David Ortinau", Time = "08:30" }, new Speaker { Name = "Gerald Versluis", Time = "10:30" } } },

                new Agenda { Topic = "Test 3 Event", Duration = "07:30 UTC - 11:30 UTC", Color = "#FDA838", Date = new DateTime(2020, 3, 25),
                    Speakers = new ObservableCollection<Speaker>{ new Speaker { Name = "Maddy Leger", Time = "07:30" }, new Speaker { Name = "David Ortinau", Time = "08:30" }, new Speaker { Name = "Gerald Versluis", Time = "10:30" } } },

                new Agenda { Topic = "Test 4 Event", Duration = "07:30 UTC - 11:30 UTC", Color = "#F75355", Date = new DateTime(2020, 3, 26),
                    Speakers = new ObservableCollection<Speaker>{ new Speaker { Name = "Maddy Leger", Time = "07:30" }, new Speaker { Name = "David Ortinau", Time = "08:30" }, new Speaker { Name = "Gerald Versluis", Time = "10:30" } } },

                new Agenda { Topic = "Test 5 Event", Duration = "07:30 UTC - 11:30 UTC", Color = "#00C6AE", Date = new DateTime(2020, 3, 27),
                    Speakers = new ObservableCollection<Speaker>{ new Speaker { Name = "Maddy Leger", Time = "07:30" }, new Speaker { Name = "David Ortinau", Time = "08:30" }, new Speaker { Name = "Gerald Versluis", Time = "10:30" } } },

                new Agenda { Topic = "Test 6 Event", Duration = "07:30 UTC - 11:30 UTC", Color = "#455399", Date = new DateTime(2020, 3, 28),
                    Speakers = new ObservableCollection<Speaker>{ new Speaker { Name = "Maddy Leger", Time = "07:30" }, new Speaker { Name = "David Ortinau", Time = "08:30" }, new Speaker { Name = "Gerald Versluis", Time = "10:30" } } }
            };
        }

    }


    public class Agenda
    {
        public string Topic { get; set; }
        public string Duration { get; set; }
        public DateTime Date { get; set; }
        public ObservableCollection<Speaker> Speakers { get; set; }
        public string Color { get; set; }
    }

    public class Speaker
    {
        public string Name { get; set; }
        public string Time { get; set; }
    }
}
