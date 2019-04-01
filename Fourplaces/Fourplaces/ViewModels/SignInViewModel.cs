using System.Windows.Input;
using Fourplaces.Model;
using Fourplaces.Pages;
using Fourplaces.Services;
using Storm.Mvvm;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    class SignInViewModel : ViewModelBase
    {
    
        private readonly INavigation _navigation;

        private readonly IPlaceService _pService = App.PService;

        private bool _signInButtonEnabled;

        public bool SignInButtonEnabled
        {
            get => _signInButtonEnabled;
            set => SetProperty(ref _signInButtonEnabled, value);
        }

        private string _email;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _password;

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private ICommand _signInCommand;

        public ICommand SignInCommand
        {
            get => _signInCommand;
            set => SetProperty(ref _signInCommand, value);
        }

        public SignInViewModel(INavigation navigation)
        {
            _navigation = navigation;
            SignInButtonEnabled = true;
            SignInCommand = new Command(SignIn);
        }

        public async void SignIn()
        {
            SignInButtonEnabled = false;
            LoginRequest request = new LoginRequest()
            {
                Email = Email,
                Password = Password
            };
            Response<LoginResult> registerResult = await _pService.PostLogin(request);
            if (registerResult.IsSuccess)
            {
                //Disable button pour empecher d'ouvrir plusieurs pages 

                //var access = _pService.AccessToken;
                await Application.Current.MainPage.DisplayAlert("Connexion", "La connexion a bien été effectuée!", "Ok");
                await _navigation.PushAsync(new PlaceListPage());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", registerResult.ErrorMessage, "Ok");
                SignInButtonEnabled = true;
            }
        }
    }
}
