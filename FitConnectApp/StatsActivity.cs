using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FitConnectApp.ViewModel;
using Firebase.Auth;
using Firebase;
using Firebase.Database;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Views;
using FitConnectApp.Activities.WorkoutActivities.Listeners;
using Android.Util;


namespace FitConnectApp
{
    [Activity(Label = "Stats")]
    public class StatsActivity : ActivityBase, View.IOnClickListener
    {
        private String TAG = "Stats Activity";
        private EditText _heightField;
        private EditText _weightField;
        private EditText _genderField;
        private Button _statsUpdateButton;

        public EditText HeightField => _heightField = FindViewById<EditText>(Resource.Id.heightText);
        public EditText WeightField => _weightField = FindViewById<EditText>(Resource.Id.weightText);
        public EditText GenderField => _genderField = FindViewById<EditText>(Resource.Id.genderText);
        public Button StatsUpdateButton => _statsUpdateButton = FindViewById<Button>(Resource.Id.StatsUpdateButton);
        
       
        public StatsViewModel Vm => App.Locator.Stats;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Stats);
            var user = App.mAuth.CurrentUser;
            var uid = App.getUid(this.ApplicationContext);
            var db = FirebaseDatabase.GetInstance(App.fbApp);
            StatsUpdateButton.SetOnClickListener(this);

            // EditText a = FindViewById<EditText>(Resource.Id.genderText);
            if (GenderField.Text == "")
            {
                db.GetReference("users").Child(uid).Child("gender").SetValue("");
                db.GetReference("users").Child(uid).Child("height").SetValue("");
                db.GetReference("users").Child(uid).Child("weight").SetValue("");
                db.GetReference("users").Child(uid).Child("gender").AddValueEventListener(new ValueEventListener(genUpdate));
                db.GetReference("users").Child(uid).Child("height").AddValueEventListener(new ValueEventListener(heightUpdate));
                db.GetReference("users").Child(uid).Child("weight").AddValueEventListener(new ValueEventListener(weightUpdate));
            }
            

        }

        public void OnClick(View v)
        {
            var db = FirebaseDatabase.GetInstance(App.fbApp);
            var uid = App.getUid(this.ApplicationContext);
            switch (v.Id)
            {
                case Resource.Id.StatsUpdateButton:
                    Dictionary<String, Java.Lang.Object> genderUpdate = new Dictionary<string, Java.Lang.Object>();
                    Dictionary<String, Java.Lang.Object> heightUpdate = new Dictionary<string, Java.Lang.Object>();
                    Dictionary<String, Java.Lang.Object> weightUpdate = new Dictionary<string, Java.Lang.Object>();

                    String a = FindViewById<EditText>(Resource.Id.genderText).Text;
                    String b = FindViewById<EditText>(Resource.Id.heightText).Text;
                    String c = FindViewById<EditText>(Resource.Id.weightText).Text;
                    Log.Debug(TAG, a);
                    genderUpdate.Add("gender", a);
                    db.GetReference("users").Child(uid).UpdateChildren(genderUpdate);
                    Log.Debug(TAG, b);
                    heightUpdate.Add("height", b);
                    db.GetReference("users").Child(uid).UpdateChildren(heightUpdate);
                    Log.Debug(TAG, c);
                    weightUpdate.Add("weight", c);
                    db.GetReference("users").Child(uid).UpdateChildren(weightUpdate);
                    break;
                default:
                    Log.Debug(TAG, "onclick: " + v.Id);
                    break;

            }
        }

        public void genUpdate(DataSnapshot snapshot)
        {
            GenderField.Text = snapshot.Value.ToString();
        }
        public void heightUpdate(DataSnapshot snapshot)
        {
            HeightField.Text = snapshot.Value.ToString();
        }
        public void weightUpdate(DataSnapshot snapshot)
        {
            WeightField.Text = snapshot.Value.ToString();
        }
        public void userExist(DataSnapshot snapshot)
        {
            bool a = snapshot.Exists();
        }
       
    }
}