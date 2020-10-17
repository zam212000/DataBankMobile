using Xamarin.Forms;
using System;
using System.Runtime.CompilerServices;
using Prism.Navigation;
using Xamarin.Forms.Xaml;


namespace MyBodyTemperature.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : TabbedPage
    {
        public MainTabbedPage()
        {
            InitializeComponent();
        }
    }
}