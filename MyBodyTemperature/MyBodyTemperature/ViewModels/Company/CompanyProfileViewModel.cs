using Acr.UserDialogs;
using MyBodyTemperature.Models;
using MyBodyTemperature.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using Xamarin.Forms;

namespace MyBodyTemperature.ViewModels.Company
{
    public class CompanyProfileViewModel : BaseViewModel
    {
        private readonly IDbService _dbService;
        private readonly IPageDialogService _pageDialogService;
        public CompanyProfileViewModel(INavigationService navigationService, IDbService dbService, IPageDialogService dialogService) : base(navigationService)
        {
            _dbService = dbService;
            _pageDialogService = dialogService;
            NextProfileCommand = new DelegateCommand(OnNextProfileCommandExecuted, () => false);
            TakePhotoCommand = new DelegateCommand(OnPhotoTakenCommandExecuted, () => false);
            CancelCommand = new DelegateCommand(OnCancelCommandExecuted);
        }

        public DelegateCommand NextProfileCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand TakePhotoCommand { get; }

        private string _companyName= string.Empty;
        public string CompanyName
        {
            get => _companyName;
            set
            {
                SetProperty(ref _companyName, value);
            }
        }

        private string _companyAddresss = string.Empty;
        public string CompanyAddresss
        {
            get => _companyAddresss;
            set
            {
                SetProperty(ref _companyAddresss, value);
            }
        }

        private string _companyEmail = string.Empty;
        public string CompanyEmail
        {
            get => _companyEmail;
            set
            {
                SetProperty(ref _companyEmail, value);
            }
        }
        private string _phoneNumber = string.Empty;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                SetProperty(ref _phoneNumber, value);
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

        private byte[] _imageContent;
        public byte[] ImageContent
        {
            get => _imageContent;
            set
            {
                SetProperty(ref _imageContent, value);
            }
        }

        public ImageSource ImageProperty { get; set; }


        private async void OnCancelCommandExecuted()
        {
            await NavigationService.GoBackAsync();
        }

        private async void OnPhotoTakenCommandExecuted()
        {
            var mediaFile = await MediaService.GetMediaFileFromCamera(CompanyName).ConfigureAwait(false);

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
                var company = new  Models.Company();
                company.CompanyName = CompanyName;
                company.CompanyAddresss = CompanyAddresss;
                company.AvatarUrl = ImageUrl;
                company.ImageContent = ImageContent;
                company.CompanyEmail = CompanyEmail;
                company.PhoneNumber = PhoneNumber;

                var _companyId = await _dbService.AddNewCompanyAsync(company);

                //if (_userId > 0)
                //{
                //    var historyTemp = new UserTemperature
                //    {
                //        Temperature = userProfile.Temperature,
                //        TemperatureDate = userProfile.TemperatureDate,
                //        UserId = userProfile.UserId
                //    };
                //    var tempResult = await _dbService.InsertUserTemperatureAsync(historyTemp);
                //}

                await _pageDialogService.DisplayAlertAsync("Success", "Succcessfully registered the company", "Ok");
                //OnCancelCommandExecuted();

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
