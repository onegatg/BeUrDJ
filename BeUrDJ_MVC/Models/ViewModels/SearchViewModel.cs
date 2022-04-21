using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeUrDJ_MVC.Models.ViewModels
{
    public class SearchViewModel
    {
        public List<TrackDTO> Songs { get; set; }
        public FiltersModel Filters { get; set; }
    }
}
