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
<<<<<<< HEAD
        public const string StartWorkoutKey = "StartWorkoutScreen";
        public const string CreateWorkoutKey = "CreateWorkoutScreen";
        //public const string ExerciseSelectKey = "ExerciseSelectScreen";
=======
		public const string SocialScreenKey = "SocialScreen";
>>>>>>> 277ba4f... * font-awesome.css: * font-awesome.min.css: * fontawesome-webfont.eot: * fontawesome-webfont.svg: * fontawesome-webfont.ttf: * fontawesome-webfont.woff: * fontawesome-webfont.woff2: add font awesome for glyphs and styling

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
<<<<<<< HEAD
            SimpleIoc.Default.Register<HomeScreenViewModel>();
            SimpleIoc.Default.Register<StartWorkoutViewModel>();
            SimpleIoc.Default.Register<CreateWorkoutViewModel>();
            SimpleIoc.Default.Register<ExerciseSelectViewModel>();
=======
			SimpleIoc.Default.Register<HomeScreenViewModel>();
			SimpleIoc.Default.Register<SocialViewModel>();
>>>>>>> 277ba4f... * font-awesome.css: * font-awesome.min.css: * fontawesome-webfont.eot: * fontawesome-webfont.svg: * fontawesome-webfont.ttf: * fontawesome-webfont.woff: * fontawesome-webfont.woff2: add font awesome for glyphs and styling
        }

        public LoginViewModel Login
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginViewModel>();
            }
        }

<<<<<<< HEAD
        public HomeScreenViewModel Home
=======
		public HomeScreenViewModel Home
>>>>>>> 277ba4f... * font-awesome.css: * font-awesome.min.css: * fontawesome-webfont.eot: * fontawesome-webfont.svg: * fontawesome-webfont.ttf: * fontawesome-webfont.woff: * fontawesome-webfont.woff2: add font awesome for glyphs and styling
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HomeScreenViewModel>();
            }
        }

<<<<<<< HEAD
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

        public ExerciseSelectViewModel ExerciseSelect
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ExerciseSelectViewModel>();
            }
        }
=======
		public SocialViewModel Social
		{
			get
			{
				return ServiceLocator.Current.GetInstance<SocialViewModel>();
			}
		}

>>>>>>> 277ba4f... * font-awesome.css: * font-awesome.min.css: * fontawesome-webfont.eot: * fontawesome-webfont.svg: * fontawesome-webfont.ttf: * fontawesome-webfont.woff: * fontawesome-webfont.woff2: add font awesome for glyphs and styling
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}