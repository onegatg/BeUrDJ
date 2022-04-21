using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeUrDJ_MVC.Models.SQL
{
    public class DjDTO
    {
        public int DjID { get; set; }
        //Value betweem 0.0 and 1.0
        public string DjName { get; set; }
        public string DjDescription { get; set; }
        //Value betweem 0.0 and 1.0
        public string DjImage { get; set; }
        //Value betweem 0.0 and 1.0
        public int TokenID { get; set; }
    }
}
