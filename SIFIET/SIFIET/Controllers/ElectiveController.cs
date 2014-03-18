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
    /// Clase controlador que permite gestión de electivas.
    /// </summary>
    public class ElectiveController : Controller
    {
        private DbSIEPISContext db = new DbSIEPISContext();

        //
        // GET: /Elective/
        /// <summary>
        /// Método que carga la vista que contiene todas las electivas registradas en el sistema
        /// </summary>
        /// <returns>Vista que contine los datos de las instituciones</returns>
        #region Index()
        public ViewResult Index()
        {
            return View(db.Electives.ToList());
        }
        #endregion

        //
        // GET: /Elective/Details/5
        /// <summary>
        /// Método que carga la vista con los datos de una electiva
        /// </summary>
        /// <param name="id">Id de la electiva</param>
        /// <returns>Vista para consultar los datos de una electiva</returns>
        #region Details(id)
        public ViewResult Details(Guid id)
        {
            Elective elective = db.Electives.Find(id);
            return View(elective);
        }
        #endregion

        //
        // GET: /Elective/Edit/5
        /// <summary>
        /// Método que carga la vista con el formulario para editar la información de una electiva
        /// </summary>
        /// <param name="id">Id de la electiva</param>
        /// <returns>Vista que despliega el formulario con los datos para editarlos</returns>
        #region Edit(id)
        public ActionResult Edit(Guid id)
        {
            Elective elective = db.Electives.Find(id);
            return View(elective);
        }
        #endregion

        //
        // POST: /Elective/Edit/5
        /// <summary>
        /// Guarda los cambios de la información de la electiva recibidos en el formulario
        /// </summary>
        /// <param name="elective">Electiva a editar</param>
        /// <returns>Redirecciona al inicio</returns>
        /// <returns>Redirecciona a la vista de electivas</returns>
        [HttpPost]
        #region Edit(elective)
        public ActionResult Edit(Elective elective)
        {
            if (ModelState.IsValid)
            {
                db.Entry(elective).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(elective);
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