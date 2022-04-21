using System.Collections.Generic;
using BeUrDJ_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using BeUrDJ_MVC.Service;
using Microsoft.AspNetCore.Http;
using BeUrDJ_MVC.Models.SQL;
using BeUrDJ_MVC.Models.ViewModels;
using System.Threading.Tasks;

namespace BeUrDJ_MVC.Controllers
{
    public class SongController : Controller
    {
        private AccountService _accountService = new AccountService();
        private SongService _songService = new SongService();
        private FilterService _filterService = new FilterService();
        private SessionService _sessionService = new SessionService();
        private DjService _djService = new DjService();

        [Route("dj/search")]
        public IActionResult Index()
        {
            return PartialView("Search");
        }
        // GET dj?searchText=
        [HttpGet]
        [Route("dj/gettracks")]
        public SearchViewModel GetTracks(string searchText)
        {
            var sessionId = HttpContext.Session.GetInt32("SessionId");
            var token = _accountService.SelectTokenByUserId(HttpContext.Session.GetInt32("User")).AccessToken;
            var tracks = _songService.GetSongs(searchText, token, sessionId);
            var filterdTracks = _filterService.FilterSongs(tracks, token, sessionId);
            return filterdTracks;
        }

        // POST dj/queue
        [HttpPost]
        [Route("dj/addTrack")]
        public bool AddTrack(TrackDTO addedTrack)
        {
            var sessionId = (int)HttpContext.Session.GetInt32("SessionId");
            var song = _songService.ConvertTrackToSong(addedTrack, sessionId);
            _songService.AddTrack(song);
            return true;
        }
        // POST dj/queue
        [HttpPost]
        [Route("dj/addRecommendedTrack")]
        public bool AddRecommendedTrack([FromBody]QueueDTO addedTrack)
        {
            var sessionId = HttpContext.Session.GetInt32("SessionId");
            addedTrack.SessionID = (int)sessionId;
            _songService.AddTrack(addedTrack);
            return true;
        }
        [HttpGet]
        [Route("dj/queue")]
        public ActionResult DisplayQueue(int? sessionId)
        {
            var userId = HttpContext.Session.GetInt32("User");
            if (userId != null)
            {
                if (!sessionId.HasValue)
                {
                    sessionId = HttpContext.Session.GetInt32("SessionId");
                    if (!sessionId.HasValue)
                    {
                        return View(new List<QueueDTO>());
                    }
                    var sessionValue = _sessionService.GetSession(sessionId.Value);
                    var userValue = _accountService.SelectUser(userId);
                    return View(new SessionViewModel { SessionDTO = sessionValue, User = userValue });
                }
                var id = sessionId.Value;
                HttpContext.Session.SetInt32("SessionId", id);
                var session = _sessionService.GetSession(id);
                var user = _accountService.SelectUser(userId);
                return View(new SessionViewModel { SessionDTO = session, User = user });
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }
        [HttpGet]
        [Route("dj/userQueue")]
        public ActionResult DisplayUserQueue(int? sessionId)
        {
            var userId = HttpContext.Session.GetInt32("User");
            if(userId != null)
            {
                if (!sessionId.HasValue)
                {
                    sessionId = HttpContext.Session.GetInt32("SessionId");
                    if (!sessionId.HasValue)
                    {
                        return View(new List<QueueDTO>());
                    }
                    var sessionValue = _sessionService.GetSession(sessionId.Value);
                    var userValue = _accountService.SelectUser(HttpContext.Session.GetInt32("User").Value);
                    return View(new SessionViewModel { SessionDTO = sessionValue, User = userValue });
                }
                var id = sessionId.Value;
                HttpContext.Session.SetInt32("SessionId", id);
                var session = _sessionService.GetSession(id);
                var user = _accountService.SelectUser(userId);
                return View(new SessionViewModel { SessionDTO = session, User = user });
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        [HttpPost]
        [ResponseCache(NoStore = true, Duration = 0)]
        [Route("dj/updateQueue")]
        public IActionResult UpdateQueue(bool isMobile)
        {
            var userId = HttpContext.Session.GetInt32("User");
            if (userId != 0)
            {
                ModelState.Clear();
                var sessionId = HttpContext.Session.GetInt32("SessionId");
                List<QueueDTO> songs = new List<QueueDTO>();
                songs = _songService.GetQueue(sessionId.Value).Result;
                var user = _accountService.SelectUser(userId);
                //refactor the model to a viewmodel to include the session - session can then be used anywhere on the page. 

                if (isMobile)
                {
                    return PartialView("~/Views/Song/UpdateQueueMobile.cshtml", new UpdateQueueViewModel { QueueDTOs = songs, User = user });
                }
                else
                {
                    return PartialView("~/Views/Song/UpdateQueue.cshtml", new UpdateQueueViewModel { QueueDTOs = songs, User = user });
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpPost]
        [ResponseCache(NoStore = true, Duration = 0)]
        [Route("dj/updateUserQueue")]
        public IActionResult UpdateUserQueue(bool isMobile)
        {
            var userId = HttpContext.Session.GetInt32("User");
            if (userId != 0)
            {
                ModelState.Clear();
                var sessionId = HttpContext.Session.GetInt32("SessionId");
                List<QueueDTO> songs = new List<QueueDTO>();
                songs = _songService.GetQueue(sessionId.Value).Result;
                var user = _accountService.SelectUser(userId);
                if (isMobile)
                {
                    return PartialView("~/Views/Song/UpdateQueueUserMobile.cshtml", new UpdateQueueViewModel { QueueDTOs = songs, User = user });
                }
                else
                {
                    return PartialView("~/Views/Song/UpdateQueueUser.cshtml", new UpdateQueueViewModel { QueueDTOs = songs, User = user });
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        /*
         * ---------------- TODO --------------------- 
         * Link to spotifyplayer.js to play queue from here
         * Get UserId from HttpRequest; ensure it matches with a token
         * Get DeviceID from Session Table with sessionId
         */
        [HttpPost]
        [Route("dj/playCurrent")]
        public async Task<bool> PlayCurrentSongAsync()
        {
            var sessionId = HttpContext.Session.GetInt32("SessionId").Value;
            var userId =  HttpContext.Session.GetInt32("User").Value;
            string tokenId = _accountService.GetUserToken(userId);
            string deviceId = _sessionService.GetDeviceId(sessionId);
            List<QueueDTO> songs = await Task.Run(() => _songService.GetQueue(sessionId));
            return await _songService.Play(tokenId, deviceId, sessionId);
        }

        [HttpGet]
        [Route("dj/pause")]
        public void PauseSong(string tokenId)
        {
            _songService.Pause(tokenId);
        }

        [HttpGet]
        [Route("dj/skip")]
        public void SkipSong(string tokenId)
        {
            _songService.Skip(tokenId);
        }

        [HttpGet]
        [Route("dj/getRecommended")]
        public void GetRecommended(string tokenId)
        {
            //Gets DJ's recommended song and plays it
        }

        [HttpGet]
        [Route("dj/recommended")]
        public PartialViewResult GetRecommended()
        {
            var userId = HttpContext.Session.GetInt32("User").Value;
                string tokenId = _accountService.GetUserToken(userId);
                var sessionId = HttpContext.Session.GetInt32("SessionId");
                var recommendedSongs = _songService.GetRecommended(tokenId, sessionId);
                return PartialView("~/Views/Song/GetRecommended.cshtml", recommendedSongs);

            /* Gets song analysis for each song and 
            * returns the songs that are allowed to play with filters
            */
        }

        [HttpPost]
        [Route("dj/updateLike")]
        public bool UpdateLikes(int songId, int likes)
        {
            ModelState.Clear();
            var sessionId = HttpContext.Session.GetInt32("SessionId").Value;
            var userId = HttpContext.Session.GetInt32("User").Value;
           return  _songService.UpdateLikes(songId,likes,sessionId,userId);
        }

        [HttpPost]
        [Route("dj/getQueueHeader")]
        public ActionResult QueueHeader()
        {
            var sessionId = HttpContext.Session.GetInt32("SessionId");
            DjDTO dj = _djService.GetDjProfile(sessionId.Value);
            var userId = HttpContext.Session.GetInt32("User");
            var user = _accountService.SelectUser(userId);
            var session = _sessionService.GetSession(sessionId.Value);
            return PartialView("~/Views/Song/QueueHeader.cshtml", new QueueHeader { DjDTO = dj, User = user,Session = session }) ;
        }

    }
}