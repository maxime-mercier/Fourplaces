using System;
using System.Collections.Generic;
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

namespace Fourplaces.ViewModels
{
    class PlaceListViewModel : ViewModelBase
    {
        public ObservableCollection<PlaceItemSummary> Places { get; set; }

        private PlaceItemSummary _selectedPlace;

        private readonly INavigation _navigation;

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
                if (!Barrel.Current.IsExpired(App.PlaceListCacheUrl))
                {
                    var list = Barrel.Current.Get<List<PlaceItemSummary>>(App.PlaceListCacheUrl);
                    Places.Clear();
                    foreach (PlaceItemSummary item in list)
                    {
                        Places.Add(item);
                    }
                }
                else
                {
                    Places.Clear();
                    Barrel.Current.Empty(key: App.PlaceListCacheUrl);
                    await Application.Current.MainPage.DisplayAlert("Erreur",
                        "Impossible d'afficher la liste de lieux !", "Ok");
                }
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

                    Barrel.Current.Add(App.PlaceListCacheUrl, placesResponse.Data, TimeSpan.FromDays(App.CacheTime));
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erreur", placesResponse.ErrorMessage, "Ok");
                }
            }
        }
    }
}