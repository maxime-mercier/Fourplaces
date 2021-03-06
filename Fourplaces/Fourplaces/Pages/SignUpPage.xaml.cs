﻿using Fourplaces.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fourplaces.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
            BindingContext = new SignUpViewModel(Navigation);
        }
    }
}