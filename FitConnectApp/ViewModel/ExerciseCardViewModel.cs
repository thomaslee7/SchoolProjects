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

namespace FitConnectApp.ViewModel
{
    public class ExerciseCardViewModel : ViewModelBase
    {
        private ExerciseData exData;
        private RelayCommand<ExerciseSetData> saveSetData;

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
            ExData = new ExerciseData { ExName = name ?? "TEST! " + Guid.NewGuid().ToString() };
            exerciseList.Add(exerciseList.Count, ExData);
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
    }
}