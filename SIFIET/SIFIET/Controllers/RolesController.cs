using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIFIET.Models;

namespace SIFIET.Controllers
{ 
    public class RolesController : Controller
    {
        private DbSIEPISContext db = new DbSIEPISContext();

        //
        // GET: /Roles/

        public ViewResult Index()
        {
            var aspnet_roles = db.aspnet_Roles.Include(a => a.aspnet_Applications);
            return View(aspnet_roles.ToList());
        }

        //
        // GET: /Roles/Details/5

        public ViewResult Details(Guid id)
        {
            aspnet_Roles aspnet_roles = db.aspnet_Roles.Find(id);
            return View(aspnet_roles);
        }

        //
        // GET: /Roles/Create

        public ActionResult Create()
        {
            //ViewBag.ApplicationId = new SelectList(db.aspnet_Applications, "ApplicationId", "ApplicationName");
            //ViewBag.Id = new SelectList(db.aspnet_Users, "RolesId", "UserName");
            return View();
        } 

        //
        // POST: /Roles/Create

        [HttpPost]
        public ActionResult Create(aspnet_Roles aspnet_roles)
        {
            if (ModelState.IsValid)
            {
                var IdApplication = db.aspnet_Applications.First().ApplicationId;
                aspnet_roles.ApplicationId = IdApplication;
                var minuscula = aspnet_roles.RoleName;
                aspnet_roles.LoweredRoleName = minuscula.ToLower();
                aspnet_roles.RoleId = Guid.NewGuid();
                db.aspnet_Roles.Add(aspnet_roles);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.ApplicationId = new SelectList(db.aspnet_Applications, "ApplicationId", "ApplicationName", aspnet_roles.ApplicationId);
            return View(aspnet_roles);
        }
        
        //
        // GET: /Roles/Edit/5
 
        public ActionResult Edit(Guid id)
        {
            aspnet_Roles aspnet_roles = db.aspnet_Roles.Find(id);
            ViewBag.ApplicationId = new SelectList(db.aspnet_Applications, "ApplicationId", "ApplicationName", aspnet_roles.ApplicationId);
            return View(aspnet_roles);
        }

        //
        // POST: /Roles/Edit/5

        [HttpPost]
        public ActionResult Edit(aspnet_Roles aspnet_roles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspnet_roles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationId = new SelectList(db.aspnet_Applications, "ApplicationId", "ApplicationName", aspnet_roles.ApplicationId);
            return View(aspnet_roles);
        }

        //
        // GET: /Roles/Delete/5
 
        public ActionResult Delete(Guid id)
        {
            aspnet_Roles aspnet_roles = db.aspnet_Roles.Find(id);
            return View(aspnet_roles);
        }

        //
        // POST: /Roles/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {            
            aspnet_Roles aspnet_roles = db.aspnet_Roles.Find(id);
            db.aspnet_Roles.Remove(aspnet_roles);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}