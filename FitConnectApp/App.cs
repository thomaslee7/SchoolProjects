using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using FitConnectApp.ViewModel;
using Android.Gms.Common.Apis;
using Firebase.Auth;
using Android.Gms.Auth.Api;
using Android.Content;

namespace FitConnectApp
{
    public static class App
    {
        private static ViewModelLocator locator;

        public static ViewModelLocator Locator
        {
            get
            {
                if (locator == null)
                {
                    DispatcherHelper.Initialize();

                    var nav = new NavigationService();

                    SimpleIoc.Default.Register<INavigationService>(() => nav);

                    nav.Configure(ViewModelLocator.HomeScreenKey, typeof(HomeScreen));
                    nav.Configure(ViewModelLocator.LoginScreenKey, typeof(LoginActivity));

                    locator = new ViewModelLocator();
                }

                return locator;
            }
        }
    }
}