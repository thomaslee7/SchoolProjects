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
    public class AccountViewModel : ViewModelBase
    {
        private RelayCommand _showAccount;
        private INavigationService _navService;

        public AccountViewModel(INavigationService navService)
        {
            _navService = navService;
        }

        public RelayCommand ShowAccount
        {
            get
            {
                Log.Debug("AccountVM", "Show Account RC");
                return _showAccount ??
                    (_showAccount = new RelayCommand(() => _navService.NavigateTo(ViewModelLocator.AccountScreenKey)));
            }
        }
    }
}