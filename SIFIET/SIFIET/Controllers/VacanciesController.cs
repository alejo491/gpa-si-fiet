using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIFIET.Models;
using PagedList;


namespace SIFIET.Controllers
{
   
    public class VacanciesController : Controller
    {
        private DbSIEPISContext db = new DbSIEPISContext();
        private int pageSize = 10;
        private int pageNumber;

        /// <summary>
        /// Renderiza la pagina principal de vacantes
        /// </summary>
        /// <param name="page">Indicador de paginacion</param>
        /// <returns>La vista con el listado de vacantes sacadas de la base de datos</returns>

        public ViewResult Index(int? page)
        {
            var vacancies = db.Vacancies.Include(v => v.Company).Include(v => v.User);
            pageNumber = (page ?? 1);

            try { 
                Session["userID"] = db.aspnet_Users.Where(u => u.UserName.Equals(HttpContext.User.Identity.Name)).First().UserId;
                ;
            }
            catch (Exception e) { }
                            
            return View(vacancies.ToList().OrderByDescending(v => v.PublicationDate).ToPagedList(pageNumber, pageSize));
            // return View(vacancies.ToList().OrderByDescending(v => v.PublicationDate));
        }

        /// <summary>
        /// Renderiza el panel que contine las 5 ultimas vacantes ingresadas para ser mostradas en la página principal
        /// </summary>
        /// <returns>La vista con las 5 ultimas vacantes</returns>

        public ActionResult Index2()
        {
            var vacancies = db.Vacancies.Include(v => v.Company).Include(v => v.User).OrderByDescending(v => v.PublicationDate).Take(5);                  
            return PartialView(vacancies.ToList());
        }


        /// <summary>
        /// Muestra los detalles para una vacante en especial
        /// </summary>
        /// <param name="id">Contiene el id de la vacante de la cual se desean los detalles</param>
        /// <returns>La vista con la vacante en detalle</returns>
        public ViewResult Details(Guid id)
        {
            
            Vacancy vacancy = db.Vacancies.Find(id);
            try
            {
                Session["allowVacancyEdit"] = "false";
                Session["userID"] = db.aspnet_Users.Where(u => u.UserName.Equals(HttpContext.User.Identity.Name)).First().UserId;
                if (vacancy.IdUser.ToString().Equals(Session["userID"].ToString()) || System.Web.Security.Roles.GetRolesForUser().Contains("Administrador"))
                {
                    Session["allowVacancyEdit"] = "true";
                }
            }
            catch (Exception e) { }
            return View(vacancy);
        }


        /// <summary>
        /// Atiende el resultado de pulsar el boton de Crear Nueva vacante en la vista principal
        /// </summary>
        /// <returns>La vista de Creacion solamente en el caso de que el usuario este loggeado</returns>
        [Authorize]
        public ActionResult Create()
        {
           
                ViewBag.IdCompanie = new SelectList(db.Companies, "Id", "Name");
                ViewBag.IdUser = new SelectList(db.Users, "Id", "Id");
                Guid g = System.Guid.Empty;
                foreach (var e in db.aspnet_Users)
                {
                    if (e.UserName == HttpContext.User.Identity.Name)
                    {
                        g = e.UserId;
                    }
                }
                var IdUser = g;

                bool dataUpdate = false;
                foreach (var User in db.Users) // Se busca si el usuario ha actualizado los datos de la cuenta
                {
                    if (User.Id == IdUser)
                    {
                        dataUpdate = true;
                    }

                }

                if (dataUpdate)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("ErrorDataUpdate", "Error");
                }
            
          
        }


        /// <summary>
        /// Atiende el resultado de hacer clic en el boton de Crear Nueva desde la vista de Creacion de Vacantes
        /// </summary>
        /// <param name="vacancy">Contiene los datos de la vacantes para ser llevados a la base de datos</param>
        /// <returns>La vista al listado de vacantes</returns>
        [HttpPost]
        public ActionResult Create(Vacancy vacancy)
        {

            string nombreCompania = Request.Params[0];

            Company Comp = null;

            if (nombreCompania != null)
            {
                foreach (var e in db.Companies)
                {
                    if (e.Name == nombreCompania)
                    {
                        Comp = e;
                    }
                }
            }

            if (Comp == null)
            {
                TempData["alertMessage"] = "Error: Compañia Inexistente";
                return View();             
            }

            var IdUser = db.aspnet_Users.Where(u => u.UserName.Equals(HttpContext.User.Identity.Name)).First().UserId;


            if (ModelState.IsValid)
            {
                vacancy.Id = Guid.NewGuid();
                vacancy.IdUser = IdUser;
                vacancy.PublicationDate = DateTime.Now;
                vacancy.Company = Comp;
                db.Vacancies.Add(vacancy);
                db.SaveChanges();
                TempData["Create"] = "Se ha ingresado correctamente la vacante !";
                return RedirectToAction("Index");
            }

            ViewBag.IdCompanie = new SelectList(db.Companies, "Id", "Name", vacancy.IdCompanie);


            return View(vacancy);
        }

