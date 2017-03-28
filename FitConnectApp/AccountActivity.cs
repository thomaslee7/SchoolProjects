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

namespace FitConnectApp
{
    [Activity(Label = "Account")]
    public class AccountActivity : ActivityBase
    {
        

        private EditText accountUsernameText;

        public EditText AccountUsernameText => accountUsernameText = FindViewById<EditText>(Resource.Id.accountUsernameText);
        public AccountViewModel Vm => App.Locator.Account;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Account);
            var user = App.mAuth.CurrentUser;
            //EditText a = (EditText)FindViewById(Resource.Id.accountUsernameText);
            String name = user.Email;
            AccountUsernameText.Text = name;
            
            
            
            
        }
    }
}