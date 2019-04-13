using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Fourplaces.Model;
using Fourplaces.Services;
using Plugin.Connectivity;
using Storm.Mvvm;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    class AddCommentViewModel : ViewModelBase
    {

        private readonly INavigation _navigation;

        private readonly int _placeId;

        private string _comment;

        public string Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
        }

        private ICommand _postCommentCommand;

        public ICommand PostCommentCommand
        {
            get => _postCommentCommand;
            set => SetProperty(ref _postCommentCommand, value);
        }

        private readonly IPlaceService _pService = App.PService;

        public AddCommentViewModel(int placeID, INavigation navigation)
        {
            PostCommentCommand = new Command(PostComment);
            _placeId = placeID;
            _navigation = navigation;
        }

        private async void PostComment()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (string.IsNullOrEmpty(Comment))
                {
                    await Application.Current.MainPage.DisplayAlert("Erreur", "Veuillez ajouter un commentaire", "Ok");
                }
                else
                {
                    CreateCommentRequest request = new CreateCommentRequest() {Text = Comment};
                    Response addCommentResult = await _pService.PostComment(request, _placeId);
                    if (addCommentResult.IsSuccess)
                    { 
                        await Application.Current.MainPage.DisplayAlert("Succès", "Le commentaire à bien été ajouté!", "Ok");
                        await _navigation.PopAsync();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Erreur", addCommentResult.ErrorMessage, "Ok");
                    }
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", "Une connexion internet est nécessaire pour ajouter un commentaire.", "Ok");
            }
        }
    }
}
