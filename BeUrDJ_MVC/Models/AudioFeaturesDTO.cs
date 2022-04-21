using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeUrDJ_MVC.Models
{
    // https://developer.spotify.com/documentation/web-api/reference-beta/#objects-index
    public class AudioFeatures
    {
        [JsonProperty("audio_features")] 
        public AudioFeaturesDTO[] TrackFeatures;
    }
    public class AudioFeaturesDTO
    {
        [JsonProperty("acousticness")]
        public float Acousticness { get; set; }
        [JsonProperty("danceability")]
        public float Danceability { get; set; }
        [JsonProperty("energy")]
        public float Energy { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("instrumentalness")]
        public float Instrumentalness { get; set; }
        [JsonProperty("key")]
        public int Key { get; set; }
        [JsonProperty("liveness")]
        public float Liveness { get; set; }
        [JsonProperty("loadness")]
        public float Loadness { get; set; }
        [JsonProperty("mode")]
        public int Mode { get; set; }
        [JsonProperty("speechiness")]
        public float Speechiness { get; set; }
        [JsonProperty("tempo")]
        public float Tempo { get; set; }
        [JsonProperty("time_signature")]
        public int TimeSignature { get; set; }
        [JsonProperty("uri")]
        public string Uri { get; set; }
        [JsonProperty("valence")]
        public float Valence { get; set; }
    }
}