using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Fourplaces.Model;
using Fourplaces.Services;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Storm.Mvvm;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    class AddPlaceViewModel : ViewModelBase
    {
        private readonly IPlaceService _pService = App.PService;

        private readonly INavigation _navigation;

        private ICommand _addImageCommand;

        public ICommand AddImageCommand
        {
            get => _addImageCommand;
            set => SetProperty(ref _addImageCommand, value);
        }

        private ICommand _addPlaceCommand;

        public ICommand AddPlaceCommand
        {
            get => _addPlaceCommand;
            set => SetProperty(ref _addPlaceCommand, value);
        }

        private ICommand _takePictureCommand;

        public ICommand TakePictureCommand
        {
            get => _takePictureCommand;
            set => SetProperty(ref _takePictureCommand, value);
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

        private string _latitude;

        public string Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        private string _longitude;

        public string Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        private int _imageId;

        public AddPlaceViewModel(INavigation navigation)
        {
            _navigation = navigation;
            AddImageCommand = new Command(PickPicture);
            TakePictureCommand = new Command(TakePhoto);

            AddPlaceCommand = new Command(AddPlace);
            _imageId = -1;
        }

        private void TakePhoto()
        {
            AddImage(true);
        }

        private void PickPicture()
        {
            AddImage(false);
        }

        private async void AddPlace()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (_imageId != -1)
                {
                    if (string.IsNullOrEmpty(Description) || string.IsNullOrEmpty(Title) ||
                        string.IsNullOrEmpty(Latitude) || string.IsNullOrEmpty(Longitude))
                    {
                        await Application.Current.MainPage.DisplayAlert("Erreur", "Champs vides !", "Ok");
                    }
                    else
                    {
                        CreatePlaceRequest request = new CreatePlaceRequest()
                        {
                            ImageId = _imageId,
                            Description = Description,
                            Title = Title,
                            Latitude = Convert.ToDouble(Latitude),
                            Longitude = Convert.ToDouble(Longitude)
                        };
                        Response res = await _pService.PostPlace(request);
                        if (res.IsSuccess)
                        {
                            await Application.Current.MainPage.DisplayAlert("Succès", "Le lieu a bien été ajouté !",
                                "Ok");
                            await _navigation.PopAsync();
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Erreur", res.ErrorMessage, "Ok");
                        }
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erreur", "Veuillez ajouter une image !", "Ok");
                }
            }

            else
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", "Une connexion internet est nécessaire pour ajouter un lieu", "Ok");
            }
        }

        public bool IsLocationAvailable()
        {
            if (!CrossGeolocator.IsSupported)
                return false;

            return CrossGeolocator.Current.IsGeolocationAvailable;
        }


        private async void AddImage(bool takePicture)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                MediaFile file;
                if (takePicture)
                {
                    await CrossMedia.Current.Initialize();

                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    {
                        await Application.Current.MainPage.DisplayAlert("Pas d'appareil photo",
                            "Pas d'appareil photo disponible.", "OK");
                        return;
                    }

                    file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small
                    });
                }
                else
                {
                    file = await CrossMedia.Current.PickPhotoAsync();
                }

                if (file != null)
                {
                    Response<ImageItem> res = await _pService.PostImage(file);
                    if (res.IsSuccess)
                    {
                        await Application.Current.MainPage.DisplayAlert("Succès", "L'image a bien été ajoutée !", "Ok");
                        _imageId = res.Data.Id;
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Erreur", res.ErrorMessage, "Ok");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erreur", "Impossible d'ajouter l'image", "Ok");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", "Une connexion internet est nécessaire pour ajouter l'image.", "Ok");
            }
        }

        public async Task<Position> GetCurrentLocation()
        {
            Position position = null;
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;
                if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                {
                    return null;
                }

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location: " + ex);
            }

            if (position == null)
                return null;

            var output =
                $"Time: {position.Timestamp} \nLat: {position.Latitude} \nLong: {position.Longitude} \nAltitude: {position.Altitude} \nAltitude Accuracy: {position.AltitudeAccuracy} \nAccuracy: {position.Accuracy} \nHeading: {position.Heading} \nSpeed: {position.Speed}";

            Debug.WriteLine(output);

            return position;
        }

        public override async Task OnResume()
        {
            Position pos = await GetCurrentLocation();
            if (pos != null)
            {
                Latitude = pos.Latitude.ToString(CultureInfo.InvariantCulture);
                Longitude = pos.Longitude.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}