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
    public class RoleController : Controller
    {
        private DbSIEPISContext db = new DbSIEPISContext();

        //
        // GET: /Role/

        public ViewResult Index()
        {
            var aspnet_roles = db.aspnet_Roles.Include(a => a.aspnet_Applications);
            return View(aspnet_roles.ToList());
        }

        //
        // GET: /Role/Details/5

        public ViewResult Details(Guid id)
        {
            aspnet_Roles aspnet_roles = db.aspnet_Roles.Find(id);
            return View(aspnet_roles);
        }

        //
        // GET: /Role/Create

        public ActionResult Create()
        {
            ViewBag.ApplicationId = new SelectList(db.aspnet_Applications, "ApplicationId", "ApplicationName");
            return View();
        } 

        //
        // POST: /Role/Create

        [HttpPost]
        public ActionResult Create(aspnet_Roles aspnet_roles)
        {
            if (ModelState.IsValid)
            {
                aspnet_roles.RoleId = Guid.NewGuid();
                db.aspnet_Roles.Add(aspnet_roles);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.ApplicationId = new SelectList(db.aspnet_Applications, "ApplicationId", "ApplicationName", aspnet_roles.ApplicationId);
            return View(aspnet_roles);
        }
        
        //
        // GET: /Role/Edit/5
 
        public ActionResult Edit(Guid id)
        {
            aspnet_Roles aspnet_roles = db.aspnet_Roles.Find(id);
            ViewBag.ApplicationId = new SelectList(db.aspnet_Applications, "ApplicationId", "ApplicationName", aspnet_roles.ApplicationId);
            return View(aspnet_roles);
        }

        //
        // POST: /Role/Edit/5

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
        // GET: /Role/Delete/5
 
        public ActionResult Delete(Guid id)
        {
            aspnet_Roles aspnet_roles = db.aspnet_Roles.Find(id);
            return View(aspnet_roles);
        }

        //
        // POST: /Role/Delete/5

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