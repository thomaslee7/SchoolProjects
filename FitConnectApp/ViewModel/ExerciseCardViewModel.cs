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
        private RelayCommand deleteExercise;
        private Action<Guid> removeCardFromActivity;
        private RelayCommand<Guid> deleteSet;

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

        public Action<Guid> RemoveCardFromActivity
        {
            get
            {
                return removeCardFromActivity;
            }
            set
            {
                Set(() => RemoveCardFromActivity, ref removeCardFromActivity, value);
            }
        }

        public ExerciseCardViewModel(int id, string name, ExerciseData data = null)
        {
            if(data != null)
            {
                ExData = data;
                Log.Debug(TAG, "Loading data for " + ExData.ExName + ": " + ExData.ExNumber);
                return;
            }

            var exerciseList = App.Locator.CreateWorkout.Workout.Exercises;
            ExData = new ExerciseData { ExName = name, ExNumber = exerciseList.Count + 1 };            
            exerciseList.Add(ExData);
            Log.Debug(TAG, "Exnumber for " + ExData.ExName + ": " + ExData.ExNumber);            
        }

        public RelayCommand<ExerciseSetData> SaveSetData
        {
            get
            {
                return saveSetData ?? (saveSetData = new RelayCommand<ExerciseSetData>((set) => 
                {
                    ExData.SetData.Add(set);
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

        public RelayCommand DeleteExercise
        {
            get
            {
                return deleteExercise ?? (deleteExercise = new RelayCommand( () =>
                {
                    var exList = App.Locator.CreateWorkout.Workout.Exercises;
                    
                    exList.Remove(exData);
                    if (RemoveCardFromActivity != null)
                        RemoveCardFromActivity(exData.ExerciseInstanceId);
                }));
            }
        }

        public RelayCommand<Guid> DeleteSet
        {
            get
            {
                return deleteSet ?? (deleteSet = new RelayCommand<Guid>((setId) =>
                {
                    var setToRemove = exData.SetData.Find(set => set.SetId == setId);
                    int index = exData.SetData.IndexOf(setToRemove);
                    for(int i = index +1; i < exData.SetData.Count; i++)
                    {
                        exData.SetData[i].SetNumber--;
                    }

                    exData.SetData.Remove(setToRemove);
                    
                }));
            }
        }
    }
}