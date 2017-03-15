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
using FitConnectApp.Activities.WorkoutActivities;
using Android.Util;
//using Android.Support.V4.App;

namespace FitConnectApp.ViewModel
{
    public class CreateWorkoutViewModel : ViewModelBase
    {
        private const string TAG = "CreateWorkoutViewModel";
        private RelayCommand<FragmentManager> _addExercise;
        private INavigationService _navService;
                
        public CreateWorkoutViewModel(INavigationService navService)
        {
            _navService = navService;
        }

        public RelayCommand<FragmentManager> AddExercise
        {
            get
            {
                return _addExercise ?? (_addExercise = new RelayCommand<FragmentManager>((manager) =>
                {
                    var transaction = manager.BeginTransaction();
                    ExerciseSelectFragment addExerciseFragment = new ExerciseSelectFragment();
                    addExerciseFragment.Show(transaction, "Add new exercise");                    
                }));
            }
        }

    }
}