using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using FitConnectApp.ViewModel;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Views;
using Android.App;

namespace FitConnectApp.Activities.WorkoutActivities
{
    [Activity(Label = "Create Workout")]
    public class CreateWorkoutActivity : ActivityBase
    {
        const string TAG = "CreateWorkout";
        private Button addExercise;

        public Button AddExercise => addExercise ?? (addExercise = FindViewById<Button>(Resource.Id.AddExercise));
        private CreateWorkoutViewModel Vm => App.Locator.CreateWorkout;      

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                // Create your fragment here            

                SetContentView(Resource.Layout.CreateWorkoutScreen);
                //AddExercise.SetCommand(eventName: "Click", command: Vm.AddExercise, commandParameter: FragmentManager);
                AddExercise.Click += AddExerciseCard;                
            }
            catch (Exception ex)
            {
                Log.Error(TAG, ex.ToString());                
            }
        }

        public void AddExerciseCard(object sender, EventArgs e)
        {
            Vm.AddExercise.Execute(FragmentManager);
            //if (FindViewById(Resource.Id.exerciseCardsFrame) != null)
            //{
            //    Log.Debug(TAG, "Adding exercise card");                
            //    ExerciseCardFragment card = new ExerciseCardFragment();

            //    Bundle bundle = new Bundle();
            //    bundle.PutString("exname", App.Locator.ExerciseSelect.SelectedExerciseName);
            //    bundle.PutInt("exid", App.Locator.ExerciseSelect.SelectedExerciseId);

            //    card.Arguments.PutBundle("exdata", bundle);
            //    FragmentManager.BeginTransaction().Add(Resource.Id.exerciseCardsFrame, card);

            //}
        }
    }
}