using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIFIET.Models;
using System.Web.Security;
using System.Web.Routing;
using iTextSharp.text;


namespace SIFIET.Controllers
{
    /// <summary>
    /// Controlador para gestionar los estudios que estan registrados en el sistema
    /// </summary>
    public class StudyController : Controller
    {
        /// <summary>
        /// Atributo que consulta la base de datos.
        /// </summary>
        private DbSIEPISContext db = new DbSIEPISContext();

        /// <summary>
        /// Atributos que permite controlar la paginación de las vistas.
        /// </summary>

        //
        // GET: /Study/

        /// <summary>
        /// Método que carga la vista que contiene todos los estudios registrados en el sistema de un usuario en particular
        /// </summary>
        /// <param name="Id">Id del usuario</param>
        /// <param name="page">Paginación</param>
        /// <param name="wizard">paso del wizard</param>
        /// <returns>Vista que contine los datos de los estudios de un usuario</returns>

        #region Index(id page)
        public ActionResult Index(int wizardStep = 0)
        {
            //UserController u = new UserController();
            ViewBag.WizardStep = wizardStep;
            User user = db.Users.Find(searchId());

            if (user != null)
            {
                ViewBag.UserId = user.Id;
                var aspnetuser = db.aspnet_Users.Find(user.Id);
                var roles = Roles.GetRolesForUser(aspnetuser.UserName).ToList();
                if (roles.Contains("Administrador"))
                {
                    var lstudies = db.Studies;
                    return View(lstudies.ToList());
                }
                else
                {
                    ViewBag.UserId = user.Id;
                    var studies =
                        db.Studies.Include(s => s.School)
                          .Include(s => s.User)
                          .Include(s => s.Thesis)
                          .Where(s => s.IdUser == user.Id);
                    return View(studies.ToList());
                }

            }
            //return RedirectToAction("Begin", "User", new RouteValueDictionary(new { controller = "User", action = "Begin", Id = user.Id }));
            return RedirectToAction("Index", "Home");
        }


        /// <summary>
        /// Método que busca el usuario que esta en la secion actual
        /// </summary>
        /// <returns>El identificador del usuario de tipo Guid</returns>
        public Guid searchId()
        {
            Guid g = System.Guid.Empty;
            foreach (var e in db.aspnet_Users)
            {
                if (e.UserName == HttpContext.User.Identity.Name)
                {
                    g = e.UserId;
                }
            }
            return g;
        }

        #endregion

        //
        // GET: /Study/Details/5
        /// <summary>
        /// Método que carga la vista que contine los datos del estudio de un usuario
        /// </summary>
        /// <param name="id">Id del estudio</param>
        /// <param name="idUser">Id del usuario</param>
        /// <param name="wizardStep">paso del wizard</param>
        /// <returns>Vista para consultar los datos de un estudio</returns>

        #region details(id)
        public ActionResult Details(Guid id, int wizardStep = 0)
        {
            User user = db.Users.Find(searchId());
            if (user != null)
            {
                var aspnetuser = db.aspnet_Users.Find(user.Id);
                var roles = Roles.GetRolesForUser(aspnetuser.UserName).ToList();
                Study study = db.Studies.Find(id);
                if (roles.Contains("Administrador") || study.IdUser == aspnetuser.UserId)
                {
                    ViewBag.WizardStep = wizardStep;
                    ViewBag.thewizard = wizardStep;
                    ViewBag.UserId = user.Id;
                    return View(study);
                }
                return RedirectToAction("Index", "Home");

            }
            return RedirectToAction("Index", "Home");
        }

        #endregion

        //
        // GET: /Study/Create
        /// <summary>
        /// Método que carga la vista con el formulario para crear la información del estudio de un usuario
        /// </summary>
        /// <param name="id">Id del usuario</param>
        /// <returns>Vista que despliega el formulario que permite crear los datos</returns>

