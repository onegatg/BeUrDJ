using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeUrDJ_MVC.Models
{
    public class SessionDTO
    {
        public int SessionID { get; set; }
        public string SessionName { get; set; }
        public string SessionDescription { get; set; }
        public int DjID { get; set; }
        public string DeviceID { get; set; }
    }
}
