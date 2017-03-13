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
    class StatsViewModel : ViewModelBase
    {
        private RelayCommand _stats;
        private INavigationService _navService;

        public StatsViewModel(INavigationService navService)
        {
            _navService = navService;
        }

        public RelayCommand Stats
        {
            get
            {
                return _stats ??
                    (_stats = new RelayCommand(() => Log.Debug("StatsVM",
                    "Stats RC")));
            }
        }
    }
}