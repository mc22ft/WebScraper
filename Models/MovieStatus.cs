using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebScraper.Models
{
    public class MovieStatus
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("movie")]
        public string Movie { get; set; }

        public string Day { get; set; }

        public int Id { get; set; }

}
}