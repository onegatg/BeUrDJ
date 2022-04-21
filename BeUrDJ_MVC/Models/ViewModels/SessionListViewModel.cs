using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeUrDJ_MVC.Models.ViewModels
{
    public class SessionListViewModel
    {
            public List<SessionDTO> createdSessions { get; set; }
            public List<SessionDTO> joinSessions { get; set; }
            public bool spotifyStatus { get; set; }
    }
}
