using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;

namespace FitConnectApp.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {

        private RelayCommand navigateCommand;
        private string welcomeTitle;

        private readonly INavigationService navigationService;

        public LoginViewModel(INavigationService navService)
        {
            navigationService = navService;
            WelcomeTitle = "Welcome to FitConnect";
        }

        public string WelcomeTitle
        {
            get
            {
                return welcomeTitle;
            }
            set
            {
                Set(ref welcomeTitle, value);
            }
        }

        public RelayCommand NavigateCommand
        {
            get
            {
                return navigateCommand ??
                    (navigateCommand = new RelayCommand(() => navigationService.NavigateTo(ViewModelLocator.HomeScreenKey)));
            }
        }

    }
}