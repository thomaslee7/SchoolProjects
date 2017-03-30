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
    [Activity(Label = "Create Workout", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
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
        private Dictionary<Guid, ExerciseCardFragment> cards;

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
                Log.Error(TAG, "CreateWorkoutActivity OnCreate");
                Log.Debug(TAG, "FragMan in oncreate: isDestroyed = " + FragmentManager.IsDestroyed);
                Log.Debug(TAG, "LoadExCards IsFinishing: " + IsFinishing + "; IsDestroyed: " + IsDestroyed);
                
                SetContentView(Resource.Layout.CreateWorkoutScreen);
                //AddExercise.SetCommand(eventName: "Click", command: Vm.AddExercise, commandParameter: FragmentManager);
                AddExercise.Click += ShowExerciseSelect;

                try
                {
                    SaveWorkout.SetCommand("Click", Vm.SaveWorkout, this);//this.ApplicationContext
                }
                catch (Exception ex)
                {
                    Log.Error(TAG, ex.ToString());                    
                }
                DateContainer.Click += WorkoutDate_Click;

                Binding b = this.SetBinding(() => Vm.Workout.Date, () => WorkoutDate.Text).ConvertSourceToTarget((date) => { return date.ToShortDateString(); });                
                bindings.Add(b);

                App.Locator.CreateWorkout.AddCardToActivity += AddExerciseCard;
                cards = new Dictionary<Guid, ExerciseCardFragment>();

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

                LoadExerciseCards();

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

        public void ShowExerciseSelect(object sender, EventArgs e)
        {
            //Vm.AddExercise.Execute(FragmentManager);
            var transaction = FragmentManager.BeginTransaction();
            ExerciseSelectFragment addExerciseFragment = new ExerciseSelectFragment();
            addExerciseFragment.Show(transaction, "Add new exercise");
        }

        public void AddExerciseCard()
        {            
            Log.Debug(TAG, "AddExerciseCard IsFinishing: " + IsFinishing + "; IsDestroyed: " + IsDestroyed);
            if(!IsFinishing && !IsDestroyed)
            {
                Log.Debug(TAG, "Adding exercise card");
                ExerciseCardFragment card = new ExerciseCardFragment();
                card.Vm = new ExerciseCardViewModel(App.Locator.ExerciseSelect.SelectedExerciseId, App.Locator.ExerciseSelect.SelectedExerciseName);
                card.Vm.RemoveCardFromActivity += RemoveExerciseCard;

                FragmentTransaction tx = FragmentManager.BeginTransaction();            
                
                Log.Debug(TAG, "FragMan destroyed = " + FragmentManager.IsDestroyed.ToString());                
                tx.Add(Resource.Id.exerciseCardsFrame, card);
                //tx.AddToBackStack(null);
                tx.Commit();

                cards.Add(card.Vm.ExData.ExerciseInstanceId, card);
            }
        }

        private void LoadExerciseCards()
        {   
            Log.Debug(TAG, "LoadExCards IsFinishing: " + IsFinishing + "; IsDestroyed: " + IsDestroyed);
            if (!IsFinishing && !IsDestroyed)
            {
                try
                {
                    for (int i = 0; i < Vm.Workout.Exercises.Count; i++)
                    {
                        var exercise = Vm.Workout.Exercises[i];
                        ExerciseCardFragment card = new ExerciseCardFragment();
                        card.Vm = new ExerciseCardViewModel(exercise.ExId, exercise.ExName, exercise);

                        FragmentTransaction tx = FragmentManager.BeginTransaction();                        
                        tx.Add(Resource.Id.exerciseCardsFrame, card);
                        //tx.AddToBackStack(null);
                        tx.Commit();
                        
                        card.Vm.RemoveCardFromActivity += RemoveExerciseCard;
                        cards.Add(card.Vm.ExData.ExerciseInstanceId, card);

                    }
                }
                catch (Exception ex)
                {
                    Log.Error(TAG, ex.ToString());
                }
            }
        }

        private void RemoveExerciseCard(Guid exerciseInstanceId)
        {
            try
            {
                var tx = FragmentManager.BeginTransaction();
                tx.Remove(cards[exerciseInstanceId]);
                tx.Commit();

                cards.Remove(exerciseInstanceId);

                UpdateCardOrderNumbers();
            }
            catch (Exception ex)
            {
                Log.Error(TAG, ex.ToString());
            }
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
            if (dragMessage != null && !IsDestroyed && !IsFinishing)
            {
                Log.Debug(TAG, "cardframe null = " + (ExerciseCardsFrame == null));
                exerciseCardsFrame = FindViewById<LinearLayout>(Resource.Id.exerciseCardsFrame);
                Log.Debug(TAG, "frame #children: " + exerciseCardsFrame.ChildCount);

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

        protected override void OnDestroy()
        {
            Log.Debug(TAG, "WID: " + Vm.Workout.WorkoutId.ToString() + " on destroy");
                        
            base.OnDestroy();
        }

    }
}