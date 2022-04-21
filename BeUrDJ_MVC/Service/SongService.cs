using BeUrDJ_MVC.Models;
using BeUrDJ_MVC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BeUrDJ_MVC.Service
{
    public class SongService
    {
        private SongRepository _songRepository = new SongRepository();
        private FilterRepository _filterRepository = new FilterRepository();

        public bool UpdateLikes(int songId, int likes, int? sessionId, int? userId)
        {
            if(!sessionId.HasValue && !userId.HasValue)
            {
                return false;
            }
             return _songRepository.UpdateLikes(songId, likes,sessionId.Value,userId.Value).Result;
        }
        public async Task<List<QueueDTO>> GetQueue(int sessionId)
        {
            var currentQueue = await _songRepository.GetQueue(sessionId);
            return currentQueue;
        }
        public void AddTrack(QueueDTO track)
        {
            _songRepository.AddTrack(track);
        }
        public void Pause(string tokenId)
        {

            _songRepository.Pause(tokenId);
        }
        public void Start(string tokenId, string deviceId, int sessionId)
        {
            _songRepository.Start(tokenId, deviceId, sessionId);
        }
        public async Task<bool> Play(string tokenId, string deviceId, int sessionId)
        {

           return await _songRepository.Play(tokenId, deviceId, sessionId);
        }
        public void Skip(string tokenId)
        {

            _songRepository.Skip(tokenId);
        }

        public Tracks GetSongs(string searchText, string tokenId, int? sessionId)
        {
            //TokenDTO token = GetToken(tokenId);
            FiltersModel filters = _filterRepository.GetFilter(sessionId);
            var searchFilters = _filterRepository.GetFilterString(searchText, filters);
            Tracks trackList = new Tracks();
            List<TrackDTO> allTracks = new List<TrackDTO>();
            trackList.Content = new Content();
            if(searchFilters.Count > 0)
            {
                foreach (var filter in searchFilters)
                {
                    string searchUrl = "https://api.spotify.com/v1/search?q=" + searchText + "%20genre:%22" + filter + "%22&type=track&market=US";
                    Tracks tracks = _songRepository.GetSongs(searchUrl, tokenId);
                    if (tracks.Content != null)
                    {
                        foreach (var track in tracks.Content.Tracks)
                        {
                            allTracks.Add(track);
                        }
                    }
                }
            }
            else
            {
                string searchUrl = "https://api.spotify.com/v1/search?q=" + searchText + "%22&type=track&market=US";

                Tracks tracks = _songRepository.GetSongs(searchUrl, tokenId);
                if(tracks.Content != null)
                {
                    foreach (var track in tracks.Content.Tracks)
                    {
                        allTracks.Add(track);
                    }
                }
            }
            trackList.Content.Tracks = allTracks.ToArray();
            return trackList;
        }
        public QueueDTO ConvertTrackToSong(TrackDTO track, int sessionId)
        {
            QueueDTO queue = new QueueDTO();
            queue.SongUri = track.Uri;
            queue.SongName = track.Name;
            queue.ArtistName = track.Artists[0].Name;
            queue.SongImage = track.Album.Images[1].Url.ToString();
            queue.PlayStateID = 1;
            queue.Likes = 0;
            queue.SessionID = sessionId;
            return queue;
        }
        public object GetRecommended(string tokenId, int? sessionId)
        {
            var recommendedSongs = _songRepository.GetRecommended(tokenId, sessionId);
            return recommendedSongs;
        }
        public async Task<bool> UpdateQueue(string tokenId, int sessionId)
        {
            return await _songRepository.UpdateQueue(tokenId, sessionId);
        }
    }
}