        #region Create(id)
        public ActionResult Create(int wizardStep = 0)
        {
            User user = db.Users.Find(searchId());
            if (user != null)
            {
                ViewBag.WizardStep = wizardStep;
                ViewBag.thewizard = wizardStep;
                ViewBag.Seleccionado1 = false;
                ViewBag.Seleccionado11 = "";
                ViewBag.Seleccionado2 = false;
                ViewBag.Seleccionado22 = "";
                ViewBag.Seleccionado3 = false;
                ViewBag.Seleccionado33 = "";
                ViewBag.Seleccionado4 = false;
                ViewBag.Seleccionado44 = "";
                ViewBag.Electiva1 = "";
                ViewBag.Electiva2 = "";
                ViewBag.Electiva3 = "";
                ViewBag.Electiva4 = "";
                ViewBag.Electiva5 = "";
                ViewBag.Thesis = "";
                ViewBag.Descripcion = "";
                ViewBag.UserId = user.Id;
                ViewBag.IdSchool = new SelectList(db.Schools, "Id", "Name");
                ViewBag.IdUser = new SelectList(db.Users, "Id", "Name");
                ViewBag.Id = new SelectList(db.Theses, "IdStudies", "Title");
                return View(user);
            }
            return RedirectToAction("Index", "Home");
        }

        #endregion

        //
        // POST: /Study/Create
        /// <summary>
        /// Guarda los datos del estudio recibidos en el formulario, ademas de los datos adicionales en las demas tablas asociadas al estudio
        /// </summary>
        /// <param name="form">formulario con toda la información</param>
        /// <param name="id">Id del usuario</param>
        /// <returns></returns>
        [HttpPost]

        #region create(form, id)

