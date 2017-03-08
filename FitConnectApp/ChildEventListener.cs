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

namespace FitConnectApp
{
    class ChildEventListener : Java.Lang.Object, IChildEventListener
    {
       
        public void OnCancelled(DatabaseError error)
        {
            throw new NotImplementedException();
        }

        public void OnChildAdded(DataSnapshot snapshot, string previousChildName)
        {
            throw new NotImplementedException();
        }

        public void OnChildChanged(DataSnapshot snapshot, string previousChildName)
        {
            throw new NotImplementedException();
        }

        public void OnChildMoved(DataSnapshot snapshot, string previousChildName)
        {
            throw new NotImplementedException();
        }

        public void OnChildRemoved(DataSnapshot snapshot)
        {
            throw new NotImplementedException();
        }
    }
}