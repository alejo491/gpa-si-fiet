using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using SIFIET.Models;
using System.ComponentModel.DataAnnotations;

namespace SIFIET.Controllers
{ 
    /// <summary>
    /// Controlador para asignar temas a una encuesta.
    /// </summary>
    public class SurveysTopicsController : Controller
    {
        private DbSIEPISContext db = new DbSIEPISContext();


        /// <summary>
        /// Esta funcion se llama cuando se van a agregar temas a una encuesta, redirecciona al usuario a la pagina principal si el id no es valido
        /// </summary>
        /// <param name="id">Identificador de la Encuesta</param>
        /// <returns>El listado de temas asociados a la encuesta</returns>
        public ActionResult Index(Guid? id)
        {
            if (id != Guid.Empty && id != null)
            {
                var surveystopics =
                    db.SurveysTopics.Include(s => s.Survey).Include(s => s.Topic).Where(s => s.IdSurveys == id).OrderBy(s=> s.TopicNumber);

                Survey auxsurvey = (Survey)db.Surveys.Find(id);
                if (auxsurvey != null)
                {
                   
                    ViewBag.Encuesta = auxsurvey;
                    return View(surveystopics.ToList());
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

        /// <summary>
        /// Llama a la vista para asociar a una encuesta un tema especifico.
        /// </summary>
        /// <param name="id">Guid de la encuesta</param>
        /// <returns>Un ActionResult con la vista y formulario para asociar la encuesta</returns>
        public ActionResult Create(Guid? id)
        {
            var auxsurvey = (Survey) db.Surveys.Find(id);
            if (auxsurvey != null)
            {
                ViewBag.survey = auxsurvey;
                ViewBag.IdTopic = new SelectList(db.Topics, "Id", "Description");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        } 

        //
        // POST: /SurveysTopics/Create

        /// <summary>
        /// Se llama despues de que el usuario oprime el boton de Guardar, si la operacion no es correcta redireccionara al usuario a una pagina de error.
        /// </summary>
        /// <param name="id">Identificador de la encuesta</param>
        /// <param name="surveystopic">Objeto que se crea en el formulario</param>
        /// <returns>La redirección al listado de temas asociado a la encuesta </returns>
        [HttpPost]
        public ActionResult Create(Guid id, SurveysTopic surveystopic)
        {
            if (ModelState.IsValid && ((Survey) db.Surveys.Find(id) != null))
            {
                surveystopic.IdSurveys = id;
                var b = db.SurveysTopics.Find(surveystopic.IdSurveys, surveystopic.IdTopic);
                bool existeNumero = ExisteNumero(surveystopic.TopicNumber, id, surveystopic.IdTopic);
                if ( b == null && existeNumero )
                {
                    db.SurveysTopics.Add(surveystopic);
                    db.SaveChanges();
                    TempData["Success"] = "Se ha agregado el tema a la encuesta correctamente";
                    return RedirectToAction("Index", new {id = surveystopic.IdSurveys});
                }
                else
                {
                    if (b!= null)
                    {
                        TempData["Error"] = "Este tema ya esta asociado a esta encuesta";                                                
                    }
                    if (!existeNumero)
                    {
                        TempData["Error2"] = "Ya existe un tema con ese numero asignado para esta encuesta";
                    }
                    var auxsurvey = (Survey)db.Surveys.Find(id);
                    ViewBag.survey = auxsurvey;
                    ViewBag.IdTopic = new SelectList(db.Topics, "Id", "Description");
                    return View();
                }

            }

            var auxsurvey2 = (Survey)db.Surveys.Find(id);
            ViewBag.survey = auxsurvey2;
            ViewBag.IdTopic = new SelectList(db.Topics, "Id", "Description");
            return View();
            
        }
        


        /// <summary>
        /// Edita el numero de tema para una encuesta especifica, si el identificador no es correcto redirecciona al usuario la pagina principal
        /// </summary>
        /// <param name="ids">Identificador de la encuesta</param>
        /// <param name="idt">Identificador del tema</param>
        /// <returns>Retorna la vista con los datos del objeto</returns>
        public ActionResult Edit(Guid? ids, Guid? idt)
        {
            if (ids != Guid.Empty && ids != null && idt != Guid.Empty && idt != null)
            {
                SurveysTopic surveystopic = db.SurveysTopics.Find(ids, idt);
                var survey = db.Surveys.Find(ids);
                var topic = db.Topics.Find(idt);
                if (survey != null && topic != null)
                {
                    ViewBag.survey = survey;
                    
                    ViewBag.topic = topic;
                    return View(surveystopic);
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

        
        /// <summary>
        /// Se llama despues de que el usuario oprime guardar 
        /// </summary>
        /// <param name="ids">Identificador de la encuesta</param>
        /// <param name="idt">Identificador de los temas</param>
        /// <param name="surveystopic">Objeto que se modifica en la encuesta</param>
        /// <returns>La redireccion a la pagina principal despues de que el objeto se modifique correctmanete</returns>
        [HttpPost]
        public ActionResult Edit(Guid ids, Guid idt, SurveysTopic surveystopic)
        {
            
            if (ModelState.IsValid && ((Survey) db.Surveys.Find(ids) != null) && ((Topic) db.Topics.Find(idt) != null))
            {
                surveystopic.IdSurveys = ids;
                surveystopic.IdTopic = idt;
                db.Entry(surveystopic).State = EntityState.Modified;
                bool existeNumero = ExisteNumero(surveystopic.TopicNumber, ids, idt);
                if (!existeNumero)
                {
                    TempData["Error2"] = "Ya existe un tema con ese numero asignado para esta encuesta";
                    var auxsurvey = (Survey)db.Surveys.Find(ids);
                    ViewBag.survey = auxsurvey;
                    var topic = db.Topics.Find(idt);
                    ViewBag.topic = topic;
                    //ViewBag.IdTopic = new SelectList(db.Topics, "Id", "Description", surveystopic.IdTopic);
                    return View(surveystopic);

                }
                else
                {
                    db.SaveChanges();
                    TempData["Success"] = "Se ha editado el orden del tema en la encuesta correctamente";
                    return RedirectToAction("Index", new { id = ids });
                }
                
            }
            else
            {
                
                
                var auxsurvey = (Survey)db.Surveys.Find(ids);
                ViewBag.survey = auxsurvey;
                var topic = db.Topics.Find(idt);
                ViewBag.topic = topic;
                //ViewBag.IdTopic = new SelectList(db.Topics, "Id", "Description", surveystopic.IdTopic);
                return View(surveystopic);
            }
            //ViewBag.IdSurveys = new SelectList(db.Surveys, "Id", "Name", surveystopic.IdSurveys);
           
            //return View(surveystopic);
        }

        
        /// <summary>
        /// Elimina la asociacion entre un tema y una encuesta
        /// </summary>
        /// <param name="ids">Identificador de la encuesta</param>
        /// <param name="idt">Identificador del tema</param>
        /// <returns>La redireccion a la vista para confirmar la operacion</returns>
        public ActionResult Delete(Guid ids, Guid idt)
        {
            SurveysTopic surveystopic = db.SurveysTopics.Find(ids, idt);
            ViewBag.survey = surveystopic.IdSurveys;
            return View(surveystopic);
        }

        //
        // POST: /SurveysTopics/Delete/5


        /// <summary>
        /// Se llama despues de que el usuario confirma eliminar la encuesta
        /// </summary>
        /// <param name="ids">Identificador de la encuesta</param>
        /// <param name="idt">Identificador del tema</param>
        /// <returns>Redirecciona al listado de tema para la encuesta despues de que borra el objeto</returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid ids, Guid idt)
        {            
            SurveysTopic surveystopic = db.SurveysTopics.Find(ids, idt);
            
            db.SurveysTopics.Remove(surveystopic);
            db.SaveChanges();
            TempData["Success"] = "Se ha eliminado el tema de la encuesta correctamente";
            return RedirectToAction("Index", new {id = ids});
        }


        /// <summary>
        /// Metodo dispose
        /// </summary>
        /// <param name="disposing">bandera dispose</param>
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Metodo que retorna una validacion si el numero para la el tema que se va a asociar a la encuesta  es único
        /// </summary>
        /// <param name="TopicNumber">Numero del tema</param>
        /// <returns>Verdadero si el numero no existe, falso si el numero ya existe</returns>       
        public bool  ExisteNumero(Decimal TopicNumber, Guid IdSurveys, Guid IdTopic)
        {
            var surveys = db.SurveysTopics.Where(s => s.IdSurveys == IdSurveys).Where(s => s.TopicNumber == TopicNumber);
            foreach (SurveysTopic surveysTopic in surveys )
            {
                if (surveysTopic.TopicNumber == TopicNumber && surveysTopic.IdSurveys == IdSurveys && surveysTopic.IdTopic != IdTopic)
                {
                    return false;
                }
            }
            return true;
        }

        

        
    }
}