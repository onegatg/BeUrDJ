using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using BeUrDJ_MVC.Models;
using Dapper;
using System.Threading.Tasks;

namespace BeUrDJ_MVC.Repository
{
    public class AccountRepository
    {
        public TokenDTO GetAuthToken(string code)
        {
            try
            {
                //TODO Change to config files
                var clientId = "47a5c9cece574b54bd77ab03ddc871a8";
                var clientSecret = "2bf2b897723844f5a1e2c1c094f10e49";
                string redirectUri = "https://beurdj.com/dj";
                string grantType = "authorization_code";
                var authorizationBytes = System.Text.Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}");
                var authorization = System.Convert.ToBase64String(authorizationBytes);
                string searchUrl = "https://accounts.spotify.com/api/token";
                string jsonResponse = String.Empty;
                TokenDTO token = new TokenDTO();
                var webRequest = WebRequest.Create(searchUrl);
                if (webRequest != null)
                {
                    webRequest.Method = "POST";
                    webRequest.Timeout = 12000;
                    webRequest.ContentType = "application/x-www-form-urlencoded";
                    string poststring = String.Format("code={0}&redirect_uri={1}&grant_type={2}", code, redirectUri, grantType);

                    byte[] bytedata = System.Text.Encoding.UTF8.GetBytes(poststring);
                    webRequest.ContentLength = bytedata.Length;

                    webRequest.Headers.Add("Authorization", "Basic " + (authorization));

                    Stream requestStream = webRequest.GetRequestStream();
                    requestStream.Write(bytedata, 0, bytedata.Length);
                    requestStream.Close();
                    using (System.IO.Stream s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(s))
                        {
                            jsonResponse = sr.ReadToEnd();
                            token = JsonConvert.DeserializeObject<TokenDTO>(jsonResponse);                           
                        }
                    }                  
                }
                if (token != null)
                {
                    return token;
                }
                else
                {
                    return null;
                    //TODO: send user to redirect page
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
        public bool InsertToken(TokenDTO token )
        {            
            try
            {
                SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                using (var sqlConnection = _sqlConnection)
                {
                    sqlConnection.Open();
                    sqlConnection.Query("INSERT INTO [dbo].[Token] ([AccessToken],[TokenType],[RefreshToken],[Scope],[ExpiresIn]) VALUES" +
                       $"('{token.AccessToken}'," +
                       $"'{token.TokenType}'," +
                       $"'{token.RefreshToken}'," +
                       $"'{token.Scope}'," +
                       $"{token.ExpiresIn})");
                    return true;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        } 
        public TokenDTO SelectToken(TokenDTO token)
        {
            try
            {
                SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                using (var sqlConnection = _sqlConnection)
                {
                    sqlConnection.Open();
                    token = sqlConnection.Query<TokenDTO>($"Select * from Token where AccessToken = '{token.AccessToken}'").FirstOrDefault();
                    return token;
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        //Override Method
        public TokenDTO SelectToken(int tokenId)
        {
            try
            {
                SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                using (var sqlConnection = _sqlConnection)
                {
                    sqlConnection.Open();
                    return sqlConnection.Query<TokenDTO>($"Select * from Token where tokenId = {tokenId}").FirstOrDefault();                    
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool DeleteToken(TokenDTO token)
        {
            try
            {
                SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                using (var sqlConnection = _sqlConnection)
                {
                    sqlConnection.Open();
                    sqlConnection.Query($"DELETE Token where TokenID = {token.TokenID}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }       
        public ApplicationUser SelectUser(string userName)
        {
            ApplicationUser user = new ApplicationUser();
            try
            {
                SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                using (var sqlConnection = _sqlConnection)
                {
                    sqlConnection.Open();                   
                    user = sqlConnection.Query<ApplicationUser>($"Select * from ApplicationUser where UserName = '{userName}'").FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return user;
        }
        //Override Method
        public ApplicationUser SelectUser(int? userId)
        {
            ApplicationUser user = new ApplicationUser();
            try
            {
                SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                using (var sqlConnection = _sqlConnection)
                {
                    sqlConnection.Open();
                    user = sqlConnection.Query<ApplicationUser>($"Select * from ApplicationUser where Id = {userId}").FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return user;
        }
        public bool UpdateTokenIDInUsersTable(int? tokenID,int? userId)
        {
            try
            {
                SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                using (var sqlConnection = _sqlConnection)
                {
                    sqlConnection.Open();
                    sqlConnection.Query($"Update [dbo].[ApplicationUser] SET TokenID = {tokenID} where Id = {userId}");                     
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }            
        }
        public async Task<bool> UpdateDJInUserTable(int? isDJ, int? userId)
        {
            try
            {
                SqlConnection _sqlConnection = new SqlConnection("Data Source=aawabw08ci4bks.ca43hba1mcki.us-east-2.rds.amazonaws.com,1433;Initial Catalog=BeUrDj;Persist Security Info=True;User ID=admin;Password=cincinnatiDj2020;Encrypt=False");
                using (var sqlConnection = _sqlConnection)
                {
                    sqlConnection.Open();
                    sqlConnection.Query($"Update [dbo].[ApplicationUser] SET isDJ = {isDJ} where Id = {userId}");
                    /*
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IsDJ", isDJ);
                    queryParameters.Add("@UserId", userId);
           
                    var result = await sqlConnection.QueryAsync(
                    "UpdateisDJUser",
                    queryParameters,
                    commandType: CommandType.StoredProcedure);
                    */
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }    
}