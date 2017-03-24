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

namespace FitConnectApp.Models
{
    public class WorkoutData : ObservableObject
    {
        private Guid workoutId;
        private DateTime date;
        private string workoutNotes;
        private Dictionary<int, ExerciseData> exercises;

        public Guid WorkoutId
        {
            get
            {
                return workoutId;
            }
            set
            {
                Set(() => WorkoutId, ref workoutId, value);
            }
        }

        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                Set(() => Date, ref date, value);
            }
        }

        public string WorkoutNotes
        {
            get
            {
                return workoutNotes;
            }
            set
            {
                Set(() => WorkoutNotes, ref workoutNotes, value);
            }
        }

        public Dictionary<int, ExerciseData> Exercises
        {
            get
            {
                return exercises;
            }
            set
            {
                Set(() => Exercises, ref exercises, value);
            }
        }

        public WorkoutData()
        {
            WorkoutId = Guid.NewGuid();
            Exercises = new Dictionary<int, ExerciseData>();
        }
    }

    public class ExerciseData : ObservableObject
    {
        private string exName;
        private int exNumber; //the order of the exercise
        private int exId; //currently the database ID of the exercise
        private Guid exerciseInstanceId;
        private Dictionary<int, ExerciseSetData> setData;

        public ExerciseData()
        {            
            SetData = new Dictionary<int, ExerciseSetData>();
            ExerciseInstanceId = Guid.NewGuid();
        }

        public string ExName
        {
            get
            {
                return exName;
            }
            set
            {
                Set(() => ExName, ref exName, value);
            }
        }
        
        public int ExNumber
        {
            get
            {
                return exNumber;
            }
            set
            {
                Set(() => ExNumber, ref exNumber, value);
            }
        }

        public int ExId
        {
            get
            {
                return exId;
            }
            set
            {
                Set(() => ExId, ref exId, value);
            }
        }

        public Guid ExerciseInstanceId
        {
            get
            {
                return exerciseInstanceId;
            }
            set
            {
                Set(() => ExerciseInstanceId, ref exerciseInstanceId, value);
            }
        }

        public Dictionary<int, ExerciseSetData> SetData
        {
            get
            {
                return setData; 
            }
            set
            {
                Set(() => SetData, ref setData, value);
            }
        }
    }

    public class ExerciseSetData : ObservableObject
    {
        private double weight;
        private int reps;
        private int rpe;
        private string notes;
        private int setNum;
        private Guid setId;

        public ExerciseSetData()
        {
            SetId = Guid.NewGuid();
        }

        public Guid SetId
        {
            get
            {
                return setId;
            }
            set
            {
                Set(() => SetId, ref setId, value);
            }
        }

        public int SetNumber
        {
            get
            {
                return setNum;
            }
            set
            {
                Set(() => SetNumber, ref setNum, value);
            }
        }

        public double Weight
        {
            get
            {
                return weight;
            }
            set
            {
                Set(() => Weight, ref weight, value);
            }
        }
        public int Reps
        {
            get
            {
                return reps;
            }
            set
            {
                Set(() => Reps, ref reps, value);
            }
        }
        public int Rpe
        {
            get
            {
                return rpe;
            }
            set
            {
                Set(() => Rpe, ref rpe, value);
            }
        }
        public string Notes
        {
            get
            {
                return notes;
            }
            set
            {
                Set(() => Notes, ref notes, value);
            }
        }
    }
}