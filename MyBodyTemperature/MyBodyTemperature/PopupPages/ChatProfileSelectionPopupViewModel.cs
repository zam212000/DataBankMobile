using MyBodyTemperature.ViewModels;
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



        public ChatProfileSelectionPopupViewModel(ImageSource source, string contact)
        {
            SelectedProfileImage = source;
            Contact = contact;
        }
    }
}
