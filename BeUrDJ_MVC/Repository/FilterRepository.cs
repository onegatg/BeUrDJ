using BeUrDJ_MVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Dapper;
using System.Data.SqlClient;

namespace BeUrDJ_MVC.Repository
{
    public class FilterRepository
    {
        public FilterRepository()
        {

        }

        public void UpdateFilter(FiltersModel filter)
        {
            var filters = ConverFilterToInt(filter);
            try
            {
                SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                using (var sqlConnection = _sqlConnection)
                {
                    sqlConnection.Open();
                    sqlConnection.Query($"UPDATE [FiltersCopy] SET [Tempo]= {filters.tempo},[Danceability]= {filters.danceability},[Popularity]= {filters.popularity},[Rap]= {filters.rap}," +
                                        $"[Electronic]= {filters.electronic},[Rock]= {filters.rock},[Punk]= {filters.punk},[Blues]= {filters.blues},[Classical]= {filters.classical},[Reggae]= {filters.reggae},[Jazz]= {filters.jazz} where [SessionId]= {filters.sessionId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }           
        }
        public void CreateFilters(int? sessionId)
        {
            var filter = GetFilter(sessionId);
            if (filter == null)
            {
                try
                {
                    SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                    using (var sqlConnection = _sqlConnection)
                    {
                        sqlConnection.Open();
                        sqlConnection.Query($"INSERT INTO FiltersCopy (Electronic, Rock, Punk, Blues, Classical, Reggae, Jazz, Rap, Tempo, Danceability, Popularity, SessionID) VALUES(0,0,0,0,0,0,0,0, 0, 0.0, 0.0,{(int)sessionId})");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }

        public FiltersModel GetFilter(int? sessionId)
        {
            try
            {
                SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                using (var sqlConnection = _sqlConnection)
                {
                    sqlConnection.Open();
                    var filter = sqlConnection.Query<FilterDTO>($"Select * from [FiltersCopy] where SessionId = {sessionId}").FirstOrDefault();
                    var boolFilters = ConverFilterToBool(filter);
                    return boolFilters; 
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public FiltersModel ConverFilterToBool(FilterDTO filter) {
            FiltersModel boolFilter = new FiltersModel();
            boolFilter.filterId = filter.filterId;
            boolFilter.electronic = Convert.ToBoolean(filter.electronic);
            boolFilter.rock = Convert.ToBoolean(filter.rock);
            boolFilter.punk = Convert.ToBoolean(filter.punk);
            boolFilter.blues = Convert.ToBoolean(filter.blues);
            boolFilter.classical = Convert.ToBoolean(filter.classical);
            boolFilter.reggae = Convert.ToBoolean(filter.reggae);
            boolFilter.jazz = Convert.ToBoolean(filter.jazz);
            boolFilter.rap = Convert.ToBoolean(filter.rap);
            boolFilter.tempo = filter.tempo;
            boolFilter.danceability = filter.danceability;
            boolFilter.popularity = filter.popularity;
            boolFilter.filterId = filter.filterId;
            boolFilter.sessionId = filter.sessionId;
            return boolFilter;
        }
        public FilterDTO ConverFilterToInt(FiltersModel boolFilter)
        {
            FilterDTO filter = new FilterDTO();
            filter.filterId = boolFilter.filterId;
            filter.electronic = boolFilter.electronic ? 1 : 0;
            filter.rock = boolFilter.rock ? 1 : 0;
            filter.punk = boolFilter.punk ? 1 : 0;
            filter.blues = boolFilter.blues ? 1 : 0;
            filter.reggae = boolFilter.reggae ? 1 : 0;
            filter.jazz = boolFilter.jazz ? 1 : 0;
            filter.rap = boolFilter.rap ? 1 : 0;
            filter.tempo = boolFilter.tempo;
            filter.danceability = boolFilter.danceability;
            filter.popularity = boolFilter.popularity;
            filter.filterId = boolFilter.filterId;
            filter.sessionId = boolFilter.sessionId;
            return filter;
        }
        public List<TrackDTO> FilterSongs(Tracks tracks, FiltersModel filter,string tokenId)
        {
            List<TrackDTO> filterTracks = new List<TrackDTO>();
            if (tracks.Content != null)
            {
                TrackDTO[] trackList = tracks.Content.Tracks;                

                if(filter == null)
                {
                    return trackList.ToList<TrackDTO>();
                }
                else
                {
                    int i = 0;

                    var audioFeatures = GetAudioFeatures(trackList, tokenId);
                    foreach (TrackDTO track in tracks.Content.Tracks)
                    {
                        var filterAudio = audioFeatures.TrackFeatures[i];
                        bool addTrack = FilterEachTrack(track, filterAudio, filter);
                        if (addTrack)
                        {
                            filterTracks.Add(track);
                        }
                    }
                    return filterTracks;
                }
            }
            else
            {
                return filterTracks;
            }
        }
        public bool FilterEachTrack(TrackDTO track, AudioFeaturesDTO audioFeatures, FiltersModel filters)
        {
            if(Convert.ToDecimal(audioFeatures.Tempo) >= filters.tempo && Convert.ToDecimal(audioFeatures.Danceability) >= filters.danceability && track.Popularity >= filters.popularity)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        private AudioFeatures GetAudioFeatures(TrackDTO[] tracks, string tokenId)
        {
            string searchString = "";
            int i = 0;
            foreach (TrackDTO track in tracks)
            {
                i++;
                if (tracks.Length == i)
                {
                    searchString += track.Id;
                }
                else
                {
                    searchString += track.Id + ",";
                }
            }
            string searchUrl = "https://api.spotify.com/v1/audio-features?ids=" + searchString;
            AudioFeatures trackList = new AudioFeatures();
            try
            {
                var webRequest = WebRequest.Create(searchUrl);
                var jsonResponse = "";
                if (webRequest != null)
                {
                    webRequest.Method = "GET";
                    webRequest.Timeout = 12000;
                    webRequest.ContentType = "application/json";

                    //TODO replace with hard coded token with parameter token
                    webRequest.Headers.Add("Authorization", $"Bearer {tokenId}");

                    using (System.IO.Stream s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                        {

                            jsonResponse = sr.ReadToEnd();

                            trackList = JsonConvert.DeserializeObject<AudioFeatures>(jsonResponse);
                            Console.WriteLine(String.Format("Response: {0}", jsonResponse));
                        }
                    }
                }
                return trackList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return trackList;
            }
    }
        public List<string> GetFilterString(string searchText, FiltersModel filter)
        {
            List<string> searchStrings = new List<string>();
            if (filter.electronic == true)
            {
                searchStrings.Add("Electronic");
            }
            if (filter.rap == true)
            {
                searchStrings.Add("Hip-Hop");
            }
            if (filter.rock == true)
            {
                searchStrings.Add("Rock");
            }
            if (filter.punk == true)
            {
                searchStrings.Add("Punk");
            }
            if (filter.blues == true)
            {
                searchStrings.Add("Blues");
            }
            if (filter.classical == true)
            {
                searchStrings.Add("Classical");

            }
            if (filter.reggae == true)
            {
                searchStrings.Add("Reggae");
            }
            if (filter.jazz == true)
            {
                searchStrings.Add("Jazz");
            }
            return searchStrings;
        }
    }
}