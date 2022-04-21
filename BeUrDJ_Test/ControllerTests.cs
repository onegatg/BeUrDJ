using BeUrDJ_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using app = BeUrDJ_MVC;


namespace BeUrDJ_Test
{
    class ControllerTests
    {
        #region LoginController

        [Test]
        public void AuthorizeUserViewBagTest()
        {
            //var controller = new app.Controllers.AccountController();
            //controller.AuthorizeUser();
            //Assert.AreNotEqual(null, controller.ViewBag.AuthUri);
        }

        

        [Test]
        public void AuthorizeUserViewResultTest()
        {
           // var controller = new app.Controllers.AccountController();
            //var viewResult = controller.AuthorizeUser() as ViewResult;

           // Assert.AreEqual("AuthorizeView", viewResult.ViewName);
        }
        [Test]
        public void LoginIndexViewResultTest()
        {
           // var controller = new app.Controllers.AccountController();
           // var viewResult = controller.Index() as ViewResult;

           // Assert.AreEqual("LoginView", viewResult.ViewName);
        }
        #endregion

        #region FilterController

        [Test]
        public void FilterIndexViewResultTest()
        {
            var controller = new app.Controllers.FilterController();
            var viewResult = controller.Index() as ViewResult;
            var model = viewResult.Model;
            Assert.IsNotNull(model);
        }


        [Test]
        public void UpdateFilterViewResultTest()
        {
            var controller = new app.Controllers.FilterController();
            FiltersModel filterDTO = new FiltersModel();
            filterDTO.danceability = 0.10m;
            filterDTO.electronic = true;
            filterDTO.rock = true;
            filterDTO.punk = true;
            filterDTO.jazz = true;
            filterDTO.rap = true;
            filterDTO.classical = true;
            filterDTO.blues = true;
            filterDTO.reggae = true;
            filterDTO.popularity = 0.00m;
            filterDTO.tempo = 50;
        }
        #endregion

        #region QueueController
        [Test]
        public void QueueIndexViewResultTest()
        {
            //var controller = new app.Controllers.SongController();
            //var viewResult = controller.DisplayQueue(1) as ViewResult;
            //var model = viewResult.Model;
            //Assert.IsNotNull(model);
        }
        #endregion

        #region SessionController
        [Test]
        public void SessionIndexViewResultTest()
        {
            var controller = new app.Controllers.SessionController();
            var viewResult = controller.Index() as ViewResult;

            Assert.AreEqual("Index", viewResult.ViewName);
        }
        #endregion

        #region SearchController
        [Test]
        public void SearchIndexViewResultTest()
        {
           // var controller = new app.Controllers.SongController();
           // var viewResult = controller.Index() as ViewResult;

           // Assert.AreEqual("Index", viewResult.ViewName);
        }
        #endregion
    }
}
