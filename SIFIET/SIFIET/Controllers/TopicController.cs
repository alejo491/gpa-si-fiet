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
    public class TopicController : Controller
    {
        /// <summary>
        /// Se encarga de los procesos relacionados con los temas
        /// de las encuestas; acciones tales como: crear, ver, eliminar, editar y listar
        /// </summary>
        private DbSIEPISContext db = new DbSIEPISContext();

        //
        // GET: /Topic/
        /// <summary>
        /// Esta función muestra un listado de todos los temas(Topics) registrados en el sistema
        /// </summary>
        /// <returns>Listado de temas</returns>
        public ViewResult Index()
        {
            return View(db.Topics.ToList());
        }

        //
        // GET: /Topic/Details/5

        /// <summary>
        /// Muestra en detalle un determinado tema(Topic)
        /// </summary>
        /// <param name="id">Identificador del tema(Topic)</param>
        /// <returns>Una Vista que contiene la información del Tema</returns>
        public ViewResult Details(Guid id)
        {
            Topic topic = db.Topics.Find(id);
            return View(topic);
        }

        //
        // GET: /Topic/Create

        /// <summary>
        /// Crea una Tema(Topic) por medio de una vista
        /// </summary>
        /// <returns>Una Vista donde se diligenciaran los datos del nuevo tema(Topic)</returns>
        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Topic/Create

        [HttpPost]
        public ActionResult Create(Topic topic)
        {
            if (ModelState.IsValid)
            {
                topic.Id = Guid.NewGuid();
                db.Topics.Add(topic);
                db.SaveChanges();
                TempData["Create"] = "Se registró correctamente el tema " + topic.Description + "!";
                return RedirectToAction("Index");  
            }

            return View(topic);
        }
        
        //
        // GET: /Topic/Edit/5
        /// <summary>
        /// Edita un tema(Topic) en especifico
        /// </summary>
        /// <param name="id">Identificador del Tema(Topic)</param>
        /// <returns>Una Vista donde podra actualizar los datos del tema(Topic) que desea modificar</returns>
        public ActionResult Edit(Guid id)
        {
            Topic topic = db.Topics.Find(id);
            return View(topic);
        }

        //
        // POST: /Topic/Edit/5

        [HttpPost]
        public ActionResult Edit(Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(topic).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Update"] = "Se actualizó correctamente el tema " + topic.Description + "!";
                return RedirectToAction("Index");
            }
            return View(topic);
        }

        //
        // GET: /Topic/Delete/5
        /// <summary>
        /// Elimina un tema(Topic) determinado
        /// </summary>
        /// <param name="id">Identificador del Tema(Topic)</param>
        /// <returns>Una Vista con la información del tema(Topic).
        /// Tendra las opciones para eliminar el tema si así lo desea</returns>
        public ActionResult Delete(Guid id)
        {
            Topic topic = db.Topics.Find(id);
            return View(topic);
        }

        //
        // POST: /Topic/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {            
            Topic topic = db.Topics.Find(id);
            db.Topics.Remove(topic);
            db.SaveChanges();
            TempData["Delete"] = "Se eliminó correctamente el tema " + topic.Description + "!";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}