using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Fourplaces.Model;
using Fourplaces.Services;
using Storm.Mvvm;
using Xamarin.Forms.Maps;

namespace Fourplaces.ViewModels
{
    class PageDetailViewModel : ViewModelBase
    {
        private string _imageSrc;

        public Map MyMap { get; set; }

        public ObservableCollection<CommentItem> Comments { get; set; }

        public PlaceItemSummary CurrentPlace { get; set; }

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

        public PageDetailViewModel(PlaceItemSummary selectedPlace, Map myMap)
        {
            CurrentPlace = selectedPlace;
            Description = CurrentPlace.Description;
            Title = CurrentPlace.Title;
            ImageSrc = CurrentPlace.ImageSrc;
            MyMap = myMap;
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(CurrentPlace.Latitude, CurrentPlace.Longitude), Distance.FromKilometers(1)));
            var pin = new Pin()
            {
                Position = new Position(CurrentPlace.Latitude, CurrentPlace.Longitude),
                Label = CurrentPlace.Title
            };
            MyMap.Pins.Add(pin);
            Comments = new ObservableCollection<CommentItem>();
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            Response<PlaceItem> placesResponse = await _pService.GetPlace(CurrentPlace.Id);
            if (placesResponse.IsSuccess)
            {
                foreach (var comment in placesResponse.Data.Comments)
                {
                    Comments.Add(comment);
                }
            }
            else

            {
                Console.WriteLine(placesResponse.ErrorMessage);
            }

        }
    }
}
