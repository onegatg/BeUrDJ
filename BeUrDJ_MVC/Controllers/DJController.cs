using BeUrDJ_MVC.Service;
using BeUrDJ_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using BeUrDJ_MVC.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BeUrDJ_MVC.Models.SQL;

namespace BeUrDJ_MVC.Controllers
{
    public class DJController : Controller
    {       
        private readonly DjService _djService = new DjService();
        private readonly AccountService _accountService = new AccountService();
        public DJController()
        {
        }
        [HttpPost]
        [Route("dj/votes")]
        public void UpdateVotes(List<string> songsId, int sessionId)
        {
            //Updates Votes
        }

        [HttpPost]
        [Route("dj/votes")]
        public void GetVotes(int sessionId)
        {
            //Gets Votes
        }

        //GET dj/djProfile
        [HttpGet]
        [Route("dj/djProfile")]
        public ActionResult DjProfile()
        {
            var sessionId = HttpContext.Session.GetInt32("SessionId");
            DjDTO dj = _djService.GetDjProfile(sessionId.Value);
            return PartialView("~/Views/Song/DjProfile.cshtml",dj);
        }
    }
}
