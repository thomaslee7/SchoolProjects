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
    public class StartWorkoutViewModel : ViewModelBase
    {
        private RelayCommand _startNewWorkout;
        private INavigationService _navService;

        public StartWorkoutViewModel(INavigationService navService)
        {
            _navService = navService;
        }

        public RelayCommand StartNewWorkout
        {
            get
            {
                return _startNewWorkout ?? 
                    (_startNewWorkout = new RelayCommand(() => Log.Debug("StartWorkoutVM", "StartNew workout RC")));
            }
        }
    }
}