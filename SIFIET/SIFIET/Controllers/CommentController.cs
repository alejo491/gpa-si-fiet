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
    /// Controlador para la gestión de los comentarios de las noticias
    /// </summary
    public class CommentController : Controller
    {
        /// <summary>
        /// Atributo que consulta la base de datos
        /// </summary>
        private DbSIEPISContext db = new DbSIEPISContext();

        #region Comentarios de las noticias
        /// <summary>
        /// Muestra los comentarios por cada noticia que han publicado los usuarios
        /// </summary>
        /// <returns>Retorna los comentarios en el formulario</returns>
        public ViewResult Index(Guid id)
        {
            var comments = db.Comments.Where(c => c.IdPost.Equals(id)).Include(c => c.User);
            return View(comments.ToList().OrderByDescending(c => c.CommentDate));
        }
        #endregion

        #region Crear un Commentario
        /// <summary>
        /// Permite registrar un comentario a una noticia
        /// </summary>
        /// <param name="post">commentario que se recibe desde un formulario</param>
        /// <returns>Retorna la vista para crear el comentario de la noticia</returns>
        public ActionResult Create(Guid id, string text)
        {
            Comment comment = new Comment();
            comment.IdPost = id;
            comment.IdUser = (Guid)Membership.GetUser().ProviderUserKey;
            comment.Content = text;
            comment.CommentDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                comment.Id = Guid.NewGuid();
                db.Comments.Add(comment);
                db.SaveChanges();
            }
            return null;
        }
        #endregion
    }
}