using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Fourplaces.Model;
using Fourplaces.Pages;
using Fourplaces.Services;
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

        public UserPageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            UserImage = "userLogo.png";
            GoToUserNamePage = new Command(GoToModifyUserNamePage);
            GoToPasswordPageCommand = new Command(GoToPasswordPage);
        }

        private async void GoToPasswordPage()
        {
            await _navigation.PushAsync(new ModifyUserPage(true));
        }

        private async void GoToModifyUserNamePage()
        {
            await _navigation.PushAsync(new ModifyUserPage(false));
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            Response<UserItem> response = await _pService.GetUser();
            if (response.IsSuccess)
            {
                UserItem item = response.Data;
                if (item.ImageId != null) UserImage = "https://td-api.julienmialon.com/images/" + item.ImageId;
                Email = item.Email;
                Name = item.FirstName + " " + item.LastName;
            }
            else

            {
                await Application.Current.MainPage.DisplayAlert("Erreur", response.ErrorMessage, "Ok");
            }
        }
    }
}
