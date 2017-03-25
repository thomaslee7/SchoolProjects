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
using GalaSoft.MvvmLight.Command;
using Android.Util;
using GalaSoft.MvvmLight.Views;

namespace FitConnectApp.ViewModel
{
	public class SocialViewModel : ViewModelBase
	{
		private RelayCommand _showSocial;
		private INavigationService _navService;

		public SocialViewModel(INavigationService navService)
		{
			_navService = navService;
		}

		public RelayCommand ShowSocial
		{
			get 
			{
				return _showSocial ??
					(_showSocial = new RelayCommand(() => Log.Debug("ShowSocialVM", "Here are your connections")));
			}
		}
	}
}
