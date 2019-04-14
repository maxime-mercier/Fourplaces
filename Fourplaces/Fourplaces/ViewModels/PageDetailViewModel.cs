using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Fourplaces.Model;
using Fourplaces.Pages;
using Fourplaces.Services;
using MonkeyCache.SQLite;
using Plugin.Connectivity;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Storm.Mvvm;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Fourplaces.ViewModels
{
    class PageDetailViewModel : ViewModelBase
    {
        private readonly INavigation _navigation;

        private string _imageSrc;

        public Map MyMap { get; set; }

        public ObservableCollection<CommentItem> Comments { get; set; }

        private int CurrentPlaceId { get; set; }

        private ICommand _addCommentCommand;

        public ICommand AddCommentCommand
        {
            get => _addCommentCommand;
            set => SetProperty(ref _addCommentCommand, value);
        }

        public string ImageSrc
        {
            get => _imageSrc;
            set => SetProperty(ref _imageSrc, value);
        }

        private string _title;


        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _description;


        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private readonly IPlaceService _pService = App.PService;

        private bool _buttonEnabled;

        public bool ButtonEnabled
        {
            get => _buttonEnabled;
            set => SetProperty(ref _buttonEnabled, value);
        }

        public PageDetailViewModel(int selectedPlaceId, Map myMap, INavigation navigation)
        {
            CurrentPlaceId = selectedPlaceId;
            MyMap = myMap;
            _navigation = navigation;
            Comments = new ObservableCollection<CommentItem>();
            AddCommentCommand = new Command(AddComment);
            ButtonEnabled = true;
        }

        private async void AddComment()
        {
            ButtonEnabled = false;
            await _navigation.PushAsync(new AddCommentPage(CurrentPlaceId));
            ButtonEnabled = true;
        }

        private async Task<PermissionStatus> CheckLocationPermission()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                {
                    await Application.Current.MainPage.DisplayAlert("Localisation",
                        "La localisation est nécessaire pour afficher la position du lieu.", "Ok");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                if (results.ContainsKey(Permission.Location))
                    status = results[Permission.Location];
            }

            return status;
        }

        private void SetPlace(PlaceItem place)
        {
            Description = place.Description;
            Title = place.Title;
            ImageSrc = "https://td-api.julienmialon.com/images/" + place.ImageId;
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(place.Latitude, place.Longitude),
                Distance.FromKilometers(1)));
            var pin = new Pin()
            {
                Position = new Position(place.Latitude, place.Longitude),
                Label = place.Title
            };
            MyMap.Pins.Add(pin);
            foreach (var comment in place.Comments)
            {
                Comments.Add(comment);
            }
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            PlaceItem place = null;
            if (!CrossConnectivity.Current.IsConnected)
            {
                if (!Barrel.Current.IsExpired(App.PlaceDetailCacheUrl + CurrentPlaceId))
                    place = Barrel.Current.Get<PlaceItem>(App.PlaceDetailCacheUrl + CurrentPlaceId);
                else
                {
                    Barrel.Current.Empty(key: App.PlaceDetailCacheUrl + CurrentPlaceId);
                }
            }
            else
            {
                Response<PlaceItem> placesResponse = await _pService.GetPlace(CurrentPlaceId);
                if (placesResponse.IsSuccess)
                {
                    place = placesResponse.Data;
                    Barrel.Current.Add(App.PlaceDetailCacheUrl + CurrentPlaceId, place, TimeSpan.FromHours(1));
                }
                else

                {
                    await Application.Current.MainPage.DisplayAlert("Erreur", placesResponse.ErrorMessage, "Ok");
                    await _navigation.PopAsync();
                }
            }

            if (place != null)
            {
                SetPlace(place);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erreur",
                    "Impossible d'accéder au détail du lieu sélectionné", "Ok");
                await _navigation.PopAsync();
            }
        }
    }
}