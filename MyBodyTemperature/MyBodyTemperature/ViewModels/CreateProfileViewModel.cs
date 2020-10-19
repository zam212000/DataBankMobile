using Acr.UserDialogs;
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
            NextProfileCommand = new DelegateCommand(OnNextProfileCommandExecuted);
            TakePhotoCommand = new DelegateCommand(OnPhotoTakenCommandExecuted);
        }

        public DelegateCommand NextProfileCommand { get; }
        public DelegateCommand TakePhotoCommand { get; }


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
                var userProfile = new UserProfile();
                userProfile.PhoneNumber = CellPhoneNumber;
                userProfile.FirstNames = FirstName;
                userProfile.ImageContent = ImageContent;
                userProfile.AvatarUrl = ImageUrl;
                userProfile.Temperature = double.Parse(Temperature);
                userProfile.TemperatureDate = DateTime.Today.AddDays(-1);

                var result = await _dbService.InsertItemAsync(userProfile);

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

        private byte[] GetImageBytes(Stream stream)
        {
            byte[] ImageBytes;
            using (var memoryStream = new System.IO.MemoryStream())
            {
                stream.CopyTo(memoryStream);
                ImageBytes = memoryStream.ToArray();
            }
            return ImageBytes;
        }

        public Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

    }
}