        public ActionResult Create(FormCollection form)
        {
            TempData["Error1"] = "El campo Institucion es obligatorio";
            TempData["Error2"] = "El campo Titulo es obligatorio";
            TempData["Error3"] = "El campo Fecha Inicio es obligatorio";
            TempData["Error4"] = "El campo Fecha Fin es obligatorio";

            User user = db.Users.Find(searchId());
            if (user != null)
            {
                ViewBag.UserId = user.Id;
                int wizard = 0;
                var IdUser = user.Id;
                Study study = new Study();
                Thesis tesis = new Thesis();
                study.IdUser = IdUser;
                study.Id = Guid.NewGuid();
                var selected = new Dictionary<string, string>();
                bool continuar = false;
                bool continuar2 = false;
                bool continuar3 = false;
                bool continuar4 = false;
                DateTime fechaDesde = DateTime.Now;
                DateTime fechaHasta = DateTime.Now;

                foreach (String key in form)
                {
                    ViewBag.Electiva1 = form["txtElective1"];
                    ViewBag.Electiva2 = form["txtElective2"];
                    ViewBag.Electiva3 = form["txtElective3"];
                    ViewBag.Electiva4 = form["txtElective4"];
                    ViewBag.Electiva5 = form["txtElective5"];
                    ViewBag.Tesis = form["txtTesis"];
                    ViewBag.Descripcion = form["comentarios"];
                    var k = form[key];
                    if (continuar && continuar2 && continuar3 && continuar4)
                    {
                        break;
                    }

                    #region Validar campos vacios

                    switch (key)
                    {
                        case "txtSchool":

                            if (k.Length == 0)
                            {
                                ViewBag.Seleccionado1 = false;
                            }
                            else
                            {
                                ViewBag.Seleccionado1 = true;
                                ViewBag.Seleccionado11 = k;
                                continuar = true;
                            }
                            break;

                        case "Grade":
                            if (k.Length == 0)
                            {
                                ViewBag.Seleccionado2 = false;
                            }
                            else
                            {
                                ViewBag.Seleccionado2 = true;
                                ViewBag.Seleccionado22 = k;
                                continuar2 = true;
                            }
                            break;

                        case "txtStartDate":
                            if (k.Length == 0)
                            {
                                ViewBag.Seleccionado3 = false;
                            }
                            else
                            {
                                var fechatry = new DateTime();
                                if (!DateTime.TryParse(k, out fechatry))
                                {
                                    ViewBag.Seleccionado3 = false;
                                    TempData["Error3"] = "El formato de fecha no es correcto";
                                }
                                else
                                {
                                    ViewBag.Seleccionado3 = true;
                                    ViewBag.Seleccionado33 = k;
                                    fechaDesde = DateTime.Parse(k);
                                    continuar3 = true;
                                }
                            }
                            break;

                        case "txtEndDate":
                            if (k.Length == 0)
                            {
                                ViewBag.Seleccionado4 = false;
                            }
                            else
                            {
                                var fechatry = new DateTime();
                                if (!DateTime.TryParse(k, out fechatry))
                                {
                                    ViewBag.Seleccionado3 = false;
                                    TempData["Error4"] = "El formato de fecha no es correcto";
                                }
                                else
                                {
                                    ViewBag.Seleccionado4 = true;
                                    ViewBag.Seleccionado44 = k;
                                    fechaHasta = DateTime.Parse(k);
                                    continuar4 = true;
                                }
                            }
                            break;
                    }
                }

                #endregion

                if (continuar && continuar2 && continuar3 && continuar4)
                {
                    #region Validar fechas erroneas

                    if (fechaDesde >= DateTime.Now)
                    {
                        TempData["Error3"] = "¡La Fecha Inicio no pueden ser mayor al dia actual!";
                        ViewBag.Seleccionado3 = false;
                        continuar3 = false;
                    }
                    if (fechaHasta > DateTime.Now)
                    {
                        TempData["Error4"] = "¡La Fecha Fin no pueden ser mayor al dia actual!";
                        ViewBag.Seleccionado4 = false;
                        continuar4 = false;
                    }
                    if (fechaDesde >= fechaHasta)
                    {
                        TempData["Error4"] = "¡La Fecha Inicio no puede ser mayor a la fecha final!";
                        ViewBag.Seleccionado4 = false;
                        continuar3 = false;
                    }

                    #endregion

                    if (continuar3 && continuar4)
                    {
                        foreach (string key in form)
                        {
                            if (key.Contains("txtSchool"))
                            {
                                string var = form[key];
                                bool temp = false;
                                foreach (School school in db.Schools)
                                {
                                    if (school.Name == var)
                                    {
                                        study.IdSchool = school.Id;
                                        temp = true;
                                    }
                                }
                                if (!temp)
                                {
                                    School school = new School();
                                    school.Id = Guid.NewGuid();
                                    school.Name = var;
                                    db.Schools.Add(school);
                                    study.IdSchool = school.Id;
                                }
                            }
                            if (key.Contains("programas"))
                            {
                                study.Programs = form[key];
                            }
                            if (key.Contains("Grade"))
                            {
                                study.Grade = form[key];
                            }
                            if (key.Contains("Elective1") || key.Contains("Elective2") || key.Contains("Elective3") ||
                                key.Contains("Elective4") || key.Contains("Elective5"))
                            {
                                if (form[key].Length != 0)
                                {
                                    string var = form[key];
                                    bool temp = false;
                                    foreach (Elective elective in db.Electives)
                                    {
                                        if (elective.Name == var)
                                        {
                                            study.Electives.Add(elective);
                                            temp = true;
                                        }
                                    }
                                    if (!temp)
                                    {
                                        Elective elective = new Elective();
                                        elective.Id = Guid.NewGuid();
                                        elective.Name = var;
                                        db.Electives.Add(elective);
                                        study.Electives.Add(elective);
                                    }
                                }
                            }
                            if (key.Contains("txtStartDate"))
                            {
                                DateTime var = DateTime.Parse(form[key]);
                                study.StartDate = var;
                            }
                            if (key.Contains("txtEndDate"))
                            {
                                DateTime var = DateTime.Parse(form[key]);
                                study.EndDate = var;
                            }
                            if (key.Contains("txtTesis") && form[key].Length != 0)
                            {
                                tesis.IdStudies = study.Id;
                                tesis.Title = form[key];
                                study.Thesis = tesis;
                            }
                            if (key.Contains("comentarios") && study.Thesis != null)
                            {
                                if (form[key].Length == 0)
                                {
                                    study.Thesis = tesis;
                                }
                                else
                                {
                                    study.Thesis.Description = form[key];
                                    tesis.Description = form[key];
                                    study.Thesis = tesis;
                                }
                            }
                            if (key.Contains("wizard"))
                            {
                                if (form[key] == "1")
                                {
                                    wizard = 1;
                                }
                            }

                        }
                        db.Studies.Add(study);
                        db.SaveChanges();
                        TempData["Exito"] = "Estudio Creado con éxito";
                        return RedirectToAction("Index",new RouteValueDictionary(new{controller = "Study",action = "Index",user.Id,wizardStep = wizard}));
                    }
                }
            }
            return View();
        }

        #endregion

        //
        // GET: /Study/Edit/5

