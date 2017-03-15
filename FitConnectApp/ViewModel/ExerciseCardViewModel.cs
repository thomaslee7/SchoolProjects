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

namespace FitConnectApp.ViewModel
{
    public class ExerciseCardViewModel : ViewModelBase
    {
        private string exerciseName;
        private int exerciseId;

        public string ExerciseName
        {
            get
            {
                return exerciseName;
            }
            set
            {
                Set(() => ExerciseName, ref exerciseName, value);
            }
        }

        public int ExerciseId
        {
            get
            {
                return exerciseId;
            }
            set
            {
                Set(() => ExerciseId, ref exerciseId, value);
            }
        }
        public ExerciseCardViewModel(int id, string name)
        {
            ExerciseName = name;
            ExerciseId = id;
        }
    }
}