using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyBodyTemperature.Models
{
    public class MenuItem : BindableObject
    {
        string title;
        MenuItemType menuItemType;
        Type viewModelType;
        bool isEnabled;

        public string Title
        {
            get => title;

            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        public MenuItemType MenuItemType
        {
            get => menuItemType;

            set
            {
                menuItemType = value;
                OnPropertyChanged();
            }
        }

        public Type ViewModelType
        {
            get => viewModelType;

            set
            {
                viewModelType = value;
                OnPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get => isEnabled;

            set
            {
                isEnabled = value;
                OnPropertyChanged();
            }
        }

        public Action AfterNavigationAction { get; set; }
    }
}