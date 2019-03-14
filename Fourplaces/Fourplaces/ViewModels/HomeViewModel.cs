using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Storm.Mvvm;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    class HomeViewModel : ViewModelBase
    {

        public INavigation Navigation { get; set; }

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

        public HomeViewModel(INavigation navigation)
        {
            Navigation = navigation;
            SignInCommand = new Command(SignIn);
            SignUpCommand = new Command(SignUp);
        }

        public async void SignUp()
        {
            await Navigation.PushAsync(new SignUpPage());
        }

        public void SignIn()
        {

        }
    }
}
