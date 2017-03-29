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
using Android.Gms.Common.Apis;
using Android.Gms.Common;
using Android.Gms.Auth.Api;
using Firebase.Database;

namespace FitConnectApp.ViewModel
{
    public class CreateWorkoutViewModel : ViewModelBase
    {
        private const string TAG = "CreateWorkoutViewModel";
        //private RelayCommand<FragmentManager> _addExercise;
        private RelayCommand _addExerciseCard;
        private RelayCommand<Context> saveWorkout;
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

        public RelayCommand<Context> SaveWorkout
        {
            get
            {
                return saveWorkout ?? (saveWorkout = new RelayCommand<Context>((context) =>
                {

                    var uid = App.getUid(context);

                    Log.Debug(TAG, "Saving workout...");
                    Log.Debug(TAG, "NumExercises: " + Workout.Exercises.Count.ToString());
                    var thing = Workout;

                    var db = FirebaseDatabase.GetInstance(App.fbApp);
                    var test = db.GetReference("WorkoutDateList").Child(uid).Child(workout.WorkoutId.ToString()).SetValue(workout.Date.ToString());

                    try
                    {
                        var workouts = db.GetReference("Workouts").Child(uid);                        
                        workouts.Child(workout.WorkoutId.ToString()).Child("Date").SetValue(workout.Date.ToString());

                        foreach(var exercise in workout.Exercises)
                        {
                            workouts.Child(workout.WorkoutId.ToString()).Child(exercise.ExerciseInstanceId.ToString()).Child("ExerciseNumber").SetValue(exercise.ExNumber);
                            workouts.Child(workout.WorkoutId.ToString()).Child(exercise.ExerciseInstanceId.ToString()).Child("Name").SetValue(exercise.ExName);
                            //workouts.Child(workout.WorkoutId.ToString()).Child(exercise.ExerciseInstanceId.ToString()).Child("DbId").SetValue(exercise.ExId);

                            foreach (var set in exercise.SetData)
                            {
                                var dbSetInfo = workouts.Child(workout.WorkoutId.ToString()).Child(exercise.ExerciseInstanceId.ToString()).Child(set.Value.SetId.ToString());
                                dbSetInfo.Child("SetNumber").SetValue(set.Value.SetNumber);
                                dbSetInfo.Child("Weight").SetValue(set.Value.Weight);
                                dbSetInfo.Child("Reps").SetValue(set.Value.Reps);
                                dbSetInfo.Child("Rpe").SetValue(set.Value.Rpe);
                                dbSetInfo.Child("Notes").SetValue(set.Value.Notes);
                            }
                        }                        
                    }
                    catch (Exception ex)
                    {
                        Log.Debug(TAG, ex.ToString());
                    }

                    foreach (var item in Workout.Exercises)
                    {
                        Log.Debug(TAG, "Numsets for " + item.ExName + ": " + item.SetData.Count);
                    }
                }));
            }

        }

    }
}