using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SIFIET.Models;

namespace SIFIET.Controllers
{
    /// <summary>
    /// Permite realizar un Paso a Paso, al iniciar sesion el usuario 
    /// </summary>
    public class WizardController : Controller
    {
        private DbSIEPISContext db = new DbSIEPISContext();

        //
        // GET: /Wizard/

        /// <summary>
        /// Determina la vista que se mostrara en el Wizard, la primera vez que se inicia muestra la vista 0
        /// </summary>
        /// <param name="ActualStep">Recibe el numero de pagina del Wizard</param>
        /// <returns>Retorna Vista, que en su interior contiene una de las vista seleccionadas</returns>
        
        public ActionResult Index(int ActualStep = 0)
        {
            @ViewBag.ActualStep = ActualStep;

            

            /*
            switch (ActualStep)
            {
                case 0:
                    @ViewBag.ActualRoute = @"/Topic/Index?wizardStep=1";
                    break;
                case 1:
                    @ViewBag.BackRoute = @ViewBag.ActualRoute;
                    @ViewBag.ActualRoute = @"/Topic/Index?wizardStep=1";
                    break;
                case 2:
                    @ViewBag.ActualRoute = @"/Surveys/Index?wizardStep=1";
                    break;
                case 3:
                    @ViewBag.ActualRoute = @"/FillSurveys/Fill?wizardStep=1";
                    break;

            }*/
            var steps = (List<UsersStep>)Session["steps"];
            if (steps != null) {
                Session["Wizard"] = "1";
                var step = steps.ElementAt(ActualStep).Step;
                ViewBag.ActualRoute = step.SPath;
                ViewBag.StepsCount = steps.Count();
                ViewBag.StepId = step.Id;
                return View();
            }
            //En Caso de Error
            return RedirectToAction("Index", "Home");


        }
        
        /// <summary>
        /// Responde a la opcion del Usuario de Omitir un paso en el Wizard, si es el ultimo paso lo salta y finaliza el wizard
        /// </summary>
        /// <param name="step">Indica el numero del paso en el que se encuentra</param>
        /// <param name="end">Valor que sirve para determinar que hacer si es el ultimo paso</param>
        /// <returns></returns>
        
        public ActionResult Skip(int step, int end = 0)
        {
            //Add BD record
            /*var obj = GetActualUserStep((Guid)TempData["uStep"]);
            db.UsersSteps.Add(obj);
            db.SaveChanges();*/
            if (end != 0)
            {
                return RedirectToAction("End");
            }
            else
            {
                return RedirectToAction("Index", "Wizard", new { ActualStep = step });
            }
            
        }

        /// <summary>
        /// Cuando el Usuario Selecciona siguiente, se asume que ya no desea ver este paso o que finalizo correctamente
        /// </summary>
        /// <param name="step">Paso en el que se encuentra</param>
        /// <param name="end">Determina que hacer si esta en el ultimo paso</param>
        /// <returns></returns>
        
        public ActionResult Next(int step, int end = 0)
        {
            //Remove BD record
            //var obj = db.UsersSteps.Find((Guid)Session["uStep"]);
            var sI = (Guid) TempData["uStep"];
            var uI = GetUserId();
            var obj = db.UsersSteps.Find(uI, sI);
            db.UsersSteps.Remove(obj);
            db.SaveChanges();
            if (end != 0)
            {
                return RedirectToAction("End");
            }
            else
            {
                return RedirectToAction("Index", "Wizard", new { ActualStep = step });
            }
            
        }

        /// <summary>
        /// Finaliza el Wizard, modifica la variable de Session["Wizard"] para que la vistas se muestren bien fuera del wizard
        /// </summary>
        /// <returns>Llama a la vista que le sigue</returns>

        public ActionResult End()
        {
            Session["Wizard"] = "0";

            return RedirectToAction("Begin", "User", new { id = db.aspnet_Users.First(u => u.UserName == HttpContext.User.Identity.Name).UserId });
        }

        /// <summary>
        /// Obtiene el GUID de un Usuario, especificamente el que se acaba de loguear
        /// </summary>
        /// <returns>GUID (Identificador) </returns>
        private Guid GetUserId()
        {
            var result = Guid.Empty;
            foreach (var e in db.aspnet_Users.Where(e => e.UserName == HttpContext.User.Identity.Name))
            {
                result = e.UserId;
            }
            return result;
        }



        /*
        private UsersStep CreateUserStep(Guid sId)
        {
            var obj = new UsersStep {IdSteps = sId, Ok = "f"};
            foreach (var e in db.aspnet_Users.Where(e => e.UserName == HttpContext.User.Identity.Name))
            {
                obj.UserId = e.UserId;
            }

            return obj;
        }
        */
    }
}
