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
    /// Clase controlador que permite gestion de Instituciones.
    /// </summary>
    public class SchoolController : Controller
    {
        private DbSIEPISContext db = new DbSIEPISContext();

        //
        // GET: /School/
        /// <summary>
        /// Método que carga la vista que contiene todas las instituciones registradas en el sistema
        /// </summary>
        /// <returns>Vista que contine los datos de las instituciones</returns>
        #region Index()
        public ViewResult Index()
        {
            return View(db.Schools.ToList());
        }
        #endregion

        //
        // GET: /School/Details/5
        /// <summary>
        /// Método que carga la vista que contine los datos de una institución
        /// </summary>
        /// <param name="id">Id de la institución</param>
        /// <returns>Vista para consultar los datos de una institución</returns>
        #region Details(id)
        public ViewResult Details(Guid id)
        {
            School school = db.Schools.Find(id);
            return View(school);
        }
        #endregion
        
        //
        // GET: /School/Edit/5
        /// <summary>
        /// Método que carga la vista con el formulario para editar la información de una institución
        /// </summary>
        /// <param name="id">Id de la escuela</param>
        /// <returns>Vista que despliega el formulario con los datos para editarlos</returns>
        #region Edit(id)
        public ActionResult Edit(Guid id)
        {
            School school = db.Schools.Find(id);
            return View(school);
        }
        #endregion

        //
        // POST: /School/Edit/5
        /// <summary>
        /// Guarda los cambios de la información de la escuela recibidos en el formulario
        /// </summary>
        /// <param name="school">institución a editar</param>
        /// <returns>Redirecciona al inicio</returns>
        /// redirecciona a la vista de Instituciones
        [HttpPost]
        #region Edit(school)
        public ActionResult Edit(School school)
        {
            if (ModelState.IsValid)
            {
                db.Entry(school).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(school);
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