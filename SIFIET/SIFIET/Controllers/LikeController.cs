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
    /// Controlador para la gestión de los likes de las noticias
    /// </summary
    public class LikeController : Controller
    {
        /// <summary>
        /// Atributo que consulta la base de datos
        /// </summary>
        private DbSIEPISContext db = new DbSIEPISContext();

        #region Crear un like
        /// <summary>
        /// Permite hacer un like a una noticia
        /// </summary>
        /// <param name="post">Like que se recibe desde un formulario</param>
        /// <returns>Retorna la vista para crear el like de la noticia</returns>
        [HttpPost]
        public Guid Create(Guid post)
        {
            Like like = new Like();
            like.Id_Post = post;
            like.Id_User = (Guid)Membership.GetUser().ProviderUserKey;
            if (ModelState.IsValid)
            {
                like.Id = Guid.NewGuid();
                db.Likes.Add(like);
                db.SaveChanges();
            }
            return db.Likes.First().Id;
            
        }
        #endregion        

        #region Eliminar Like HttpPost
        /// <summary>
        /// Elimina el like que corresponde al id
        /// </summary>
        /// <param name="id">Identificador del like</param>
        /// <param name="post">Identificador de la noticia</param>
        /// <returns>Retorna el resultado del cambio del like</returns>
        //[HttpPost, ActionName("Delete")]
        public void DeleteConfirmed(Guid post, Guid id)
        {            
            Like like = db.Likes.Find(id);
            if (like != null)
            {
                db.Likes.Remove(like);
                db.SaveChanges();
            }
        }
        #endregion       

        #region get_likes
        /// <summary>
        /// Obtiene el nummero de likes que se han hecho para una noticia
        /// </summary>
        /// <param name="id">Identificador de determinada noticia</param>
        /// <returns>Retorna el numero de likes de la noticia con el id correspondiente</returns>
        public int get_likes(Guid id)
        {
            var likes = db.Likes.SqlQuery("exec dbo.get_like '" + id + "'");
            IList<Like> list = (IList<Like>)likes.ToList();
            return list.Count;
        }
        #endregion

        #region getMyLikePost
        /// <summary>
        /// Obtiene los likes hechos por un usuario
        /// </summary>
        /// <param name="idpost">Identificador del post</param>
        /// <param name="iduser">Identificador del usuario</param>
        /// <returns>Retorna el like que ha hecho el usuario en un post</returns>
        public IList<Like> getMyLikePost(Guid idpost, Guid iduser)
        {
            var likes = db.Likes.Where(l => l.Id_Post.Equals(idpost) && l.Id_User.Equals(iduser))
                .Include(l => l.Post).Include(l => l.User);
            return (IList<Like>)likes.ToList();
        }
        #endregion

        #region Num_likes
        /// <summary>
        /// Obtiene el numero de likes que se han hecho para una noticia y los pasa a una vista
        /// </summary>
        /// <param name="id">Identificador de determinada noticia</param>
        /// <returns>Retorna el numero de likes de la noticia con el id correspondiente</returns>
        public ActionResult Num_likes(Guid id)
        {
            var likes = db.Likes.SqlQuery("exec dbo.get_like '" + id + "'");
            IList<Like> list = (IList<Like>)likes.ToList();
            return View(list.Count);
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