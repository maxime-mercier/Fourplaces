using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Fourplaces
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            /*NavigationService.Configure("MainPage", typeof(MainPage));
            NavigationService.Configure("PageDetail", typeof(PageDetail));
            var mainPage = ((ViewNavigationService)NavigationService).SetRootPage("MainPage");*/
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
