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
//using Android.Support.V4.App;
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
        private Button saveWorkout;

        public Button AddExercise => addExercise ?? (addExercise = FindViewById<Button>(Resource.Id.AddExercise));
        public Button SaveWorkout => saveWorkout ?? (saveWorkout = FindViewById<Button>(Resource.Id.saveWorkoutBtn));
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
                SaveWorkout.SetCommand(Vm.SaveWorkout);


                // use this to add blank exercise cards
                //ExerciseCardFragment card;
                //FragmentTransaction tx = FragmentManager.BeginTransaction();
                //for (int i = 0; i < 40; i++)
                //{
                //    card = new ExerciseCardFragment();
                //    Guid exercisetag = new Guid();
                //    Log.Debug(TAG, i.ToString());
                //    //App.Locator.CreateWorkout.ExerciseTags.Add(exercisetag);
                //    tx.Add(Resource.Id.exerciseCardsFrame, card, exercisetag.ToString());

                //}
                //tx.Commit();
            }
            catch (Exception ex)
            {
                Log.Error(TAG, ex.ToString());                
            }
        }

        public void AddExerciseCard(object sender, EventArgs e)
        {
            Vm.AddExercise.Execute(FragmentManager);
        }
    }
}