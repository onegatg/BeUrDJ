using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeUrDJ_MVC.Models.ViewModels
{
    public class QueueHeader
    {
        public BeUrDJ_MVC.Models.SQL.DjDTO DjDTO { get; set; }
        public ApplicationUser User { get; set; }
        public SessionDTO Session { get; set; }
    }
}
