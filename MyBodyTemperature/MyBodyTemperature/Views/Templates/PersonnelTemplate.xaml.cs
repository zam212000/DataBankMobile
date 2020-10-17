using FFImageLoading.Forms;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsApp.PopupPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyBodyTemperature.Views.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonnelTemplate : ContentView
    {
        public PersonnelTemplate()
        {
            InitializeComponent();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var imageSender = (CachedImage)sender;
            await Navigation.PushPopupAsync(new ChatProfileSelectionPopupView(imageSender.Source));

        }
    }
}