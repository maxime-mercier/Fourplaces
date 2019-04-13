using System;
using Fourplaces.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Fourplaces.Pages;
using MonkeyCache.SQLite;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Fourplaces
{
    public partial class App : Application
    {
        public static IPlaceService PService { get; } = new PlaceService();
        public static string TokenScheme = "Bearer";

        public App()
        {
            InitializeComponent();
            /*NavigationService.Configure("MainPage", typeof(MainPage));
            NavigationService.Configure("PageDetail", typeof(PageDetail));
            var mainPage = ((ViewNavigationService)NavigationService).SetRootPage("MainPage");*/
            Barrel.ApplicationId = "Fourplaces";
            MainPage = new NavigationPage(new HomePage());
            //MainPage = new MainPage();
        }

        //public static INavigationService NavigationService { get; } = new ViewNavigationService();

        

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
