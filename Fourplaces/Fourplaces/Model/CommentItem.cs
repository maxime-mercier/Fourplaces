using System;
using Newtonsoft.Json;

namespace Fourplaces.Model
{
	public class CommentItem
	{
		[JsonProperty("date")]
		public DateTime Date { get; set; }
		
		[JsonProperty("author")]
		public UserItem Author { get; set; }
		
		[JsonProperty("text")]
		public string Text { get; set; }

        public string CommentAuthorAndDate
        {
            get { return string.Format("{0}    {1} ", Author.FirstName, Date.ToShortDateString()); }
        }
	}
}