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
using Firebase.Database;
using Android.Util;

namespace FitConnectApp
{
    class ValueEventListener : Java.Lang.Object, IValueEventListener
    {
        
        public void OnCancelled(DatabaseError error)
        {
            Log.Debug("VEL OnCancelled", error.ToString());
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            var thing = snapshot.Value;
            Log.Debug("VEL OnDataChange", snapshot.Value.ToString());
        }
    }
}