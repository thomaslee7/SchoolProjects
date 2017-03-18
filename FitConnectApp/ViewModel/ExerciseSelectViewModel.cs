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
using FitConnectApp.Activities.WorkoutActivities;
using Android.Util;

namespace FitConnectApp.ViewModel
{
    public class ExerciseSelectViewModel : ViewModelBase
    {
        private const string TAG = "ExerciseSelectViewModel";

        private RelayCommand<ExerciseSelectFragment> dismiss;
        private RelayCommand<int> setExerciseType;
        private RelayCommand<int> setMuscleGroup;
        private RelayCommand<Tuple<int, string>> setExerciseName;

        private int selectedExerciseTypeId;
        private int selectedMuscleGroupId;
        private int selectedExerciseId;
        private string selectedExerciseName;
        public int SelectedExerciseTypeId
        {
            get
            {
                return selectedExerciseTypeId;
            }
            set
            {
                Set(() => SelectedExerciseTypeId, ref selectedExerciseTypeId, value);
            }
        }

        public string SelectedExerciseName
        {
            get
            {
                return selectedExerciseName;
            }
            set
            {
                Set(() => SelectedExerciseName, ref selectedExerciseName, value);
            }
        }

        public int SelectedMuscleGroupId
        {
            get
            {
                return selectedMuscleGroupId;
            }
            set
            {
                Set(() => SelectedMuscleGroupId, ref selectedMuscleGroupId, value);
            }
        }

        public int SelectedExerciseId
        {
            get
            {
                return selectedExerciseId;
            }
            set
            {
                Set(() => SelectedExerciseId, ref selectedExerciseId, value);
            }
        }

        public RelayCommand<ExerciseSelectFragment> Dismiss
        {
            get
            {
                return dismiss ?? (dismiss = new RelayCommand<ExerciseSelectFragment>((fragment) => {
                    fragment.Dismiss();

                    try
                    {
                        Log.Debug(TAG, "Adding exercise card");
                        ExerciseCardFragment card = new ExerciseCardFragment();

                        Bundle bundle = new Bundle();
                        bundle.PutString("exname", App.Locator.ExerciseSelect.SelectedExerciseName);
                        bundle.PutInt("exid", App.Locator.ExerciseSelect.SelectedExerciseId);

                        Guid exercisetag = new Guid();
                        App.Locator.CreateWorkout.ExerciseTags.Add(exercisetag);

                        card.Arguments = bundle;
                        FragmentTransaction tx = fragment.FragmentManager.BeginTransaction();
                        tx.Add(Resource.Id.exerciseCardsFrame, card, exercisetag.ToString());
                        //tx.AddToBackStack(null);
                        tx.Commit();

                    }
                    catch (Exception ex)
                    {
                        Log.Error(TAG, ex.ToString());
                    }
                }));
            }
        }

        public RelayCommand<int> SetExerciseType
        {
            get
            {
                return setExerciseType ?? (setExerciseType = new RelayCommand<int>((exTypeId) =>
                {
                    SelectedExerciseTypeId = exTypeId;
                }));
            }
        }

        public RelayCommand<int> SetMuscleGroup
        {
            get
            {
                return setMuscleGroup ?? (setMuscleGroup = new RelayCommand<int>(
                    (muscleGroupId) =>
                    {
                        SelectedMuscleGroupId = muscleGroupId;
                    },
                    (exTypeId) => { return SelectedExerciseTypeId != 0; })); //if true, this command is enabled. If false, it is disabled.
            }
        }

        public RelayCommand<Tuple<int, string>> SetExerciseName
        {
            get
            {
                return setExerciseName ?? (setExerciseName = new RelayCommand<Tuple<int, string>>((exercise) => {
                    SelectedExerciseId = exercise.Item1;
                    SelectedExerciseName = exercise.Item2.ToString();
                },
                (muscleGroupId) => { return SelectedMuscleGroupId != 0; }
                ));
            }
        }

    }
}