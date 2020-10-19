using Xamarin.Forms;
using System;
using System.Runtime.CompilerServices;
using Prism.Navigation;
using Xamarin.Forms.Xaml;
using MyBodyTemperature.Controls;

namespace MyBodyTemperature.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : TabbedPage, ISearchPage
    {
        public MainTabbedPage()
        {
            InitializeComponent();

            SearchBarTextChanged += HandleSearchBarTextChanged;
        }

        private void HandleSearchBarTextChanged(object sender, string e)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<string> SearchBarTextChanged;

        //void ISearchPage.OnSearchBarTextChanged((string text) => SearchBarTextChanged?.Invoke(this, text);

        public void OnSearchBarTextChanged(in string text)
        {
          //  throw new NotImplementedException();
        }
    }
}