        /// <summary>
        /// Método que carga la vista con el formulario para editar la información de un estudio
        /// </summary>
        /// <param name="id">Id del estudio</param>
        /// <param name="wizardStep">paso del wizard</param>
        /// <returns>Vista que despliega el formulario con los datos para editarlos</returns>

        #region Edit(id)
        public ActionResult Edit(Guid id, int wizardStep = 0)
        {
            Study study2 = db.Studies.Find(id);
            var user = db.Users.Find(searchId());
            if (user != null)
            {
                ViewBag.contadorElectivas = study2.Electives.Count();
                ViewBag.Seleccionado1 = true;
                ViewBag.Seleccionado11 = study2.School.Name;
                ViewBag.Seleccionado2 = true;
                ViewBag.Seleccionado22 = study2.Grade;
                ViewBag.Seleccionado3 = true;
                ViewBag.Seleccionado33 = study2.StartDate.ToString();
                ViewBag.Seleccionado4 = true;
                ViewBag.Seleccionado44 = study2.EndDate.ToString();
                
                var aspnetuser = db.aspnet_Users.Find(user.Id);
                var roles = Roles.GetRolesForUser(aspnetuser.UserName).ToList();

                if (roles.Contains("Administrador") || study2.IdUser == aspnetuser.UserId)
                {
                    ViewBag.WizardStep = wizardStep;
                    ViewBag.thewizard = wizardStep;

                    var studio = db.Studies.Find(id);
                    ViewBag.UserId = studio.IdUser;
                    Study study = db.Studies.Find(id);
                    ViewBag.IdSchool = new SelectList(db.Schools, "Id", "Name", study.IdSchool);
                    ViewBag.IdUser = new SelectList(db.Users, "Id", "PhoneNumber", study.IdUser);
                    ViewBag.Id = new SelectList(db.Theses, "IdStudies", "Title", study.Id);
                    var electives = study.Electives;
                    for (int i = electives.Count; i < 5; i++)
                    {
                        electives.Add(new Elective());
                    }

                    return View(study);
                }
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        #endregion

        //
        // POST: /Study/Edit/5
        /// <summary>
        /// Guarda los cambios de la información del estudio recibido en el formulario, asi como de las tablas relacionadas
        /// </summary>
        /// <param name="id">Id del estudio</param>
        /// <param name="form">formulario con la información digitada</param>
        /// <param name="study">Estudio modificado</param>
        /// <returns></returns>
        [HttpPost]

        #region Edit(id, study, form)

        public ActionResult Edit(Guid id, Study study, FormCollection form)
        {
            TempData["Error1"] = "El campo Institucion es obligatorio";
            TempData["Error2"] = "El campo Titulo es obligatorio";
            TempData["Error3"] = "El campo Fecha Inicio es obligatorio";
            TempData["Error4"] = "El campo Fecha Fin es obligatorio";
            var last = db.Studies.Find(id);
            var listaElectiva = new List<String>();
            bool continuar = false;
            bool continuar2 = false;
            bool continuar3 = false;
            bool continuar4 = false;
            DateTime fechaDesde = DateTime.Now;
            DateTime fechaHasta = DateTime.Now;
            foreach (String key in form)
            {
                var k = form[key];
                if (continuar && continuar2 && continuar3 && continuar4)
                {
                    break;
                }

                #region Validar campos vacios

                switch (key)
                {
                    case "txtSchool":

                        if (k.Length == 0)
                        {
                            ViewBag.Seleccionado1 = false;
                        }
                        else
                        {
                            ViewBag.Seleccionado1 = true;
                            ViewBag.Seleccionado11 = k;
                            continuar = true;
                        }
                        break;

                    case "Grade":
                        if (k.Length == 0)
                        {
                            ViewBag.Seleccionado2 = false;
                        }
                        else
                        {
                            ViewBag.Seleccionado2 = true;
                            ViewBag.Seleccionado22 = k;
                            continuar2 = true;
                        }
                        break;

                    case "StartDate":
                        if (k.Length == 0)
                        {
                            ViewBag.Seleccionado3 = false;
                        }
                        else
                        {
                            var fechatry = new DateTime();
                            if (!DateTime.TryParse(k, out fechatry))
                            {
                                ViewBag.Seleccionado3 = false;
                                TempData["Error3"] = "El formato de fecha no es correcto";
                            }
                            else
                            {
                                ViewBag.Seleccionado3 = true;
                                ViewBag.Seleccionado33 = k;
                                fechaDesde = DateTime.Parse(k);
                                continuar3 = true;
                            }
                        }
                        break;

                    case "EndDate":
                        if (k.Length == 0)
                        {
                            ViewBag.Seleccionado4 = false;
                        }
                        else
                        {
                            var fechatryfin = new DateTime();
                            if (!DateTime.TryParse(k, out fechatryfin))
                            {
                                ViewBag.Seleccionado4 = false;
                                TempData["Error4"] = "El formato de fecha no es correcto";
                            }
                            else
                            {
                                ViewBag.Seleccionado4 = true;
                                ViewBag.Seleccionado44 = k;
                                fechaHasta = DateTime.Parse(k);
                                continuar4 = true;
                            }
                        }
                        break;
                }

                #endregion
            }

            if (continuar && continuar2 && continuar3 && continuar4)
            {
                #region Validar fechas erroneas

                if (fechaDesde >= DateTime.Now)
                {
                    TempData["Error3"] = "¡La Fecha Inicio no pueden ser mayor al dia actual!";
                    ViewBag.Seleccionado3 = false;
                    continuar3 = false;
                }
                if (fechaHasta > DateTime.Now)
                {
                    TempData["Error4"] = "¡La Fecha Fin no pueden ser mayor al dia actual!";
                    ViewBag.Seleccionado4 = false;
                    continuar4 = false;
                }
                if (fechaDesde >= fechaHasta)
                {
                    TempData["Error4"] = "¡La Fecha Inicio no puede ser mayor a la fecha final!";
                    ViewBag.Seleccionado4 = false;
                    continuar3 = false;
                }

                #endregion

                if (continuar3 && continuar4)
                { 
                    User user = db.Users.Find(searchId());
                    if (user != null)
                    {
                        ViewBag.userId = user.Id;
                        int wizard = 0;
                        foreach (string key in form)
                        {
                            if (key.Contains("txtSchool"))
                            {
                                string var = form[key];
                                var s = form[key];
                                var flag = false;
                                foreach (var i in db.Schools)
                                {
                                    if (i.Name == var)
                                    {
                                        last.IdSchool = i.Id;
                                        flag = true;
                                    }
                                }
                                if (!flag)
                                {
                                    var newid = Guid.NewGuid();
                                    db.Schools.Add(new School {Id = newid, Name = s});
                                    db.SaveChanges();
                                    last.IdSchool = newid;
                                }
                            }
                            last.Grade = study.Grade;
                            if (key.Contains("programas"))
                            {
                                last.Programs = form[key];
                            }
                            if (key.Contains("wizard"))
                            {
                                if (form[key] == "1")
                                {
                                    wizard = 1;
                                }
                            }
                            if (key.Contains("Elective1") || key.Contains("Elective2") || key.Contains("Elective3") ||
                                key.Contains("Elective4") || key.Contains("Elective5"))
                            {
                                if (form[key].Length != 0)
                                {
                                    string var = form[key];
                                    bool temp = false;
                                    foreach (Elective elective in db.Electives)
                                    {
                                        if (elective.Name == var)
                                        {
                                            bool el2 = false;
                                            foreach (var i in last.Electives)
                                            {
                                                if (i.Id == elective.Id)
                                                {
                                                    el2 = true;
                                                }
                                            }
                                            if (el2 == false)
                                            {
                                                last.Electives.Add(elective);
                                            }
                                            temp = true;
                                        }
                                    }
                                    if (temp == false)
                                    {
                                        Elective elective = new Elective();
                                        elective.Id = Guid.NewGuid();
                                        elective.Name = var;
                                        db.Electives.Add(elective);
                                        last.Electives.Add(elective);
                                    }
                                }
                            }
                            last.StartDate = study.StartDate;
                            last.EndDate = study.EndDate;
                            if (key.Contains("Thesis.Title"))
                            {
                                Thesis tesis = new Thesis();
                                if (study.Thesis.Title != null)
                                {
                                    tesis.IdStudies = last.Id;
                                    tesis.Title = study.Thesis.Title;
                                    tesis.Description = study.Thesis.Description;
                                    if (last.Thesis != null)
                                    {
                                        db.Theses.Remove(last.Thesis);
                                    }
                                    db.Theses.Add(tesis);
                                    last.Thesis = tesis;
                                }
                                else
                                {
                                    if (last.Thesis != null)
                                    {
                                        db.Theses.Remove(last.Thesis);
                                        last.Thesis = null;
                                    }
                                }
                            }
                        }
                        if (ModelState.IsValid)
                        {
                            db.Entry(last).State = EntityState.Modified;
                            db.SaveChanges();

                        }
                        TempData["Exito"] = "Se ha editado el estudio correctamente";
                        return RedirectToAction("Index",new RouteValueDictionary(new{controller = "Study",action = "Index",Id = last.IdUser,wizardStep = wizard}));
                    }
                    return View();
                }
            }
            return View();
        }

    #endregion

        

        //
        // GET: /Study/Delete/5
        /// <summary>
        /// Método que carga la vista con el formulario para eliminar un estudio
        /// </summary>
        /// <param name="id">Id del estudio</param>
        /// <param name="wizardStep">paso del wizard</param>
        /// <returns>Vista que despliega el formulario con los datos para eliminar</returns>
        #region Delete(id, idUser)
        public ActionResult Delete(Guid id, int wizardStep = 0)
        {
            User user = db.Users.Find(searchId());
            var aspnetuser = db.aspnet_Users.Find(user.Id);
            var roles = Roles.GetRolesForUser(aspnetuser.UserName).ToList();
            Study study2 = db.Studies.Find(id);
            if (roles.Contains("Administrador") || study2.IdUser == aspnetuser.UserId)
            {
                ViewBag.thewizar = wizardStep;
                ViewBag.WizardStep = wizardStep;
                var studio = db.Studies.Find(id);
                ViewBag.IdUser = studio.IdUser;

                Study study = db.Studies.Find(id);
                return View(study);
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion

        //
        // POST: /Study/Delete/5
        /// <summary>
        /// Elimina el estudio solicitado por el usuario
        /// </summary>
        /// <param name="id">Id del estudio</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        #region Delete(id, idUser)
        public ActionResult DeleteConfirmed(Guid id)
        {
            User user = db.Users.Find(searchId());
            var aspnetuser = db.aspnet_Users.Find(user.Id);
            var roles = Roles.GetRolesForUser(aspnetuser.UserName).ToList();
            Study study2 = db.Studies.Find(id);
            if (roles.Contains("Administrador") || study2.IdUser == aspnetuser.UserId)
            {
                var studio = db.Studies.Find(id);
                ViewBag.UserId = studio.IdUser;

                Study study = db.Studies.Find(id);
                db.Studies.Remove(study);
                db.SaveChanges();
                TempData["Exito"] = "Se Elimino el estudio correctamente";
                return RedirectToAction("Index",new RouteValueDictionary(new {controller = "Study", action = "Index", Id = studio.IdUser}));
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion

        /// <summary>
        /// Permite hacer autocompletar en las vistas, consultando en la tabla de schools de la base de datos
        /// </summary>
        /// <param name="term">Termino digitado</param>
        /// <returns>resultados de la consulta</returns>
        #region AutocompleteSchool(term)
        public ActionResult AutocompleteSchool(string term)
        {
            var items = (from u in db.Schools select u.Name).ToArray();
            var filteredItems = items.Where(
                item => item.StartsWith(term, StringComparison.InvariantCultureIgnoreCase));
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /// <summary>
        /// Permite hacer autocompletar en las vistas, consultando en la tabla de Electivas de la base de datos
        /// </summary>
        /// <param name="term">Termino digitado</param>
        /// <returns>resultados de la consulta</returns>
        #region AutocompleteElective(term)
        public ActionResult AutocompleteElective(string term)
        {
            var items = (from u in db.Electives select u.Name).ToArray();
            var filteredItems = items.Where(
                item => item.StartsWith(term, StringComparison.InvariantCultureIgnoreCase));
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /// <summary>
        /// Permite hacer autocompletar en las vistas, consultando en la tabla de Tesis de la base de datos
        /// </summary>
        /// <param name="term">Termino digitado</param>
        /// <returns>resultados de la consulta</returns>
        #region AutocompleteThesis(term)
        public ActionResult AutocompleteThesis(string term)
        {
            var items = (from u in db.Theses select u.Title).ToArray();
            var filteredItems = items.Where(
                item => item.StartsWith(term, StringComparison.InvariantCultureIgnoreCase));
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
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