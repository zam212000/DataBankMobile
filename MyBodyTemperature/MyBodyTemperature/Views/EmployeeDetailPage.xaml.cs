using Microcharts;
using MyBodyTemperature.ViewModels;
using SkiaSharp;
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
    public partial class EmployeeDetailPage : ContentPage
    {
        private EmployeeDetailViewModel pageViewModel;
        public EmployeeDetailPage()
        {
            InitializeComponent();
            pageViewModel = (EmployeeDetailViewModel)BindingContext;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var chartsData = await pageViewModel.GetChartEntriesData();
            chartTempHistory.Chart =   new BarChart() { Entries = chartsData, BackgroundColor = SKColors.White };

        }

    }
}