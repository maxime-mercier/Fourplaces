using Newtonsoft.Json;

namespace Fourplaces.Model
{
	public class CreateCommentRequest
	{
		[JsonProperty("text")]
		public string Text { get; set; }
	}
}