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
using Android.Preferences;
using FitConnectApp.Models;


namespace FitConnectApp
{
    [Activity(Label = "Login", MainLauncher = true, Icon = "@drawable/icon")]
    public class LoginActivity : ActivityBase, GoogleApiClient.IOnConnectionFailedListener, View.IOnClickListener , IOnCompleteListener, FirebaseAuth.IAuthStateListener
    {
        private static int RC_SIGN_IN = 9001;
        public static string TAG = "LoginActivity";
                
        private GoogleApiClient mGoogleApiClient;
        private TextView mStatusTextView;
        private TextView welcomeText;
        private SignInButton signInButton;
        private Button basicSignInButton;
        private Button signOutButton;
        private Button createAccountButton;
        private EditText loginUsernameField;
        private EditText passwordField;
        private bool exit = false;

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
        public EditText LoginUsernameField => loginUsernameField ?? (loginUsernameField = FindViewById<EditText>(Resource.Id.loginUsernameText));
        public EditText PasswordField => passwordField ?? (passwordField = FindViewById<EditText>(Resource.Id.passwordText));
        public Button BasicSignInButton => basicSignInButton ?? (basicSignInButton = FindViewById<Button>(Resource.Id.basic_sign_in_button));
        public Button CreateAccountButton => createAccountButton ?? (createAccountButton = FindViewById<Button>(Resource.Id.create_account_button));
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

                App.fbApp = FirebaseApp.InitializeApp(this);

                App.mAuth = FirebaseAuth.GetInstance(App.fbApp);

                FirebaseAuth.Instance.AuthState += (sender, e) =>
                {
                    Log.Info(TAG, "Attempting to get UID in AuthState");
                    var user = e?.Auth?.CurrentUser;

                    if (user != null)
                    {
                        //if (App.appUser != null)
                        //{
                        App.saveUid(this.ApplicationContext, user.Uid);
                        App.appUser.FirebaseUserId = user.Uid;
                        Log.Info(TAG, "UID: " + user.Uid);
                            //App.appUser.FirebaseToken = user.GetTokenAsync(true).Result.Token;
                        //}
                        //else
                        //{
                        //    App.appUser = new User { FirebaseUserId = user.Uid }; //, FirebaseToken = user.GetTokenAsync(true).Result.Token };
                        //}
                    }
                    else
                    {
                        updateUI(false);
                        // User is signed out
                    }
                };

                SignInButton.SetOnClickListener(this);
                BasicSignInButton.SetOnClickListener(this);//non-google sign in button
                CreateAccountButton.SetOnClickListener(this);
                SignOutButton.SetOnClickListener(this);
            }
            catch (Exception ex)
            {
                Log.Debug("OnCreate", ex.ToString());
                throw;
            }
        }

        private void signIn(int x)
        {
            if (x == 0)
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
            if(x == 1)
            {
                try
                {
                    Log.Debug(TAG, "creating intent");
                    EditText a = (EditText)FindViewById(Resource.Id.loginUsernameText);
                    String username = a.Text;
                    EditText b = (EditText)FindViewById(Resource.Id.passwordText);
                    String pwd = b.Text;
                    Log.Debug(TAG, "signIn: " + username);
                    if (!validateForm())
                    {
                        return;
                    }
                    App.mAuth.SignInWithEmailAndPassword(username, pwd).AddOnCompleteListener(this, this);

                }
                catch(Exception ex)
                {
                    Log.Debug("signIn", ex.ToString());
                    throw;
                }
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
                    App.saveAuthToken(this.ApplicationContext, "GOOGLE", acct.IdToken);
                    App.appUser.FirstName = acct.GivenName;
                    App.appUser.LastName = acct.FamilyName;

                    FirebaseAuthWithGoogle(acct);
                    updateUI(true);
                    //NavigationService nav = (NavigationService)ServiceLocator.Current.GetInstance<INavigationService>();
                    //nav.NavigateTo(ViewModelLocator.HomeScreenKey);
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
                AuthCredential credential = GoogleAuthProvider.GetCredential(acct.IdToken, null);
                
                App.mAuth.SignInWithCredential(credential).AddOnCompleteListener(this, this);
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
            App.mAuth.RemoveAuthStateListener(this);
            mGoogleApiClient.Disconnect();
        }

        protected override void OnStart()
        {
            base.OnStart();
            App.mAuth.AddAuthStateListener(this);
            mGoogleApiClient.Connect();
            
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (App.appUser.IsLoggedIn || !string.IsNullOrWhiteSpace(App.getAuthToken(this, "GOOGLE")))
            {
                try
                {
                    //NavigationService nav = (NavigationService)ServiceLocator.Current.GetInstance<INavigationService>();
                    //nav.NavigateTo(ViewModelLocator.HomeScreenKey);
                    Vm.ShowHomeScreen.Execute(null);
                }
                catch (Exception ex)
                {
                    Log.Error(TAG, ex.ToString());
                    //throw;
                }
            }
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
                    signIn(0);
                    break;
                case Resource.Id.basic_sign_in_button:
                    signIn(1);
                    break;
                case Resource.Id.create_account_button:
                    createAccount(LoginUsernameField.Text.ToString(), PasswordField.Text.ToString());
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
            App.mAuth.SignOut();
            Auth.GoogleSignInApi.SignOut(mGoogleApiClient)
                .SetResultCallback(new ResultCallback<IResult>(delegate
                {
                    Log.Debug(TAG, "Auth.GoogleSignInApi.SignOut");
                    updateUI(false);
                }));
            App.removeAuthToken(this.ApplicationContext, "GOOGLE");
        }

        public void OnComplete(Task task)
        {

            Log.Debug(TAG, "SignInWithCredential:OnComplete:" + task.IsSuccessful);

            try
            {
                Vm.ShowHomeScreen.Execute(null);
            }
            catch (Exception ex)
            {
                Log.Error(TAG, ex.ToString());
                //throw;
            }
   
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
        private Boolean validateForm()
        {
            Boolean valid = true;
            String email = LoginUsernameField.Text.ToString();
            if(email == null)
            {
                valid = false;
            }
            String pwd = PasswordField.Text.ToString();
            if(pwd == null)
            {
                valid = false;
            }
            if(pwd.Length < 6)
            {
                Toast.MakeText(this, "Password must be at least 6 characters.", ToastLength.Short).Show();
                valid = false;
            }
            return valid;
        }
        private void createAccount(String email, String password)
        {
            Log.Debug(TAG, "createAccount:" + email);
            if (!validateForm())
            {
                return;
            }
            try
            {
                App.mAuth.CreateUserWithEmailAndPassword(email, password).AddOnCompleteListener(this, this);
                
            }
            catch (Exception ex)
            {
                Log.Debug("createAccount", ex.ToString());
                throw;
            }
        }

        public override void OnBackPressed()
        {
            Toast.MakeText(this, "Press back once again to exit.", ToastLength.Short);
            if (exit)
            {            
                FinishAffinity();
            }
            else
            {
                Toast.MakeText(this, "Press Back again to Exit.", ToastLength.Short).Show();
                exit = true;
                new Handler().PostDelayed(new Java.Lang.Runnable(() =>
                {
                    exit = false;
                }), 3 * 1000);

            }            
        }
    }
}