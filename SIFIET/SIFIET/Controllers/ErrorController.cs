using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIFIET.Controllers
{
    /// <summary>
    /// Clase que maneja las vistas que se van a mostrar cuando ocurren errores
    /// </summary>
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        /// <summary>
        /// Metodo que llama a las vistas de errores.
        /// </summary>
        /// <param name="aspxerrorpath">Se recibe si la pagina a la que se esta accediento no existe</param>
        /// <returns>Retorna la vista con el mensaje de error.</returns>
        public ViewResult Index(string aspxerrorpath)
        {

            if (aspxerrorpath != null)
            {
                ViewBag.ErrorType = "La pagina " + aspxerrorpath + " no existe";
            }
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ViewResult ErrorDataUpdate()
        {
            return View();
        }

    }
}
