using System;
using System.Windows.Input;
using Fourplaces.Model;
using Fourplaces.Services;
using Plugin.Connectivity;
using Storm.Mvvm;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    class SignUpViewModel : ViewModelBase
    {
        private readonly INavigation _navigation;

        private readonly IPlaceService _pService = App.PService;

        private bool _signUpButtonEnabled;

        public bool SignUpButtonEnabled
        {
            get => _signUpButtonEnabled;
            set => SetProperty(ref _signUpButtonEnabled, value);
        }

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
            SignUpButtonEnabled = true;
        }

        public async void SignUp()
        {
            SignUpButtonEnabled = false;
            if (CrossConnectivity.Current.IsConnected)
            {
                if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(Password))
                {
                    await Application.Current.MainPage.DisplayAlert("Erreur", "Champs vides !", "Ok");
                }
                else
                {
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
                        await Application.Current.MainPage.DisplayAlert("Inscription", "L'inscription a bien été effectuée!", "Ok");
                        await _navigation.PopToRootAsync();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Erreur", registerResult.ErrorMessage, "Ok");
                    }
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erreur",
                    "Une connexion internet est nécessaire pour s'inscrire.", "Ok");
            }

            SignUpButtonEnabled = true;
        }

    }
}
