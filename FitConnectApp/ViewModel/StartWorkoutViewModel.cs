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
using Firebase.Database;
using FitConnectApp.Activities.WorkoutActivities.Listeners;
using System.Reflection;

namespace FitConnectApp.ViewModel
{
    public class StartWorkoutViewModel : ViewModelBase
    {
        private RelayCommand _startNewWorkout;
        private RelayCommand<Tuple<Guid, string>> loadWorkout;
        private INavigationService _navService;
        private WorkoutData savedWorkoutData;
        private const string TAG = "StartWorkoutViewModel";

        public StartWorkoutViewModel(INavigationService navService)
        {
            _navService = navService;
        }

        public RelayCommand StartNewWorkout
        {
            get
            {
                return _startNewWorkout ??
                    (_startNewWorkout = new RelayCommand(() => {
                        App.Locator.CreateWorkout.Workout = new WorkoutData();
                        _navService.NavigateTo(ViewModelLocator.CreateWorkoutKey);

                    }));
            }
        }

        public RelayCommand<Tuple<Guid, string>> LoadWorkout
        {
            get
            {             
                return loadWorkout ??
                    (loadWorkout = new RelayCommand<Tuple<Guid,string>>((data) => 
                    {                        
                        App.Locator.CreateWorkout.Workout = loadWorkoutData(data.Item1, data.Item2);
                        _navService.NavigateTo(ViewModelLocator.CreateWorkoutKey);
                    }));
            }
        }

        private WorkoutData loadWorkoutData(Guid workoutId, string uid)
        {
            var db = FirebaseDatabase.GetInstance(App.fbApp);
            savedWorkoutData = new WorkoutData { WorkoutId = workoutId };

            db.GetReference("Workouts").Child(uid).Child(workoutId.ToString()).AddValueEventListener(new ValueEventListener(GetWorkoutData));

            return savedWorkoutData;
        }

        public void GetWorkoutData(DataSnapshot snapshot)
        {
            savedWorkoutData.Date = DateTime.Parse(snapshot.Child("Date").GetValue(true).ToString());

            try
            {

                //int i = 0;
                foreach (DataSnapshot snap in snapshot.Children.ToEnumerable())
                {
                    if(snap.ChildrenCount > 0)
                    {
                        var ex = new ExerciseData
                        {
                            //ExerciseInstanceId = Guid.Parse(snap.Key),
                            ExName = snap.Child("Name").GetValue(true).ToString(),                        
                            ExNumber = int.Parse(snap.Child("ExerciseNumber").GetValue(true).ToString())
                        };

                        int j = 0;
                        Log.Debug(TAG, ex.ExName);
                        foreach(DataSnapshot setSnap in snap.Children.ToEnumerable())
                        {
                            var set = new ExerciseSetData();
                            if(setSnap.ChildrenCount > 0)
                            {
                                set.Weight = int.Parse(setSnap.Child("Weight").GetValue(true).ToString());
                                set.Reps = int.Parse(setSnap.Child("Reps").GetValue(true).ToString());
                                set.Rpe = int.Parse(setSnap.Child("Rpe").GetValue(true).ToString());
                                set.SetNumber = int.Parse(setSnap.Child("SetNumber").GetValue(true).ToString());
                                set.Notes = setSnap.Child("Notes")?.GetValue(true)?.ToString() ?? "";                            

                                ex.SetData.Add(j, set);
                                Log.Debug(TAG, set.Weight.ToString() + " " + set.Reps.ToString() + " " + set.Rpe.ToString() + "; #" + set.SetNumber);
                                j++;
                            }                            
                        }

                        savedWorkoutData.Exercises.Add(ex);
                    }

                }
                
            }
            catch (Exception ex)
            {
                Log.Debug(TAG, ex.ToString());
            }

           
            //var workouts = db.GetReference("Workouts").Child(uid);
            //workouts.Child(workout.WorkoutId.ToString()).Child("Date").SetValue(workout.Date.ToString());

            //foreach (var exercise in workout.Exercises)
            //{
            //    workouts.Child(workout.WorkoutId.ToString()).Child(exercise.ExerciseInstanceId.ToString()).Child("ExerciseNumber").SetValue(exercise.ExNumber);
            //    workouts.Child(workout.WorkoutId.ToString()).Child(exercise.ExerciseInstanceId.ToString()).Child("Name").SetValue(exercise.ExName);
            //    //workouts.Child(workout.WorkoutId.ToString()).Child(exercise.ExerciseInstanceId.ToString()).Child("DbId").SetValue(exercise.ExId);

            //    foreach (var set in exercise.SetData)
            //    {
            //        var dbSetInfo = workouts.Child(workout.WorkoutId.ToString()).Child(exercise.ExerciseInstanceId.ToString()).Child(set.Value.SetId.ToString());
            //        dbSetInfo.Child("SetNumber").SetValue(set.Value.SetNumber);
            //        dbSetInfo.Child("Weight").SetValue(set.Value.Weight);
            //        dbSetInfo.Child("Reps").SetValue(set.Value.Reps);
            //        dbSetInfo.Child("Rpe").SetValue(set.Value.Rpe);
            //        dbSetInfo.Child("Notes").SetValue(set.Value.Notes);
            //    }
            //}
        }
        //private void SetPropertyValue(object instance, string propertyName, object value)
        //{
        //    Type type = instance.GetType();
        //    PropertyInfo propertyInfo = type.GetProperty(propertyName);
        //    if (propertyInfo.PropertyType == typeof(double))
        //        propertyInfo.SetValue(instance, double.Parse(value.ToString()));
        //    else if (propertyInfo.PropertyType == typeof(int))
        //        propertyInfo.SetValue(instance, int.Parse(value.ToString()));
        //    else if (propertyInfo.PropertyType == typeof(string))
        //        propertyInfo.SetValue(instance, value);
        //}
    }
}