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

namespace FitConnectApp.ViewModel
{
    public class HomeScreenViewModel : ViewModelBase
    {
        private User _currentUser;
        private INavigationService _navService;
        private RelayCommand _showStartWorkout;
        private RelayCommand _showAccount;
        private RelayCommand _showStats;
        private RelayCommand<GoogleApiClient> _logout;

        private const string TAG = "HomeScreenViewModel";

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
        public RelayCommand ShowAccount
        {
            get
            {
                return _showAccount ??
                    (_showAccount = new RelayCommand(() =>
                        _navService.NavigateTo(ViewModelLocator.AccountScreenKey)
                        ));
            }
        }
        public RelayCommand ShowStats
        {
            get
            {
                return _showStats ??
                    (_showStats = new RelayCommand(() =>
                        _navService.NavigateTo(ViewModelLocator.StatsScreenKey)
                        ));
            }
        }

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
                        App.removeAuthToken(Android.App.Application.Context.ApplicationContext, "GOOGLE");
                    }
                    catch (Exception ex)
                    {
                        Log.Debug(TAG, ex.ToString());
                    }
                }));
            }
        }
    }
}