        /// <summary>
        /// Atiende el resultado de hacer clic en Editar, en las opciones de cada vacante
        /// </summary>
        /// <param name="vacancy">Contiene el id de la vacante que se desea modificar</param>
        /// <returns>La vista con los datos a editar de la vacante</returns>
        [Authorize]
        public ActionResult Edit(Guid id)
        {
            Vacancy vacancy = db.Vacancies.Find(id);
            ViewBag.IdCompanie = new SelectList(db.Companies, "Id", "Name", vacancy.IdCompanie);
            // ViewBag.IdUser = new SelectList(db.Users, "Id", "PhoneNumber", vacancy.IdUser);
            return View(vacancy);
        }


        /// <summary>
        /// Atiende el resultado de hacer clic en Editar de la vista de Edicion de vacantes
        /// </summary>
        /// <param name="vacancy">Contiene los datos de la vacante a actualizar</param>
        /// <returns>La vista con el listado de vacantes</returns>
        [HttpPost]
        public ActionResult Edit(Vacancy vacancy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vacancy).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Update"] = "Se ha actualizado correctamente la información de la vacante !";
                return RedirectToAction("Index");
            }
            ViewBag.IdCompanie = new SelectList(db.Companies, "Id", "Name", vacancy.IdCompanie);
            //   ViewBag.IdUser = new SelectList(db.Users, "Id", "PhoneNumber", vacancy.IdUser);

            return View(vacancy);
        }

        /// <summary>
        /// Atiende el resultado de hacer clic en Eliminar, en las opciones de cada vacante
        /// </summary>
        /// <param name="id">Contiene el id de la vacante que se desea Eliminar</param>
        /// <returns>La vista de confirmación</returns>
         [Authorize]
        public ActionResult Delete(Guid id)
        {
            Vacancy vacancy = db.Vacancies.Find(id);
            return View(vacancy);
        }


        /// <summary>
        ///Atiende el resultado de hacer clic en Eliminar de la vista de Confirmacion de eliminacion de vacantes
        /// </summary>
        /// <param name="id">Id de la vacante que se confirma se desea eliminar</param>
        /// <returns>La vista con el listado de vacantes, con la vacante confirmada ya eliminada</returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Vacancy vacancy = db.Vacancies.Find(id);
            db.Vacancies.Remove(vacancy);
            db.SaveChanges();
            TempData["Delete"] = "Se ha borrado la vacante seleccionada!";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }



        /// <summary>
        ///Atiende el resultado de hacer clic en Buscar de la vista Principal
        /// </summary>
        /// <param name="criteria">Contiene las palabras clave con las que se desea hacer la busquedar</param>
        /// <param name="page">Elemento de control para la paginación</param>
        /// <returns>El listado de vacantes encontrados con el termindo de busqueda deseado</returns>
        public ActionResult Search(string criteria, int? page)
        {
            try
            {
                Session["userID"] = db.aspnet_Users.Where(u => u.UserName.Equals(HttpContext.User.Identity.Name)).First().UserId;
            }
            catch (Exception e)
            { }
            ViewBag.CurrentFilter = criteria;

            if (criteria == null)
            {
                criteria = "";
            }

            string searchText = criteria.ToLower().Trim();


            //Búsqueda
            var vacancies = db.Vacancies.Where(v => v.Charge.ToLower().Contains(criteria) || v.Description.Contains(criteria) ||
                v.ProfessionalProfile.Contains(criteria));

            //Ordenar por fecha de publicación
            var results = vacancies.ToList().OrderByDescending(c => c.PublicationDate);


            pageNumber = (page ?? 1);
            return View(results.ToPagedList(pageNumber, pageSize));

        }



        /// <summary>
        ///Funcion que apoya el autocompletar de compañias desde la vista
        /// </summary>
        /// <param name="term">Contiene las letras con las que se quiere buscar coincidencias</param>
        /// <returns>El listado de vacantes que coincidan con los terminos de busqueda</returns>
        public ActionResult AutocompleteCompanies(string term)
        {
            var items = (from u in db.Companies select u.Name).ToArray();
            var filteredItems = items.Where(
                item => item.StartsWith(term, StringComparison.InvariantCultureIgnoreCase));
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }


    }
}




//---------------- FRAGMENTOS DE CODIGO ANTIGUOS ---------------------

/*    if (criteria.ToLower().Trim().Equals(searchText))
    {
        pageNumber = (page ?? 1);
        return View(results.ToPagedList(pageNumber, pageSize));
    }
*/

//---------------------------------------------------------------------

