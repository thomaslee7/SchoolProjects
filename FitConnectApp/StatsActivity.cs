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
    class StatsActivity : ActivityBase
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Stats);
            var user = App.mAuth.CurrentUser;
            String name = user.Email;
            //UsernameField.Text = name;
        }
    }
}