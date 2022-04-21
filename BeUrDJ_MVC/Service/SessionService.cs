using BeUrDJ_MVC.Models;
using BeUrDJ_MVC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeUrDJ_MVC.Repository;
namespace BeUrDJ_MVC.Service
{
    public class SessionService
    {
        private  SessionRepository _sessionRepository = new SessionRepository();

        public SessionListViewModel GetAllSessions(int? userId)
        {
            return _sessionRepository.GetAllSessions(userId);
        }
        public void PostDeviceId(string deviceId, int sessionId)
        {
            _sessionRepository.PostDeviceId(deviceId, sessionId);
        }
        public string GetDeviceId(int sessionId)
        {
            return _sessionRepository.GetDeviceId(sessionId);
        }

        public int InsertDJ(string DJName, string DJDescription,int tokenID)
        {
            return _sessionRepository.InsertDJ(DJName, DJDescription, tokenID).Result;
        }

        public int InsertSession(string SessionName, string SessionDescription, int djId)
        {
            return _sessionRepository.InsertSession(SessionName, SessionDescription, djId);
        }

        public SessionDTO GetSession(int sessionId)
        {
            return _sessionRepository.GetSession(sessionId);
        }

    }
}
