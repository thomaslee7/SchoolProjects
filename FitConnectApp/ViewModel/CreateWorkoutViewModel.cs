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
        private RelayCommand<FragmentManager> _addExercise;
        private RelayCommand saveWorkout;
        private INavigationService _navService;

        private List<Guid> exerciseTags;
        private WorkoutData workout;

        public List<Guid> ExerciseTags
        {
            get
            {
                return exerciseTags;
            }
            set
            {
                Set(() => ExerciseTags, ref exerciseTags, value);
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
            ExerciseTags = new List<Guid>();
            Workout = new WorkoutData();
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
                        Log.Debug(TAG, "Numsets for " + item.Value.ExName + ": " + item.Value.SetData.Count);
                    }
                }));
            }

        }


    }
}