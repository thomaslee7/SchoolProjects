using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using FitConnectApp.ViewModel;
using Android.Gms.Common.Apis;
using Firebase.Auth;
using Android.Gms.Auth.Api;
using Android.Content;
using Android.Preferences;
using Android.Util;
using FitConnectApp.Models;
using Firebase;
using FitConnectApp.Activities.WorkoutActivities;

namespace FitConnectApp
{
    public static class App
    {
        public const string TAG = "App";

        private static ViewModelLocator locator;
        private static User appuser;
        private static FirebaseAuth firebaseAuth;
        private static FirebaseApp firebaseApp;

        public static ViewModelLocator Locator
        {
            get
            {
                if (locator == null)
                {
                    DispatcherHelper.Initialize();

                    var nav = new NavigationService();

                    SimpleIoc.Default.Register<INavigationService>(() => nav);

                    nav.Configure(ViewModelLocator.HomeScreenKey, typeof(HomeScreen));
                    nav.Configure(ViewModelLocator.LoginScreenKey, typeof(LoginActivity));
                    nav.Configure(ViewModelLocator.StartWorkoutKey, typeof(StartWorkoutActivity));
                    nav.Configure(ViewModelLocator.CreateWorkoutKey, typeof(CreateWorkoutActivity));
		    		nav.Configure(ViewModelLocator.SocialScreenKey, typeof(SocialActivity));
                    locator = new ViewModelLocator();
                }

                return locator;
            }
        }

        public static FirebaseAuth mAuth
        {
            get
            {
                return firebaseAuth;
            }
            set
            {
                firebaseAuth = value;
            }
        }

        public static FirebaseApp fbApp
        {
            get
            {
                return firebaseApp;
            }
            set
            {
                firebaseApp = value;
            }
        }

        public static void saveAuthToken(Context ctx, string provider, string token)
        {
            try
            {
                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ctx);
                ISharedPreferencesEditor editor = prefs.Edit();
                editor.PutString(provider + "_authToken", token);
                editor.Apply();
            }
            catch (System.Exception ex)
            {
                Log.Debug(TAG, ex.ToString());
                //throw;
            }
        }

        public static string getAuthToken(Context ctx, string provider)
        {
            try
            {

                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ctx);
                return prefs.GetString(provider.ToUpper() + "_authToken", null);
            }
            catch (System.Exception ex)
            {
                Log.Debug(TAG, ex.ToString());
                //throw;
            }

            return null;
        }

        public static void removeAuthToken(Context ctx, string provider)
        {
            try
            {
                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ctx);
                ISharedPreferencesEditor editor = prefs.Edit();
                editor.Remove(provider.ToUpper() + "_authToken");
                editor.Apply();
            }
            catch (System.Exception ex)
            {
                Log.Debug(TAG, ex.ToString());
                //throw;
            }
        }

        public static void saveUid(Context ctx, string uid)
        {
            try
            {
                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ctx);
                ISharedPreferencesEditor editor = prefs.Edit();
                editor.PutString("FIREBASE_UID", uid);
                editor.Apply();
            }
            catch (System.Exception ex)
            {
                Log.Debug(TAG, ex.ToString());
                //throw;
            }
        }

        public static string getUid(Context ctx)
        {
            try
            {

                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ctx);
                return prefs.GetString("FIREBASE_UID", null);
            }
            catch (System.Exception ex)
            {
                Log.Debug(TAG, ex.ToString());
            }

            return null;
        }

        public static User appUser
        {
            get
            {
                if (appuser == null)
                {
                    appuser = new User();
                }
                return appuser;
            }

        }
    }
}
