using BeUrDJ_MVC.Models;
using BeUrDJ_MVC.Models.SQL;
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
    public class DjRepository
    {
        private readonly AccountRepository _accountRepository = new AccountRepository();
        public DjDTO GetDjProfile(int sessionId)
        {
            DjDTO dj = new DjDTO();
            try
            {
                SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                using (var sqlConnection = _sqlConnection)
                {
                    sqlConnection.Open();
                    var session = sqlConnection.Query<SessionDTO>($"Select * from [Session] WHERE SessionId={sessionId}").First();
                    dj = sqlConnection.Query<DjDTO>($"Select * from [DJ] WHERE DjID={session.DjID}").First();
                }
                return dj;

            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR GetDjProfile: " + e.Message);
                return dj;
            }
        }
        public bool GetUserSpotify(string token)
        {
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
                    webRequest.Headers.Add("Authorization", $"Bearer {token}");

                    using (System.IO.Stream s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                        {

                            jsonResponse = sr.ReadToEnd();

                            var djProfile = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                            var djProduct = djProfile.product;
                            if(djProduct == "premium")
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }
    }
}
