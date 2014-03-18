using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIFIET.Models;
using System.Web.Security;
using System.IO;
using PagedList;

namespace SIFIET.Controllers
{
    /// <summary>
    /// Controlador para la gestión de noticias
    /// </summary>
    public class PostController : Controller
    {
        /// <summary>
        /// Atributo que consulta la base de datos
        /// </summary>
        private DbSIEPISContext db = new DbSIEPISContext();

        /// <summary>
        /// Atributos que permiten controlar la paginación de las vistas
        /// </summary>
        private int pageSize = 10;
        private int pageNumber;

        #region ListarNoticiasGlobal
        /// <summary>
        /// Muestra las noticias autorizadas que han publicado los usuarios
        /// </summary>
        /// <param name="page">Elemento de control para la paginación</param>
        /// <returns>Retorna las noticias en el formulario</returns>
        public ViewResult GlobalIndex(int? page)
        {
            List<string> criteria = new List<string>() { "Mostrar Todas", "Mas Likes", "Mas Votadas" };
            ViewBag.Filtros = new SelectList(criteria);
            var posts = db.Posts.Where(i => i.Autorized == 1);
            pageNumber = (page ?? 1);
            return View(posts.ToList().OrderByDescending(c => c.PublicationDate).ToPagedList(pageNumber, pageSize));
        }
        #endregion

        #region ListarNoticiasAdministrador
        /// <summary>
        /// Muestra todas las noticias que han publicado los usuarios
        /// limpiando antes de mostrar las noticias que no se han creado correctamente.
        /// </summary>
        /// <param name="page">Elemento de control para la paginación</param>
        /// <returns>Retorna las noticias en el formulario</returns>
        public ViewResult Index(int? page)
        {
            var posts = db.Posts.Include(p => p.User);
            pageNumber = (page ?? 1);
            var post_basura = db.Posts.SqlQuery("exec PostBasura"); 
            List<Post> list = post_basura.ToList();
            foreach (var x in list)
            {
                var filepost = db.FilesPosts.SqlQuery("exec relacionfilepost '" + x.Id + "'");
                List<FilesPost> listfp = filepost.ToList();
                foreach (var y in listfp)
                {
                    SIFIET.Models.File file = db.Files.Find(y.IdFile);
                    var filename = file.Name;
                    var filePath = Path.Combine(Server.MapPath("~/UploadFiles"), filename);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    db.FilesPosts.Remove(y);
                    db.SaveChanges();
                    db.Files.Remove(file);
                    db.SaveChanges();
                }
                db.Posts.Remove(x);
                db.SaveChanges();
            }
            return View(posts.ToList().OrderByDescending(c => c.PublicationDate).ToPagedList(pageNumber, pageSize));
        }
        #endregion

        #region ListarNoticiasUsuario
        /// <summary>
        /// Muestra todas las noticias que ha publicado el usuario autenticado
        /// </summary>
        /// <param name="page">Elemento de control para la paginación</param>
        /// <returns>Retorna las noticias en el formulario</returns>
        public ViewResult UserIndex(int? page)
        {
            List<string> criteria = new List<string>() { "Mostrar Todas", "Mas Likes", "Mas Votadas" };
            ViewBag.Filtros = new SelectList(criteria);
            Guid g = System.Guid.Empty;
            g = (Guid)Membership.GetUser().ProviderUserKey;
            var post_basura = db.Posts.SqlQuery("exec PostBasura");
            List<Post> list = post_basura.ToList();
            foreach (var x in list)
            {
                var filepost = db.FilesPosts.SqlQuery("exec relacionfilepost '" + x.Id + "'");
                List<FilesPost> listfp = filepost.ToList();
                foreach (var y in listfp)
                {
                    SIFIET.Models.File file = db.Files.Find(y.IdFile);
                    var filename = file.Name;
                    var filePath = Path.Combine(Server.MapPath("~/UploadFiles"), filename);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    db.FilesPosts.Remove(y);
                    db.SaveChanges();
                    db.Files.Remove(file);
                    db.SaveChanges();
                }
                db.Posts.Remove(x);
                db.SaveChanges();
            }
            var posts = db.Posts.Where(p => (p.IdUser.Equals(g))).Include(p => p.User);
            pageNumber = (page ?? 1);
            return View(posts.ToList().OrderByDescending(c => c.PublicationDate).ToPagedList(pageNumber, pageSize));
        }
        #endregion

