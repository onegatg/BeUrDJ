using BeUrDJ_MVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using Dapper;
using System.IO;
using System.Threading.Tasks;
using System.Data;

namespace BeUrDJ_MVC.Repository
{
    public class SongRepository
    {
        #region Songs

        public Tracks GetSongs(string searchUrl,string tokenId)
        {
            Tracks trackList = new Tracks();
            try { 
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

                            trackList = JsonConvert.DeserializeObject<Tracks>(jsonResponse);
                            Console.WriteLine(String.Format("Response: {0}", jsonResponse));
                        }
                    }
                }
                return trackList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                    return new Tracks();
            }
        }
        public void AddTrack(QueueDTO addedTrack)
        {
            var current = GetCurrentlyPlaying(addedTrack.SessionID);
            var next = GetQueueSong(addedTrack.SessionID);
            var upNext = ManageQueue(addedTrack.SessionID);
            if (current != null && next == null && upNext == null)
            {
                try
                {
                    SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                    using (var sqlConnection = _sqlConnection)
                    {
                        sqlConnection.Open();
                        sqlConnection.Query("INSERT INTO Songs VALUES('" + addedTrack.SongUri + "', '" + addedTrack.SongName + "', '" + addedTrack.ArtistName + "', '" + addedTrack.SongImage + "', 0," + addedTrack.SessionID + ", 2)");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR AddTrack");
                    Console.WriteLine(e.Message);
                }
            }
            else if (current != null && next == null && upNext != null)
            {
                try
                {
                    SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                    using (var sqlConnection = _sqlConnection)
                    {
                        sqlConnection.Open();
                        sqlConnection.Query("INSERT INTO Songs VALUES('" + addedTrack.SongUri + "', '" + addedTrack.SongName + "', '" + addedTrack.ArtistName + "', '" + addedTrack.SongImage + "', 0," + addedTrack.SessionID + ", 1)");
                        var song = sqlConnection.Query<QueueDTO>($"Select * From Songs Where SessionId = " + addedTrack.SessionID + " AND PlayStateID = 1 Order By PlayStateID desc,Likes desc,SongId asc;").First();
                        sqlConnection.Query($"UPDATE Songs Set PlayStateID = 2 Where SessionId = " + addedTrack.SessionID + " AND SongID=" + song.SongID + ";");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR AddTrack");
                    Console.WriteLine(e.Message);
                }
            }
            else if(current == null && next == null)
            {
                try
                {
                    SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                    using (var sqlConnection = _sqlConnection)
                    {
                        sqlConnection.Open();
                        sqlConnection.Query("INSERT INTO Songs VALUES('" + addedTrack.SongUri + "', '" + addedTrack.SongName + "', '" + addedTrack.ArtistName + "', '" + addedTrack.SongImage + "', 0," + addedTrack.SessionID + ", 3)");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR AddTrack");
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                try
                {
                    SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                    using (var sqlConnection = _sqlConnection)
                    {
                        sqlConnection.Open();
                        sqlConnection.Query("INSERT INTO Songs VALUES('" + addedTrack.SongUri + "', '" + addedTrack.SongName + "', '" + addedTrack.ArtistName + "', '" + addedTrack.SongImage + "', 0," + addedTrack.SessionID + ", 1)");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR AddTrack");
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void AddManagedTrack(QueueDTO addedTrack)
        {
            try
            {
                SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                using (var sqlConnection = _sqlConnection)
                {
                    sqlConnection.Open();
                    sqlConnection.Query($"UPDATE Songs Set PlayStateID = 2 Where SessionId = " + addedTrack.SessionID + " AND SongID=" + addedTrack.SongID + ";");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR AddTrack");
                Console.WriteLine(e.Message);
            }

        }

        public void Pause(string tokenId)
        {

            string pauseUrl = "https://api.spotify.com/v1/me/player/pause";
            try
            {
                var webRequest = WebRequest.Create(pauseUrl);
                if (webRequest != null)
                {
                    webRequest.Method = "PUT";
                    webRequest.Timeout = 12000;
                    webRequest.ContentType = "application/json";
                    webRequest.ContentLength = 0;
                    //TODO replace with hard coded token with parameter token
                    webRequest.Headers.Add("Authorization", $"Bearer {tokenId}");
                    webRequest.GetResponse();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        /*
         * Require link between deviceId and tokenId before moving forward with this
         * Will remove tokenId from parameter value once above is complete
         */
        public bool Start(string tokenId, string deviceId, int sessionId)
        {
            var current = GetCurrentlyPlaying(sessionId);
            var next = ManageQueue(sessionId);
            if(current != null )
            {
                string[] songs = { current.SongUri };
                var songsJson = JsonConvert.SerializeObject(songs);
                string playUrl = "https://api.spotify.com/v1/me/player/play?device_id=" + deviceId;
                try
                {
                    var webRequest = WebRequest.Create(playUrl);
                    if (webRequest != null)
                    {
                        webRequest.Method = "PUT";
                        webRequest.Timeout = 12000;
                        webRequest.ContentType = "application/json";
                        using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                        {
                            string json = "{\"uris\":" + songsJson + "}";
                            streamWriter.Write(json);
                        }
                        //TODO replace with hard coded token with parameter token
                        webRequest.Headers.Add("Authorization", $"Bearer {tokenId}");
                        webRequest.GetResponse();

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            if (current == null && next != null)
            {
                string[] songs = { next.SongUri };
                var songsJson = JsonConvert.SerializeObject(songs);
                string playUrl = "https://api.spotify.com/v1/me/player/play?device_id=" + deviceId;
                try
                {
                    var webRequest = WebRequest.Create(playUrl);
                    if (webRequest != null)
                    {
                        webRequest.Method = "PUT";
                        webRequest.Timeout = 12000;
                        webRequest.ContentType = "application/json";
                        using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                        {
                            string json = "{\"uris\":" + songsJson + "}";
                            streamWriter.Write(json);
                        }
                        //TODO replace with hard coded token with parameter token
                        webRequest.Headers.Add("Authorization", $"Bearer {tokenId}");
                        webRequest.GetResponse();

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            if(current == null && next == null)
            {
                var initialRecommended = GetRecommended(tokenId, sessionId);
                List<QueueDTO> recommended = initialRecommended as List<QueueDTO>;
                QueueDTO newRecommended = new QueueDTO();
                newRecommended.ArtistName = recommended[0].ArtistName;
                newRecommended.SongUri = recommended[0].SongUri;
                newRecommended.SongName = recommended[0].SongName;
                newRecommended.SongImage = recommended[0].SongImage;
                newRecommended.SessionID = sessionId;
                AddTrack(newRecommended);
                var song = GetCurrentlyPlaying(sessionId);
                string[] songs = { song.SongUri };
                var songsJson = JsonConvert.SerializeObject(songs);
                string playUrl = "https://api.spotify.com/v1/me/player/play?device_id=" + deviceId;
                try
                {
                    var webRequest = WebRequest.Create(playUrl);
                    if (webRequest != null)
                    {
                        webRequest.Method = "PUT";
                        webRequest.Timeout = 12000;
                        webRequest.ContentType = "application/json";
                        using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                        {
                            string json = "{\"uris\":" + songsJson + "}";
                            streamWriter.Write(json);
                        }
                        //TODO replace with hard coded token with parameter token
                        webRequest.Headers.Add("Authorization", $"Bearer {tokenId}");
                        webRequest.GetResponse();

                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return true;
        }
        public async Task<bool> Play(string tokenId, string deviceId, int sessionId)
        {
            await UpdateQueue(tokenId, sessionId);
            var song = GetCurrentlyPlaying(sessionId);
            string[] songs = { song.SongUri };
            var songsJson = JsonConvert.SerializeObject(songs);
            string playUrl = "https://api.spotify.com/v1/me/player/play?device_id=" + deviceId;
            try
                {
                    var webRequest = WebRequest.Create(playUrl);
                    if (webRequest != null)
                    {
                        webRequest.Method = "PUT";
                        webRequest.Timeout = 12000;
                        webRequest.ContentType = "application/json";
                        using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                        {
                            string json = "{\"uris\":" + songsJson + "}";
                            streamWriter.Write(json);
                        }
                        //TODO replace with hard coded token with parameter token
                        webRequest.Headers.Add("Authorization", $"Bearer {tokenId}");
                        webRequest.GetResponse();

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            return true;
        }
        public QueueDTO ManageQueue(int sessionId)
        {
            try
            {
                SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                using (var sqlConnection = _sqlConnection)
                {
                    sqlConnection.Open();
                    QueueDTO song = sqlConnection.Query<QueueDTO>($"Select * From Songs Where SessionId = " + sessionId + " AND PlayStateID = 1 Order By PlayStateID desc,Likes desc,SongId asc;").First();
                    return song;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public void Skip(string tokenId)
        {

            string skipUrl = "https://api.spotify.com/v1/me/player/next";
            try
            {
                var webRequest = WebRequest.Create(skipUrl);
                if (webRequest != null)
                {
                    webRequest.Method = "POST";
                    webRequest.Timeout = 12000;
                    webRequest.ContentType = "application/json";
                    webRequest.ContentLength = 0;
                    //TODO replace with hard coded token with parameter token
                    webRequest.Headers.Add("Authorization", $"Bearer {tokenId}");
                    webRequest.GetResponse();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        #endregion

        #region Queue
        public async Task<List<QueueDTO>> GetQueue(int sessionId)
        {
            List<QueueDTO> currentQueue = new List<QueueDTO>();
            try
            {
                SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                using (var sqlConnection = _sqlConnection)
                {
                    sqlConnection.Open();
                    return sqlConnection.QueryAsync<QueueDTO>($"Select * from [Songs] WHERE SessionId={sessionId} AND PlayStateID <> 4 Order By PlayStateID desc,Likes desc,SongId asc").Result.ToList();
                }

            }
            catch (Exception e)
            {
                return currentQueue;
            }
        }
        public async Task<bool> UpdateLikes(int songId, int likes, int sessionId, int userId)
        {
            try
            {
                SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                using (var sqlConnection = _sqlConnection)
                {
                    sqlConnection.Open();
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@SongId", songId);
                    queryParameters.Add("@UserId", userId);
                    queryParameters.Add("@SessionId", sessionId);
                    queryParameters.Add("@Like", likes);
                    var result =  await sqlConnection.QueryAsync(
                    "UpdateLikes",
                    queryParameters,
                    commandType: CommandType.StoredProcedure);

                    return true;
                }

            }
            catch (Exception e)
            {
                return false;
            }
        }

        /*
        * Will be used to update the currently played song to has been played and will set new song to currently played
        * Need to update QueueDTO and Table to include varchar Play column with values 'Not Played, Currently Playing, Played'
        */
        public async Task<bool> UpdateQueue(string tokenId, int sessionId)
        {
            var current = GetCurrentlyPlaying(sessionId);
            var next = GetQueueSong(sessionId);
            var nextUpSong = ManageQueue(sessionId);
            if (current != null && next != null)
            {
                try
                {
                    SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                    using (var sqlConnection = _sqlConnection)
                    {
                        sqlConnection.Open();
                        sqlConnection.Query($"UPDATE Songs Set PlayStateID = 4 Where SessionId = " + sessionId + " AND PlayStateID = 3");
                        sqlConnection.Query($"UPDATE Songs Set PlayStateID = 3 Where SessionId = " + sessionId + " AND PlayStateID = 2");
                        var songList = sqlConnection.Query<QueueDTO>($"Select * From Songs Where SessionId = " + sessionId + " AND PlayStateID = 1;").ToList();
                        var song = songList.OrderByDescending(item => item.Likes).First();
                        sqlConnection.Query($"UPDATE Songs Set PlayStateID = 2 Where SessionId = " + sessionId + " AND SongID=" + song.SongID + ";");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR UpdateQueue");
                    Console.WriteLine(ex.Message);
                }
            }
                else if (next == null && nextUpSong != null)
                {
                    try
                    {
                        SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                        using (var sqlConnection = _sqlConnection)
                        {
                            sqlConnection.Open();
                            sqlConnection.Query($"UPDATE Songs Set PlayStateID = 4 Where SessionId = " + sessionId + " AND PlayStateID = 3");
                            var songs = sqlConnection.Query<QueueDTO>($"Select * From Songs Where SessionId = " + sessionId + " AND PlayStateID = 1;").ToList();
                            var song = songs.OrderByDescending(item => item.Likes).First();
                            sqlConnection.Query($"UPDATE Songs Set PlayStateID = 3 Where SessionId = " + sessionId + " AND SongID=" + song.SongID + ";");
                            var nextSongs = sqlConnection.Query<QueueDTO>($"Select * From Songs Where SessionId = " + sessionId + " AND PlayStateID = 1;").ToList();
                            var nextSong = songs.OrderByDescending(item => item.Likes).First();
                            sqlConnection.Query($"UPDATE Songs Set PlayStateID = 2 Where SessionId = " + sessionId + " AND SongID=" + nextSong.SongID + ";");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR UpdateQueue");
                        Console.WriteLine(ex.Message);
                    }
                }
            else if (next == null && nextUpSong == null)
            {
                try
                {
                    if(current != null)
                    {
                        SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                        using (var sqlConnection = _sqlConnection)
                        {
                            sqlConnection.Open();
                            sqlConnection.Query($"UPDATE Songs Set PlayStateID = 4 Where SessionId = " + sessionId + " AND PlayStateID = 3");
                        }
                    }
                    var initialRecommended = GetRecommended(tokenId, sessionId);
                    List<QueueDTO> recommended = initialRecommended as List<QueueDTO>;
                    QueueDTO newRecommended = new QueueDTO();
                    newRecommended.ArtistName = recommended[0].ArtistName;
                    newRecommended.SongUri = recommended[0].SongUri;
                    newRecommended.SongName = recommended[0].SongName;
                    newRecommended.SongImage = recommended[0].SongImage;
                    newRecommended.SessionID = sessionId;
                    AddTrack(newRecommended);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR UpdateQueue");
                    Console.WriteLine(ex.Message);
                }
            }
            return true;
        }
        /*
         * Will return the song with the highest amount of likes and Play column with a 'Not Played' value
         */
        public QueueDTO GetQueueSong(int sessionId)
        {
            try
            {
                SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                using (var sqlConnection = _sqlConnection)
                {
                    sqlConnection.Open();
                    QueueDTO song = sqlConnection.Query<QueueDTO>($"Select * From Songs Where SessionId = " + sessionId + " AND PlayStateID = 2;").First();
                    return song;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public QueueDTO GetCurrentlyPlaying(int sessionId)
        {
            try
            {
                SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                using (var sqlConnection = _sqlConnection)
                {
                    sqlConnection.Open();
                    QueueDTO song = sqlConnection.Query<QueueDTO>($"Select * From Songs Where SessionId = " + sessionId + " AND PlayStateID = 3;").First();
                    return song;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public QueueDTO GetMostLiked(int? sessionId)
        {
            try
            {
                SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                using (var sqlConnection = _sqlConnection)
                {
                    sqlConnection.Open();
                    QueueDTO song = sqlConnection.Query<QueueDTO>($"Select * From Songs Where SessionId = " + sessionId + "Order By Likes Desc").First();
                    return song;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public object GetRecommended(string tokenId, int? sessionId)
        {
            var trackList = new List<TrackDTO>();
            var mostLikedSong = GetMostLiked(sessionId);
            if(mostLikedSong != null)
            {
                var songId = mostLikedSong.SongUri.Remove(0, 14);
                string searchUrl = "https://api.spotify.com/v1/recommendations?seed_tracks=" + songId;
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

                                var recommended = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                                var recommendedSongs = recommended.tracks;
                                Console.WriteLine(String.Format("Response: {0}", recommendedSongs));
                                var queueRecommended = SelectRecommendedSongs(recommendedSongs);
                                return queueRecommended;
                            }
                        }
                    }
                    return trackList;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return trackList;
                }
            }
            else
            {
                string searchUrl = "https://api.spotify.com/v1/recommendations?seed_tracks=" + "0wXuerDYiBnERgIpbb3JBR";
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

                                var recommended = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                                var recommendedSongs = recommended.tracks;
                                Console.WriteLine(String.Format("Response: {0}", recommendedSongs));
                                var queueRecommended = SelectRecommendedSongs(recommendedSongs);
                                return queueRecommended;
                            }
                        }
                    }
                    return trackList;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return trackList;
                }
            }

        }
        public List<QueueDTO> SelectRecommendedSongs(dynamic recommendedSongs)
        {
            List<QueueDTO> recommended = new List<QueueDTO>();
            foreach(var track in recommendedSongs)
            {
                QueueDTO newRecommended = new QueueDTO();
                newRecommended.ArtistName = track.artists[0].name;
                newRecommended.SongUri = track.uri;
                newRecommended.SongName = track.name;
                newRecommended.SongImage = track.album.images[1].url;
                recommended.Add(newRecommended);
            }
            return recommended;
        }
        public void SetToCurrentlyPlaying(QueueDTO song)
        {
            try
            {
                SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                using (var sqlConnection = _sqlConnection)
                {
                    sqlConnection.Open();
                    sqlConnection.Query($"UPDATE Songs Set PlayStateID = 2 Where SessionId = " + song.SessionID + " AND SongID=" + song.SongID + ";");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR UpdateQueue");
                Console.WriteLine(ex.Message);
            }
        }

        #endregion
    }
}