using System;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Fourplaces.Model
{
	public class PlaceItemSummary
	{

        public static string ImageUrl = "https://td-api.julienmialon.com/images/";

        [JsonProperty("id")]
		public int Id { get; set; }
		
		[JsonProperty("title")]
		public string Title { get; set; }
		
		[JsonProperty("description")]
		public string Description { get; set; }

        public int _imageId;

        [JsonProperty("image_id")]
        public int ImageId
        {
            get => _imageId;
            set
            {
                _imageId = value;
                ImageSrc = ImageUrl + _imageId;
            } 
        }
		
		[JsonProperty("latitude")]
		public double Latitude { get; set; }
		
		[JsonProperty("longitude")]
		public double Longitude { get; set; }

        public string ImageSrc { get; set; }
    }
}