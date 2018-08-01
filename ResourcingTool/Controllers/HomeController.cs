using ResourcingTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ResourcingTool.Controllers
{
    public class HomeController : Controller
    {
        private ResourcingToolEntities db = new ResourcingToolEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Resourcer, Requester")]
        public ActionResult ChangePassword(int userId, string currentPassword, string newPassword, UserErrors userModel)
        {
            if (!ModelState.IsValid) //Checks if input fields have the correct format
            {
                return View(); //Returns the view with the input values so that the user doesn't have to retype again
            }
            using (ResourcingToolEntities db = new ResourcingToolEntities())
            {
                // hash the password and compare against database
                if (!(userId == null || currentPassword == null))
                {
                    var hashedPassword = Sha256encrypt(currentPassword);
                    var leaderDetails = db.Users.Where(x => x.Id == userId && x.Password == hashedPassword).FirstOrDefault();

                    if (leaderDetails != null)
                    {
                        var newHashedPassword = Sha256encrypt(newPassword);
                        db.Set<User>().SingleOrDefault(o => o.Id == userId).Password = newHashedPassword;
                        db.SaveChanges();

                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        //User authentication failed
                        userModel.ErrorMessage = "The current password you've entered is incorrect. Please try again.";
                        return View(userModel);
                    }
                }
                else
                {
                    userModel.ErrorMessage = "Please enter your current password and your new password.";
                    //User authentication failed - blank 
                }

            }
            return View(userModel); //Should always be declared on the end of an action method

        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(UserErrors userModel)
        {
            if (!ModelState.IsValid) //Checks if input fields have the correct format
            {
                return View(userModel); //Returns the view with the input values so that the user doesn't have to retype again
            }
            using (ResourcingToolEntities db = new ResourcingToolEntities())
            {
                // hash the password and compare against database
                if (!(userModel.UserName == null || userModel.Password == null))
                {
                    var hashedPassword = Sha256encrypt(userModel.Password);
                    var userDetails = db.Users.Where(x => x.UserName == userModel.UserName && x.Password == hashedPassword).FirstOrDefault();

                    if (userDetails != null)
                    {

                        var identity = new ClaimsIdentity(new[] {
                             new Claim(ClaimTypes.Role, userDetails.Role),
                             new Claim(ClaimTypes.Name, userDetails.UserName),
                             new Claim(ClaimTypes.NameIdentifier, userDetails.Id.ToString())
                        },
                            "ApplicationCookie");

                        // get owin context
                        var ctx = Request.GetOwinContext();
                        // get authentication manager
                        var authManager = ctx.Authentication;
                        //sign in as claimed identity- in this case the admin
                        //A user is authenticated by calling AuthenticationManager.SignIn
                        authManager.SignIn(identity);


                        //User is authenticated and redirected
                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        userModel.ErrorMessage = "The username or password entered is incorrected. Please try again.";
                        //User authentication failed
                    }
                }
                else
                {
                    userModel.ErrorMessage = "The username or password entered is incorrected. Please try again.";
                    //User authentication failed - blank 
                }

            }
            return View(userModel); //Should always be declared on the end of an action method

        }

        // log the user out.
        [AllowAnonymous]
        public ActionResult LogOut()
        {
            // get owin context
            var ctx = Request.GetOwinContext();
            // get authentication manager
            var authManager = ctx.Authentication;
            //Calling SignOut passing the authentication type (so the manager knows exactly what cookie to remove).
            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Home");
        }

        //method to hash the password using SHA256 encryption
        [AllowAnonymous]
        public static string Sha256encrypt(string phrase)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            SHA256Managed sha256hasher = new SHA256Managed();
            byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(phrase));
            return Convert.ToBase64String(hashedDataBytes);
        }

    }
}