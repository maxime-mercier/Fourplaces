using System;
using System.Threading.Tasks;
using Fourplaces.Common.Api.Dtos;
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

        private PlaceService PService = new PlaceService();

        public PageDetailViewModel(PlaceItemSummary SelectedPlace, Map myMap)
        {
            CurrentPlace = SelectedPlace;
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
        }

        public async override Task OnResume()
        {
            await base.OnResume();
            Response<PlaceItem> PlacesResponse = await PService.GetPlaces();
            if (PlacesResponse.IsSuccess)
            {
                Places.Clear();
                foreach (PlaceItemSummary item in PlacesResponse.Data)
                {
                    Places.Add(item);
                }
            }
            else

            {
                Console.WriteLine(PlacesResponse.ErrorMessage);
            }

        }
    }
}
