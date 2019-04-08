using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Fourplaces.Model;
using Fourplaces.Pages;
using Fourplaces.Services;
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

        public PlaceItemSummary SelectedPlace
        {
            get => _selectedPlace;
            set
            {
                _selectedPlace = value;
                GoToDetailPage();
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
            throw new NotImplementedException();
        }


        public async void GoToDetailPage()
        {
            await _navigation.PushAsync(new PageDetail(SelectedPlace));
        }

        public async void GoToUserPage()
        {
            await _navigation.PushAsync(new UserPage());
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