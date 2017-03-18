using System;
using System.Collections.Generic;
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
using FitConnectApp.Models;

namespace FitConnectApp.ViewModel
{
    public class AccountViewModel : ViewModelBase
    {
        private User  _currentUser;
        private RelayCommand _account;
        private INavigationService _navService;

        private const string TAG = "AccountViewModel";


        public AccountViewModel(INavigationService navService)
        {
            _navService = navService;
        }

        public RelayCommand Account
        {
            get
            {
                Log.Debug("AccountVM", "Account RC");
                return _account ??
                    (_account = new RelayCommand(() => _navService.NavigateTo(ViewModelLocator.AccountScreenKey)));
            }
        }

    }
}