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
using static Android.App.DatePickerDialog;
using Android.Graphics;
using FitConnectApp.Services;

namespace FitConnectApp.Activities.WorkoutActivities
{
    [Activity(Label = "Create Workout")]
    public class CreateWorkoutActivity : ActivityBase, View.IOnDragListener, IOnDateSetListener
    {
        const string TAG = "CreateWorkout";
        private Button addExercise;
        private Button saveWorkout;
        private LinearLayout exerciseCardsFrame;
        private LinearLayout workoutScreen;
        private LinearLayout dateContainer;
        private DragMessage dragMessage;
        private TextView workoutDate;
        private TextView workoutDateIcon;

        private readonly List<Binding> bindings = new List<Binding>();

        public Button AddExercise => addExercise ?? (addExercise = FindViewById<Button>(Resource.Id.AddExercise));
        public Button SaveWorkout => saveWorkout ?? (saveWorkout = FindViewById<Button>(Resource.Id.saveWorkoutBtn));
        public LinearLayout ExerciseCardsFrame => exerciseCardsFrame ?? (exerciseCardsFrame = FindViewById<LinearLayout>(Resource.Id.exerciseCardsFrame));
        public LinearLayout WorkoutScreen => workoutScreen ?? (workoutScreen = FindViewById<LinearLayout>(Resource.Id.workoutScreen));
        public LinearLayout DateContainer => dateContainer ?? (dateContainer = FindViewById<LinearLayout>(Resource.Id.dateContainer));
        public TextView WorkoutDate => workoutDate ?? (workoutDate= FindViewById<TextView>(Resource.Id.workoutDate));
        public TextView WorkoutDateIcon => workoutDateIcon ?? (workoutDateIcon = FindViewById<TextView>(Resource.Id.workoutDateIcon));
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
                DateContainer.Click += WorkoutDate_Click;
                Binding b = this.SetBinding(() => Vm.Workout.Date, () => WorkoutDate.Text).ConvertSourceToTarget((date) => { return date.ToShortDateString(); });
                
                bindings.Add(b);

                ExerciseCardsFrame.SetOnDragListener(this);

                GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<DragMessage>(this, (msg) => {
                    dragMessage = msg;
                });

                GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<DropMessage>(this, (msg) => {
                    ReorderDroppedCard(msg);
                });
                
                //https://code.tutsplus.com/tutorials/how-to-use-fontawesome-in-an-android-app--cms-24167 this helped get font awesome going.
                Typeface iconFont = FontManager.getTypeface(this, FontManager.FONTAWESOME);
                FontManager.markAsIconContainer(WorkoutDateIcon, iconFont);
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

        private void WorkoutDate_Click(object sender, EventArgs e)
        {
            DatePickerDialog d = new DatePickerDialog(this, this, Vm.Workout.Date.Year, Vm.Workout.Date.Month -1, Vm.Workout.Date.Day );
            d.Show();
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

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            Vm.Workout.Date = new DateTime(year, month+1, dayOfMonth);
        }
    }
}