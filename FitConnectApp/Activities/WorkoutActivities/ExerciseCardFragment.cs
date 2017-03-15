using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using FitConnectApp.ViewModel;
using GalaSoft.MvvmLight.Helpers;

namespace FitConnectApp.Activities.WorkoutActivities
{
    public class ExerciseCardFragment : Fragment
    {
        private readonly List<Binding> bindings = new List<Binding>();
        public ExerciseCardViewModel Vm { get; set; }                
        public TextView ExName { get; set; }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);            
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            
            Vm = new ExerciseCardViewModel(App.Locator.ExerciseSelect.SelectedExerciseId, App.Locator.ExerciseSelect.SelectedExerciseName);
            //Vm = new ExerciseCardViewModel(savedInstanceState.GetInt("exid"), savedInstanceState.GetString("exname"));
            var view = inflater.Inflate(Resource.Layout.ExerciseCard, container, false);

            ExName = view.FindViewById<TextView>(Resource.Id.ExerciseName);

            bindings.Add(
                this.SetBinding(
                    () => Vm.ExerciseName,
                    () => ExName.Text));
            return view;

            //return base.OnCreateView(inflater, container, savedInstanceState);
        }
    }
}