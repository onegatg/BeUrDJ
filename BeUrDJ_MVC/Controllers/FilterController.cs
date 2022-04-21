using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeUrDJ_MVC.Models;
using BeUrDJ_MVC.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeUrDJ_MVC.Controllers
{
    public class FilterController : Controller
    {

        private FilterService _filterService = new FilterService();
        [HttpGet]
        [Route("dj/filters")]
        public ActionResult Index()
        {
            FiltersModel filters = _filterService.GetFilter(HttpContext.Session.GetInt32("SessionId"));
            return PartialView("~/Views/Filter/Index.cshtml", (filters));
        }
        [HttpPost]
        [Route("dj/filters")]
        public void Index(FiltersModel filter)
        {
            filter.sessionId = HttpContext.Session.GetInt32("SessionId");
            FiltersModel filters = _filterService.UpdateFilters(filter);
        }
    }
}