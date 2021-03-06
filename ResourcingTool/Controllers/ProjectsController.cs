﻿using System;
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
        private ResourcingToolConnection db = new ResourcingToolConnection();

        // GET: Projects
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View(db.Projects.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
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

        public ActionResult Create([Bind(Include = "ProjectId,ProjectName,ProjectSector,ProjectClient,ProjectLead,ProjectNature,ProjectScope,ContractStart,ContractFinish,EstimatedBriefing,FieldworkStart,FieldworkEnd,AlysRepStart,AlysRepEnd,EstimatedDebriefing,KeyInformation,SpecialRequirements,NumDirectorsRequired,NumSeniorManagersRequired,NumManagersRequired,NumSeniorAssociate2Required,NumSeniorAssociate1Required,NumAssociate2Required,NumAssociate1Required,DaysDirectorsRequired,DaysSeniorManagersRequired,DaysManagersRequired,DaysSeniorAssociate2Required,DaysSeniorAssociate1Required,DaysAssociate2Required,DaysAssociate1Required,ResponseNeededBy,fk_UserId_Requester,fk_UserName_Requester,DateSubmitted,Status,DCStartDate,DCEndDate,DCTotalM,DCTotalSA2,DCTotalSA1,DCTotalA2,DCTotalA1,,DAStartDate,DAEndDate,DATotalM,DATotalSA2,DATotalSA1,DATotalA2,DATotalA1,DVStartDate,DVEndDate,DVTotalM,DVTotalSA2,DVTotalSA1,DVTotalA2,DVTotalA1,Date1, DTime1, SMTime1, MTime1, SA2Time1, SA1Time1, A2Time1, A1Time1, Date2, DTime2, SMTime2, MTime2, SA2Time2, SA1Time2, A2Time2, A1Time2, Date3, DTime3, SMTime3, MTime3, SA2Time3, SA1Time3, A2Time3, A1Time3, Date4, DTime4, SMTime4, MTime4, SA2Time4, SA1Time4, A2Time4, A1Time4, Date5, DTime5, SMTime5, MTime5, SA2Time5, SA1Time5, A2Time5, A1Time5, Date6, DTime6, SMTime6, MTime6, SA2Time6, SA1Time6, A2Time6, A1Time6, Date7, DTime7, SMTime7, MTime7, SA2Time7, SA1Time7, A2Time7, A1Time7, Date8, DTime8, SMTime8, MTime8, SA2Time8, SA1Time8, A2Time8, A1Time8, Date9, DTime9, SMTime9, MTime9, SA2Time9, SA1Time9, A2Time9, A1Time9, Date10, DTime10, SMTime10, MTime10, SA2Time10, SA1Time10, A2Time10, A1Time10, Date11, DTime11, SMTime11, MTime11, SA2Time11, SA1Time11, A2Time11, A1Time11, Date12, DTime12, SMTime12, MTime12, SA2Time12, SA1Time12, A2Time12, A1Time12, Date13, DTime13, SMTime13, MTime13, SA2Time13, SA1Time13, A2Time13, A1Time13, Date14, DTime14, SMTime14, MTime14, SA2Time14, SA1Time14, A2Time14, A1Time14, Date15, DTime15, SMTime15, MTime15, SA2Time15, SA1Time15, A2Time15, A1Time15, TechRequired")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (User.Identity.IsAuthenticated)
            {
                return View(project);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

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
        public ActionResult Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        public ActionResult Statistics()
        {
            return View(db.Projects.ToList());
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

        public ActionResult DeleteFromDB(int projectId)
        {
            Project project = db.Projects.Find(projectId);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize(Roles = "Admin, Resourcer")]
        [ValidateInput(false)]
        public ActionResult SubmitStatus(string LastEditBy, string ActionDetails, int projectID, string projectStatus)
        {
            using (db)
            {
                if (ModelState.IsValid)
                {
                    db.Set<Project>().SingleOrDefault(o => o.ProjectId == projectID).Status = projectStatus;
                    db.Set<Project>().SingleOrDefault(o => o.ProjectId == projectID).ActionDetails = ActionDetails;
                    db.Set<Project>().SingleOrDefault(o => o.ProjectId == projectID).LastEditBy = LastEditBy;
                    db.Set<Project>().SingleOrDefault(o => o.ProjectId == projectID).EditTime = DateTime.Now;
                    db.Set<Project>().SingleOrDefault(o => o.ProjectId == projectID).Status = projectStatus;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Projects");
                }
                else
                {
                    return RedirectToAction("Index", "Projects");
                }
            }
        }

        [Authorize(Roles = "Admin, Resourcer")]
        public ActionResult Approve(int projectID)
        {
            using (db)
            {
                if (ModelState.IsValid)
                {
                    db.Set<Project>().SingleOrDefault(o => o.ProjectId == projectID).Status = "Completed";
                    db.SaveChanges();
                    return RedirectToAction("Index", "Projects");
                }
                else
                {
                    return RedirectToAction("Index", "Projects");
                }
            }
        }
        [Authorize(Roles = "Admin, Resourcer")]
        public ActionResult Inprogress(int projectID)
        {
            using (db)
            {
                if (ModelState.IsValid)
                {
                    db.Set<Project>().SingleOrDefault(o => o.ProjectId == projectID).Status = "In Progress";
                    db.SaveChanges();
                    return RedirectToAction("Index", "Projects");
                }
                else
                {
                    return RedirectToAction("Index", "Projects");
                }
            }
        }

        [Authorize(Roles = "Admin, Resourcer")]
        public ActionResult Deny(int projectID)
        {
            using (db)
            {
                if (ModelState.IsValid)
                {
                    db.Set<Project>().SingleOrDefault(o => o.ProjectId == projectID).Status = "Submitted";
                    db.SaveChanges();
                    return RedirectToAction("Index", "Projects");
                }
                else
                {
                    return RedirectToAction("Index", "Projects");
                }
            }
        }

    }
}
