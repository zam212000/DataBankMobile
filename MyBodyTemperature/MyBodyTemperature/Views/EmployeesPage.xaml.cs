using MyBodyTemperature.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyBodyTemperature.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmployeesPage : ContentPage, ISearchPage
    {
        public EmployeesPage()
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