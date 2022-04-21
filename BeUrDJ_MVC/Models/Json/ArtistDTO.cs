using Newtonsoft.Json;
using System;


namespace BeUrDJ_MVC.Models
{
    public class ArtistDTO
    {
        [JsonProperty("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }

        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public ArtistType Type { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
}