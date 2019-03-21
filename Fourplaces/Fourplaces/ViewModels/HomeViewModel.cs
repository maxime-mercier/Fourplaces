﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
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

        public HomeViewModel(INavigation navigation)
        {
            _navigation = navigation;
            SignInCommand = new Command(SignIn);
            SignUpCommand = new Command(SignUp);
        }

        public async void SignUp()
        {
            await _navigation.PushAsync(new SignUpPage());
        }

        public async void SignIn()
        {
            await _navigation.PushAsync(new SignInPage());
        }
    }
}
