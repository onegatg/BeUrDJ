using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeUrDJ_MVC.Models;
using BeUrDJ_MVC.Models.ViewModels;
using BeUrDJ_MVC.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BeUrDJ_MVC.Controllers
{
    public class SessionController : Controller
    {
        private readonly SessionService _sessionService = new SessionService();
        private readonly AccountService _accountService = new AccountService();
        private readonly FilterService _filterService = new FilterService();
        private readonly DjService _djService = new DjService();
        private readonly SongService _songService = new SongService();

        [HttpGet]
        [Route("dj/session")]
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("User");
            if (userId != null)
            {
                var sessions = _sessionService.GetAllSessions(userId);
                var token = _accountService.GetUserToken((int)userId);
                var mobile = HttpContext.Request.Headers[HeaderNames.UserAgent].ToString();
                var isMobile = mobile.ToUpper().Contains(("Mobile").ToUpper());
                if (isMobile)
                {
                    sessions.spotifyStatus = false;
                }
                else
                {
                    sessions.spotifyStatus = _djService.GetUserSpotify(token);
                }
                return View("Index", sessions);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        /*
         * Will place browser/Spotify App Device ID into Session row
         */ 
        [HttpPost]
        [Route("dj/deviceId")]
        public void PostDeviceId(string deviceId)
        {
            var sessionId = HttpContext.Session.GetInt32("SessionId").Value;
            var userId = HttpContext.Session.GetInt32("User");
            var token = _accountService.GetUserToken((int)userId);
            Console.WriteLine("UserID: " + userId.ToString());
            Console.WriteLine("DeviceID: " + deviceId);
            Console.WriteLine("SessionID: " + sessionId.ToString());
            _sessionService.PostDeviceId(deviceId, sessionId);
            _songService.Start(token, deviceId, sessionId);
        }
        /*
         * Will be used to create a new session
         */
        [HttpGet]
        [Route("dj/createSession")]
        public IActionResult CreateSession()
        {
            return View("CreateSession");
        }

        [HttpPost]
        [Route("dj/createSession")]
        public IActionResult CreateSession(CreateSessionViewModel viewModel)
        {
            var userId = HttpContext.Session.GetInt32("User");
            var token  = _accountService.SelectTokenByUserId(userId);
            var djId = _sessionService.InsertDJ(viewModel.DJName, viewModel.DJDescription,token.TokenID);
            var sessionId = _sessionService.InsertSession(viewModel.SessionName, viewModel.SessionDescription, djId);
            HttpContext.Session.SetInt32("SessionId", sessionId);
            _accountService.UpdateDJInUserTable(djId, userId);
            _filterService.CreateFilters(sessionId);
            return RedirectToAction("DisplayQueue", "Song", new { sessionId = sessionId });
        }

        public IActionResult GoToSessionPage(int? sessionId)
        {
            var userId = HttpContext.Session.GetInt32("User");
            if (userId != null)
            {
                _accountService.UpdateDJInUserTable(0, userId);
                HttpContext.Session.SetInt32("SessionId", sessionId.Value);
                return RedirectToAction("DisplayQueue", "Song", sessionId);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

    }
}
