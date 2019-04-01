using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Fourplaces.Model;
using Fourplaces.Pages;
using Fourplaces.Services;
using Storm.Mvvm;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    class PlaceListViewModel : ViewModelBase
    {
        //private readonly INavigationService _navigationService;

        public ObservableCollection<PlaceItemSummary> Places { get; set; }

        private PlaceItemSummary _selectedPlace;

        //public INavigationService _navigation { get; set;}

        private readonly INavigation _navigation;

        public PlaceItemSummary SelectedPlace
        {
            get => _selectedPlace;
            set
            {
                _selectedPlace = value;
                GoToDetailPage();
            }
        }

        private readonly IPlaceService _pService = App.PService;


        public PlaceListViewModel(INavigation navigation)
        {
            _navigation = navigation;
            Places = new ObservableCollection<PlaceItemSummary>();
            //Get images https://td-api.julienmialon.com/swagger/images/ + image_id dans Image_Src
        }


        public async void GoToDetailPage()
        {
            await _navigation.PushAsync(new PageDetail(SelectedPlace));

            /*IDataService*/
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            Response<List<PlaceItemSummary>> PlacesResponse = await _pService.GetPlaces();
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