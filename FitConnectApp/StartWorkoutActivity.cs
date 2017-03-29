using Android.App;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using FitConnectApp.Activities.WorkoutActivities.Listeners;
using FitConnectApp.ViewModel;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitConnectApp
{
    [Activity(Label = "Start Workout")]
    public class StartWorkoutActivity : ActivityBase
    {
        private Button startNewWorkout;
        private Button loadSavedWorkout;
        private Dictionary<int, Tuple<Guid, string>> dateList;
        private Guid selectedWorkout;
        private const string TAG = "StartWorkoutActivity";

        public Button StartNewWorkout => startNewWorkout = FindViewById<Button>(Resource.Id.StartNewWorkoutBtn);
        public Button LoadSavedWorkout => loadSavedWorkout = FindViewById<Button>(Resource.Id.loadSavedWorkout);
        public StartWorkoutViewModel Vm => App.Locator.StartWorkout;
        private string uid;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.StartWorkout);
            StartNewWorkout.SetCommand("Click", Vm.StartNewWorkout);
            //LoadSavedWorkout.SetCommand("Click", Vm.LoadSavedWorkout);
            LoadSavedWorkout.Click += LoadSavedWorkout_Click;

            var db = FirebaseDatabase.GetInstance(App.fbApp);
            uid = App.getUid(this.ApplicationContext);

            db.GetReference("WorkoutDateList").Child(uid).AddValueEventListener(new ValueEventListener(GetExerciseDateList));
        }

        private void LoadSavedWorkout_Click(object sender, System.EventArgs e)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            AlertDialog alert = builder.Create();
            LinearLayout layout = new LinearLayout(this);
            
            LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent, 1f);

            //layout.AddView();

            Spinner dateSpinner = new Spinner(this);
            //var dateList = Resources.GetStringArray(Resource.Array.RpeList);
            var dates = new List<string>();
            try
            {
                dates = dateList.Select(kvp => kvp.Value.Item2).ToList();
            }
            catch (Exception ex)
            {
                Log.Debug(TAG, ex.ToString());                
            }
            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, dates);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            dateSpinner.Adapter = adapter;
            dateSpinner.LayoutParameters = lp;
            
            layout.AddView(dateSpinner);

            alert.SetView(layout);
            alert.SetButton("Load", (senderAlert, args) => {
                selectedWorkout = dateList[dateSpinner.SelectedItemPosition].Item1;
                //Vm.LoadWorkout.Execute(new Tuple<Guid, string>(selectedWorkout, uid));
            });

            alert.SetButton2("Cancel", (senderAlert, args) =>
            {

            });
                        

            alert.Show();
        }

        public void GetExerciseDateList(DataSnapshot snapshot)
        {
            dateList = new Dictionary<int, Tuple<Guid, string>>();

            int i = 0;
            foreach (DataSnapshot snap in snapshot.Children.ToEnumerable())
            {
                dateList.Add(i, new Tuple<Guid, string>(Guid.Parse(snap.Key), snap.Value.ToString()));
                i++;
            }
        }

    }
}