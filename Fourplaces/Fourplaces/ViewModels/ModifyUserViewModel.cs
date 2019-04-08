using System;
using System.Windows.Input;
using Fourplaces.Model;
using Fourplaces.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Storm.Mvvm;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    class ModifyUserViewModel : ViewModelBase
    {

        private readonly INavigation _navigation;

        private readonly IPlaceService _pService = App.PService;

        private bool _modifyUserInfo;

        public bool ModifyUserInfo
        {
            get => _modifyUserInfo;
            set => SetProperty(ref _modifyUserInfo, value);
        }

        private bool _modifyPassword;

        public bool ModifyPassword
        {
            get => _modifyPassword;
            set => SetProperty(ref _modifyPassword, value);
        }

        private string _newFirstName;

        public string NewFirstName
        {
            get => _newFirstName;
            set => SetProperty(ref _newFirstName, value);
        }

        private string _newLastName;

        public string NewLastName
        {
            get => _newLastName;
            set => SetProperty(ref _newLastName, value);
        }

        private ICommand _modifyUserCommand;

        public ICommand ModifyUserCommand
        {
            get => _modifyUserCommand;
            set => SetProperty(ref _modifyUserCommand, value);
        }

        private string _oldPassword;

        public string OldPassword
        {
            get => _oldPassword;
            set => SetProperty(ref _oldPassword, value);
        }

        private string _newPassword;

        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        private ICommand _modifyPasswordCommand;

        public ICommand ModifyPasswordCommand
        {
            get => _modifyPasswordCommand;
            set => SetProperty(ref _modifyPasswordCommand, value);
        }

        private ICommand _addImageCommand;

        public ICommand AddImageCommand
        {
            get => _addImageCommand;
            set => SetProperty(ref _addImageCommand, value);
        }

        private int? _imageId;

        public ModifyUserViewModel(bool isModifyPasswordPage, INavigation navigation)
        {
            _navigation = navigation;
            _imageId = null;
            ModifyPassword = isModifyPasswordPage;
            ModifyUserInfo = !isModifyPasswordPage;
            ModifyUserCommand = new Command(ModifyUser);
            ModifyPasswordCommand = new Command(ChangePassword);
            AddImageCommand = new Command(AddImage);
        }

        private async void AddImage()
        {
            MediaFile file = await CrossMedia.Current.PickPhotoAsync();
            if (file != null)
            {
                Response<ImageItem> res = await _pService.PostImage(file);
                if (res.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert("Succès", "L'image a bien été ajoutée !", "Ok");
                    _imageId = res.Data.Id;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erreur", res.ErrorMessage, "Ok");
                }
            }
        }

        private async void ChangePassword()
        {
            if (String.IsNullOrEmpty(OldPassword) || String.IsNullOrEmpty(NewPassword))
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", "Champs vides !", "Ok");
            }
            else
            {
                UpdatePasswordRequest updatePasswordRequest = new UpdatePasswordRequest()
                {
                    NewPassword = NewPassword,
                    OldPassword = OldPassword
                };
                Response res = await _pService.PatchPassword(updatePasswordRequest);
                if (res.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert("Modification", "Votre mot de passe a bien été modifié !", "Ok");
                    await _navigation.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erreur", res.ErrorMessage, "Ok");
                }
            }
        }

        private async void ModifyUser()
        {
            UpdateProfileRequest request = new UpdateProfileRequest()
            {
                ImageId = _imageId,
                FirstName = NewFirstName,
                LastName = NewLastName,
            };
            Response<UserItem> res = await _pService.PatchProfile(request);
            if (res.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Modification", "Votre profil a bien été modifié !", "Ok");
                await _navigation.PopAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", res.ErrorMessage, "Ok");
            }
        }
    }
}