        #region Detalles Administrador
        /// <summary>
        /// Muestra en detalle una noticia para el usuario administrador 
        /// </summary>
        /// <param name="id">Identificador de la noticia</param>
        /// <returns>Retorna el contenido de la noticia para el id correspondiente</returns>
        public ViewResult DetailsAdmin(Guid id)
        {
            Post post = db.Posts.Find(id);
            IList<Post> datos = new List<Post>();
            datos.Add(post);
            return View(datos);
        }
        #endregion

        #region Detalles 
        /// <summary>
        /// Muestra en detalle una noticia para el usuario logueado
        /// </summary>
        /// <param name="id">Identificador de la noticia</param>
        /// <returns>Retorna el contenido de la noticia para el id correspondiente</returns>
        public ViewResult Details(Guid id)
        {
            Post post = db.Posts.Find(id);
            IList<Post> datos = new List<Post>();
            datos.Add(post);
            return View(datos);
        }
        #endregion

        #region Crear noticia
        /// <summary>
        /// Permite crear una nueva noticia para un usuario registrado
        /// </summary>
        /// <returns>Retorna la vista para crear la noticia</returns>
        public ActionResult Create()
        {
            Post post = new Post();
            post.IdUser = (Guid)Membership.GetUser().ProviderUserKey;
            User user = db.Users.Find(post.IdUser);
            if (user == null )
            {
                TempData["Error"] = "Primero debes Ingresar tus datos personales para Registrar Noticia";
                return RedirectToAction("Create", "User");
            }
            post.PublicationDate = DateTime.Now;
            post.Autorized = 0; 
            post.Main = 0;
            post.Estate = 0;
            post.Id = Guid.NewGuid();
            post.Title = "Remplace Por el Titulo de la Noticia";
            post.Abstract = "Remplace Por el Resumen de la Noticia";
            post.Content = "Remplace Por el Contenido de la Noticia";
            db.Posts.Add(post);
            db.SaveChanges();
            post.Title = "";
            post.Abstract = "";
            post.Content = "";

            return View(post);
        }
        #endregion

        #region Crear noticia HttpPost
        /// <summary>
        /// Guarda la noticia que se recibe en el formulario en la base de datos
        /// </summary>
        /// <param name="post">Noticia recibida desde un formulario</param>
        /// <returns>Retorna las noticias recibidas en el formulario</returns>
        [HttpPost]
        public ActionResult Create(Post post)
        {
            post.IdUser = (Guid)Membership.GetUser().ProviderUserKey;
            post.Autorized = 0;
            var list = Roles.GetRolesForUser();
            foreach (var i in list) {
                if (i == "Administrador" || i == "Publicador") { post.Autorized = 1; }
            }
            post.Main = 0;
            post.Estate = 1;
            post.PublicationDate = DateTime.Now;
            post.UpdateDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserIndex", "Post", new { id = post.IdUser });
            }
            //ViewBag.IdUser = new SelectList(db.Users, "Id", "LastNames", post.IdUser);
            return RedirectToAction("UserIndex", "Post", new { id = post.IdUser });
        }
        #endregion

        #region Crear Admin noticia
        /// <summary>
        /// Permite crear una nueva noticia al administrador
        /// </summary>
        /// <returns>Retorna la vista para crear la noticia</returns>
        public ActionResult CreateAdmin()
        {
            Post post = new Post();
            post.IdUser = (Guid)Membership.GetUser().ProviderUserKey;
            User user = db.Users.Find(post.IdUser);
            if (user == null)
            {
                TempData["Error"] = "Primero debes Ingresar tus datos personales para Registrar Noticia";
                return RedirectToAction("Create", "User");
            }
            post.PublicationDate = DateTime.Now;
            post.Autorized = 0; 
            post.Main = 0;
            post.Estate = 0;
            post.Id = Guid.NewGuid();
            post.Title = "Remplace Por el Titulo de la Noticia";
            post.Abstract = "Remplace Por el Resumen de la Noticia";
            post.Content = "Remplace Por el Contenido de la Noticia";
            db.Posts.Add(post);
            db.SaveChanges();
            post.Title = "";
            post.Abstract = "";
            post.Content = "";

            return View(post);
        }
        #endregion

