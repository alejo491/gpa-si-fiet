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
    /// <summary>
    /// Clase controlador que permite gestión de Tesis.
    /// </summary>
    public class ThesisController : Controller
    {
        private DbSIEPISContext db = new DbSIEPISContext();

        //
        // GET: /Thesis/
        /// <summary>
        /// Método que carga la vista que contiene todas las tesis registradas en el sistema
        /// </summary>
        /// <returns>Vista que contine los datos de las tesis</returns>
        #region Index()
        public ViewResult Index()
        {
            var theses = db.Theses.Include(t => t.Study);
            return View(theses.ToList());
        }
        #endregion

        //
        // GET: /Thesis/Details/5
        /// <summary>
        /// Método que carga la vista que contine los datos de una tesis
        /// </summary>
        /// <param name="id">Id de la tesis</param>
        /// <returns>Vista para consultar los datos de una tesis</returns>
        #region Details(id)
        public ViewResult Details(Guid id)
        {
            Thesis thesis = db.Theses.Find(id);
            return View(thesis);
        }
        #endregion
       
        //
        // GET: /Thesis/Edit/5
        /// <summary>
        /// Método que carga la vista con el formulario para editar la información de una tesis
        /// </summary>
        /// <param name="id">Id de la tesis</param>
        /// <returns>Vista que despliega el formulario con los datos para editarlos</returns>
        #region Edit(id)
        public ActionResult Edit(Guid id)
        {
            Thesis thesis = db.Theses.Find(id);
            ViewBag.IdStudies = new SelectList(db.Studies, "Id", "Grade", thesis.IdStudies);
            return View(thesis);
        }
        #endregion

        //
        // POST: /Thesis/Edit/5
        /// <summary>
        /// Guarda los cambios de la información de la tesis recibidos en el formulario
        /// </summary>
        /// <param name="school">Tesis a editar</param>
        /// <returns> Redirecciona al inicio</returns>
        /// <returns> Redirecciona a la vista de editar la tesis</returns>
        [HttpPost]
        #region Edit(thesis)
        public ActionResult Edit(Thesis thesis)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thesis).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdStudies = new SelectList(db.Studies, "Id", "Grade", thesis.IdStudies);
            return View(thesis);
        }
        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        #endregion
    }
}