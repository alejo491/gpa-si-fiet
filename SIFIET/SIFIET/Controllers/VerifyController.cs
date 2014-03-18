using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIFIET.Models;
namespace SIFIET.Controllers
{
    /// <summary>
    /// Controlador que apoya al UserController verificando direccionamientos
    /// </summary>
    public class VerifyController : Controller
    {
        /// <summary>
        /// Atributo que consulta la base de datos.
        /// </summary>
        private DbSIEPISContext db = new DbSIEPISContext();

        //
        // GET: /Verify/
        /// <summary>
        /// Metodo que ayuda al direccionamiento a la hora de autenticarse en el sistema
        /// </summary>
        /// <returns>Redirecciona al perfil de usuario</returns>
        /// <returns>Redirecciona al index del wizard</returns>
        /// <returns>Redirecciona a crear usuario</returns>
        public ActionResult Index()
        {
            Guid g = System.Guid.Empty;
            foreach (var e in db.aspnet_Users)
            {
                if (e.UserName == HttpContext.User.Identity.Name)
                {
                    g = e.UserId;
                }
            }
            foreach (var e2 in db.Users)
            {
                if (e2.Id == g)
                {
                    var steps = db.UsersSteps.Where(s => s.UserId == e2.Id).OrderBy(s => s.Step.SOrder);

                    if (!steps.Any())
                    {
                        return RedirectToAction("Begin", "User", new { id = g });
                    }
                    else
                    {
                        var tmp = (List<UsersStep>)steps.ToList();
                        Session["steps"] = tmp;
                        var ActualStep = Convert.ToInt16(tmp.ElementAt(0).Step.SOrder);
                        return RedirectToAction("Index", "Wizard", ActualStep);
                    }
                }
            }
            return RedirectToAction("Create", "User");
        }

        /// <summary>
        /// Metodo que asigna el rol de Egresado al usuario que acaba de registrarse
        /// </summary>
        /// <returns>Redirecciona al index de este controlador</returns>
        public ActionResult AsignarRol()
        {
            Guid g = System.Guid.Empty;
            Guid role = System.Guid.Empty;
            foreach (var e in db.aspnet_Users)
            {
                if (e.UserName == HttpContext.User.Identity.Name)
                {
                    g = e.UserId;
                }
            }
            foreach (var e in db.aspnet_Roles)
            {
                if (e.RoleName == "Egresado")
                {
                    role = e.RoleId;
                }
            }

            if (role != System.Guid.Empty)
            {
                var userrole = new aspnet_UsersInRoles(g, role);
                db.aspnet_UsersInRoles.Add(userrole);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Metodo que ayuda al crear información personal del usuario
        /// </summary>
        /// <returns>Redirecciona a editar el usuario </returns>
        /// <returns>Redirecciona a crear un usuario </returns>
        public ActionResult Edit()
        {
            Guid g = System.Guid.Empty;
            foreach (var e in db.aspnet_Users)
            {
                if (e.UserName == HttpContext.User.Identity.Name)
                {
                    g = e.UserId;
                }
            }
            foreach (var e2 in db.Users)
            {
                if (e2.Id == g)
                {
                    return RedirectToAction("Edit", "User", new { id = g });
                }
            }
            return RedirectToAction("Create", "User");
        }

        /// <summary>
        /// Metodo que verifica si el usuario tiene el rol de administrador
        /// </summary>
        /// <param name="id">Id del usuario que ingresa al sistema</param>
        /// <returns>TRUE, si tiene el rol administrador, sino FALSE</returns>
        public Boolean Begin(Guid id)
        {
            User user = db.Users.Find(id);
            bool roluser = false;

            foreach (var ca in db.aspnet_Roles)
            {
                if (ca.LoweredRoleName == "administrador")
                {
                    aspnet_UsersInRoles v = db.aspnet_UsersInRoles.Find(user.Id, ca.RoleId);
                    if (v != null)
                    {
                        roluser = true;
                    }
                    else { roluser = false; }
                }
            }
            return roluser;
        }

        /// <summary>
        /// Metodo que ayuda al direccionamiento dependiendo del usuario
        /// </summary>
        /// <returns>Redirecciona a la vista principal del Usuario</returns>
        public ActionResult Out()
        {
            Guid g = System.Guid.Empty;
            foreach (var e in db.aspnet_Users)
            {
                if (e.UserName == HttpContext.User.Identity.Name)
                {
                    g = e.UserId;
                }
            }
            return RedirectToAction("Begin", "User", new { id = g });
        }

        /// <summary>
        /// Metodo que redirecciona a la vista begin deacuerdo al usuario del momento
        /// </summary>
        /// <returns>Redirecciona hacia el perfil de usuario</returns>
        /// <returns>Redirecciona al inicio</returns>
        public ActionResult Redirect()
        {
            Guid g = System.Guid.Empty;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                foreach (var e in db.aspnet_Users)
                {
                    if (e.UserName == HttpContext.User.Identity.Name)
                    {
                        g = e.UserId;
                    }
                }
                return RedirectToAction("Begin", "User", new { id = g });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
