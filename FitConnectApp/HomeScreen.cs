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
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Android.Gms.Auth.Api;
using Android.Gms.Common.Apis;
using FitConnectApp.ViewModel;
using Android.Gms.Tasks;
using Firebase.Auth;
using Android.Gms.Common;
using static Android.Gms.Common.Apis.GoogleApiClient;
using Android.Util;
using Firebase;
using Firebase.Database;

namespace FitConnectApp
{
    [Activity(Label = "HomeScreen")]
    public class HomeScreen : ActivityBase, GoogleApiClient.IOnConnectionFailedListener,
        View.IOnClickListener, IOnCompleteListener, FirebaseAuth.IAuthStateListener
    {
        private Button logout;
        private Button social;
        private Button workouts;
        private Button account;
        private Button stats;
        private GoogleApiClient mGoogleApiClient;
        //private FirebaseAuth mAuth;
        public const string TAG = "HomeScreen";

        public Button Logout => logout ?? (logout = FindViewById<Button>(Resource.Id.Logout));
        public Button Social => social ?? (social = FindViewById<Button>(Resource.Id.Social));
        public Button Workouts => workouts ?? (workouts = FindViewById<Button>(Resource.Id.Workouts));
        public Button Account => account ?? (account = FindViewById<Button>(Resource.Id.Account));
        public Button Stats => stats ?? (stats = FindViewById<Button>(Resource.Id.Stats));

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.HomeScreen);

            mGoogleApiClient = new GoogleApiClient.Builder(this)
                .AddOnConnectionFailedListener(this)
                .AddApi(Auth.GOOGLE_SIGN_IN_API)
                .Build();

            var nav = (NavigationService)ServiceLocator.Current.GetInstance<INavigationService>();
            Logout.Click += (s, e) =>
            {
                App.mAuth.SignOut();
                try
                {

                    Auth.GoogleSignInApi.SignOut(mGoogleApiClient)
                        .SetResultCallback(new ResultCallback<IResult>(delegate
                        {
                            Log.Debug("HomeScreen", "Auth.GoogleSignInApi.SignOut");
                            nav.NavigateTo(ViewModelLocator.LoginScreenKey);
                        }));
                }
                catch (Exception ex)
                {
                    Log.Debug(TAG, ex.ToString());
                }
            };

            try
            {

                var uid = App.getUid(this.ApplicationContext);

                try
                {
                    Log.Debug(TAG, "TESTVALUE:");
                    var db = FirebaseDatabase.GetInstance(App.fbApp);

                    var test = db.GetReference("users").Child(uid).Child("TestVal").SetValue("Updated!");
                    var test2 = db.GetReference("users").Child(uid).Child("TestVal").AddValueEventListener(new ValueEventListener());//.AddChildEventListener(new IChildEventListener());

                }
                catch (Exception ex)
                {
                    Log.Debug(TAG, ex.ToString());
                }

            }
            catch (Exception ex)
            {
                Log.Debug(TAG, ex.ToString());
            }

        }

        protected override void OnStart()
        {
            base.OnStart();
            App.mAuth.AddAuthStateListener(this);
            mGoogleApiClient.Connect();
        }
        protected override void OnStop()
        {
            base.OnStop();
            App.mAuth.RemoveAuthStateListener(this);
            mGoogleApiClient.Disconnect();
        }
        

        public void OnConnectionFailed(ConnectionResult result)
        {
            Log.Debug("HomeScreen", "Connection Failed.");
        }

        public void OnComplete(Task task)
        {
            Log.Debug(TAG, "SignInWithCredential:OnComplete:" + task.IsSuccessful);
            // If sign in fails, display a message to the user. If sign in succeeds
            // the auth state listener will be notified and logic to handle the
            // signed in user can be handled in the listener.
            if (!task.IsSuccessful)
            {
                Log.Wtf(TAG, "SignInWithCredential", task.Exception);
                Log.Debug(TAG, "SignInWithCredential failed: " + task.Exception.Cause + "; " + task.Exception.Message);
                Toast.MakeText(this, "Authentication failed.", ToastLength.Long).Show();
            }
        }

        public void OnClick(View v)
        {
            //throw new NotImplementedException();
        }

        public void OnAuthStateChanged(Firebase.Auth.FirebaseAuth auth)
        {
            Log.Debug(TAG, "onAuthStateChanged:");
        }

     
    }
}