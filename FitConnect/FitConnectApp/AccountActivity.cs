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
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Views;
using FitConnectApp.Activities.WorkoutActivities.Listeners;
using Android.Util;

namespace FitConnectApp
{
    [Activity(Label = "Account")]
    public class AccountActivity : ActivityBase, View.IOnClickListener
    {
        

        private EditText accountUsernameText;
        private EditText _fNameField;
        private EditText _lNameField;
        private Button _accountUpdateButton;

        public EditText AccountUsernameText => accountUsernameText = FindViewById<EditText>(Resource.Id.accountUsernameText);
        public EditText FNameField => _fNameField = FindViewById<EditText>(Resource.Id.fNameText);
        public EditText LNameField => _lNameField = FindViewById<EditText>(Resource.Id.lNameText);
        public Button AccountUpdateButton => _accountUpdateButton = FindViewById<Button>(Resource.Id.AccountUpdateButton);
        public AccountViewModel Vm => App.Locator.Account;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Account);
            var user = App.mAuth.CurrentUser;
            var uid = App.getUid(this.ApplicationContext);
            var db = FirebaseDatabase.GetInstance(App.fbApp);
            AccountUpdateButton.SetOnClickListener(this);
            DatabaseReference dbRef = db.GetReference("users").Child(uid);
            dbRef.AddListenerForSingleValueEvent(new ValueEventListener(existCheck));

            db.GetReference("users").Child(uid).Child("customUsername").AddValueEventListener(new ValueEventListener(usernameUpdate));
            db.GetReference("users").Child(uid).Child("FirstName").AddValueEventListener(new ValueEventListener(fNameUpdate));
            db.GetReference("users").Child(uid).Child("LastName").AddValueEventListener(new ValueEventListener(lNameUpdate));
           
        }
        public void OnClick(View v)
        {
            var db = FirebaseDatabase.GetInstance(App.fbApp);
            var uid = App.getUid(this.ApplicationContext);
            switch (v.Id)
            {
                case Resource.Id.AccountUpdateButton:
                    Dictionary<String, Java.Lang.Object> usernameUpdate = new Dictionary<string, Java.Lang.Object>();
                    Dictionary<String, Java.Lang.Object> fnameupdate = new Dictionary<string, Java.Lang.Object>();
                    Dictionary<String, Java.Lang.Object> lnameupdate = new Dictionary<string, Java.Lang.Object>();

                    String a = FindViewById<EditText>(Resource.Id.accountUsernameText).Text;
                    String b = FindViewById<EditText>(Resource.Id.fNameText).Text;
                    String c = FindViewById<EditText>(Resource.Id.lNameText).Text;

                    usernameUpdate.Add("customUsername", a);
                    db.GetReference("users").Child(uid).UpdateChildren(usernameUpdate);
                    fnameupdate.Add("FirstName", b);
                    db.GetReference("users").Child(uid).UpdateChildren(fnameupdate);
                    lnameupdate.Add("LastName", c);
                    db.GetReference("users").Child(uid).UpdateChildren(lnameupdate);
                    Toast.MakeText(this, "Account Info Saved.", ToastLength.Long).Show();
                    break;
                default:
                    break;
            }
        }
        public void existCheck(DataSnapshot snapshot)
        {
            if (!snapshot.HasChild("customUsername"))
            {
                var uid = App.getUid(this.ApplicationContext);
                var db = FirebaseDatabase.GetInstance(App.fbApp);
                db.GetReference("users").Child(uid).Child("customUsername").SetValue("");
                db.GetReference("users").Child(uid).Child("FirstName").SetValue("");
                db.GetReference("users").Child(uid).Child("LastName").SetValue("");
            }
        }

        public void usernameUpdate(DataSnapshot snapshot)
        {
            if(snapshot.Value == null)
            {
                AccountUsernameText.Text = "";
            }
            else
            {
                AccountUsernameText.Text = snapshot.Value.ToString();
            }
        }
        public void fNameUpdate(DataSnapshot snapshot)
        {
            if(snapshot.Value == null)
            {
                FNameField.Text = "";
            }
            else
            {
                FNameField.Text = snapshot.Value.ToString();
            }
        }
        public void lNameUpdate(DataSnapshot snapshot)
        {
            if(snapshot.Value == null)
            {
                LNameField.Text = "";
            }
            else
            {
                LNameField.Text = snapshot.Value.ToString();
            }
        }
    }
}