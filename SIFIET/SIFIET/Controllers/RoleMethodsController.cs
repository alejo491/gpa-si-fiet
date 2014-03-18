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
    /// Clase controlador que sirver para manejar permisos de los metodos a los roles.
    /// </summary>
    public class RoleMethodsController : Controller
    {
        //
        // GET: /RoleMethods/
        private DbSIEPISContext db = new DbSIEPISContext();

       
        /// <summary>
        /// Metodo que carga la vista para asignar permisos  de acceso a un rol
        /// </summary>
        /// <param name="id">Id del rol al cual se le van a asignar los permisos</param>
        /// <returns>v¿Vista para asignar permisos  de acceso a un rol</returns>
        public ActionResult AssignRolesMethods(Guid id)
        {
            var aspnet_Role = db.aspnet_Roles.Find(id);
            if (aspnet_Role != null)
            {
                var Method = db.Methods.OrderBy(s=>s.IdController);
                var model = new RoleMethodsViewModel(aspnet_Role, Method);
                ViewBag.ControllerList = db.SecureControllers.ToList();
                return View(model);
            }
            else
            {
                
                return RedirectToAction("Index", "Aspnet_Roles");
            }

        }

       /// <summary>
       /// Metodo POST del formulario
       /// </summary>
        /// <param name="id">Id del rol al cual se le van a asignar los permisos</param>
       /// <param name="postedForm">Contiene los datos que vienen del formulario</param>
       /// <returns>Redieccion al listado de Roles</returns>
        [HttpPost]
        public ActionResult AssignRolesMethods(Guid id, FormCollection postedForm)
        {
            List<Method> MethodToAdd = new List<Method>();
            aspnet_Roles aspnet_Role = db.aspnet_Roles.Find(id);
            foreach (var role in db.Methods)
            {
                if (postedForm[role.FullName].ToString().Contains("true"))
                {
                    MethodToAdd.Add(role);
                }
            }
            ReassignMethod(aspnet_Role, MethodToAdd);
            db.SaveChanges();
            TempData["Success"] = "Se ha han cambiado los permisos correctamente";
            return RedirectToAction("Index", "Aspnet_Roles");
        }

        
        /// <summary>
        /// Metodo que reasigna los permisos de acceso a un rol
        /// </summary>
        /// <param name="aspnet_Role">Rol que se va a asignar</param>
        /// <param name="MethodToAdd">Metodos a los cuales va a tener permiso.</param>
        public void ReassignMethod(aspnet_Roles aspnet_Role, List<Method> MethodToAdd)
        {           
            foreach (var cat in db.Methods)
            {
                RoleMethod v = db.RoleMethods.Find(aspnet_Role.RoleId, cat.Id);
                if (v != null)
                {
                    v.Method = null;
                    v.aspnet_Roles = null;
                    db.RoleMethods.Remove(v);                   
                }
                
            }
            db.SaveChanges();
            foreach (var category in MethodToAdd)
            {
                var auxUsersInrole = new RoleMethod(aspnet_Role.RoleId, category.Id);
                db.RoleMethods.Add(auxUsersInrole);
            }

        
            
        }
    }
}
