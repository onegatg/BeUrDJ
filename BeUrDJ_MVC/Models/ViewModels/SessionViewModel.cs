using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeUrDJ_MVC.Models
{
    public class SessionViewModel
    {
        public SessionDTO SessionDTO { get; set; }
        public ApplicationUser User { get; set; }
    }
}
