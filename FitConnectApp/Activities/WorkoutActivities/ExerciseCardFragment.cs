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
using System.Reflection;
using Android.Text;

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

        private View view;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);            
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment            
            Vm = new ExerciseCardViewModel(App.Locator.ExerciseSelect.SelectedExerciseId, App.Locator.ExerciseSelect.SelectedExerciseName);
            
            view = inflater.Inflate(Resource.Layout.ExerciseCard, container, false);

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
            alert.SetPositiveButton("Save", (senderAlert, args) =>{
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
            ExerciseSetData set = GetSetDataFromControls();

            Vm.SaveSetData.Execute(set);

            Log.Debug("Doneclick", "Set guid: " + set.SetId.ToString());

            TableRow row = new TableRow(this.Context);
            row.Tag = set.SetId.ToString();
            var lp = new TableRow.LayoutParams(TableRow.LayoutParams.MatchParent, TableRow.LayoutParams.MatchParent);
            row.LayoutParameters = lp;

            var tvlp = new TableRow.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 1f);

            var setTv = new TextView(this.Context);
            setTv.Text = (Vm.ExData.SetData.Count).ToString();
            setTv.LayoutParameters = tvlp;
            setTv.Gravity = GravityFlags.Center;
            
            var weightTv = CreateTextView(InputTypes.ClassNumber, set.Weight.ToString(), nameof(set.Weight));
            //    new TextView(this.Context);
            //weightTv.Text = set.Weight.ToString();
            //weightTv.LayoutParameters = tvlp;
            //weightTv.Gravity = GravityFlags.Center;
            //weightTv.Tag = nameof(set.Weight);
            //weightTv.Click += EditCell;
            //weightTv.SetRawInputType(InputTypes.NumberFlagDecimal);

            Binding<double, string> weight = new Binding<double, string>(set, nameof(set.Weight), weightTv, nameof(weightTv.Text), BindingMode.TwoWay);
            bindings.Add(weight);

            var repsTv = CreateTextView(InputTypes.ClassNumber, set.Reps.ToString(), nameof(set.Reps));
            //    new TextView(this.Context);
            //repsTv.Tag = nameof(set.Reps);
            //repsTv.Text = set.Reps.ToString();
            //repsTv.LayoutParameters = tvlp;
            //repsTv.Gravity = GravityFlags.Center;
            //repsTv.Click += EditCell;
            //repsTv.SetRawInputType(InputTypes.NumberFlagDecimal);

            Binding<int, string> reps = new Binding<int, string>(set, nameof(set.Reps), repsTv, nameof(repsTv.Text), BindingMode.TwoWay);
            bindings.Add(reps);
            
            var rpeTv = CreateTextView(InputTypes.ClassNumber, set.Rpe.ToString(), nameof(set.Rpe));
            //    = new TextView(this.Context);
            //rpeTv.Text = set.Rpe.ToString();
            //rpeTv.LayoutParameters = tvlp;
            //rpeTv.Gravity = GravityFlags.Center;
            //rpeTv.Tag = nameof(set.Rpe);
            //rpeTv.Click += EditCell;
            Binding<int, string> rpe = new Binding<int, string>(set, nameof(set.Rpe), rpeTv, nameof(rpeTv.Text), BindingMode.TwoWay);
            bindings.Add(rpe);

            var notesTv = CreateTextView(InputTypes.ClassText, (string.IsNullOrWhiteSpace(SetNotes) ? "N/A" : "Note"), nameof(set.Notes));
            //    new TextView(this.Context);
            //notesTv.Text = (string.IsNullOrWhiteSpace(SetNotes) ? "N/A" : "Note");
            //notesTv.LayoutParameters = tvlp;
            //notesTv.Gravity = GravityFlags.Center;
            //notesTv.Tag = nameof(set.Notes);
            //notesTv.Click += EditCell;


            row.AddView(setTv);
            row.AddView(weightTv);
            row.AddView(repsTv);
            row.AddView(rpeTv);
            row.AddView(notesTv);
            Table.AddView(row);            

            SetNotes = "";
        }
        
        private TextView CreateTextView(InputTypes celltype, string data, string tag)
        {
            var tvlp = new TableRow.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 1f);

            var view = new TextView(this.Context);
            view.Text = data;
            view.LayoutParameters = tvlp;
            view.Gravity = GravityFlags.Center;
            view.Tag = tag;
            view.Click += EditCell;
            view.SetRawInputType(celltype);

            return view;
        }

        private ExerciseSetData GetSetDataFromControls()
        {            
            bool validWeight = int.TryParse(Weight.Text, out int w);

            bool validReps = int.TryParse(Reps.Text, out int r);

            bool validRpe = int.TryParse(RpeSpinner.SelectedItem.ToString(), out int rpe);

            ExerciseSetData set = new ExerciseSetData
            {
                Weight = (validWeight ? w : 0),
                Reps = (validReps ? r : 0),
                Notes = SetNotes,
                Rpe = (validRpe ? rpe : 0)
            };

            return set;
        }

        private ExerciseSetData GetSetDataFromTable(Java.Lang.Object rowTag)
        {
            return Vm.ExData.SetData.Where(ex => ex.Value.SetId.ToString() == rowTag.ToString()).Select(kvp => kvp.Value).FirstOrDefault();
            //Log.Debug("GETDATA", set.Weight + " x " + set.Reps + "; id: " + set.SetId);            
        }

        private void EditCell(object sender, EventArgs e)
        {
            var cell = (TextView)sender;
            var parentRow = (TableRow)cell.Parent;
            //Log.Debug("EditCell", "row guid: " + parentRow.Tag.ToString());

            ExerciseSetData set = GetSetDataFromTable(parentRow.Tag);

            AlertDialog.Builder alert = new AlertDialog.Builder(this.Context);
            alert.SetTitle("Edit " + cell.Tag);
            LinearLayout layout = new LinearLayout(this.Context);
            EditText text = new EditText(this.Context);

            LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent, 1f);
            text.LayoutParameters = lp;
            
            text.Text = cell.Text;
            layout.AddView(text);

            alert.SetView(layout);
            alert.SetPositiveButton("Save", (senderAlert, args) => {
                if(set != null)
                {
                    SetPropertyValue(set, cell.Tag.ToString(), text.Text);
                    
                }
            });

            alert.SetNegativeButton("Cancel", (senderAlert, args) =>
            {

            });

            //alert.SetNeutralButton("Delete", (senderAlert, args) =>
            //{
                
            //});

            alert.Show();
        }

        //private void Edit_Notes(object sender, EventArgs e)
        //{
        //    var cell = (TextView)sender;
        //    var parentRow = (TableRow)cell.Parent;            

        //    ExerciseSetData set = GetSetDataFromTable(parentRow.Tag);

        //    AlertDialog.Builder alert = new AlertDialog.Builder(this.Context);
        //    alert.SetTitle("Edit " + cell.Tag);
        //    LinearLayout layout = new LinearLayout(this.Context);
        //    EditText text = new EditText(this.Context);

        //    LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent, 1f);
        //    text.LayoutParameters = lp;

        //    text.Text = cell.Text;
        //    layout.AddView(text);

        //    alert.SetView(layout);
        //    alert.SetPositiveButton("Save", (senderAlert, args) => {
        //        if (set != null)
        //        {
        //            SetPropertyValue(set, cell.Tag.ToString(), text.Text);

        //        }
        //    });

        //    alert.SetNegativeButton("Cancel", (senderAlert, args) =>
        //    {

        //    });

        //    //alert.SetNeutralButton("Delete", (senderAlert, args) =>
        //    //{

        //    //});

        //    alert.Show();
        //}

        void SetPropertyValue(object instance, string propertyName, object value)
        {
            Type type = instance.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propertyName);
            if(propertyInfo.PropertyType == typeof(double))
                propertyInfo.SetValue(instance, double.Parse(value.ToString()));            
            else if (propertyInfo.PropertyType == typeof(int))
                propertyInfo.SetValue(instance, int.Parse(value.ToString()));
            else if(propertyInfo.PropertyType == typeof(string))
                propertyInfo.SetValue(instance, value);
        }
        
    }
}