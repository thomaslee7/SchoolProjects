/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:FitConnectApp"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace FitConnectApp.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        public const string HomeScreenKey = "HomeScreen";
        public const string LoginScreenKey = "LoginScreen";
        public const string StartWorkoutKey = "StartWorkoutScreen";
        public const string CreateWorkoutKey = "CreateWorkoutScreen";
        public const string AccountScreenKey = "AccountScreen";
        public const string StatsScreenKey = "StatsScreen";

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<HomeScreenViewModel>();
            SimpleIoc.Default.Register<StartWorkoutViewModel>();
            SimpleIoc.Default.Register<CreateWorkoutViewModel>();
            SimpleIoc.Default.Register<StatsViewModel>();
            SimpleIoc.Default.Register<AccountViewModel>();
        }

        public LoginViewModel Login
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginViewModel>();
            }
        }
        public HomeScreenViewModel Home
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HomeScreenViewModel>();
            }
        }

        public StartWorkoutViewModel StartWorkout
        {
            get
            {
                return ServiceLocator.Current.GetInstance<StartWorkoutViewModel>();
            }
        }

        public CreateWorkoutViewModel CreateWorkout
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CreateWorkoutViewModel>();
            }
        }

        public AccountViewModel Account
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AccountViewModel>();
            }
        }

        public StatsViewModel Stats
        {
            get
            {
                return ServiceLocator.Current.GetInstance<StatsViewModel>();
            }
        }

        public ExerciseSelectViewModel ExerciseSelect
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ExerciseSelectViewModel>();
            }
        }
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}