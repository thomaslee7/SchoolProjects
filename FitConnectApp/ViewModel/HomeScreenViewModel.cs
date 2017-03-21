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
using FitConnectApp.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;
using Android.Gms.Auth.Api;
using Android.Gms.Common.Apis;
using Android.Util;

using FitConnectApp.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;
using Android.Gms.Auth.Api;
using Android.Gms.Common.Apis;
using Android.Util;

namespace FitConnectApp.ViewModel
{
<<<<<<< HEAD
    public class HomeScreenViewModel : ViewModelBase
    {
        private User _currentUser;
        private INavigationService _navService;
        private RelayCommand _showStartWorkout;
        private RelayCommand<GoogleApiClient> _logout;

        private const string TAG = "HomeScreenViewModel"; 

=======
	public class HomeScreenViewModel : ViewModelBase
	{
        private User _currentUser;
        private INavigationService _navService;
		private RelayCommand _showSocial;
        private RelayCommand<GoogleApiClient> _logout;
 
        private const string TAG = "HomeScreenViewModel"; 
 
>>>>>>> 277ba4f... * font-awesome.css: * font-awesome.min.css: * fontawesome-webfont.eot: * fontawesome-webfont.svg: * fontawesome-webfont.ttf: * fontawesome-webfont.woff: * fontawesome-webfont.woff2: add font awesome for glyphs and styling
        public User CurrentUser
        {
            get
            {
                return _currentUser;
            }
            set
            {
                Set(() => CurrentUser, ref _currentUser, value);
            }
        }
<<<<<<< HEAD

        public HomeScreenViewModel(INavigationService navService)
        {
            _navService = navService;
            
        }

        public RelayCommand ShowStartWorkout
        {
            get
            {
                return _showStartWorkout ??
                    (_showStartWorkout = new RelayCommand(() =>
                        _navService.NavigateTo(ViewModelLocator.StartWorkoutKey)
                    ));
            }
        }

=======
 
        public HomeScreenViewModel(INavigationService navService)
        {
            _navService = navService;    
        }

		public RelayCommand ShowSocial
		{
			get
			{
				Log.Debug("SocialScreen", "Navigate to my social connections ");
				return _showSocial ??
					(
						_showSocial = new RelayCommand(() => 
						_navService.NavigateTo(ViewModelLocator.SocialScreenKey))
					);
			}
		}
 
>>>>>>> 277ba4f... * font-awesome.css: * font-awesome.min.css: * fontawesome-webfont.eot: * fontawesome-webfont.svg: * fontawesome-webfont.ttf: * fontawesome-webfont.woff: * fontawesome-webfont.woff2: add font awesome for glyphs and styling
        public RelayCommand<GoogleApiClient> Logout
        {
            get
            {
                return _logout ?? (_logout = new RelayCommand<GoogleApiClient>((mGoogleApiClient) => {
                    App.mAuth.SignOut();
                    try
                    {

                        Auth.GoogleSignInApi.SignOut(mGoogleApiClient)
                            .SetResultCallback(new ResultCallback<IResult>(delegate
                            {
                                Log.Debug("HomeScreen", "Auth.GoogleSignInApi.SignOut");
                                _navService.NavigateTo(ViewModelLocator.LoginScreenKey);
                            }));
<<<<<<< HEAD
                        App.removeAuthToken(Android.App.Application.Context.ApplicationContext, "GOOGLE");
=======
>>>>>>> 277ba4f... * font-awesome.css: * font-awesome.min.css: * fontawesome-webfont.eot: * fontawesome-webfont.svg: * fontawesome-webfont.ttf: * fontawesome-webfont.woff: * fontawesome-webfont.woff2: add font awesome for glyphs and styling
                    }
                    catch (Exception ex)
                    {
                        Log.Debug(TAG, ex.ToString());
                    }
                }));
            }
        }
<<<<<<< HEAD
    }
=======
     }
>>>>>>> 277ba4f... * font-awesome.css: * font-awesome.min.css: * fontawesome-webfont.eot: * fontawesome-webfont.svg: * fontawesome-webfont.ttf: * fontawesome-webfont.woff: * fontawesome-webfont.woff2: add font awesome for glyphs and styling
}