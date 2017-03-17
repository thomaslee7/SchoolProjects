using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using FitConnectApp.ViewModel;
using GalaSoft.MvvmLight.Helpers;
using FitConnectApp.Models;

namespace FitConnectApp.Activities.WorkoutActivities
{
    public class ExerciseCardFragment : Fragment
    {
        private readonly List<Binding> bindings = new List<Binding>();
        //private ExerciseData exData;
        public ExerciseCardViewModel Vm { get; set; }     
        
        public TextView ExName { get; set; }
        public Spinner RpeSpinner { get; set; }
        public Button Done { get; set; }
        public Button Note { get; set; }
        public EditText Weight { get; set; }
        public EditText Reps { get; set; }        
        public TableLayout Table { get; set; }
        public string SetNotes { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);            
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment            
            Vm = new ExerciseCardViewModel(App.Locator.ExerciseSelect.SelectedExerciseId, App.Locator.ExerciseSelect.SelectedExerciseName);
            
            var view = inflater.Inflate(Resource.Layout.ExerciseCard, container, false);

            ExName = view.FindViewById<TextView>(Resource.Id.ExerciseName);
            Table = view.FindViewById<TableLayout>(Resource.Id.exDataTable);

            RpeSpinner = view.FindViewById<Spinner>(Resource.Id.rpeSpinner);
            Weight = view.FindViewById<EditText>(Resource.Id.weight);
            Reps = view.FindViewById<EditText>(Resource.Id.reps);
            Note = view.FindViewById<Button>(Resource.Id.addNote);
            Done = view.FindViewById<Button>(Resource.Id.addSet);
            Note.Click += Note_Click;
            Done.Click += Done_Click;
            
            bindings.Add(this.SetBinding(() => Vm.ExData.ExName, () => ExName.Text));
            
            return view;            
        }

        private void Note_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this.Context);
            alert.SetTitle("Add notes to this set");
            LinearLayout layout = new LinearLayout(this.Context);
            EditText text = new EditText(this.Context);

            LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent, 1f);
            text.LayoutParameters = lp;
            text.Text = SetNotes;
            layout.AddView(text);

            alert.SetView(layout);
            alert.SetPositiveButton("Save", 
                (senderAlert, args) =>
                {
                    SetNotes = text.Text;
                });
            alert.SetNegativeButton("Cancel", (senderAlert, args) =>
            {
                
            });

            alert.SetNeutralButton("Delete", (senderAlert, args) =>
            {
                SetNotes = "";
            });
                        
            alert.Show();
        }

        private void Done_Click(object sender, EventArgs e)
        {
            int w;
            bool validWeight = int.TryParse(Weight.Text, out w);

            int r;
            bool validReps = int.TryParse(Reps.Text, out r);

            int rpe;
            bool validRpe = int.TryParse(RpeSpinner.SelectedItem.ToString(), out rpe);

            ExerciseSetData set = new ExerciseSetData
            {
                Weight = (validWeight ? w : 0),
                Reps = (validReps ? r : 0),
                Notes = SetNotes,
                Rpe = (validRpe ? rpe : 0)
            };

            Vm.SaveSetData.Execute(set);
            //var ExerciseList = App.Locator.CreateWorkout.Workout.Exercises;

            //var exercise = ExerciseList.Where(ex => ex.Value.ExId == Vm.ExData.ExId).FirstOrDefault().Value;

            //if (exercise != null)
            //{
            //    exercise.SetData.Add(exercise.SetData.Count +1, set);
            //}
            //else
            //{
            //    var exdata = new ExerciseData();
            //    exdata.SetData.Add(1, set);
            //    ExerciseList.Add(ExerciseList.Count + 1, exdata);
            //}
            
            TableRow row = new TableRow(this.Context);

            var lp = new TableRow.LayoutParams(TableRow.LayoutParams.MatchParent, TableRow.LayoutParams.MatchParent);
            row.LayoutParameters = lp;

            var tvlp = new TableRow.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 1f);

            var setTv = new TextView(this.Context);
            setTv.Text = (Vm.ExData.SetData.Count + 1).ToString();
            setTv.LayoutParameters = tvlp;
            setTv.Gravity = GravityFlags.Center;

            var weightTv = new TextView(this.Context);
            weightTv.Text = set.Weight.ToString();
            weightTv.LayoutParameters = tvlp;
            weightTv.Gravity = GravityFlags.Center;

            var repsTv = new TextView(this.Context);
            repsTv.Text = set.Reps.ToString();
            repsTv.LayoutParameters = tvlp;
            repsTv.Gravity = GravityFlags.Center;

            var rpeTv = new TextView(this.Context);
            rpeTv.Text = set.Rpe.ToString();
            rpeTv.LayoutParameters = tvlp;
            rpeTv.Gravity = GravityFlags.Center;

            var notesTv = new TextView(this.Context);
            notesTv.Text = (string.IsNullOrWhiteSpace(SetNotes) ? "N/A" : "Note");
            notesTv.LayoutParameters = tvlp;
            notesTv.Gravity = GravityFlags.Center;

            row.AddView(setTv);
            row.AddView(weightTv);
            row.AddView(repsTv);
            row.AddView(rpeTv);
            row.AddView(notesTv);
            Table.AddView(row);            

            SetNotes = "";
        }
    }
}