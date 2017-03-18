using Android.App;
using Android.OS;
using Android.Widget;
using FitConnectApp.ViewModel;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Views;

namespace FitConnectApp
{
    [Activity(Label = "Start Workout")]
    public class StartWorkoutActivity : ActivityBase
    {
        private Button _startNewWorkout;

        public Button StartNewWorkout => _startNewWorkout = FindViewById<Button>(Resource.Id.StartNewWorkoutBtn);
        public StartWorkoutViewModel Vm => App.Locator.StartWorkout;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.StartWorkout);
            StartNewWorkout.SetCommand("Click", Vm.StartNewWorkout);

        }

    }
}