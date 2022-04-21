using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeUrDJ_MVC.Models
{
    public class FiltersModel
    {
        public int filterId { get; set; }
        public bool electronic { get; set; }
        public bool rock { get; set; }
        public bool punk { get; set; }
        public bool blues { get; set; }
        public bool classical { get; set; }
        public bool reggae { get; set; }
        public bool jazz { get; set; }
        public bool rap { get; set; }
        public int tempo { get; set; }
        //Value betweem 0.0 and 1.0
        public decimal danceability { get; set; }
        //Value betweem 0.0 and 1.0
        public decimal popularity { get; set; }
        public int? sessionId { get; set; }
    }
}
