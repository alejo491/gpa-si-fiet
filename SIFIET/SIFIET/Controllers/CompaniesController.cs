using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIFIET.Models;
using System.Web.Routing;

namespace SIFIET.Controllers
{
    [Authorize]
    public class CompaniesController : Controller
    {
        
        private DbSIEPISContext db = new DbSIEPISContext();

        //
        // GET: /Companies/

        public ViewResult Index()
        {
            return View(db.Companies.ToList());
        }

        //
        // GET: /Companies/Details/5

        public ViewResult Details(Guid id)
        {
            Company company = db.Companies.Find(id);
            return View(company);
        }

        //
        // GET: /Companies/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Companies/Create

        [HttpPost]
        public ActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                company.Id = Guid.NewGuid();
                db.Companies.Add(company);
                db.SaveChanges();
                TempData["Create"] = "Se ha ingresado correctamente la compañía!";
                return RedirectToAction("Create","Vacancies");  
            }

            return View(company);
        }

        //
        // GET: /Companies/CreateForExperience

        public ActionResult CreateForExperience(int wizardStep = 0)
        {
            ViewBag.wizardStep = wizardStep;
            Session["wizard"] = wizardStep;
            return View();
        }

        //
        // POST: /Companies/CreateForExperience

        [HttpPost]
        public ActionResult CreateForExperience(Company company, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                int wizard=0;
                 foreach(String key in form){
                if (key.Contains("wizard"))
                {
                    if (form[key].ToString() == "1") { wizard = 1; }
                }
                }
                
                company.Id = Guid.NewGuid();
                db.Companies.Add(company);
                db.SaveChanges();
                TempData["Create"] = "Se ha ingresado correctamente la compañía!";

                TempData["Sucess"] = "Exito : Se creo la compañia ...";
                return RedirectToAction("Create", new RouteValueDictionary(new { controller = "Experiences", action = "Create", wizardStep = Session["wizard"] }));
                //return RedirectPermanent("/Experiences/create?wizardStep=1");
            }

            return View(company);
        }

        
        //
        // GET: /Companies/Edit/5
 
        public ActionResult Edit(Guid id)
        {
            Company company = db.Companies.Find(id);
            return View(company);
        }

        //
        // POST: /Companies/Edit/5

        [HttpPost]
        public ActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Update"] = "Se ha actualizado correctamente la información de la compañía!";
                return RedirectToAction("Index");
            }
            return View(company);
        }

        //
        // GET: /Companies/Delete/5
 
        public ActionResult Delete(Guid id)
        {
            Company company = db.Companies.Find(id);
            return View(company);
        }

        //
        // POST: /Companies/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {            
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
            db.SaveChanges();
            TempData["Delete"] = "Se ha borrado la compañía seleccionada!";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}