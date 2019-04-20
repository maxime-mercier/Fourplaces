using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Fourplaces.Model;
using Fourplaces.Pages;
using Fourplaces.Services;
using MonkeyCache.SQLite;
using Plugin.Connectivity;
using Storm.Mvvm;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    class UserPageViewModel : ViewModelBase
    {
        private readonly INavigation _navigation;

        private readonly IPlaceService _pService = App.PService;

        private string _userImage;

        public string UserImage
        {
            get => _userImage;
            set => SetProperty(ref _userImage, value);
        }

        private string _email;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private ICommand _goToUserNamePage;

        public ICommand GoToUserNamePage
        {
            get => _goToUserNamePage;
            set => SetProperty(ref _goToUserNamePage, value);
        }

        private ICommand _goToPasswordPageCommand;

        public ICommand GoToPasswordPageCommand
        {
            get => _goToPasswordPageCommand;
            set => SetProperty(ref _goToPasswordPageCommand, value);
        }

        private ICommand _disconnectCommand;

        public ICommand DisconnectCommand
        {
            get => _disconnectCommand;
            set => SetProperty(ref _disconnectCommand, value);
        }

        

        private bool _buttonEnabled;

        public bool ButtonEnabled
        {
            get => _buttonEnabled;
            set => SetProperty(ref _buttonEnabled, value);
        }

        public UserPageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            UserImage = "userLogo.png";
            GoToUserNamePage = new Command(GoToModifyUserNamePage);
            GoToPasswordPageCommand = new Command(GoToPasswordPage);
            DisconnectCommand = new Command(Disconnect);
            ButtonEnabled = true;
        }

        private async void Disconnect()
        {
            ButtonEnabled = false;
            Barrel.Current.Empty(key: App.UserCacheUrl);
            Barrel.Current.EmptyExpired();
            App.AccessToken = null;
            App.RefreshToken = null;
            List<Page> existingPages = _navigation.NavigationStack.ToList();
            _navigation.InsertPageBefore(new HomePage(), existingPages[0]);
            await _navigation.PopToRootAsync();
            ButtonEnabled = true;
        }

        private async void GoToPasswordPage()
        {
            ButtonEnabled = false;
            await _navigation.PushAsync(new ModifyUserPage(true));
            ButtonEnabled = true;
        }

        private async void GoToModifyUserNamePage()
        {
            ButtonEnabled = false;
            await _navigation.PushAsync(new ModifyUserPage(false));
            ButtonEnabled = true;
        }

        private void SetUser(UserItem item)
        {
            if (item.ImageId != null) UserImage = "https://td-api.julienmialon.com/images/" + item.ImageId;
            Email = item.Email;
            Name = item.FirstName + " " + item.LastName;
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            Barrel.Current.GetKeys();
            UserItem item = null;
            if (!CrossConnectivity.Current.IsConnected)
            {
                if (!Barrel.Current.IsExpired(App.UserCacheUrl))
                    item = Barrel.Current.Get<UserItem>(App.UserCacheUrl);
                else
                    Barrel.Current.Empty(key: App.UserCacheUrl);
            }
            else
            {
                Response<UserItem> response = await _pService.GetUser();
                if (response.IsSuccess)
                {
                    item = response.Data;
                    Barrel.Current.Add(App.UserCacheUrl, item, TimeSpan.FromDays(App.CacheTime));
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erreur", response.ErrorMessage, "Ok");
                }
            }

            if (item != null)
            {
                SetUser(item);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", "Impossible d'accéder au profil !", "Ok");
                await _navigation.PopAsync();
            }
        }
    }
}