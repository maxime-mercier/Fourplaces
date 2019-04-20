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
        public static string AccessToken { get; set; }
        public static string RefreshToken { get; set; }
        public static int ExpiresIn { get; set; }
        public static DateTime TokenTime { get; set; }
        public static readonly string TokenScheme = "Bearer";
        public static readonly string PlaceListCacheUrl = "placeListCache";
        public static readonly string PlaceDetailCacheUrl = "placeDetailCache";
        public static readonly string UserCacheUrl = "userCache";

        public App()
        {
            InitializeComponent();
            Barrel.ApplicationId = "Fourplaces";
            MainPage = new NavigationPage(new HomePage());
        }


        

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
