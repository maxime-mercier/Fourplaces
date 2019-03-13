using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Fourplaces.Common.Api.Dtos;
using Fourplaces.Model;
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

        //public INavigationService Navigation { get; set;}

        public INavigation Navigation { get; set; }

        public PlaceItemSummary SelectedPlace
        {
            get => _selectedPlace;
            set
            {
                _selectedPlace = value;
                GoToDetailPage();
            }
        }

        private PlaceService PService = new PlaceService();


        public PlaceListViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Places = new ObservableCollection<PlaceItemSummary>();
            //Get images https://td-api.julienmialon.com/swagger/images/ + image_id dans Image_Src
        }


        public async void GoToDetailPage()
        {
            Dictionary<string, object> dico = new Dictionary<string, object>();
            dico.Add("SelectedPlace", SelectedPlace);
            await Navigation.PushAsync(new PageDetail(SelectedPlace));

            /*IDataService*/
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            Response<List<PlaceItemSummary>> PlacesResponse = await PService.GetPlaces();
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