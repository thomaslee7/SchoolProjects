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
using FitConnectApp.ViewModel;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Views;
using Android.App;
using GalaSoft.MvvmLight.Messaging;
using FitConnectApp.Models;

namespace FitConnectApp.Activities.WorkoutActivities
{
    [Activity(Label = "Create Workout")]
    public class CreateWorkoutActivity : ActivityBase, View.IOnDragListener
    {
        const string TAG = "CreateWorkout";
        private Button addExercise;
        private Button saveWorkout;
        private LinearLayout exerciseCardsFrame;
        private DragMessage dragMessage;        

        public Button AddExercise => addExercise ?? (addExercise = FindViewById<Button>(Resource.Id.AddExercise));
        public Button SaveWorkout => saveWorkout ?? (saveWorkout = FindViewById<Button>(Resource.Id.saveWorkoutBtn));
        public LinearLayout ExerciseCardsFrame => exerciseCardsFrame ?? (exerciseCardsFrame = FindViewById<LinearLayout>(Resource.Id.exerciseCardsFrame));
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

                ExerciseCardsFrame.SetOnDragListener(this);

                GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<DragMessage>(this, (msg) => {
                    dragMessage = msg;
                });

                GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<DropMessage>(this, (msg) => {
                    ReorderDroppedCard(msg);
                });
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

        public bool OnDrag(View v, DragEvent e)
        {
            if(dragMessage != null)
            {
                switch(e.Action)
                {
                    case DragAction.Drop:
                        try
                        {
                            var card = ExerciseCardsFrame.GetChildAt(dragMessage.Order -1);
                            ExerciseCardsFrame.RemoveViewAt(dragMessage.Order -1);
                            ExerciseCardsFrame.AddView(card);
                            UpdateCardOrderNumbers();

                        }
                        catch (Exception ex)
                        {
                            Log.Debug(TAG, ex.ToString());
                            
                        }
                        break;

                    default:            
                        break;
                }
            }
            
            return true;
        }

        private void ReorderDroppedCard(DropMessage msg)
        {
            if (dragMessage != null)
            {
                var card = ExerciseCardsFrame.GetChildAt(dragMessage.Order - 1);
                ExerciseCardsFrame.RemoveViewAt(dragMessage.Order - 1);
                ExerciseCardsFrame.AddView(card, msg.Order - 1);
                UpdateCardOrderNumbers();
            }
        }

        private void UpdateCardOrderNumbers()
        {
            for (int i = 0; i < exerciseCardsFrame.ChildCount; i++)
            {
                var cardId = Guid.Parse(ExerciseCardsFrame.GetChildAt(i).Tag.ToString());
                
                DragMessage msg = new DragMessage { Id = cardId, Order = i + 1 };
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<DragMessage, ExerciseCardFragment>(msg);
            }
        }
    }
}