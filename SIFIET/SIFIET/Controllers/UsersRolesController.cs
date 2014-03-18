using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SIFIET.Models;
using SIFIET.Models.ViewModels;

namespace SIFIET.Controllers
{
    /// <summary>
    /// Controlador para asignar roles a un usuario
    /// </summary>
    public class UsersRolesController : Controller
    {
        //
        // GET: /UsersRoles/
        private DbSIEPISContext db = new DbSIEPISContext();

        /// <summary>
        /// Metodo que carga la vista para asignar un rol a un usuario
        /// </summary>
        /// <param name="id">Id del usuario al que se le van a asignar los roles</param>
        /// <returns>Vista para agregar los roles al usuario</returns>
        #region Asignar Roles
        public ActionResult AssignUserRoles(Guid id)
        {
            var aspnet_User = db.aspnet_Users.Find(id);
            if (aspnet_User != null)
            {
                var aspnet_Roles = db.aspnet_Roles;
                var model = new UsersInRolesViewModel(aspnet_User, aspnet_Roles);
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "User");
            }

        }
        #endregion

        /// <summary>
        /// Metodo POST del formulario
        /// </summary>
        /// <param name="id">Id del usuario al que se le van a asignar los roles</param>
        /// <param name="postedForm">Contiene todos los datos del formulario</param>
        /// <returns>Redirecciona al listado de usuarios</returns>
        #region Asignar Roles (Guardar cambios)
        [HttpPost]
        public ActionResult AssignUserRoles(Guid id, FormCollection postedForm)
        {
            List<aspnet_Roles> aspnet_RolesToAdd = new List<aspnet_Roles>();
            aspnet_Users aspnet_User = db.aspnet_Users.Find(id);
            foreach (var role in db.aspnet_Roles)
            {
                if (postedForm[role.RoleName].ToString().Contains("true"))
                {
                    aspnet_RolesToAdd.Add(role);
                }
            }
            Reassignaspnet_Roles(aspnet_User, aspnet_RolesToAdd);
            db.SaveChanges();
            TempData["Success"] = "Se han asignado los roles al usuario correctamente";
            return RedirectToAction("Index", "User");
        }
        #endregion

        /// <summary>
        /// Metodo que reasigna los roles de un usuario especifico
        /// </summary>
        /// <param name="aspnet_User">Usuario al que se le van a cambiar los roles</param>
        /// <param name="aspnet_RolesToAdd">Roles que se le van a asignar al usuario</param>
        #region Reasignar Roles
        public void Reassignaspnet_Roles(aspnet_Users aspnet_User, List<aspnet_Roles> aspnet_RolesToAdd)
        {           
            foreach (var cat in db.aspnet_Roles)
            {
                aspnet_UsersInRoles v = db.aspnet_UsersInRoles.Find(aspnet_User.UserId, cat.RoleId);
                if (v != null)
                {
                    v.aspnet_Roles = null;
                    v.aspnet_Users = null;
                    db.aspnet_UsersInRoles.Remove(v);                   
                }
                
            }
            db.SaveChanges();
            foreach (var category in aspnet_RolesToAdd)
            {
                var auxUsersInrole = new aspnet_UsersInRoles(aspnet_User.UserId, category.RoleId);
                db.aspnet_UsersInRoles.Add(auxUsersInrole);
            }



        }
        #endregion
    }
}
