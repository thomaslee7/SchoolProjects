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
using FitConnectApp.Models;
using GalaSoft.MvvmLight.Command;
using FitConnectApp.Activities.WorkoutActivities;
using GalaSoft.MvvmLight.Helpers;
using Android.Util;

namespace FitConnectApp.ViewModel
{
    public class ExerciseCardViewModel : ViewModelBase
    {
        private const string TAG = "ExerciseCardViewModel";
        private ExerciseData exData;
        private RelayCommand<ExerciseSetData> saveSetData;
        private RelayCommand<FragmentManager> _editExercise;

        public ExerciseData ExData
        {
            get
            {
                return exData;
            }
            set
            {
                Set(() => ExData, ref exData, value);
            }
        }

        public ExerciseCardViewModel(int id, string name)
        {
            var exerciseList = App.Locator.CreateWorkout.Workout.Exercises;
            ExData = new ExerciseData { ExName = name ?? "TEST! " + Guid.NewGuid().ToString(), ExNumber = exerciseList.Count + 1 };
            exerciseList.Add(exerciseList.Count, ExData);
            Log.Debug(TAG, "Exnumber for " + ExData.ExName + ": " + ExData.ExNumber);
            //Binding<int, int> b = new Binding<int, int>(ExData, nameof(ExData.ExNumber), App.Locator.CreateWorkout.Workout.Exercises)
        }

        public RelayCommand<ExerciseSetData> SaveSetData
        {
            get
            {
                return saveSetData ?? (saveSetData = new RelayCommand<ExerciseSetData>((set) => 
                {
                    ExData.SetData.Add(ExData.SetData.Count, set);
                }));
            }
        }

        public RelayCommand<FragmentManager> EditExercise
        {
            get
            {
                return _editExercise ?? (_editExercise = new RelayCommand<FragmentManager>((manager) =>
                {
                    var transaction = manager.BeginTransaction();
                    ExerciseSelectFragment addExerciseFragment = new ExerciseSelectFragment { Editing = true, ExCardVm = this };
                    addExerciseFragment.Show(transaction, "Add new exercise");                    
                    
                }));
            }
        }
    }
}