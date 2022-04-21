using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BeUrDJ_MVC.Models
{
    
    public class FilterDTO
    {        
        public int filterId { get; set; }
        public int electronic { get; set; }
        public int rock { get; set; }
        public int punk { get; set; }
        public int blues { get; set; }
        public int classical { get; set; }
        public int reggae { get; set; }
        public int jazz { get; set; }
        public int rap { get; set; }
        //Value betweem 0 and 200
        public int tempo { get; set; }
        //Value betweem 0.0 and 1.0
        public decimal danceability { get; set; }
        //Value betweem 0.0 and 1.0
        public decimal popularity { get; set; }
        public int? sessionId { get; set; }
    }

}