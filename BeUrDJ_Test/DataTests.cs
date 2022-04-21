using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using app = BeUrDJ_MVC;
namespace BeUrDJ_Test
{
    public class DataTests
    {
        

        [SetUp]
        public void Setup()
        {
        }

        #region User Repository
        [Test]
        public void InsertUserTest()
        {
            //TODO needs to be refactored to meet new create method using UserStore
            //app.Models.ApplicationUser userModel = new app.Models.ApplicationUser
            //{
            //    UserName = "testUser",
            //    /Password = "pass",
            //    TokenID = 1
            //};

            //app.Repository.UserRepository userRepository = new app.Repository.UserRepository();
            //userRepository.InsertUser(userModel);

            //var data = userRepository.SelectUser(userModel);
            //if(data != null)
            //{
            //    userRepository.DeleteUser(data);
            //}
            //Assert.AreEqual(userModel.UserName, data.UserName);
            //var controller = new app.Controllers.AccountController();
            //ViewResult result = controller.Index() as ViewResult;
            //var model = result.Model;
        }
        [Test]
        public void UpdateTokenInUserTableTest()
        {
            //app.Repository.UserRepository userRepository = new app.Repository.UserRepository();
            //app.Models.UserDTO userModel = new app.Models.UserDTO
            //{
            //    UserName = "testUser",
            //    Password = "pass",
            //    TokenID = 8888
            //};
            //var tokenId = 9999;
            //userRepository.InsertUser(userModel);
            //var dbModel = userRepository.SelectUser(userModel);                      
            //if (dbModel != null)
            //{
            //    userRepository.UpdateTokenIDInUsersTable(tokenId, dbModel);
            //    dbModel = userRepository.SelectUser(userModel);
            //    userRepository.DeleteUser(dbModel);
            //}
            ////Verify data is not equal - data value changed from 8888 to 9999
            //Assert.AreNotEqual(userModel.TokenID, dbModel.TokenID);
        }
        [Test]
        public void InsertToken()
        {
            app.Repository.AccountRepository userRepository = new app.Repository.AccountRepository();
            app.Models.TokenDTO tokenModel = new app.Models.TokenDTO
            {
                AccessToken = "AQArnLlwSg4WOsHTE5msFz260uBOzja7Y2rtR1N5aXEnA4dVx1iTGeG-GtrBuvCZXRa_JOpOD9eGCnCv9toxuN4Vvvxv8D4MI0JazDkRJeblTHvW53joI2OWtgXDz6tC7BLhZbnCOEvq59sZhj4CfwIgll4ksuM7",
                TokenType = "Bearer",
                RefreshToken = "AQCYy0xGOeWCj98tH8yEKM12sqdOjeXxLBYV8CLWmDZbeacndhiCMCnA9iAZcnCReiSXIsE4JxP-uVoEbVq1MozORCl2zXRdcDla-CFkBaD5ivMm4mMSe7rD0cdwmvJX7Lg",
                Scope = "user-read-private",
                ExpiresIn = 3600
            };
            
            userRepository.InsertToken(tokenModel);
            

            var data = userRepository.SelectToken(tokenModel);
            if (data != null)
            {
                userRepository.DeleteToken(data);
            }
            Assert.AreEqual(tokenModel.AccessToken, data.AccessToken);
        }




        #endregion

        #region Filter Repository
        [Test]

        public void GetFilterTest()
        {
            app.Repository.FilterRepository filterRepository = new app.Repository.FilterRepository();
            /*
            app.Models.FilterDTO filterModel = new app.Models.FilterDTO
            {
                tempo = 0.0m,
                genre = "Pop",
                danceability = 0.0m,
                popularity = 0.0m,
                sessionId = 3,
            };
            */
            var filter = filterRepository.GetFilter(1);
            Assert.IsNotNull(filter);
        }
        [Test]

        public void InsertFilterTest()
        {

        }
        [Test]

        public void UpdateFilterTest()
        {
            app.Repository.FilterRepository filterRepository = new app.Repository.FilterRepository();
            app.Models.FiltersModel filterModel = new app.Models.FiltersModel
            {
                filterId = 1,
                tempo = 50,
                electronic = true,
                rock = true,
                punk = true,
                rap = true,
                blues = true,
                classical = true,
                reggae = true,
                danceability = 0.0m,
                popularity = 0.0m,
                sessionId = 1,
            };
             filterRepository.UpdateFilter(filterModel);
            var filter = filterRepository.GetFilter(filterModel.sessionId);
            Assert.AreEqual(filterModel.filterId, filter.filterId);
            Assert.AreEqual(filterModel.tempo, filter.tempo);
            Assert.AreEqual(filterModel.electronic, filter.electronic);
            Assert.AreEqual(filterModel.danceability, filter.danceability);
            Assert.AreEqual(filterModel.popularity, filter.popularity);
            Assert.AreEqual(filterModel.sessionId, filter.sessionId);

        }
        [Test]

        public void DeleteFilterTest()
        {

        }

        #endregion

        #region Queue Repository
        [Test]

        public void GetQueue()
        {
            app.Repository.SongRepository songRepository = new app.Repository.SongRepository();
            /*
            app.Models.FilterDTO filterModel = new app.Models.FilterDTO
            {
                tempo = 0.0m,
                genre = "Pop",
                danceability = 0.0m,
                popularity = 0.0m,
                sessionId = 3,
            };
            */
            var queue = songRepository.GetQueue(1);
            Assert.IsNotNull(queue);
        }
        [Test]

        public void InsertQueueTest()
        {

        }
        [Test]

        public void UpdateQueueTest()
        {

        }
        [Test]

        public void DeleteQueueTest()
        {

        }

        #endregion
    }
}