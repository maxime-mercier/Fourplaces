using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Fourplaces.Model;
using Fourplaces.Pages;
using Fourplaces.Services;
using MonkeyCache.SQLite;
using Plugin.Connectivity;
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

        
        public string Title {
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
        private string _cacheUrl;

        public PageDetailViewModel(int selectedPlaceId, Map myMap, INavigation navigation)
        {
            CurrentPlaceId = selectedPlaceId;
            MyMap = myMap;
            _navigation = navigation;
            Comments = new ObservableCollection<CommentItem>();
            AddCommentCommand = new Command(AddComment);
            _cacheUrl = "placeDetailCache" + CurrentPlaceId;
        }

        private async void AddComment()
        {
            await _navigation.PushAsync(new AddCommentPage(CurrentPlaceId));
        }



        public override async Task OnResume()
        {
            await base.OnResume();
            PlaceItem place = null;
            if (!CrossConnectivity.Current.IsConnected)
            {
                place = Barrel.Current.Get<PlaceItem>(_cacheUrl);
            }
            else
            {
                Response<PlaceItem> placesResponse = await _pService.GetPlace(CurrentPlaceId);
                if (placesResponse.IsSuccess)
                {
                    place = placesResponse.Data;
                    Barrel.Current.Add(_cacheUrl, place, TimeSpan.FromHours(1));
                }
                else

                {
                    await Application.Current.MainPage.DisplayAlert("Erreur", placesResponse.ErrorMessage, "Ok");
                }
            }

            if (place != null)
            {
                Description = place.Description;
                Title = place.Title;
                ImageSrc = "https://td-api.julienmialon.com/images" + place.ImageId;
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(place.Latitude, place.Longitude), Distance.FromKilometers(1)));
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
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", "Impossible d'accéder au détail du lieu sélectionné", "Ok");
                await _navigation.PopAsync();
            }
        }
    }
}
