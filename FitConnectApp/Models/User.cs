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
using GalaSoft.MvvmLight;

namespace FitConnectApp.Models
{
    public class User : ObservableObject //part of MVVM, implements INotifiy property changed for us
    {
        private string _firstName;
        private string _lastName;
        private string _firebaseUID;
        private string _firebaseToken;

        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                Set(() => FirstName, ref _firstName, value); //this automatically raises INotifyPropertyChanged and is part of MVVMLight
            }
        }
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                Set(() => LastName, ref _lastName, value);
            }
        }
        public string FirebaseUserId
        {
            get
            {
                return _firebaseUID;
            }
            set
            {
                Set(() => FirebaseUserId, ref _firebaseUID, value);
            }
        }

        public string FirebaseToken
        {
            get
            {
                return _firebaseToken;
            }
            set
            {
                Set(() => FirebaseToken, ref _firebaseToken, value);
            }
        }
    }
}