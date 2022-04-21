using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BeUrDJ_MVC.Models
{   
        public class Tracks
        {
            [JsonProperty("tracks")]
            public Content Content { get; set; }
        }

        public class Content
        {
            [JsonProperty("href")]
            public Uri Href { get; set; }

            [JsonProperty("items")]
            public TrackDTO[] Tracks { get; set; }

            [JsonProperty("limit")]
            public long Limit { get; set; }

            [JsonProperty("next")]
            public Uri Next { get; set; }

            [JsonProperty("offset")]
            public long Offset { get; set; }

            [JsonProperty("previous")]
            public object Previous { get; set; }

            [JsonProperty("total")]
            public long Total { get; set; }
        }

        

        public class ExternalUrls
        {
            [JsonProperty("spotify")]
            public Uri Spotify { get; set; }
        }

        public class Image
        {
            [JsonProperty("height")]
            public long Height { get; set; }

            [JsonProperty("url")]
            public Uri Url { get; set; }

            [JsonProperty("width")]
            public long Width { get; set; }
        }

        public class ExternalIds
        {
            [JsonProperty("isrc")]
            public string Isrc { get; set; }
        }

        public enum AlbumTypeEnum { Album, Compilation, Single };

        public enum ArtistType { Artist };

        public enum ReleaseDatePrecision { Day };

        public enum ItemType { Track };

        
   

       

        internal class AlbumTypeEnumConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(AlbumTypeEnum) || t == typeof(AlbumTypeEnum?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                switch (value)
                {
                    case "album":
                        return AlbumTypeEnum.Album;
                    case "compilation":
                        return AlbumTypeEnum.Compilation;
                    case "single":
                        return AlbumTypeEnum.Single;
                }
                throw new Exception("Cannot unmarshal type AlbumTypeEnum");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (AlbumTypeEnum)untypedValue;
                switch (value)
                {
                    case AlbumTypeEnum.Album:
                        serializer.Serialize(writer, "album");
                        return;
                    case AlbumTypeEnum.Compilation:
                        serializer.Serialize(writer, "compilation");
                        return;
                    case AlbumTypeEnum.Single:
                        serializer.Serialize(writer, "single");
                        return;
                }
                throw new Exception("Cannot marshal type AlbumTypeEnum");
            }

            public static readonly AlbumTypeEnumConverter Singleton = new AlbumTypeEnumConverter();
        }

        internal class ArtistTypeConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(ArtistType) || t == typeof(ArtistType?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                if (value == "artist")
                {
                    return ArtistType.Artist;
                }
                throw new Exception("Cannot unmarshal type ArtistType");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (ArtistType)untypedValue;
                if (value == ArtistType.Artist)
                {
                    serializer.Serialize(writer, "artist");
                    return;
                }
                throw new Exception("Cannot marshal type ArtistType");
            }

            public static readonly ArtistTypeConverter Singleton = new ArtistTypeConverter();
        }

        internal class ReleaseDatePrecisionConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(ReleaseDatePrecision) || t == typeof(ReleaseDatePrecision?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                if (value == "day")
                {
                    return ReleaseDatePrecision.Day;
                }
                throw new Exception("Cannot unmarshal type ReleaseDatePrecision");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (ReleaseDatePrecision)untypedValue;
                if (value == ReleaseDatePrecision.Day)
                {
                    serializer.Serialize(writer, "day");
                    return;
                }
                throw new Exception("Cannot marshal type ReleaseDatePrecision");
            }

            public static readonly ReleaseDatePrecisionConverter Singleton = new ReleaseDatePrecisionConverter();
        }

        internal class ItemTypeConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(ItemType) || t == typeof(ItemType?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                if (value == "track")
                {
                    return ItemType.Track;
                }
                throw new Exception("Cannot unmarshal type ItemType");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (ItemType)untypedValue;
                if (value == ItemType.Track)
                {
                    serializer.Serialize(writer, "track");
                    return;
                }
                throw new Exception("Cannot marshal type ItemType");
            }

            public static readonly ItemTypeConverter Singleton = new ItemTypeConverter();
        }
    }


