using Xamarin.Forms;
using System;
using System.Runtime.CompilerServices;
using Prism.Navigation;
using Xamarin.Forms.Xaml;
using MyBodyTemperature.Controls;

namespace MyBodyTemperature.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : TabbedPage
    {
        public MainTabbedPage()
        {
            InitializeComponent();

            this.SelectedItem = this.Children[4];
            //CurrentPage = Children[4];

        }
    }
}