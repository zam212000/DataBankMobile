﻿using MyBodyTemperature.ViewModels;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace MyBodyTemperature.PopupPages
{
    public class ChatProfileSelectionPopupViewModel : BindableBase
    {
        private ImageSource _selectedProfileImage;
        public ImageSource SelectedProfileImage
        {
            get { return _selectedProfileImage; }
            set { SetProperty(ref _selectedProfileImage, value); }
        }

        private string _contact;
        public string Contact
        {
            get { return _contact; }
            set { SetProperty(ref _contact, value); }
        }



        public ChatProfileSelectionPopupViewModel(ImageSource source)
        {
            SelectedProfileImage = source;
            // FileName to contact name; Example: Rita.jpg => Rita
            var fileName = source.ToString().Remove(0, 6);
            Contact = fileName.Remove(fileName.Length - 4, 4);
        }
    }
}
