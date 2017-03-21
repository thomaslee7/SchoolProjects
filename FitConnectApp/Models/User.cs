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
<<<<<<< HEAD
    public class User : ObservableObject //part of MVVM, implements INotifiy property changed for us
    {
        private string _firstName;
        private string _lastName;
        private string _firebaseUID;
        private string _firebaseToken;
        private bool _isLoggedIn;
=======
    public class User : ObservableObject
    {
		private string _firstName;
        private string _lastName;
        private string _firebaseUID;
        private string _firebaseToken;
>>>>>>> 277ba4f... * font-awesome.css: * font-awesome.min.css: * fontawesome-webfont.eot: * fontawesome-webfont.svg: * fontawesome-webfont.ttf: * fontawesome-webfont.woff: * fontawesome-webfont.woff2: add font awesome for glyphs and styling

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
<<<<<<< HEAD

        public bool IsLoggedIn
        {
            get
            {
                return _isLoggedIn;
            }
            set
            {
                Set(() => IsLoggedIn, ref _isLoggedIn, value);
            }
        }
=======
>>>>>>> 277ba4f... * font-awesome.css: * font-awesome.min.css: * fontawesome-webfont.eot: * fontawesome-webfont.svg: * fontawesome-webfont.ttf: * fontawesome-webfont.woff: * fontawesome-webfont.woff2: add font awesome for glyphs and styling
    }
}