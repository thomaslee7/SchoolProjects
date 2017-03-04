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
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Tasks;
using Android.Support.V4.App;
using Android.Util;
using Firebase.Auth;
using Firebase;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Views;
using FitConnectApp.ViewModel;
using Microsoft.Practices.ServiceLocation;

namespace FitConnectApp
{
    [Activity(Label = "LoginActivity", MainLauncher = true, Icon = "@drawable/icon")]
    public class LoginActivity : ActivityBase, GoogleApiClient.IOnConnectionFailedListener, View.IOnClickListener , IOnCompleteListener, FirebaseAuth.IAuthStateListener
    {
        private static int RC_SIGN_IN = 9001;
        public static string TAG = "LoginActivity";

        private FirebaseAuth mAuth;
        private GoogleApiClient mGoogleApiClient;
        private TextView mStatusTextView;
        private TextView welcomeText;
        private SignInButton signInButton;
        private Button signOutButton;

        private readonly List<Binding> bindings = new List<Binding>();

        private LoginViewModel Vm
        {
            get
            {
                return App.Locator.Login;
            }
        }

        public TextView StatusTextView => mStatusTextView ?? (mStatusTextView = FindViewById<TextView>(Resource.Id.statusTextView));
        public SignInButton SignInButton => signInButton ?? (signInButton = FindViewById<SignInButton>(Resource.Id.sign_in_button));
        public Button SignOutButton => signOutButton ?? (signOutButton = FindViewById<Button>(Resource.Id.sign_out_and_disconnect));
        public TextView WelcomeText => welcomeText ?? (welcomeText = FindViewById<TextView>(Resource.Id.WelcomeText));

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {

                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.Login);

                bindings.Add(
                this.SetBinding(
                    () => Vm.WelcomeTitle,
                    () => WelcomeText.Text));

                GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                    .RequestIdToken("257464791047-bq54c6mdogf15epd9tm2ja0o3m3efvq6.apps.googleusercontent.com")
                    .RequestId()
                    .RequestEmail()
                    .Build();

                mGoogleApiClient = new GoogleApiClient.Builder(this)
                    .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
                    .Build();

                FirebaseApp fa = FirebaseApp.InitializeApp(this);

                mAuth = FirebaseAuth.GetInstance(fa);

                SignInButton.SetOnClickListener(this);
                SignOutButton.SetOnClickListener(this);
            }
            catch (Exception ex)
            {
                Log.Debug("OnCreate", ex.ToString());
                throw;
            }
        }

        private void signIn()
        {
            try
            {
                Log.Debug(TAG, "creating intent");
                Intent signInIntent = Auth.GoogleSignInApi.GetSignInIntent(mGoogleApiClient);
                Log.Debug(TAG, "intent created, invoking signin");
                StartActivityForResult(signInIntent, RC_SIGN_IN);
            }
            catch (Exception ex)
            {
                Log.Debug("signIn", ex.ToString());
                throw;
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            try
            {
                Log.Debug(TAG, "starting onactresult base");
                base.OnActivityResult(requestCode, resultCode, data);
                Log.Debug(TAG, "finished onactresultbase");
                if (requestCode == RC_SIGN_IN)
                {
                    Log.Debug(TAG, "getting signin result");
                    GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                    Log.Debug(TAG, "about to handle signin result");
                    handleSignInResult(result);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OnActivityResult", ex.ToString());
                throw;
            }
        }

        public void LoginComplete(bool loginSuccess)
        {
            updateUI(loginSuccess);
            if (loginSuccess)
            {
                NavigationService nav = (NavigationService)ServiceLocator.Current.GetInstance<INavigationService>();
                nav.NavigateTo(ViewModelLocator.HomeScreenKey);
            }
        }


        private void handleSignInResult(GoogleSignInResult result)
        {
            try
            {
                Log.Debug(TAG, "handleSignInResult:" + result.IsSuccess);
                if (result.IsSuccess)
                {
                    // Signed in successfully, show authenticated UI.
                    GoogleSignInAccount acct = result.SignInAccount;
                    StatusTextView.Text = GetString(Resource.String.signed_in_fmt, acct.DisplayName);
                    Log.Debug("handleSignInresult", "signing into firebase now: " + acct.IdToken);
                    FirebaseAuthWithGoogle(acct);
                    updateUI(true);
                    NavigationService nav = (NavigationService)ServiceLocator.Current.GetInstance<INavigationService>();
                    nav.NavigateTo(ViewModelLocator.HomeScreenKey);
                }
                else
                {
                    // Signed out, show unauthenticated UI.
                    updateUI(false);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("handleSignInResult", ex.ToString());
                throw;
            }
        }

        private void FirebaseAuthWithGoogle(GoogleSignInAccount acct)
        {
            try
            {

                Log.Debug(TAG, "FirebaseAuthWithGoogle:" + acct.Id + "; acct token: " + acct.IdToken);

                AuthCredential credential = GoogleAuthProvider.GetCredential(acct.IdToken, null);
                mAuth.SignInWithCredential(credential).AddOnCompleteListener(this, this);
            }
            catch (Exception ex)
            {
                Log.Debug("FirebaseAuthWithGoogle", ex.ToString());
                throw;
            }
        }

        private void updateUI(bool signedIn)
        {
            if (signedIn)
            {
                FindViewById(Resource.Id.sign_in_button).Visibility = ViewStates.Gone;
                FindViewById(Resource.Id.sign_out_and_disconnect).Visibility = ViewStates.Visible;
            }
            else
            {
                StatusTextView.Text = GetString(Resource.String.signed_out);

                FindViewById(Resource.Id.sign_in_button).Visibility = ViewStates.Visible;
                FindViewById(Resource.Id.sign_out_and_disconnect).Visibility = ViewStates.Gone;
            }
        }

        protected override void OnStop()
        {
            base.OnStop();
            mAuth.RemoveAuthStateListener(this);
            mGoogleApiClient.Disconnect();
        }

        protected override void OnStart()
        {
            base.OnStart();
            mAuth.AddAuthStateListener(this);
            mGoogleApiClient.Connect();
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            Log.Debug(TAG, "OnConnectionFailed:" + result);
            Toast.MakeText(this, "Google Play Services error.", ToastLength.Long).Show();
        }

        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.sign_in_button:
                    signIn();
                    break;
                case Resource.Id.sign_out_and_disconnect:
                    signOut();
                    break;
                default:
                    Log.Debug(TAG, "onclick: " + v.Id);
                    break;
            }
        }

        private void signOut()
        {
            mAuth.SignOut();
            Auth.GoogleSignInApi.SignOut(mGoogleApiClient)
                .SetResultCallback(new ResultCallback<IResult>(delegate
                {
                    Log.Debug(TAG, "Auth.GoogleSignInApi.SignOut");
                    updateUI(false);
                }));
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

        public void OnAuthStateChanged(FirebaseAuth auth)
        {
            var user = auth.CurrentUser;
            Log.Debug(TAG, "onAuthStateChanged:" + (user != null ? "signed_in:" + user.Uid : "signed_out"));
            updateUI(user != null);
        }
    }
}