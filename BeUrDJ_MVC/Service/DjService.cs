using BeUrDJ_MVC.Models.SQL;
using BeUrDJ_MVC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeUrDJ_MVC.Service
{
    public class DjService
    {
        private readonly DjRepository _djRepository = new DjRepository();
        public DjDTO GetDjProfile(int sessionId)
        {
            return _djRepository.GetDjProfile(sessionId);
        }
        public bool GetUserSpotify(string token)
        {

            return _djRepository.GetUserSpotify(token);
        }
    }
}
