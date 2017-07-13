using System;
using Android.App;
using Android.OS;
using Android.Widget;
using FitConnectApp.ViewModel;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Views;
using Android.Content;

namespace FitConnectApp
{
	public class SocialActivity : ActivityBase
	{
		public SocialViewModel Vm = App.Locator.Social;

		string[] connections;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			connections = new string[] { "p1", "p2", "p3", "p4" };
			IListAdapter ListAdapter = new ArrayAdapter<string>(this, FitConnectApp.Resource.Id.listView1, connections);
		}
	}
}