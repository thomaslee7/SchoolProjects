using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
//using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using FitConnectApp.ViewModel;
using GalaSoft.MvvmLight.Helpers;
using Firebase.Database;
using FitConnectApp.Activities.WorkoutActivities.Listeners;

namespace FitConnectApp.Activities.WorkoutActivities
{
    public class ExerciseSelectFragment : DialogFragment
    {
        private const string TAG = "Exercise Select Fragment";

        public Spinner SelectExType { get; set; }
        public Spinner SelectMuscleGroup { get; set; }
        public Spinner SelectExerciseName { get; set; }
        public Dictionary<int, Tuple<int, string>> ExerciseTypeList { get; set; }
        public Dictionary<int, Tuple<int, string>> MuscleGroupList { get; set; }
        public Dictionary<int, Tuple<int, string>> ExericseList { get; set; }
        private ExerciseSelectViewModel Vm => App.Locator.ExerciseSelect;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view = inflater.Inflate(Resource.Layout.ExerciseSelect, container, false);
            view.FindViewById<Button>(Resource.Id.AddExDone).SetCommand("Click", Vm.Dismiss, this);

            var db = FirebaseDatabase.GetInstance(App.fbApp);

            SelectExType = view.FindViewById<Spinner>(Resource.Id.ExType);
            SelectMuscleGroup = view.FindViewById<Spinner>(Resource.Id.MuscleGroup);
            SelectExerciseName = view.FindViewById<Spinner>(Resource.Id.ExerciseName);

            SelectExType.ItemSelected += exerciseType_ItemSelected;
            SelectMuscleGroup.ItemSelected += muscleGroup_ItemSelected;
            SelectExerciseName.ItemSelected += exerciseName_ItemSelected;

            db.GetReference("ExerciseType").AddValueEventListener(new ValueEventListener(OnExTypeDataChange));
            db.GetReference("MuscleGroup").AddValueEventListener(new ValueEventListener(OnMuscleGroupDataChange));

            return view;

        }


        private void exerciseType_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            //Vm.SelectedExerciseTypeId = ExerciseTypeList[e.Position].Item1;
            Vm.SetExerciseType.Execute(ExerciseTypeList[e.Position].Item1);
        }

        private void muscleGroup_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Vm.SetMuscleGroup.Execute(MuscleGroupList[e.Position].Item1);
            var db = FirebaseDatabase.GetInstance(App.fbApp);
            db.GetReference("Exercises").Child(Vm.SelectedMuscleGroupId.ToString()).AddValueEventListener(new ValueEventListener(OnExerciseDataChange));
        }

        private void exerciseName_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Vm.SetExerciseName.Execute(new Tuple<int, string>(ExericseList[e.Position].Item1, ExericseList[e.Position].Item2));
        }

        public void OnExTypeDataChange(DataSnapshot snapshot)
        {
            ExerciseTypeList = addSpinnerData(snapshot, SelectExType);
        }

        public void OnMuscleGroupDataChange(DataSnapshot snapshot)
        {
            MuscleGroupList = addSpinnerData(snapshot, SelectMuscleGroup);
        }

        public void OnExerciseDataChange(DataSnapshot snapshot)
        {
            ExericseList = addSpinnerData(snapshot, SelectExerciseName);
        }

        private Dictionary<int, Tuple<int, string>> addSpinnerData(DataSnapshot snapshot, Spinner spinner)
        {
            var list = new Dictionary<int, Tuple<int, string>>();

            int i = 0;
            foreach (DataSnapshot snap in snapshot.Children.ToEnumerable())
            {
                list.Add(i, new Tuple<int, string>(int.Parse(snap.Key), snap.Value.ToString()));
                i++;
            }

            try
            {
                var adapter = new ArrayAdapter<string>(this.Context, Android.Resource.Layout.SimpleSpinnerItem, list.Select(pair => pair.Value.Item2).ToList());
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                spinner.Adapter = adapter;
                return list;
            }
            catch (Exception ex)
            {
                Log.Error(TAG, ex.ToString());
            }
            return new Dictionary<int, Tuple<int, string>>();
        }
    }
}