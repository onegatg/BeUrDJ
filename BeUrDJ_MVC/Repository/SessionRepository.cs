using BeUrDJ_MVC.Models;
using BeUrDJ_MVC.Models.ViewModels;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BeUrDJ_MVC.Repository
{
    public class SessionRepository
    {
        private readonly AccountRepository _accountRepository = new AccountRepository();
        
        public SessionListViewModel GetAllSessions(int? userId)
        {
            try
            {
                SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                using (var sqlConnection = _sqlConnection)
                {
                    sqlConnection.Open();
                    var sessions = sqlConnection.Query<SessionDTO>($"Select * from Session").ToList();
                    var allSessions = GetDjSessions(sessions, userId);
                    return allSessions;
                }
            }
            catch (Exception ex)
            {
                var empty = new SessionListViewModel();
                return empty;
            }
        }
        
        public SessionListViewModel GetDjSessions(List<SessionDTO> sessions, int? userId)
        {
            var userProfile = _accountRepository.SelectUser(userId);
            List<SessionDTO> joinSes = new List<SessionDTO>();
            List<SessionDTO> createSes = new List<SessionDTO>();
            SessionListViewModel totalSessions = new SessionListViewModel();
            totalSessions.createdSessions = createSes;
            totalSessions.joinSessions = joinSes;
            
            try { 
            foreach(SessionDTO session in sessions)
            {
                    //isDJ is the djid
                if(session.DjID == userProfile.IsDJ && userProfile.IsDJ != 0)
                {
                    totalSessions.createdSessions.Add(session);
                }
                else
                {
                    totalSessions.joinSessions.Add(session);
                }
            }
            return totalSessions;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return totalSessions;
            }
        }
        // =============== TODO =====================
        //Will need to change Session ID in 'Where' clause to accept sessionId from browser
        public void PostDeviceId(string deviceId, int sessionId)
        {
            try
            {
                SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                using (var sqlConnection = _sqlConnection)
                {
                    sqlConnection.Open();
                    var sessions = sqlConnection.Query<SessionDTO>($"Update Session Set DeviceId='" + deviceId + "' Where SessionId = " + sessionId + ";").ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR PostDeviceId");
                Console.WriteLine(ex.Message);
            }
        }
        public SessionDTO GetSession(int sessionId)
        {
            try
            {
                SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");

                using (var sqlConnection = _sqlConnection)
                {
                    sqlConnection.Open();
                    return sqlConnection.Query<SessionDTO>($"Select * from Session Where SessionId = {sessionId};").FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR PostDeviceId");
                Console.WriteLine(ex.Message);
                return new SessionDTO();
            }
        }
        public string GetDeviceId(int sessionId)
        {
            try
            {
                SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                using (var sqlConnection = _sqlConnection)
                {
                    sqlConnection.Open();
                    SessionDTO sessions = sqlConnection.Query<SessionDTO>($"Select * From Session Where SessionId = " + sessionId + ";").First();
                    return sessions.DeviceID;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> InsertDJ(string DJName, string DJDescription,int tokenId) 
        {
            var djImage = GetDJImage(tokenId);
            if(djImage == null)
            {
                djImage = "~/img/guestProfile.png";
            }
            try
            {
                SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                using (var sqlConnection = _sqlConnection)
                {
                    sqlConnection.Open();
                    sqlConnection.Query($"INSERT INTO DJ (DjName, DjDescription, DjImage, TokenID) VALUES('{DJName}','{DJDescription}', '{djImage}',{tokenId})"); 
                    var id = await sqlConnection.QueryAsync<int>($"SELECT DjID FROM DJ WHERE DjName = '{DJName}' and TokenID = {tokenId}");
                    return id.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR AddTrack");
                Console.WriteLine(e.Message);
                return 0;
            }

        }
        public string GetDJImage(int tokenId)
        {
            var token = _accountRepository.SelectToken(tokenId);
            string searchUrl = "https://api.spotify.com/v1/me";
            try
            {
                var webRequest = WebRequest.Create(searchUrl);
                var jsonResponse = "";
                if (webRequest != null)
                {
                    webRequest.Method = "GET";
                    webRequest.Timeout = 12000;
                    webRequest.ContentType = "application/json";

                    //TODO replace with hard coded token with parameter token
                    webRequest.Headers.Add("Authorization", $"Bearer {token.AccessToken}");

                    using (System.IO.Stream s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                        {

                            jsonResponse = sr.ReadToEnd();

                            var djProfile = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                            var djImage = djProfile.images[0].url.Value;
                            return djImage;
                        }
                    }
                }
                return "";
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }

        }

      

        public int InsertSession(string SessionName, string SessionDescription, int djId)
        {
            try
            {
                SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                using (var sqlConnection = _sqlConnection)
                {
                    sqlConnection.Open();
                    sqlConnection.Query($"INSERT INTO Session (SessionName, SessionDescription,DjID, DeviceID) VALUES('{SessionName}','{SessionDescription}',{djId}, '')");
                    var id = sqlConnection.Query<int>($"SELECT SessionId FROM Session WHERE SessionName = '{SessionName}' and DjId = {djId}").FirstOrDefault() ;
                    return id;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR AddTrack");
                Console.WriteLine(e.Message);
                return 0;
            }

        }

    }
}
