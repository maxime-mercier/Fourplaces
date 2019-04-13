using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Fourplaces.Model;
using Fourplaces.Pages;
using Fourplaces.Services;
using MonkeyCache.SQLite;
using Plugin.Connectivity;
using Plugin.Media;
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

        private readonly string _cacheUrl = "placeListCache";

        public PlaceItemSummary SelectedPlace
        {
            get => _selectedPlace;
            set
            {
                _selectedPlace = value;
                GoToDetailPage(_selectedPlace.Id);
            }
        }

        private ICommand _goToUserPageCommand;

        public ICommand GoToUserPageCommand
        {
            get => _goToUserPageCommand;
            set => SetProperty(ref _goToUserPageCommand, value);
        }

        private ICommand _goToAddPlacePageCommand;

        public ICommand GoToAddPlacePageCommand
        {
            get => _goToAddPlacePageCommand;
            set => SetProperty(ref _goToAddPlacePageCommand, value);
        }

        private readonly IPlaceService _pService = App.PService;


        public PlaceListViewModel(INavigation navigation)
        {
            _navigation = navigation;

            Places = new ObservableCollection<PlaceItemSummary>();
            GoToUserPageCommand = new Command(GoToUserPage);
            GoToAddPlacePageCommand = new Command(GoToAddPlacePage);
        }

        private async void GoToAddPlacePage()
        {
            await _navigation.PushAsync(new AddPlacePage());
        }


        public async void GoToDetailPage(int selectedPlaceId)
        {
            await _navigation.PushAsync(new PageDetail(selectedPlaceId));
        }

        public async void GoToUserPage()
        {
            await _navigation.PushAsync(new UserPage());
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            if (!CrossConnectivity.Current.IsConnected)
            {
                Places = Barrel.Current.Get<ObservableCollection<PlaceItemSummary>>(_cacheUrl);
            }
            else
            {
                Response<List<PlaceItemSummary>> placesResponse = await _pService.GetPlaces();
                if (placesResponse.IsSuccess)
                {
                    Places.Clear();
                    foreach (PlaceItemSummary item in placesResponse.Data)
                    {
                        Places.Add(item);
                    }
                    Barrel.Current.Add(_cacheUrl, Places, TimeSpan.FromHours(1));
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erreur", placesResponse.ErrorMessage, "Ok");
                }
            }
        }

        
    }
}