using System.Windows.Input;
using Fourplaces.Pages;
using MonkeyCache.SQLite;
using Storm.Mvvm;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    class HomeViewModel : ViewModelBase
    {

        private readonly INavigation _navigation;

        private ICommand _signUpCommand;

        public ICommand SignUpCommand
        {
            get => _signUpCommand;
            set => SetProperty(ref _signUpCommand, value);
        }

        private ICommand _signInCommand;

        public ICommand SignInCommand
        {
            get => _signInCommand;
            set => SetProperty(ref _signInCommand, value);
        }

        private bool _buttonEnabled;

        public bool ButtonEnabled
        {
            get => _buttonEnabled;
            set => SetProperty(ref _buttonEnabled, value);
        }

        public HomeViewModel(INavigation navigation)
        {
            _navigation = navigation;
            SignInCommand = new Command(SignIn);
            SignUpCommand = new Command(SignUp);
            Barrel.Current.Empty(key: App.UserCacheUrl);
            Barrel.Current.EmptyExpired();
            ButtonEnabled = true;
        }

        public async void SignUp()
        {
            ButtonEnabled = false;
            await _navigation.PushAsync(new SignUpPage());
            ButtonEnabled = true;
        }

        public async void SignIn()
        {
            ButtonEnabled = false;
            await _navigation.PushAsync(new SignInPage());
            ButtonEnabled = true;
        }
    }
}
