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
using System.Threading.Tasks;

namespace FitConnectApp.ViewModel
{
    public class StartWorkoutViewModel : ViewModelBase
    {
        private const string TAG = "StartWorkoutViewModel";
        private RelayCommand _startNewWorkout;        
        private RelayCommand<LoadWorkout> loadWorkout;
        private INavigationService _navService;
        private WorkoutData savedWorkoutData;        
        private ValueEventListener getData;
        private string userID;

        public StartWorkoutViewModel(INavigationService navService)
        {
            _navService = navService;
            getData = new ValueEventListener(GetWorkoutData);
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
                
        public RelayCommand<LoadWorkout> LoadWorkout
        {
            get
            {             
                return loadWorkout ??                    
                    (loadWorkout = new RelayCommand<LoadWorkout>((data) =>
                    {                        
                        App.Locator.CreateWorkout.Workout = loadWorkoutData(data.SelectedWorkoutId, data.UserId);
                        Log.Debug(TAG, "About to navigate to createworkout");                        
                    }));
            }
        }

        private  WorkoutData loadWorkoutData(Guid workoutId, string uid)
        {
            userID = uid;
            var db = FirebaseDatabase.GetInstance(App.fbApp);
            savedWorkoutData = new WorkoutData { WorkoutId = workoutId };

            db.GetReference("Workouts").Child(uid).Child(workoutId.ToString()).AddValueEventListener(getData);
            
            return savedWorkoutData;
        }

        public void GetWorkoutData(DataSnapshot snapshot)
        {
            savedWorkoutData.Date = DateTime.Parse(snapshot.Child("Date").GetValue(true).ToString());

            try
            {
                foreach (DataSnapshot snap in snapshot.Children.ToEnumerable())
                {
                    if(snap.ChildrenCount > 0)
                    {
                        var ex = new ExerciseData
                        {                            
                            ExName = snap.Child("Name").GetValue(true).ToString(),                        
                            ExNumber = int.Parse(snap.Child("ExerciseNumber").GetValue(true).ToString()),
                            ExerciseInstanceId = Guid.Parse(snap.Key)                            
                        };

                        int j = 0;
                        Log.Debug(TAG, ex.ExName);
                        foreach(DataSnapshot setSnap in snap.Children.ToEnumerable())
                        {
                            var set = new ExerciseSetData();
                            if(setSnap.ChildrenCount > 0)
                            {
                                set.SetId = Guid.Parse(setSnap.Key);
                                set.Weight = int.Parse(setSnap.Child("Weight").GetValue(true).ToString());
                                set.Reps = int.Parse(setSnap.Child("Reps").GetValue(true).ToString());
                                set.Rpe = int.Parse(setSnap.Child("Rpe").GetValue(true).ToString());
                                set.SetNumber = int.Parse(setSnap.Child("SetNumber").GetValue(true).ToString());
                                set.Notes = setSnap.Child("Notes")?.GetValue(true)?.ToString() ?? "";                            

                                ex.SetData.Add(set);
                                Log.Debug(TAG, set.Weight.ToString() + " " + set.Reps.ToString() + " " + set.Rpe.ToString() + "; #" + set.SetNumber);
                                j++;
                            }                            
                        }
                        ex.SetData = ex.SetData.OrderBy(item => item.SetNumber).ToList();
                        savedWorkoutData.Exercises.Add(ex);
                    }

                }
                savedWorkoutData.Exercises = savedWorkoutData.Exercises.OrderBy(item => item.ExNumber).ToList();
            }
            catch (Exception ex)
            {
                Log.Debug(TAG, ex.ToString());
            }

            Log.Debug(TAG, "Done loading workoutData from database");

            var db = FirebaseDatabase.GetInstance(App.fbApp);
            db.GetReference("Workouts").Child(userID).Child(savedWorkoutData.WorkoutId.ToString()).RemoveEventListener(getData);

            _navService.NavigateTo(ViewModelLocator.CreateWorkoutKey);             
        }

    }
}