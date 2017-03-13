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

namespace FitConnectApp
{
    [Activity(Label = "Account")]
    class AccountActivity : ActivityBase
    {
        private EditText _usernameField;
        private Button _logout;


        public EditText UsernameField => _usernameField = FindViewById<EditText>(Resource.Id.usernameField);
  

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Account);
            var user = App.mAuth.CurrentUser;
            String name = user.Email;
            UsernameField.Text = name;
        }
    }
}