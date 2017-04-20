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
using Android.Util;
using Firebase.Database;

namespace FitConnectApp.Activities.WorkoutActivities.Listeners
{
    public class ValueEventListener : Java.Lang.Object, IValueEventListener
    {
        private Action<DataSnapshot> dataChangedAction;

        public ValueEventListener()
        {
        }

        public ValueEventListener(Action<DataSnapshot> action)
        {
            dataChangedAction = action;
        }
        public void OnCancelled(DatabaseError error)
        {
            Log.Debug("VEL OnCancelled", error.ToString());
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            dataChangedAction(snapshot);         
        }
    }
}
    
 