using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using SIFIET.Models;

namespace SIFIET.Controllers
{
    [Authorize]
    public class BossesController : Controller
    {
        private DbSIEPISContext db = new DbSIEPISContext();


        /// <summary>
        /// Renderiza la pagina principal de vacantes
        /// </summary>
        /// <param name="wizardStep">Indicador de a que parte del wizard hace referencia esta funcion</param>
        /// <returns>La vista con la página principal de jefes</returns>        
        public ActionResult Index()
        {
            return View(db.Bosses.ToList());
        }

        /// <summary>
        /// Muestra los detalles para un jefe en especial
        /// </summary>
        /// <param name="id">Contiene el id del jefe de la cual se desean los detalles</param>
        /// <returns>La vista con la jefe en detalle</returns>
        public ActionResult Details(Guid id)
        {
            Boss boss = db.Bosses.Find(id);
            return View(boss);
        }

        /// <summary>
        /// Muestra los detalles para un jefe en especial teniendo en cuenta el wizard
        /// </summary>
        /// <param name="id">Contiene el id del jefe de la cual se desean los detalles</param>
        /// <param name="wizardStep">Indicador de a que parte del wizard hace referencia esta funcion</param>
        /// <returns>La vista con la jefe en detalle</returns>
        public ActionResult DetailsForExperiences(Guid id, int wizardStep=0)
        {
            ViewBag.wizardStep = wizardStep;
            Boss boss = db.Bosses.Find(id);
            return View(boss);
        }


        /// <summary>
        /// Atiende el resultado de pulsar el boton de Crear Nueva jefe en la vista principal
        /// </summary>
        /// <param name="wizardStep">Indicador de a que parte del wizard hace referencia esta funcion</param>
        /// <returns>La vista de Creacion </returns>
        public ActionResult Create()
        {           
            return View();
        }

        /// <summary>
        /// Atiende el resultado de hacer clic en el boton de Crear Nueva desde la vista de Creacion de jefes
        /// </summary>
        /// <param name="boss">Contiene los datos del jefe para ser llevados a la base de datos</param>
        /// <returns>La vista al listado de jefes</returns>
        [HttpPost]
        public ActionResult Create(Boss boss)
        {
            if (ModelState.IsValid)
            {
                //string nombre = HttpContext.cu .User.Identity
                boss.Id = Guid.NewGuid();
                db.Bosses.Add(boss);
                db.SaveChanges();
                return RedirectToAction("Create", "Bosses");
            }
            return View();
        }

        /// <summary>
        /// Atiende el resultado de pulsar el boton de Crear Nuevo jefe en la vista principal
        /// </summary>
        /// <param name="wizardStep">Indicador de a que parte del wizard hace referencia esta funcion</param>
        /// <returns>La vista de Creacion </returns>
        public ActionResult CreateForExperienceBosses(Guid id,int wizardStep = 0)
        {
            ViewBag.wizardStep = wizardStep;
            ViewBag.idExperiencia = id;
            Session["IdExp"] = id;
            return View();
        }

        //
        // POST: /Companies/CreateForExperience
        /// <summary>
        /// Atiende el resultado de hacer clic en el boton de Crear Nueva desde la vista de Creacion de jefes
        /// </summary>
        /// <param name="boss">Contiene los datos del jefe para ser llevados a la base de datos</param>
        /// <returns>La vista al listado de jefes</returns>
        [HttpPost]
        public ActionResult CreateForExperienceBosses(Boss boss, FormCollection form)
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
                boss.Id = Guid.NewGuid();
                db.Bosses.Add(boss);
                db.SaveChanges();
                TempData["Create"] = "Se ha ingresado correctamente el jefe!";
                TempData["Sucess"] = "Exito : Se creo el jefe ...";
                return RedirectToAction("Create", new RouteValueDictionary(new { controller = "ExperiencesBosses", action = "Create",Id=Session["IdExp"], wizardStep = wizard }));

                //return RedirectPermanent("/ExperiencesBosses/Create/" + Session["IdExp"] + "?wizardStep=1");
            }

            return View(boss);
        }


        /// <summary>
        /// Atiende el resultado de hacer clic en Editar, en las opciones de cada jefe
        /// </summary>
        /// <param name="boss">Contiene el id del jefe que se desea modificar</param>
        /// <param name="wizardStep">Indicador de a que parte del wizard hace referencia esta funcion</param>
        /// <returns>La vista con los datos a editar de la jefe</returns>
        public ActionResult Edit(Guid id)
        {
            Boss boss = db.Bosses.Find(id);
            return View(boss);
        }

        /// <summary>
        /// Atiende el resultado de hacer clic en Guardar Cambios de la vista de Edicion de jefes
        /// </summary>
        /// <param name="boss">Contiene los datos del jefe a actualizar</param>
        /// <returns>La vista con el listado de jefes</returns>
        [HttpPost]
        public ActionResult Edit(Boss boss)
        {
            if (ModelState.IsValid)
            {
                db.Entry(boss).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(boss);
        }

        /// <summary>
        /// Atiende el resultado de hacer clic en Eliminar, en las opciones de cada jefe
        /// </summary>
        /// <param name="id">Contiene el id del jefe que se desea Eliminar</param>
        /// <param name="wizardStep">Indicador de a que parte del wizard hace referencia esta funcion</param>
        /// <returns>La vista de confirmación</returns>
        public ActionResult Delete(Guid id)
        {
            Boss boss = db.Bosses.Find(id);
            return View(boss);
        }

        /// <summary>
        ///Atiende el resultado de hacer clic en Eliminar de la vista de Confirmacion de eliminacion de jefes
        /// </summary>
        /// <param name="id">Id de la jefe que se confirma se desea eliminar</param>
        /// <param name="wizardStep">Indicador de a que parte del wizard hace referencia esta funcion</param>
        /// <returns>La vista con el listado de jefes, con el jefe confirmado ya eliminada</returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Boss boss = db.Bosses.Find(id);
            db.Bosses.Remove(boss);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        /// <summary>
        ///Dispose
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
