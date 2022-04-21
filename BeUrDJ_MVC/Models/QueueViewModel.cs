using BeUrDJ_MVC.Models.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeUrDJ_MVC.Models
{
    public class QueueViewModel
    {
        public DjDTO Dj { get; set; }
        public List<QueueDTO> Queue { get; set; }
    }
}
