using System;
using System.Windows.Input;
using Fourplaces.Model;
using Fourplaces.Services;
using Storm.Mvvm;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    class SignUpViewModel : ViewModelBase
    {
        private readonly INavigation _navigation;

        private readonly IPlaceService _pService = App.PService;

        private string _email;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _firstName;

        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        private string _lastName;

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        private string _password;

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private ICommand _signUpCommand;

        public ICommand SignUpCommand
        {
            get => _signUpCommand;
            set => SetProperty(ref _signUpCommand, value);
        }

        public SignUpViewModel(INavigation navigation)
        {
            _navigation = navigation;
            SignUpCommand = new Command(SignUp);
        }

        public async void SignUp()
        {
            /*if (Email.Length <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", "Email non valide !", "ok", "Cancel");
            }*/
            RegisterRequest request = new RegisterRequest
            {
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                Password = Password
            };
            Response<LoginResult> registerResult = await _pService.PostRegister(request);
            if (registerResult.IsSuccess)
            {
                var access = _pService.AccessToken;
                await Application.Current.MainPage.DisplayAlert("Inscription", "L'inscription a bien été effectuée!", "Ok");
                //await _navigation.PushAsync(new SignInPage());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", registerResult.ErrorMessage + " " + _pService.AccessToken, "Ok");
            }
        }

    }
}
