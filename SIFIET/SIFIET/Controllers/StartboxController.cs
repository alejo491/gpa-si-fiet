using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIFIET.Models;
using System.Web.Security;

namespace SIFIET.Controllers
{
    /// <summary>
    /// Controlador para la gestión de calificación
    /// </summary>
    public class StartboxController : Controller
    {
        /// <summary>
        /// Atributo que consulta la base de datos
        /// </summary>
        private DbSIEPISContext db = new DbSIEPISContext();
             

        #region Calificación de las noticias
        /// <summary>
        /// Muestra la calificación por cada noticia que han publicado los usuarios
        /// </summary>
        /// <param name="id">Identificador de la calificación</param>
        /// <returns>Retorna la calificación en el formulario</returns>
        public int Index(Guid id)
        {
            var startboxs = db.Startboxs.SqlQuery("exec dbo.get_promedio_post '" + id + "'");
            IList<Startbox> list = (IList<Startbox>)startboxs.ToList();
            int suma = 0;
            foreach (Startbox l in list)
                suma = suma+l.Qualification;
            if (list.Count == 0) return 0;
            else return (suma / list.Count);
        }
        #endregion

        #region myQualification
        /// <summary>
        /// Obtiene la calificación de un usuario en una noticia
        /// </summary>
        /// <param name="idpost">Identificador de la noticia</param>
        /// <param name="iduser">Identificador del usuario</param>
        /// <returns>Retorna el número de calificación que ha hecho un usuario a una noticia</returns>
        public Startbox myQualification(Guid idpost, Guid iduser)
        {
            var startboxs = db.Startboxs.Where(s => s.Id_Post.Equals(idpost) && s.Id_User.Equals(iduser));
            if (startboxs != null)
            {
                IList<Startbox> list = (IList<Startbox>)startboxs.ToList();
                if(list.Count()>0)
                    return list.First();
            }
            return null; 
        }
        #endregion

        #region Crear calificación
        /// <summary>
        /// Permite hacer dar una calificación a una determinada noticia
        /// </summary>
        /// <returns>Retorna la vista para crear la calificación de la noticia</returns>
        [HttpPost]
        public Guid Create(Guid idPost, int q)
        {
            Startbox star = new Startbox();
            star.Id_Post = idPost;
            star.Id_User = (Guid)Membership.GetUser().ProviderUserKey;
            star.Qualification = q;
            if (ModelState.IsValid)
            {
                star.Id = Guid.NewGuid();
                db.Startboxs.Add(star);
                db.SaveChanges();
            }
            return star.Id;            
        }
        #endregion

        #region Editar calificación HttpPost
        /// <summary>
        /// Guarda las modificaciones hechas en una calificación
        /// </summary>
        /// <param name="startbox">calificación que se modificó y que se va a actualizar en el formulario</param>
        /// <returns>Retorna la calificación que se modificó</returns>
        [HttpPost]
        public void Edit(Guid idstar, int q)
        {
            Startbox startbox = db.Startboxs.Find(idstar);
            startbox.Qualification = q;
            if (ModelState.IsValid)
            {
                db.Entry(startbox).State = EntityState.Modified;
                db.SaveChanges();               
            }            
        }
        #endregion

        #region Calificación
        /// <summary>
        /// Obtiene la calificación promedio de una noticia
        /// </summary>
        /// <param name="id">Identificador de la noticia</param>
        /// <returns>Retorna la calificación promedio que tiene una noticia</returns>
        public ActionResult Calificacion(Guid id)
        {
            var startboxs = db.Startboxs.SqlQuery("exec dbo.get_promedio_post '" + id + "'");
            IList<Startbox> list = (IList<Startbox>)startboxs.ToList();
            int suma = 0;
            foreach (Startbox l in list)
                suma = suma + l.Qualification;
            if (list.Count == 0) return View(suma);
            else return View(suma / list.Count);
        }
        #endregion

        #region Método dispose
        /// <summary>
        ///Dispose
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        #endregion

        
    }
}