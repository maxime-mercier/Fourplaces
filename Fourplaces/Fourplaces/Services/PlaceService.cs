using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Fourplaces.Common.Api.Dtos;
using Fourplaces.Model;
using Newtonsoft.Json;


namespace Fourplaces.Services
{
    public interface IPlaceService
    {
        Task<Response<List<PlaceItemSummary>>> GetPlaces();

        Task<Response<PlaceItem>> GetPlace(int placeId);

        void PostImages();

        void GetImage(int id);
    }

    public class PlaceService : IPlaceService
    {
        public async Task<Response<List<PlaceItemSummary>>> GetPlaces()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("https://td-api.julienmialon.com/places");
                    response.EnsureSuccessStatusCode();
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Response<List<PlaceItemSummary>>>(responseBody);
                }
                catch (HttpRequestException e)
                {
                    return new Response<List<PlaceItemSummary>>
                    {
                        IsSuccess = false,
                        ErrorMessage = e.Message
                    };
                }
            }
        }

        public async Task<Response<PlaceItem>> GetPlace(int placeId)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("https://td-api.julienmialon.com/places/" + placeId);
                    response.EnsureSuccessStatusCode();
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Response<PlaceItem>>(responseBody);
                }
                catch (HttpRequestException e)
                {
                    return new Response<PlaceItem>
                    {
                        IsSuccess = false,
                        ErrorMessage = e.Message
                    };
                }
            }
        }

        public void PostImages()
        {
            throw new NotImplementedException();
        }

        public void GetImage(int id)
        {
            throw new NotImplementedException();
        }
    }
}
