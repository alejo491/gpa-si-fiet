using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIFIET.Models;
using System.Data;
using System.Web.Routing;
using System.Web.Security;

namespace SIFIET.Controllers
{
    public class ExperiencesController : Controller
    {
        //
        // GET: /Experiences/
        private DbSIEPISContext db = new DbSIEPISContext();


        /// <summary>
        /// Renderiza la pagina principal de vacantes
        /// </summary>
        /// <param name="wizardStep">Indicador de a que parte del wizard hace referencia esta funcion</param>
        /// <returns>La vista con la página principal de experiencias</returns>
        public ActionResult Index(int wizardStep = 0)
        {
            ViewBag.WizardStep = wizardStep;
            Guid g = System.Guid.Empty;
            foreach (var e in db.aspnet_Users)
            {

                if (e.UserName == HttpContext.User.Identity.Name)
                {
                    g = e.UserId;
                }

            }

            User user = db.Users.Find(g);
            if (user != null)
            {
                var aspnetuser = db.aspnet_Users.Find(user.Id);
                var roles = Roles.GetRolesForUser(aspnetuser.UserName).ToList();
                if (roles.Contains("Administrador"))
                {
                    var lexperiences = db.Experiences;
                    return View(lexperiences.ToList());
                }
                else
                {
                    ViewBag.UserId = user.Id;
                    return View(db.Experiences.Where(l => l.IdUser == g));
                    
                }

            }
            else return RedirectPermanent("/Account/LogOn");
        }

        /// <summary>
        /// Muestra los detalles para una experiencia en especial
        /// </summary>
        /// <param name="id">Contiene el id de la experiencia de la cual se desean los detalles</param>
        /// <param name="wizardStep">Indicador de a que parte del wizard hace referencia esta funcion</param>
        /// <returns>La vista con la experiencia en detalle</returns>
        public ActionResult Details(Guid id, int wizardStep = 0)
        {
            ViewBag.WizardStep = wizardStep;
            Experience experience = db.Experiences.Find(id);
            Boss b = new Boss();
            foreach (var e in db.ExperiencesBosses)
            {
                if (e.IdExperiences == id)
                {
                    b = db.Bosses.Find(e.IdBoss);
                }
            }
            User u = new User();
            foreach (var exp in db.Experiences)
            {
                if (exp.Id == id)
                {
                    u = db.Users.Find(exp.IdUser);
                }
            }
            Session["IdBoss"] = b.Id;
            Session["IdExperience"] = id;
            ViewBag.NameBoss = b.Name;
            ViewBag.NameUser = u.FirstNames + u.LastNames;
            return View(experience);
        }

        /// <summary>
        /// Atiende el resultado de pulsar el boton de Crear Nueva experiencia en la vista principal
        /// </summary>
        /// <param name="wizardStep">Indicador de a que parte del wizard hace referencia esta funcion</param>
        /// <returns>La vista de Creacion </returns>

        public ActionResult Create(int wizardStep = 0)
        {
            ViewBag.WizardStep = wizardStep;
            ViewBag.IdCompanie = new SelectList(db.Companies, "Id", "Name");
            ViewBag.IdUser = new SelectList(db.Users, "Id", "Id");
            Session["wizardStep"] = wizardStep;
            return View();
        }

        /// <summary>
        /// Atiende el resultado de hacer clic en el boton de Crear Nueva desde la vista de Creacion de Experiencias
        /// </summary>
        /// <param name="vacancy">Contiene los datos de la experiencia para ser llevados a la base de datos</param>
        /// <returns>La vista al listado de experiencias</returns>

        [HttpPost]
        public ActionResult Create(Experience experience, FormCollection form)
        {

            Guid g = System.Guid.Empty;
            foreach (var e in db.aspnet_Users)
            {

                if (e.UserName == HttpContext.User.Identity.Name)
                {
                    g = e.UserId;
                }

            }
            var IdUser = g;
            int wizard = 0;
            if (ModelState.IsValid)
            {
                experience.Id = Guid.NewGuid();
                experience.IdUser = IdUser;
                db.Experiences.Add(experience);
                db.SaveChanges();
                Session["IdExp"] = experience.Id;
                foreach(String key in form){
                if (key.Contains("wizard"))
                {
                    if (form[key].ToString() == "1") { wizard = 1; }
                }
                }
                return RedirectToAction("Create", new RouteValueDictionary(new { controller = "ExperiencesBosses", action = "Create", Id = experience.Id, wizardStep = Session["wizardStep"] }));
                //return RedirectPermanent("/ExperiencesBosses/Create/" + experience.Id+"?wizardStep=1");
            }

            ViewBag.IdCompanie = new SelectList(db.Companies, "Id", "Name", experience.IdCompanie);
            //ViewBag.IdUser = new SelectList(db.Users, "Id", "Id", vacancy.IdUser);
            return View(experience);
        }

