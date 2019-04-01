﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Fourplaces.Model;
using Newtonsoft.Json;


namespace Fourplaces.Services
{
    public interface IPlaceService
    {
        string AccessToken { get; }

        string RefreshToken { get; }

        int ExpiresIn { get; }

        Task<Response<List<PlaceItemSummary>>> GetPlaces();

        Task<Response<PlaceItem>> GetPlace(int placeId);

        void PostImages();

        void GetImage(int id);

        Task<Response<LoginResult>> PostRegister(RegisterRequest request);

        Task<Response<LoginResult>> PostLogin(LoginRequest request);

        Task<Response> PostComment(CreateCommentRequest commentRequest, int placeId);
    }

    public class PlaceService : IPlaceService
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; }
        public int ExpiresIn { get; set; }

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

        public async Task<Response<LoginResult>> PostRegister(RegisterRequest request)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var registerRequest = JsonConvert.SerializeObject(request);
                    HttpResponseMessage response = await client.PostAsync("https://td-api.julienmialon.com/auth/register", new StringContent(registerRequest, Encoding.UTF8, "application/json-patch+json"));
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response<LoginResult> res = JsonConvert.DeserializeObject<Response<LoginResult>>(responseBody);
                    return res;
                }
                catch (HttpRequestException e)
                {
                    return new Response<LoginResult>()
                    {
                        IsSuccess = false,
                        ErrorMessage = e.Message
                    };
                }
            }
        }

        public async Task<Response<LoginResult>> PostLogin(LoginRequest request)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var loginRequest = JsonConvert.SerializeObject(request);
                    HttpResponseMessage response = await client.PostAsync("https://td-api.julienmialon.com/auth/login", new StringContent(loginRequest, Encoding.UTF8, "application/json-patch+json"));
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response<LoginResult> res = JsonConvert.DeserializeObject<Response<LoginResult>>(responseBody);
                    if (res.IsSuccess)
                    {
                        AccessToken = res.Data.AccessToken;
                        ExpiresIn = res.Data.ExpiresIn;
                    }
                    return res;
                }
                catch (HttpRequestException e)
                {
                    return new Response<LoginResult>()
                    {
                        IsSuccess = false,
                        ErrorMessage = e.Message
                    };
                }
            }
        }

        public async Task<Response> PostComment(CreateCommentRequest createCommentRequest, int placeId)
        {
            using (HttpClient client = new HttpClient())
            {
                var commentRequest = JsonConvert.SerializeObject(createCommentRequest);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(App.TokenScheme, AccessToken);
                HttpResponseMessage response = await client.PostAsync("https://td-api.julienmialon.com/places/" + placeId + "/comments", new StringContent(commentRequest, Encoding.UTF8, "application/json-patch+json"));
                var responseBody = await response.Content.ReadAsStringAsync();
                Response res = JsonConvert.DeserializeObject<Response>(responseBody);
                return res;
            }
        }

    }
}
