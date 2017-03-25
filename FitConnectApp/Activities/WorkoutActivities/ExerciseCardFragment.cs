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
using Android.Text.Method;
using GalaSoft.MvvmLight.Messaging;
using Android.Graphics;
using FitConnectApp.Services;

namespace FitConnectApp.Activities.WorkoutActivities
{
    public class ExerciseCardFragment : Fragment, View.IOnDragListener, View.IOnTouchListener
    {
        private const string TAG = "ExCardFragment";
        private readonly List<Binding> bindings = new List<Binding>();
        
        public ExerciseCardViewModel Vm { get; set; }     
        
        public TextView ExName { get; set; }
        public Spinner RpeSpinner { get; set; }
        public Button Done { get; set; }
        public Button Note { get; set; }
        public EditText Weight { get; set; }
        public EditText Reps { get; set; }        
        public TextView dragElement { get; set; }
        //public TextView ExNo { get; set; }
        public TableLayout Table { get; set; }
        public LinearLayout FragmentContainer { get; set; }
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
            //ExNo = view.FindViewById<TextView>(Resource.Id.ExNumber);
            dragElement = view.FindViewById<TextView>(Resource.Id.dragElement);
            FragmentContainer = view.FindViewById<LinearLayout>(Resource.Id.fragmentContainer);
            
            Note.Click += Note_Click;
            Done.Click += Done_Click;

            ExName.SetCommand("Click", Vm.EditExercise, this.FragmentManager);

            bindings.Add(this.SetBinding(() => Vm.ExData.ExName, () => ExName.Text));
            //bindings.Add(this.SetBinding(() => Vm.ExData.ExNumber, () => ExNo.Text));

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<DragMessage>(this, (msg) => {
                if(Vm.ExData.ExerciseInstanceId == msg.Id)
                    Vm.ExData.ExNumber = msg.Order;
            });

            FragmentContainer.Tag = Vm.ExData.ExerciseInstanceId.ToString();

            Typeface iconFont = FontManager.getTypeface(this.Context, FontManager.FONTAWESOME);
            FontManager.markAsIconContainer(dragElement, iconFont);

            view.SetOnDragListener(this);
            dragElement.SetOnTouchListener(this);
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

            //Log.Debug("Doneclick", "Set guid: " + set.SetId.ToString());

            TableRow row = new TableRow(this.Context);
            row.Tag = set.SetId.ToString();
            var lp = new TableRow.LayoutParams(TableRow.LayoutParams.MatchParent, TableRow.LayoutParams.MatchParent);
            row.LayoutParameters = lp;

            var tvlp = new TableRow.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 1f);

            var setTv = CreateTextView(set.SetNumber.ToString(), nameof(set.SetNumber));            

            Binding<int, string> setnum = new Binding<int, string>(set, nameof(set.SetNumber), setTv, nameof(setTv.Text), BindingMode.TwoWay);
            bindings.Add(setnum);

            var weightTv = CreateTextView(set.Weight.ToString(), nameof(set.Weight), EditCell);

            Binding<double, string> weight = new Binding<double, string>(set, nameof(set.Weight), weightTv, nameof(weightTv.Text), BindingMode.TwoWay);
            bindings.Add(weight);

            var repsTv = CreateTextView(set.Reps.ToString(), nameof(set.Reps), EditCell);
            repsTv.KeyListener = DigitsKeyListener.GetInstance(false, true);


            Binding<int, string> reps = new Binding<int, string>(set, nameof(set.Reps), repsTv, nameof(repsTv.Text), BindingMode.TwoWay);
            bindings.Add(reps);
            
            var rpeTv = CreateTextView(set.Rpe.ToString(), nameof(set.Rpe), EditCell);

            Binding<int, string> rpe = new Binding<int, string>(set, nameof(set.Rpe), rpeTv, nameof(rpeTv.Text), BindingMode.TwoWay);
            bindings.Add(rpe);

            var notesTv = CreateTextView((string.IsNullOrWhiteSpace(SetNotes) ? "N/A" : "Note"), nameof(set.Notes));
 
            row.AddView(setTv);
            row.AddView(weightTv);
            row.AddView(repsTv);
            row.AddView(rpeTv);
            row.AddView(notesTv);
            Table.AddView(row);            

            SetNotes = "";
        }
        
        private TextView CreateTextView(string data, string tag, EventHandler handler = null)
        {
            var tvlp = new TableRow.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 1f);

            var view = new TextView(this.Context);
            view.Text = data;
            view.LayoutParameters = tvlp;
            view.Gravity = GravityFlags.Center;
            view.Tag = tag;
            view.Click += handler;            
            
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
                Rpe = (validRpe ? rpe : 0),
                SetNumber = Vm.ExData.SetData.Count + 1                
            };

            return set;
        }

        private ExerciseSetData GetSetDataFromTable(Java.Lang.Object rowTag)
        {
            return Vm.ExData.SetData.Where(ex => ex.Value.SetId.ToString() == rowTag.ToString()).Select(kvp => kvp.Value).FirstOrDefault();
        }

        private void EditCell(object sender, EventArgs e)
        {
            var cell = (TextView)sender;
            var parentRow = (TableRow)cell.Parent;

            ExerciseSetData set = GetSetDataFromTable(parentRow.Tag);

            AlertDialog.Builder alert = new AlertDialog.Builder(this.Context);
            alert.SetTitle("Edit set #" + set.SetNumber + " " + cell.Tag);
            LinearLayout layout = new LinearLayout(this.Context);

            EditText text = new EditText(this.Context);
            text.InputType = getCellPropertyType(cell, set);

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

            alert.Show();
        }

        private InputTypes getCellPropertyType(TextView cell, ExerciseSetData set)
        {

            Type type = set.GetType();
            PropertyInfo propertyInfo = type.GetProperty(cell.Tag.ToString());
            if (propertyInfo.PropertyType == typeof(double) || propertyInfo.PropertyType == typeof(int))
                return InputTypes.ClassNumber | InputTypes.NumberFlagDecimal;
            else
                return InputTypes.ClassText;
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

        public bool OnDrag(View v, DragEvent e)
        {
            switch (e.Action)
            {
                case DragAction.Drop:
                    try
                    {
                        Log.Debug(TAG, "Drop Detected on ex: " + Vm.ExData.ExName);
                        var dropMsg = new DropMessage { Id = Vm.ExData.ExerciseInstanceId, Order = Vm.ExData.ExNumber };
                        GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(dropMsg);
                    }
                    catch (Exception ex)
                    {
                        Log.Debug(TAG, ex.ToString());
                    }
                    break;

                default:                    
                    break;
            }
            return true;
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            var dragMessage = new DragMessage { Order = Vm.ExData.ExNumber, Id = Vm.ExData.ExerciseInstanceId };
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(dragMessage);

            ClipData cd = ClipData.NewPlainText("", "");
            View.DragShadowBuilder builder = new View.DragShadowBuilder(view);
            return view.StartDrag(cd, builder, View, 0);             
        }
    }
}