        /// <summary>
        /// Atiende el resultado de hacer clic en Editar, en las opciones de cada experiencia
        /// </summary>
        /// <param name="vacancy">Contiene el id de la experiencia que se desea modificar</param>
        /// <param name="wizardStep">Indicador de a que parte del wizard hace referencia esta funcion</param>
        /// <returns>La vista con los datos a editar de la experiencia</returns>
        public ActionResult Edit(Guid id, int wizardStep = 0)
        {

            ViewBag.WizardStep = wizardStep;
            Experience experience = db.Experiences.Find(id);
            ViewBag.IdCompanie = new SelectList(db.Companies, "Id", "Name", experience.IdCompanie);
            // ViewBag.IdUser = new SelectList(db.Users, "Id", "PhoneNumber", vacancy.IdUser);
            return View(experience);
        }

        /// <summary>
        /// Atiende el resultado de hacer clic en Guardar Cambios de la vista de Edicion de experiencias
        /// </summary>
        /// <param name="vacancy">Contiene los datos de la experiencia a actualizar</param>
        /// <returns>La vista con el listado de experiencias</returns>

        [HttpPost]
        public ActionResult Edit(Experience experience, FormCollection form)
        {
            int wizard = 0;
            if (ModelState.IsValid)
            {
                db.Entry(experience).State = EntityState.Modified;
                db.SaveChanges();
                foreach (String key in form)
                {
                    if (key.Contains("wizard"))
                    {
                        if (form[key].ToString() == "1") { wizard = 1; }
                    }
                }
                TempData["Sucess"] = "Exito: Se modifico la experiencia laboral..";
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Experiences", action = "Index", wizardStep = wizard }));
                //return RedirectPermanent("/Experiences/Index/?wizardStep=1");
            }
            ViewBag.IdCompanie = new SelectList(db.Companies, "Id", "Name", experience.IdCompanie);
            //   ViewBag.IdUser = new SelectList(db.Users, "Id", "PhoneNumber", vacancy.IdUser);
            return View(experience);
        }

        /// <summary>
        /// Atiende el resultado de hacer clic en Eliminar, en las opciones de cada experiencia
        /// </summary>
        /// <param name="id">Contiene el id de la experiencia que se desea Eliminar</param>
        /// <param name="wizardStep">Indicador de a que parte del wizard hace referencia esta funcion</param>
        /// <returns>La vista de confirmación</returns>
        public ActionResult Delete(Guid id, int wizardStep = 0)
        {
            ViewBag.WizardStep = wizardStep;
            Experience experience = db.Experiences.Find(id);
            return View(experience);
        }

        /// <summary>
        ///Atiende el resultado de hacer clic en Eliminar de la vista de Confirmacion de eliminacion de experiencias
        /// </summary>
        /// <param name="id">Id de la experiencia que se confirma se desea eliminar</param>
        /// <param name="wizardStep">Indicador de a que parte del wizard hace referencia esta funcion</param>
        /// <returns>La vista con el listado de experiencias, con la experiencia confirmada ya eliminada</returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id, FormCollection form)
        {
            int wizard = 0;
            Experience experience = db.Experiences.Find(id);
            ExperiencesBoss EB = new ExperiencesBoss();
            foreach (var e in db.ExperiencesBosses)
            {
                if (e.IdExperiences == id)
                {
                    EB = db.ExperiencesBosses.Find(e.Id);
                    db.ExperiencesBosses.Remove(EB);
                }
            }
            foreach (String key in form)
            {
                if (key.Contains("wizard"))
                {
                    if (form[key].ToString() == "1") { wizard = 1; }
                }
            }                
            db.Experiences.Remove(experience);            
            db.SaveChanges();
            TempData["Sucess"] = "Exito: Se elimino la experiencia laboral ..";
            return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Experiences", action = "Index", wizardStep = wizard }));
            //return RedirectPermanent("/Experiences/index?wizardStep=1");
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
