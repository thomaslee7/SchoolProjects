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
using FitConnectApp.Models;

namespace FitConnectApp.ViewModel
{
    public class CreateWorkoutViewModel : ViewModelBase
    {
        private const string TAG = "CreateWorkoutViewModel";
        //private RelayCommand<FragmentManager> _addExercise;
        private RelayCommand _addExerciseCard;
        private RelayCommand saveWorkout;
        private INavigationService _navService;
                
        private WorkoutData workout;
        private Action addCardToActivity;

        public Action AddCardToActivity
        {
            get
            {
                return addCardToActivity;
            }
            set
            {
                Set(() => AddCardToActivity, ref addCardToActivity, value);
            }
        }

        public WorkoutData Workout
        {
            get
            {
                return workout;
            }
            set
            {
                Set(() => Workout, ref workout, value);
            }
        }
                
        public CreateWorkoutViewModel(INavigationService navService)
        {
            _navService = navService;            
            Workout = new WorkoutData();
        }

        //public RelayCommand<FragmentManager> AddExercise
        //{
        //    get
        //    {
        //        return _addExercise ?? (_addExercise = new RelayCommand<FragmentManager>((manager) =>
        //        {
        //            var transaction = manager.BeginTransaction();
        //            ExerciseSelectFragment addExerciseFragment = new ExerciseSelectFragment();
        //            addExerciseFragment.Show(transaction, "Add new exercise");                    
        //        }));
        //    }
        //}

        public RelayCommand AddExerciseCard
        {
            get
            {
                return _addExerciseCard ?? (_addExerciseCard = new RelayCommand(() =>
                {
                    if (AddCardToActivity != null)
                        AddCardToActivity();
                }));
            }
        }

        public RelayCommand SaveWorkout
        {
            get
            {
                return saveWorkout ?? (saveWorkout = new RelayCommand(()=> {

                    Log.Debug(TAG, "Saving workout...");
                    var thing = Workout;
                    Log.Debug(TAG, "NumExercises: " + Workout.Exercises.Count.ToString());
                    foreach(var item in Workout.Exercises)
                    {
                        Log.Debug(TAG, "Numsets for " + item.ExName + ": " + item.SetData.Count);
                    }
                }));
            }

        }


    }
}