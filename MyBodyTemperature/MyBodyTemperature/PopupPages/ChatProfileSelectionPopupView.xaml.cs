using MyBodyTemperature.PopupPages;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhatsApp.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChatProfileSelectionPopupView : PopupPage
	{
        private ChatProfileSelectionPopupViewModel viewModel;
        public ChatProfileSelectionPopupView (ImageSource imageSource, string contact)
		{
			InitializeComponent ();
            BindingContext = viewModel = new ChatProfileSelectionPopupViewModel(imageSource, contact);
		}
 
    }
}