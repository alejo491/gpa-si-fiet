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
    public class ExperiencesBossesController : Controller
    {
        private DbSIEPISContext db = new DbSIEPISContext();

        /// <summary>
        /// Renderiza la pagina principal de Experiencias jefe directo 
        /// </summary>
        /// <returns>La vista con la página principal de jefes</returns>        

        public ViewResult Index()
        {
            var experiencesbosses = db.ExperiencesBosses.Include(e => e.Boss).Include(e => e.Experience);
            return View(experiencesbosses.ToList());
        }

        /// <summary>
        /// Muestra los detalles para una experiencia jefe en especial
        /// </summary>
        /// <param name="id">Contiene el id de la tabla experiencesbosses que es la relacion de experiencias de la cual se desean visualizar los detalles</param>
        /// <returns>La vista con la jefe en detalle</returns>
        public ViewResult Details(Guid id)
        {
            ExperiencesBoss experiencesboss = db.ExperiencesBosses.Find(id);

            return View(experiencesboss);
        }

        /// <summary>
        /// Atiende el resultado de pulsar el boton de Crear Nueva experiencia en la vista index de experiencias
        /// </summary>
        /// <param name="id">Contiene el id de la experiencia a la que se la asocia esta tabla </param>
        /// <param name="wizardStep">Indicador de a que parte del wizard hace referencia esta funcion</param>
        /// <returns>La vista de Creacion </returns>
        public ActionResult Create(Guid id, int wizardStep = 0)
        {
            ViewBag.WizardStep = wizardStep;
            ViewBag.IdBoss = new SelectList(db.Bosses, "Id", "Name");
            ViewBag.IdExperiences = id;
            ViewData["IdExp"] = id;
            return View();
        }


        /// <summary>
        /// Se recibe crea una experienciajefe que tiene asociada una experiencia y un jefe mas las fechas de inicio de trabajo y finalizacion de trabajo
        /// </summary>
        /// <param name="id">Contiene el id de la experiencia a la que se la asocia esta tabla </param>
        /// <param name="wizardStep">es el numero para indicar si se esta en el wizard o no</param>
        /// <returns>Retorna a la vista index de experiencias para visualizar la experiencia creada</returns>
        [HttpPost]
        public ActionResult Create(ExperiencesBoss experiencesboss, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                int wizard = 0;
                foreach (String key in form)
                {
                    if (key.Contains("wizard"))
                    {
                        if (form[key].ToString() == "1") { wizard = 1; }
                    }
                }
                experiencesboss.Id = Guid.NewGuid();
                db.ExperiencesBosses.Add(experiencesboss);
                db.SaveChanges();
                TempData["Sucess"] = "Exito: Se creo la experiencia laboral ...";
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Experiences", action = "index", wizardStep = wizard }));
                //return RedirectPermanent("/Experiences/index?wizardStep=1");
            }
            
            ViewBag.IdBoss = new SelectList(db.Bosses, "Id", "Name", experiencesboss.IdBoss);
            ViewBag.IdExperiences = experiencesboss.IdExperiences;
            return View(experiencesboss);
        }

        /// <summary>
        /// Atiende el resultado de hacer clic en Editar, en la pagina de experiencia jefe
        /// </summary>
        /// <param name="id">Contiene el id de la experienciajefe que se desea modificar y carga los valores para ser editados</param>       
        /// <returns>La vista con los datos a editar de la jefe</returns>
        public ActionResult Edit(Guid id)
        {
            ExperiencesBoss experiencesboss = db.ExperiencesBosses.Find(id);
            ViewBag.IdBoss = new SelectList(db.Bosses, "Id", "Name", experiencesboss.IdBoss);
            ViewBag.IdExperiences = new SelectList(db.Experiences, "Id", "Charge", experiencesboss.IdExperiences);
            return View(experiencesboss);
        }
        /// <summary>
        /// Atiende el resultado de hacer clic en actualizar de la vista de Edicion de experienciajefes
        /// </summary>
        /// <param name="experiencesBoss">Contiene los datos del la experienciajefe a actualizar</param>
        /// <returns>La vista con el listado de jefes</returns>
        [HttpPost]
        public ActionResult Edit(ExperiencesBoss experiencesboss)
        {
            if (ModelState.IsValid)
            {
                db.Entry(experiencesboss).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdBoss = new SelectList(db.Bosses, "Id", "Name", experiencesboss.IdBoss);
            ViewBag.IdExperiences = new SelectList(db.Experiences, "Id", "Charge", experiencesboss.IdExperiences);
            return View(experiencesboss);
        }
        
        /// <summary>
        /// Atiende el resultado de hacer clic en Eliminar, en las opciones de cada experienciasJefe
        /// </summary>
        /// <param name="id">Contiene el id de la experienciajefe que se desea Eliminar</param>        
        /// <returns>La vista de confirmación</returns>
        
        public ActionResult Delete(Guid id)
        {
            ExperiencesBoss experiencesboss = db.ExperiencesBosses.Find(id);
            return View(experiencesboss);
        }

        /// <summary>
        ///Atiende el resultado de hacer clic en Eliminar de la vista de Confirmacion de eliminacion de experienciaJefe
        /// </summary>
        /// <param name="id">Id de la experienciaJefe que se confirma a eliminar</param>        
        /// <returns>Retona a la vista index con la experienciaJefe ya eliminada</returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ExperiencesBoss experiencesboss = db.ExperiencesBosses.Find(id);
            db.ExperiencesBosses.Remove(experiencesboss);
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