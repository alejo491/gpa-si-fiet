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
    /// Clase controlador que permite gestión de roles.
    /// </summary>
    public class Aspnet_RolesController : Controller
    {
        private DbSIEPISContext db = new DbSIEPISContext();

        
        //
        // GET: /Aspnet_Roles/
        /// <summary>
        /// Muestra los Roles existentes y da la opcion de modificarles los permisos
        /// </summary>
        /// <returns> Muestra la vista de roles</returns>
        #region Listar Roles
        public ViewResult Index()
        {
            var aspnet_roles = db.aspnet_Roles.Include(a => a.aspnet_Applications);
            return View(aspnet_roles.ToList());
        }
        #endregion
        
        //
        // GET: /Aspnet_Roles/Details/5
        /// <summary>
        /// Muesta en detalle los Roles
        /// </summary>
        /// <param name="id">Identificador del Rol</param>
        /// <returns>Muestra la vista de roles del usuario</returns>
        #region Detalles
        public ViewResult Details(Guid id)
        {
            aspnet_Roles aspnet_roles = db.aspnet_Roles.Find(id);
            return View(aspnet_roles);
        }
        #endregion
       
        //
        // GET: /Aspnet_Roles/Create
        /// <summary>
        /// Opcion para crear u nuevo Rol
        /// </summary>
        /// <returns>Redirecciona a la vista del rol creado</returns>
        #region Crear Rol
        public ActionResult Create()
        {
            // ViewBag.ApplicationId = new SelectList(db.aspnet_Applications, "ApplicationId", "ApplicationName");
            return View();
        }
        #endregion
        
        //
        // POST: /Aspnet_Roles/Create
        /// <summary>
        /// Guarda un Rol recibido desde URL
        /// </summary>
        /// <param name="aspnet_roles">Recibe un Rol para ser guardado</param>
        /// <returns>Redirecciona al index</returns>
        /// <returns>Redirecciona a la vista de roles</returns>
        #region Crear Rol HTTPPost
        [HttpPost]
        public ActionResult Create(aspnet_Roles aspnet_roles)
        {
            if (ModelState.IsValid)
            {
                aspnet_roles.ApplicationId = db.aspnet_Applications.First().ApplicationId;
                aspnet_roles.RoleId = Guid.NewGuid();
                aspnet_roles.LoweredRoleName = "" + aspnet_roles.RoleName.ToLower();
                db.aspnet_Roles.Add(aspnet_roles);
                db.SaveChanges();
                TempData["Success"] = "Se ha creado el rol " + aspnet_roles.RoleName + " correctamente";
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationId = new SelectList(db.aspnet_Applications, "ApplicationId", "ApplicationName", aspnet_roles.ApplicationId);
            return View(aspnet_roles);
        }
        #endregion

        
        //
        // GET: /Aspnet_Roles/Edit/5
        /// <summary>
        /// Da la opcion para editar un Rol
        /// </summary>
        /// <param name="id">Identifiacador del Rol a Editar</param>
        /// <returns>Muestra la vista de los roles</returns>
        /// <returns>Redirecciona al inicio</returns>
        /// <returns>Redirecciona al inicio</returns>
        #region Editar Roles
        public ActionResult Edit(Guid id)
        {
            /* aspnet_Roles aspnet_roles = db.aspnet_Roles.Find(id);
             ViewBag.ApplicationId = new SelectList(db.aspnet_Applications, "ApplicationId", "ApplicationName", aspnet_roles.ApplicationId);
             return View(aspnet_roles);*/
            if (id != Guid.Empty && id != null)
            {
                aspnet_Roles aspnetroles = db.aspnet_Roles.Find(id);
                var achoice = db.aspnet_Roles.Find(id);
                if (achoice != null)
                {
                    return View(aspnetroles);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion

        
        //
        // POST: /Aspnet_Roles/Edit/5
        /// <summary>
        /// Guarda las modificaciones del Rol Que se recibe mediante un formulario
        /// </summary>
        /// <param name="aspnet_roles">Rol recibido par guardar los cambios</param>
        /// <returns>Redirecciona al inicio</returns>
        /// <returns>Muestra la vista de usuarios con el mensaje de rol cambiado</returns>
        #region Editar Roles HttpPost
        [HttpPost]
        public ActionResult Edit(aspnet_Roles aspnet_roles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspnet_roles).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "Se ha Actualizado el rol " + aspnet_roles.RoleName + " correctamente";
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationId = new SelectList(db.aspnet_Applications, "ApplicationId", "ApplicationName", aspnet_roles.ApplicationId);
            return View(aspnet_roles);
        }
        #endregion

       
        //
        // GET: /Aspnet_Roles/Delete/5
        /// <summary>
        /// Da la opcionm de eliminar un Rol
        /// </summary>
        /// <param name="id">Identificador del Rol a Eliminar</param>
        /// <returns>Redirecciona a la vista usuarios registrados</returns>
        #region Eliminar Roles
        public ActionResult Delete(Guid id)
        {
            aspnet_Roles aspnet_roles = db.aspnet_Roles.Find(id);
            return View(aspnet_roles);
        }
        #endregion

        
        //
        // POST: /Aspnet_Roles/Delete/5
        /// <summary>
        /// Elimina El rol que corresponda al identificador
        /// </summary>
        /// <param name="id">Identificador del Rol a Eliminar</param>
        /// <returns>Redirecciona al inicio</returns>
        #region Eliminar Roles HttpPost
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            aspnet_Roles aspnet_roles = db.aspnet_Roles.Find(id);
            db.aspnet_Roles.Remove(aspnet_roles);
            db.SaveChanges();
            TempData["Success"] = "Se ha Eliminado el rol " + aspnet_roles.RoleName + " correctamente";
            return RedirectToAction("Index");
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