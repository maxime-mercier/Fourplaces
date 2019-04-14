using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Fourplaces.Model;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;


namespace Fourplaces.Services
{
    public interface IPlaceService
    {
        string AccessToken { get; }

        string RefreshToken { get; }

        int ExpiresIn { get; }

        Task<Response<List<PlaceItemSummary>>> GetPlaces();

        Task<Response<PlaceItem>> GetPlace(int placeId);

        Task<Response<ImageItem>> PostImage(MediaFile image);

        Task<Response<LoginResult>> PostRegister(RegisterRequest request);

        Task<Response<LoginResult>> PostLogin(LoginRequest request);

        Task<Response> PostComment(CreateCommentRequest commentRequest, int placeId);

        Task<Response<UserItem>> GetUser();

        Task<Response<UserItem>> PatchProfile(UpdateProfileRequest updateProfileRequest);

        Task<Response> PatchPassword(UpdatePasswordRequest updatePasswordRequest);

        Task<Response> PostPlace(CreatePlaceRequest createPlaceRequest);
    }

    public class PlaceService : IPlaceService
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; }
        public int ExpiresIn { get; set; }


        public async Task<string> GenericHttpRequest<T>(string uri, string httpRequestMethod, bool token, T content)
        {
            using (HttpClient client = new HttpClient())
            {
                if (token)
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(App.TokenScheme, AccessToken);
                using (HttpRequestMessage requestMessage =
                    new HttpRequestMessage(new HttpMethod(httpRequestMethod), uri))
                {
                    if (content != null)
                    {
                        var body = JsonConvert.SerializeObject(content);
                        requestMessage.Content = new StringContent(body, Encoding.UTF8, "application/json");
                    }

                    HttpResponseMessage responseMessage = await client.SendAsync(requestMessage);
                    //responseMessage.EnsureSuccessStatusCode();
                    return await responseMessage.Content.ReadAsStringAsync();
                }
            }
        }


        public async Task<Response<List<PlaceItemSummary>>> GetPlaces()
        {
            try
            {
                string uri = "https://td-api.julienmialon.com/places";
                var responseBody = await GenericHttpRequest<object>(uri, "GET", true, null);
                return JsonConvert.DeserializeObject<Response<List<PlaceItemSummary>>>(responseBody);
            }
            catch (Exception e)
            {
                return new Response<List<PlaceItemSummary>>
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message
                };
            }
        }

        public async Task<Response<PlaceItem>> GetPlace(int placeId)
        {
            try
            {
                string uri = "https://td-api.julienmialon.com/places/" + placeId;
                var responseBody = await GenericHttpRequest<object>(uri, "GET", true, null);
                return JsonConvert.DeserializeObject<Response<PlaceItem>>(responseBody);
            }
            catch (Exception e)
            {
                return new Response<PlaceItem>
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message
                };
            }
        }

        public async Task<Response<ImageItem>> PostImage(MediaFile image)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    MemoryStream stream = new MemoryStream();
                    image.GetStream().CopyTo(stream);
                    byte[] imageData = stream.ToArray();
                    HttpRequestMessage request =
                        new HttpRequestMessage(HttpMethod.Post, "https://td-api.julienmialon.com/images");
                    request.Headers.Authorization =
                        new AuthenticationHeaderValue(App.TokenScheme, AccessToken);
                    MultipartFormDataContent requestContent = new MultipartFormDataContent();
                    var imageContent = new ByteArrayContent(imageData);
                    imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                    // Le deuxième paramètre doit absolument être "file" ici sinon ça ne fonctionnera pas
                    requestContent.Add(imageContent, "file", "file.jpg");
                    request.Content = requestContent;
                    HttpResponseMessage response = await client.SendAsync(request);
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Response<ImageItem>>(result);
                }
                catch (Exception e)
                {
                    return new Response<ImageItem>()
                    {
                        IsSuccess = false,
                        ErrorMessage = e.Message
                    };
                }
            }
        }

        public async Task<Response<LoginResult>> PostRegister(RegisterRequest request)
        {
            try
            {
                string uri = "https://td-api.julienmialon.com/auth/register";
                var responseBody = await GenericHttpRequest(uri, "POST", false, request);
                return JsonConvert.DeserializeObject<Response<LoginResult>>(responseBody);
            }
            catch (Exception e)
            {
                return new Response<LoginResult>()
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message
                };
            }
        }

        public async Task<Response<LoginResult>> PostLogin(LoginRequest request)
        {
            try
            {
                string uri = "https://td-api.julienmialon.com/auth/login";
                var responseBody = await GenericHttpRequest(uri, "POST", false, request);
                Response<LoginResult> res = JsonConvert.DeserializeObject<Response<LoginResult>>(responseBody);
                if (res.IsSuccess)
                {
                    AccessToken = res.Data.AccessToken;
                    ExpiresIn = res.Data.ExpiresIn;
                }

                return res;
            }
            catch (Exception e)
            {
                return new Response<LoginResult>()
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message
                };
            }
        }

        public async Task<Response> PostComment(CreateCommentRequest createCommentRequest, int placeId)
        {
            try
            {
                string uri = "https://td-api.julienmialon.com/places/" + placeId + "/comments";
                var responseBody = await GenericHttpRequest(uri, "POST", true, createCommentRequest);
                return JsonConvert.DeserializeObject<Response>(responseBody);
            }
            catch (Exception e)
            {
                return new Response<LoginResult>()
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message
                };
            }
        }

        public async Task<Response<UserItem>> GetUser()
        {
            try
            {
                string uri = "https://td-api.julienmialon.com/me";
                var responseBody = await GenericHttpRequest<object>(uri, "GET", true, null);
                return JsonConvert.DeserializeObject<Response<UserItem>>(responseBody);
            }
            catch (Exception e)
            {
                return new Response<UserItem>
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message
                };
            }
        }

        public async Task<Response<UserItem>> PatchProfile(UpdateProfileRequest updateProfileRequest)
        {
            try
            {
                string uri = "https://td-api.julienmialon.com/me/";
                var responseBody = await GenericHttpRequest(uri, "PATCH", true, updateProfileRequest);
                return JsonConvert.DeserializeObject<Response<UserItem>>(responseBody);
            }
            catch (Exception e)
            {
                return new Response<UserItem>
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message
                };
            }
        }

        public async Task<Response> PatchPassword(UpdatePasswordRequest updatePasswordRequest)
        {
            try
            {
                string uri = "https://td-api.julienmialon.com/me/password";
                string responseBody =
                    await GenericHttpRequest(uri, "PATCH", true, updatePasswordRequest);
                return JsonConvert.DeserializeObject<Response>(responseBody);
            }
            catch (Exception e)
            {
                return new Response
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message
                };
            }
        }

        public async Task<Response> PostPlace(CreatePlaceRequest createPlaceRequest)
        {
            try
            {
                string uri = "https://td-api.julienmialon.com/places";
                string responseBody =
                    await GenericHttpRequest(uri, "POST", true, createPlaceRequest);
                return JsonConvert.DeserializeObject<Response>(responseBody);
            }
            catch (Exception e)
            {
                return new Response
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message
                };
            }
        }
    }
}