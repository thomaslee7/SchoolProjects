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
using FitConnectApp.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;
using Android.Gms.Auth.Api;
using Android.Gms.Common.Apis;
using Android.Util;

namespace FitConnectApp.ViewModel
{
    public class StatsViewModel
    {
        private RelayCommand _showStats;
        private INavigationService _navService;
        private const string TAG = "StatsViewModel";

        public StatsViewModel(INavigationService navService)
        {
            _navService = navService;
        }

        public RelayCommand ShowStats
        {
            get
            {
                Log.Debug("StatsVM", "Stats RC");
                return _showStats ??
                    (_showStats = new RelayCommand(() =>
                        _navService.NavigateTo(ViewModelLocator.StatsScreenKey)));
            }
        }
    }
}