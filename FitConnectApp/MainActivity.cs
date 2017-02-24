using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;

namespace FitConnectApp
{
    [Activity(Label = "FitConnectApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Intent login = new Intent(this.ApplicationContext, typeof(LoginActivity));
            StartActivity(login);
            // Set our view from the "main" layout resource
            //SetContentView (Resource.Layout.Main);
        }
    }
}

