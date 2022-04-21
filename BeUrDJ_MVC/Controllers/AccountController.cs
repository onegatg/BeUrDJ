using BeUrDJ_MVC.Models;
using BeUrDJ_MVC.Repository;
using BeUrDJ_MVC.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;


namespace BeUrDJ_MVC.Controllers
{
    public class AccountController : Controller
    {

        private readonly AccountService _userService = new AccountService();

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            ViewBag.Title = "BeUrDJ";
        } 
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                //create new user
                var user = new ApplicationUser { UserName = viewModel.UserName };
                var result = await _userManager.CreateAsync(user, viewModel.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    HttpContext.Session.SetInt32("User", user.Id);
                    return Redirect(GetAuthUri());
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(viewModel);
        }
        [HttpGet]
        [Route("dj/login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        [Route("dj/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Login");
        }
        [HttpPost]
        [Route("dj/login")]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(viewModel.UserName, viewModel.Password, viewModel.RememberMe, false);                
                if (result.Succeeded)
                {
                    ApplicationUser userResult = await GetUser(viewModel.UserName);
                    if (userResult != null)
                    {
                        HttpContext.Session.SetInt32("User", userResult.Id);
                        return Redirect(GetAuthUri());
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");                
            }
            return View(viewModel);
        }

        private async Task<ApplicationUser> GetUser(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        //Redirection from spotify ap
        [HttpGet]
        //TODO Refactor Route to something better
        [Route("dj")]
        public async Task<IActionResult> RedirectToDisplaySessionDetailsAsync(string code)
        {
            //TODO REFACTOR HANDLETOKEN METHOD
            var token = _userService.HandleToken(code);

            var user = await _userManager.FindByIdAsync(HttpContext.Session.GetInt32("User").ToString());
            user.TokenID = token.TokenID;
            try
            {
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Session");
                }
                else
                {
                    //TODO refactor to return an error page
                    return View("Login");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View("Login");
            }

       
        }     
        [HttpGet]
        [Route("dj/getUserToken")]
        public string GetUserToken()
        {
            var userId = HttpContext.Session.GetInt32("User").Value;
            return _userService.GetUserToken(userId);
        }
        [HttpGet]
        [Route("dj/checkAccount")]
        public bool CheckAccount()
        {
            var userId = HttpContext.Session.GetInt32("User");
            if(userId != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private string GetAuthUri()
        {
            string clientId = "47a5c9cece574b54bd77ab03ddc871a8";
            string redirectUrl = "https://beurdj.com/dj";
            return "https://accounts.spotify.com/authorize?client_id=" + clientId +
            "&redirect_uri=" + redirectUrl +
            "&response_type=code&scope=user-read-private user-read-email user-library-read streaming user-modify-playback-state"; 
        }
        

    }
}
