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

    }
}
