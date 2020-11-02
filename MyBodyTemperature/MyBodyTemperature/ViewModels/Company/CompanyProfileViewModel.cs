using Acr.UserDialogs;
using MyBodyTemperature.Models;
using MyBodyTemperature.Services;
using MyBodyTemperature.Services.RemoteService;
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
        private readonly IRemoteDataService _remoteDataService;
        private readonly IValidationService _validationService;

        public CompanyProfileViewModel(INavigationService navigationService, IDbService dbService,
            IPageDialogService dialogService, IRemoteDataService remoteDataService, IValidationService validationService)
            : base(navigationService)
        {
            _dbService = dbService;
            _pageDialogService = dialogService;
            _remoteDataService = remoteDataService;
            _validationService = validationService;
            NextProfileCommand = new DelegateCommand(OnNextProfileCommandExecuted, () => false);
            TakePhotoCommand = new DelegateCommand(OnPhotoTakenCommandExecuted, () => false);
            CancelCommand = new DelegateCommand(OnCancelCommandExecuted);
        }

        public DelegateCommand NextProfileCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand TakePhotoCommand { get; }

        private string _companyName = string.Empty;
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

                if(! await _validationService.NetworkConnectedAsync())
                {
                    await _pageDialogService.DisplayAlertAsync("Network connectivity", "internet connection is required to create a company profile", "Ok");
                    return;
                }

                if (PhoneNumber.Length < 10)
                {
                    await _pageDialogService.DisplayAlertAsync("Create profile", "A phone number must be 10 digits", "Ok");
                    return;
                }

                if (!await _validationService.EmailValidAsync(CompanyEmail))
                {
                    await _pageDialogService.DisplayAlertAsync("Create profile", "Email address is not in the correct format", "Ok");
                    return;
                }

                Models.Company company;
                company = await _dbService.GetCompanyByName(CompanyName.Trim());
                if (company != null && company.PhoneNumberConfirmed)
                {
                    await _pageDialogService.DisplayAlertAsync("Create profile", "The company is already registered. You can use forgot password to recover your account", "Ok");
                    return;
                }
                else if (company != null && !company.PhoneNumberConfirmed)
                {
                    company.CompanyAddresss = CompanyAddresss;
                    company.AvatarUrl = ImageUrl;
                    company.ImageContent = ImageContent;
                    company.CompanyEmail = CompanyEmail;
                    company.PhoneNumber = PhoneNumber;
                    await _dbService.UpdateCompanyAsync(company);
                }
                else
                {
                    company = new Models.Company();
                    company.CompanyName = CompanyName;
                    company.CompanyAddresss = CompanyAddresss;
                    company.AvatarUrl = ImageUrl;
                    company.ImageContent = ImageContent;
                    company.CompanyEmail = CompanyEmail;
                    company.PhoneNumber = PhoneNumber;
                    await _dbService.AddNewCompanyAsync(company);
                }

                if (company != null && company.CompanyID > 0)
                {
                    var smsSend = await _remoteDataService.SendSmsAsync("", company.PhoneNumber);
                    if (string.IsNullOrEmpty(smsSend))
                    {
                        await _pageDialogService.DisplayAlertAsync("Unsuccessful", "Failed to send OTP, please ensure phone number provided is correct", "Ok");
                        return;
                    }

                    company.Token = smsSend;
                    await _dbService.UpdateCompanyAsync(company);

                    var param = new NavigationParameters();
                    param.Add("CompanyProfile", company);
                    await NavigationService.NavigateAsync("CompanyProfileOTPPage", param);

                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync("Create profile", "Failed to add the company. please retry or contact administrator", "Ok");
                }

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
