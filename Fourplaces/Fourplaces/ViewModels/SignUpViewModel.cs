using System.Windows.Input;
using Fourplaces.Model;
using Storm.Mvvm;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    class SignUpViewModel : ViewModelBase
    {
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

        public SignUpViewModel()
        {
            SignUpCommand = new Command(SignUp);
        }

        public async void SignUp()
        {
            RegisterRequest request = new RegisterRequest
            {
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                Password = Password
            };
            var email = Email;
            var first = FirstName;
            var last = LastName;
            var password = Password;
        }

    }
}
