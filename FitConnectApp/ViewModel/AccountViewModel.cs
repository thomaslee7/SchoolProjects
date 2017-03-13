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
using GalaSoft.MvvmLight.Command;
using Android.Util;
using GalaSoft.MvvmLight.Views;

namespace FitConnectApp.ViewModel
{
    class AccountViewModel : ViewModelBase
    {
        private RelayCommand _account;
        private INavigationService _navService;

        public AccountViewModel(INavigationService navService)
        {
            _navService = navService;
        }

        public RelayCommand Account
        {
            get
            {
                return _account ??
                    (_account = new RelayCommand(() => Log.Debug("AccountVM",
                    "Account RC")));
            }
        }
    }
}