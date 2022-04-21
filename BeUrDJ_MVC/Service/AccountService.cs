using BeUrDJ_MVC.Models;
using BeUrDJ_MVC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeUrDJ_MVC.Service
{
    public class AccountService
    {
        private AccountRepository _userRepository = new AccountRepository();
        
        public TokenDTO SelectTokenByUserId(int? userId)
        {
            if (!userId.HasValue)
            {
                return null;
            }
            var user = SelectUser(userId);
            if(user == null)
            {
                return null;
            }
            return SelectToken(user.TokenID);
        }
        public TokenDTO HandleToken(string code)
        {
            //Get token using code from spotify's API
            var tokenDTO = _userRepository.GetAuthToken(code);
            //TODO REFACTOR for separation issues and validation. 
            //Insert TokenDTO into SQL            
           _userRepository.InsertToken(tokenDTO);
           return _userRepository.SelectToken(tokenDTO);
            
        }
        public TokenDTO SelectToken(int tokenId)
        {
            //TODO Refactor for validation and possible errors
            return _userRepository.SelectToken(tokenId);
        }
        public void UpdateTokenIdInUserTable(int? tokenId, int? userId)
        {
            if(tokenId != null || userId != null)
            {
                _userRepository.UpdateTokenIDInUsersTable(tokenId, userId);
            }
                      
        }
        public bool UpdateDJInUserTable(int? isDJ, int? userId)
        {
            if (isDJ != null || userId != null)
            {
                return _userRepository.UpdateDJInUserTable(isDJ, userId).Result;
            }
            return false;

        }
        public ApplicationUser SelectUser(string userName)
        {
            //Get token using code from spotify's API
            return _userRepository.SelectUser(userName);
        }
        public ApplicationUser SelectUser(int? userId)
        {
            //Get token using code from spotify's API
            return _userRepository.SelectUser(userId);
        }

        public string GetUserToken(int userId)
        {
            var user = _userRepository.SelectUser(userId);
            var token = _userRepository.SelectToken(user.TokenID);
            return token.AccessToken;
        }
    }
}