using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ResourcingTool.Models;

namespace ResourcingTool.Controllers
{
    public class ProjectsController : Controller
    {
        private ResourcingToolEntitiesAzure db = new ResourcingToolEntitiesAzure();

        // GET: Projects
        [Authorize(Roles = "Admin, Resourcer, Requester")]
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Create([Bind(Include = "ProjectId,ProjectName,ProjectSector,ProjectClient,ProjectLead,ProjectNature,ProjectScope,ContractStart,ContractFinish,Briefing,Fieldwork,Analysis,Reporting,Debrief,KeyInformation,SpecialRequirements,NumDirectorsRequired,NumSeniorManagersRequired,NumManagersRequired,NumSeniorAssociate2Required,NumSeniorAssociate1Required,NumAssociate2Required,NumAssociate1Required,DaysDirectorsRequired,DaysSeniorManagersRequired,DaysManagersRequired,DaysSeniorAssociate2Required,DaysSeniorAssociate1Required,DaysAssociate2Required,DaysAssociate1Required,ResponseNeededBy,fk_UserId_Requester,Status")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectId,ProjectName,ProjectSector,ProjectClient,ProjectLead,ProjectNature,ProjectScope,ContractStart,ContractFinish,Briefing,Fieldwork,Analysis,Reporting,Debrief,KeyInformation,SpecialRequirements,NumDirectorsRequired,NumSeniorManagersRequired,NumManagersRequired,NumSeniorAssociate2Required,NumSeniorAssociate1Required,NumAssociate2Required,NumAssociate1Required,DaysDirectorsRequired,DaysSeniorManagersRequired,DaysManagersRequired,DaysSeniorAssociate2Required,DaysSeniorAssociate1Required,DaysAssociate2Required,DaysAssociate1Required,ResponseNeededBy,fk_UserId_Requester, Status")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Projects");
            }
            else
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin, Resourcer, Requester")]
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
            using (ResourcingToolEntitiesAzure db = new ResourcingToolEntitiesAzure())
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

                        return RedirectToAction("Index", "Projects");

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
            using (ResourcingToolEntitiesAzure db = new ResourcingToolEntitiesAzure())
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
                             new Claim(ClaimTypes.Name, userDetails.Name),
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
                        return RedirectToAction("Index", "Projects");

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
        [Authorize(Roles = "Admin, Resourcer, Requester")]
        public ActionResult LogOut()
        {
            // get owin context
            var ctx = Request.GetOwinContext();
            // get authentication manager
            var authManager = ctx.Authentication;
            //Calling SignOut passing the authentication type (so the manager knows exactly what cookie to remove).
            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Login", "Projects");
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
