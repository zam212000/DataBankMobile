﻿using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using ImageCircle.Forms.Plugin.Droid;
using Plugin.CurrentActivity;
using Prism;
using Prism.Ioc;
using Rg.Plugins.Popup;

namespace MyBodyTemperature.Droid
{
    [Activity(Theme = "@style/MainTheme",
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        App formsApp;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);
            ImageCircleRenderer.Init();
            Popup.Init(this, savedInstanceState);

            base.OnCreate(savedInstanceState);

            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            UserDialogs.Init(this);

            formsApp = new App(new AndroidInitializer());
            LoadApplication(new App(new AndroidInitializer()));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