        #region Crear noticia Administrador HttpPost
        /// <summary>
        /// Guarda la noticia que se recibe en el formulario en la base de datos
        /// </summary>
        /// <param name="post">Noticia recibida desde un formulario</param>
        /// <returns>Retorna las noticias recibidas en el formulario</returns>
        [HttpPost]
        public ActionResult CreateAdmin(Post post)
        {
            post.IdUser = (Guid)Membership.GetUser().ProviderUserKey;
            post.PublicationDate = DateTime.Now;
            post.Autorized = 0;
            var list = Roles.GetRolesForUser();
            foreach (var i in list)
            {
                if (i == "Administrador" || i == "Publicador") { post.Autorized = 1; }
            }
            post.Main = 0;
            post.Estate = 1;
            post.PublicationDate = DateTime.Now;
            post.UpdateDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Post", new { id = post.IdUser });
            }
            //ViewBag.IdUser = new SelectList(db.Users, "Id", "LastNames", post.IdUser);
            return RedirectToAction("Index", "Post", new { id = post.IdUser });
        }
        #endregion

        #region Editar noticia
        /// <summary>
        /// Da la opción de editar una noticia que ha sido guardada por un usuario
        /// </summary>
        /// <param name="id">Identificador de la noticia</param>
        /// <returns>Retorna la noticia a editar</returns>
        public ActionResult Edit(Guid id)
        {
            Post post = db.Posts.Find(id);
            return View(post);
        }
        #endregion

        #region Editar noticia HttpPost
        /// <summary>
        /// Guarda las modificaciones hechas en una noticia
        /// </summary>
        /// <param name="post">Noticia que se modificó y que se va a actualizar en el formulario</param>
        /// <returns>Retorna la noticia que se editó</returns>
        [HttpPost]
        public ActionResult Edit(Post post)
        {
            post.IdUser = (Guid)Membership.GetUser().ProviderUserKey;
            post.UpdateDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                var list = Roles.GetRolesForUser();
                foreach (var i in list)
                {
                    if (i == "Administrador" || i == "Publicador")
                    {
                        
                        db.Entry(post).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("UserIndex");
                    }
                }
                post.Autorized = 0;
                
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserIndex");
            }
            return View(post);
        }
        #endregion

        #region Editar noticia Administrador
        /// <summary>
        /// Da la opción a el administrador de editar una noticia que ha sido guardada en el sistema por cualquier usuario
        /// </summary>
        /// <param name="id">Identificador de la noticia</param>
        /// <returns>Retorna la noticia a editar</returns>
        public ActionResult EditAdmin(Guid id)
        {
            Post post = db.Posts.Find(id);
            return View(post);
        }
        #endregion

        #region EditarAdmin noticia HttpPost
        /// <summary>
        /// Guarda las modificaciones hechas por el administrador a una noticia
        /// </summary>
        /// <param name="post">Noticia que se modificó y que se va a actualizar en el formulario</param>
        /// <returns>Retorna la noticia que se editó</returns>
        [HttpPost]
        public ActionResult EditAdmin(Post post)
        {
            post.IdUser = (Guid)Membership.GetUser().ProviderUserKey;
            post.UpdateDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }
        #endregion

        #region ShowPosts
        /// <summary>
        /// Muestra las noticia publicadas y autorizadas en el carrucel de noticias de la pagina principal
        /// </summary>
        /// <returns>Retorna la vista con las noticias publicadas y autorizadas</returns>
        public ActionResult ShowPosts()
        {
            var posts = db.Posts.SqlQuery("exec dbo.Post_getPostMain");
            return View(posts);
        }
        #endregion

        #region ShowListPosts
        /// <summary>
        /// Muestra una lista de noticias que se han publicado pero no como principales
        /// </summary>
        /// <returns>Retorna la vista de noticias en forma de links</returns>
        public ActionResult ShowListPosts()
        {
            var posts = db.Posts.SqlQuery("exec dbo.Post_getPostList");
            return View(posts);
        }
        #endregion

        #region ShowPost
        /// <summary>
        /// Esta función se llama cuando se quiere mirar el contenido de una noticia
        /// </summary>
        /// <param name="id">Identificador del post</param>
        /// <returns>Retorna el post en la vista</returns>
        public ActionResult ShowPost(Guid id)
        {
            Post post = db.Posts.Find(id);
            Post post2 = new Post();
            Post post3 = new Post();
            IList<Post> datos = new List<Post>();
            datos.Add(post);
            SIFIET.Controllers.LikeController lc = new LikeController();
            SIFIET.Controllers.StartboxController st = new StartboxController();
            IList<Like> likes;
            int startbox = st.Index(id);            
            post2.Autorized = 0;
            Startbox mystartbox = new Startbox();
            if (Request.IsAuthenticated)
            {
                likes = lc.getMyLikePost(id, (Guid)Membership.GetUser().ProviderUserKey);
                mystartbox = st.myQualification(id, (Guid)Membership.GetUser().ProviderUserKey);
                if (mystartbox != null)
                {
                    post3.Main = mystartbox.Qualification;
                    post3.Id = mystartbox.Id;
                }
                else
                {
                    post3.Main = 0;
                }
                if (likes.Count() == 1)
                {
                    post2.Autorized = 1;
                    post2.Id = likes.First().Id;
                }
                Guid g = (Guid)Membership.GetUser().ProviderUserKey;
                User user2 = db.Users.Find(g);
                if (user2 == null)
                {
                    post2.Autorized = -2;
                }
            }
            else post2.Autorized = -1;
            post2.Estate = lc.get_likes(id);
            post3.Estate = startbox;
            datos.Add(post2);
            datos.Add(post3);
            return View(datos);
        }
        #endregion

        #region Search
        /// <summary>
        /// Esta función atiende el resultado de hacer clic en el boton Buscar de la vista de gestionar noticias del administrador
        /// </summary>
        /// <param name="criteria">Contiene las palabras clave con las que se desea hacer la busqueda</param>
        /// <param name="page">Elemento de control para la paginación</param>
        /// <returns>Retorna la vista con el listado de noticias encontradas para las palabras claves</returns>
        public ActionResult Search(string criteria, int? page)
        {
            
            ViewBag.CurrentFilter = criteria;
            if (criteria == null)
            {
                criteria = "";
            }
            string searchText = criteria.ToLower().Trim();
            //Búsqueda
            var posts = db.Posts.Where(p => p.Title.ToLower().Contains(criteria) || p.Abstract.Contains(criteria) ||
               p.Content.Contains(criteria));
            //Ordenar por fecha de publicación
            var results = posts.ToList().OrderByDescending(c => c.PublicationDate);
            pageNumber = (page ?? 1);
            return View(results.ToPagedList(pageNumber, pageSize));

        }
        #endregion

        #region GlobalSearch
        /// <summary>
        /// Esta función atiende el resultado de hacer clic en el boton Buscar de la vista de todas las noticias que puede mirar cualquier usuario
        /// </summary>
        /// <param name="criteria">Contiene las palabras clave con las que se desea hacer la busqueda</param>
        /// <param name="page">Elemento de control para la paginación</param>
        /// <returns>Retorna la vista con el listado de noticias encontradas para las palabras claves</returns>
        public ActionResult GlobalSearch(string criteria, int? page)
        {
            ViewBag.CurrentFilter = criteria;
            if (criteria == null)
            {
                criteria = "";
            }
            string searchText = criteria.ToLower().Trim();
            //Búsqueda
            var posts = db.Posts.Where(p => (p.Title.ToLower().Contains(criteria) || p.Abstract.Contains(criteria) ||
               p.Content.Contains(criteria)) 
               && (p.Autorized.Equals(1) && p.Estate.Equals(1)));
            //Ordenar por fecha de publicación
            var results = posts.ToList().OrderByDescending(c => c.PublicationDate);
            pageNumber = (page ?? 1);
            return View(results.ToPagedList(pageNumber, pageSize));

        }
        #endregion

        #region UserSearch
        /// <summary>
        /// Esta función atiende el resultado de hacer clic en el boton Buscar de la vista Principal del usuario
        /// </summary>
        /// <param name="criteria">Contiene las palabras clave con las que se desea hacer la busqueda</param>
        /// <param name="page">Elemento de control para la paginación</param>
        /// <returns>Retorna la vista con el listado de noticias encontradas para las palabras claves</returns>
        public ActionResult UserSearch(string criteria, int? page)
        {
            ViewBag.CurrentFilter = criteria;
            if (criteria == null)
            {
                criteria = "";
            }
            string searchText = criteria.ToLower().Trim();
            Guid g = System.Guid.Empty;
            foreach (var e in db.aspnet_Users)
            {
                if (e.UserName == HttpContext.User.Identity.Name)
                {
                    g = e.UserId;
                }
            }
            //Búsqueda
            var posts = db.Posts.Where(p => (p.Title.ToLower().Contains(criteria) || p.Abstract.Contains(criteria) ||
               p.Content.Contains(criteria)) && (p.IdUser.Equals(g)));
            //Ordenar por fecha de publicación
            var results = posts.ToList().OrderByDescending(c => c.PublicationDate);
            pageNumber = (page ?? 1);
            return View(results.ToPagedList(pageNumber, pageSize));

        }
        #endregion

        #region GlobalFilter
        /// <summary>
        /// Esta función atiende el resultado de hacer clic en el boton Filtrar de la vista Principal de Noticias
        /// </summary>
        /// <param name="Filtros">Contiene el criterio de filtrado seleccionado desde el fomulario</param>
        /// <param name="page">Elemento de control para la paginación</param>
        /// <returns>Retorna la vista con el listado de noticias filtradas bajo el criterio</returns>
        public ActionResult GlobalFilter(List<string> Filtros, int? page)
        {

            ViewBag.CurrentFilter = Filtros[0];
            if (Filtros[0].Equals("Mostrar Todas"))
            {
                return RedirectToAction("GlobalIndex", "Post");
            }

            if (Filtros[0].Equals("Mas Likes"))
            {
                var posts = db.Posts.SqlQuery("exec Noticias_MasLikes");
                if (posts != null)
                {
                    var results = posts.ToList().OrderByDescending(c => c.Main);
                    pageNumber = (page ?? 1);
                    return View(results.ToPagedList(pageNumber, pageSize));
                }
                return View();
            }
            if (Filtros[0].Equals("Mas Votadas"))
            {
                var posts = db.Posts.SqlQuery("exec Noticias_MasVotadas");
                if (posts != null)
                {
                    var results = posts.ToList().OrderByDescending(c => c.Main);
                    pageNumber = (page ?? 1);
                    return View(results.ToPagedList(pageNumber, pageSize));
                }
                return View();
            }
            return View();

        }
        #endregion

        #region UserFilter
        /// <summary>
        /// Esta función atiende el resultado de hacer clic en el boton Filtrar de la vista Principal de Noticias del usuario
        /// </summary>
        /// <param name="Filtros">Contiene el criterio de filtrado seleccionado desde el fomulario</param>
        /// <param name="page">Elemento de control para la paginación</param>
        /// <returns>Retorna la vista con el listado de noticias filtradas bajo el criterio y pertenecientes al usuario</returns>
        public ActionResult UserFilter(List<string> Filtros, int? page)
        {

            ViewBag.CurrentFilter = Filtros[0];
            Guid g = System.Guid.Empty;
            foreach (var e in db.aspnet_Users)
            {
                if (e.UserName == HttpContext.User.Identity.Name)
                {
                    g = e.UserId;
                }
            }

            if (Filtros[0].Equals("Mostrar Todas"))
            {
                return RedirectToAction("UserIndex", "Post");
            }

            if (Filtros[0].Equals("Mas Likes"))
            {
                var posts = db.Posts.SqlQuery("exec Noticias_MasLikes_User '" + g + "'");
                if (posts != null)
                {
                    var results = posts.ToList().OrderByDescending(c => c.Main);
                    pageNumber = (page ?? 1);
                    return View(results.ToPagedList(pageNumber, pageSize));
                }
                return View();
            }
            if (Filtros[0].Equals("Mas Votadas"))
            {
                var posts = db.Posts.SqlQuery("exec Noticias_MasVotadas_User '" + g + "'");
                if (posts != null)
                {
                    var results = posts.ToList().OrderByDescending(c => c.Main);
                    pageNumber = (page ?? 1);
                    return View(results.ToPagedList(pageNumber, pageSize));
                }
                return View();
            }
            return View();

        }
        #endregion

        #region Autorizar post
        /// <summary>
        /// Esta función se llama cuando se quiere autorizar un post que ha hecho un usuario
        /// </summary>
        /// <param name="id">Identificador del post</param>
        /// <param name="value">Valor que toma el post después de haberlo autorizado (0 o 1)</param>
        [HttpPost]
        public void AutorizarPost(Guid id, int value)
        {
            Post post = db.Posts.Find(id);
            post.Autorized = value;
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        #endregion

        #region PostPrincipal
        /// <summary>
        /// Esta función se llama cuando se quiere colocar un post como principal en el carrucel
        /// </summary>
        /// <param name="id">Identificador del post</param>
        /// <param name="value">Valor que toma el post después de colocarlo como principal (0 o 1)</param>
        [HttpPost]
        public void PostPrincipal(Guid id, int value)
        {
            Post post = db.Posts.Find(id);
            post.Main = value;
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        #endregion

        #region Cambiar estado post
        /// <summary>
        /// Esta función se llama cuando se quiere que la noticia se muestre o no en el carrucel de noticias
        /// </summary>
        /// <param name="id">Identificador del post</param>
        /// <param name="value">Valor que toma el post después de chulear el estado (0 o 1)</param>
        [HttpPost]
        public void CambiarEstadoPost(Guid id, int value)
        {
            Post post = db.Posts.Find(id);
            post.Estate = value;
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
            }
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