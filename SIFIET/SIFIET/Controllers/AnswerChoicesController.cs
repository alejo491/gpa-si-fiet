using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using SIFIET.Models;
namespace SIFIET.Controllers
{
    public class AnswerChoicesController : Controller
    {
        private DbSIEPISContext db = new DbSIEPISContext();


        #region Listar
        /// <summary>
        /// Muestra todas las opciones de respuesta de una pregunta
        /// </summary>
        /// <param name="id">identificador de la pregunta a la cual corresponden las opciones de respuesta</param>
        /// <returns></returns>
        public ActionResult Index(Guid? id)
        {
            if (id != Guid.Empty && id != null)
            {
                 var questions = (Question)db.Questions.Find(id);
                if (questions != null)
                {
                    ViewBag.Question = questions;
                    var AnswerChoices = db.AnswerChoices.Include(o => o.Question).Where(o => o.IdQuestion == id);
                    return View(AnswerChoices.OrderBy(a=>a.AnswerNumber).ToList());
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

        #region Detalles
        /// <summary>
        /// Muestara en detalle una opcion de respuesta
        /// </summary>
        /// <param name="id">identificador de la opcion de respuesta</param>
        /// <returns></returns>
        public ViewResult Details(Guid id)
        {
                AnswerChoice AnswerChoice = db.AnswerChoices.Find(id);
                return View(AnswerChoice);
        }
        #endregion

        #region Crear opcion de respuesta
        /// <summary>
        /// Permite la creacion de una nueva opcion de respuesta
        /// </summary>
        /// <param name="id">identificador de la pregunta</param>
        /// <returns></returns>
        public ActionResult Create(Guid? id)
        {
            if (id != Guid.Empty && id != null)
            {
                var questions = (Question)db.Questions.Find(id);
                if (questions != null)
                {
                    ViewBag.Question = questions;                 
                    return View();
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

        #region Crear opcion de respuesta HttpPost
        /// <summary>
        /// Guarda la opcion de respuesta que se recibe en un formulario
        /// </summary>
        /// <param name="id">identificador de la pregunta</param>
        /// <param name="answerChoice">opcion de respuesta recibida desde un formulario</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Guid? id, AnswerChoice answerChoice)
        {
            if (ModelState.IsValid)
            {
                
                answerChoice.Id = Guid.NewGuid();
                answerChoice.IdQuestion = new Guid(""+id);
                if (answerChoice.AnswerNumber < 1)
                {
                    if (answerChoice.AnswerNumber == 0)
                    {
                        answerChoice.AnswerNumber = 1;

                    }
                    else
                    {
                        answerChoice.AnswerNumber = answerChoice.AnswerNumber * -1;
                    }

                }

                
                    if (answerChoice.NumericValue < 1)
                {
                    if (answerChoice.NumericValue == 0)
                    {
                        answerChoice.NumericValue = 1;

                    }
                    else
                    {
                        answerChoice.NumericValue = answerChoice.NumericValue * -1;
                    }



                }
                var questions = (Question)db.Questions.Find(id);                
                bool existeNumero = ExisteNumero(answerChoice.AnswerNumber, answerChoice.IdQuestion, answerChoice.Id);
				if(existeNumero)
				{
                    db.AnswerChoices.Add(answerChoice);
                    db.SaveChanges();
                    TempData["Success"] = "Se ha creado la Opción de Respuesta correctamente";
                    return RedirectToAction("Index", new { id = answerChoice.IdQuestion });
				}
				else
				{
				    if (questions != null)
				    {
				        ViewBag.Question = questions;
				    }
				    TempData["Error2"] = "Este número ya ha sido asignado a otra opción de respuesta";
					return View();
				}
			}
            // ViewBag.IdQuestion = new SelectList(db.Preguntas, "Id", "Enunciado", AnswerChoice.IdQuestion);
            return View(answerChoice);			
        }
        #endregion

        #region Editar opcion de respuesta
        /// <summary>
        /// Da la opcion de editar una opcion de respuesta que ya esta guardada
        /// </summary>
        /// <param name="id">identificador de la opcion de respuesta</param>
        /// <returns></returns>
        public ActionResult Edit(Guid? id)
        {
            if (id != Guid.Empty && id != null )
            {
                 AnswerChoice answerChoice = db.AnswerChoices.Find(id);
                 var achoice = db.AnswerChoices.Find(id);
                 if(achoice != null)
                 {
                     return View(answerChoice);
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

        #region Editar opcion de respuesta HttpPost

        /// <summary>
        /// Guarda las modificaciones hechas a una ocpion de respuesta
        /// </summary>
        /// <param name="answerChoice">opcion de respuesta que se modifico y que se va a actualizar</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(AnswerChoice answerChoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(answerChoice).State = EntityState.Modified;
                
                bool existeNumero = ExisteNumero(answerChoice.AnswerNumber, answerChoice.IdQuestion, answerChoice.Id);
                if (!existeNumero)
                {
                    var questions = (Question)db.Questions.Find(answerChoice.IdQuestion);
                    if (questions != null)
                    {
                        ViewBag.Question = questions;
                    }
                    TempData["Error2"] = "Este número ya ha sido asignado a otra opción de respuesta";
                    return View(answerChoice);
                }
                else
                {
                    
                    db.SaveChanges();
                    TempData["Success"] = "Se ha editado la Opción de Respuesta correctamente";
                    return RedirectToAction("Index", new { id = answerChoice.IdQuestion });
                
                }


            }
            //ViewBag.IdQuestion = new SelectList(db.Preguntas, "Id", "Enunciado", AnswerChoice.IdQuestion);
            return View(answerChoice);
        }
        #endregion

        #region Eliminar opcion de respuesta
        /// <summary>
        /// Da la opcion de elimianr la opcion de respuesta
        /// </summary>
        /// <param name="id">identificador de la opcion de respuesta</param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {

            if (id != Guid.Empty && id != null )
            {
                 AnswerChoice answerChoice = db.AnswerChoices.Find(id);
                 var achoice = db.AnswerChoices.Find(id);
                 if(achoice != null)
                 {
                     return View(answerChoice);
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

        #region Eliminar opcion de respuesta HttpPost
        /// <summary>
        /// Elimina la opcion de respuesta ala que corresponde el id
        /// </summary>
        /// <param name="id">identificador de la opcion de respuesta</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            AnswerChoice AnswerChoice = db.AnswerChoices.Find(id);
            db.AnswerChoices.Remove(AnswerChoice);
            db.SaveChanges();
			TempData["Success"] = "Se ha eliminado la Opción de respuesta correctamente";
            return RedirectToAction("Index", new { id = AnswerChoice.IdQuestion });
        }
        #endregion

        #region Verificar existencia de numero 
        /// </summary>
        /// Metodo que retorna una validacion si el número de orden que se le va asignar a la opcion de respuesta es único
        /// </summary>
        /// <param name="TopicNumber">Numero de la Opción de respuesta</param>
        /// <returns>Verdadero si el numero no existe, falso si el numero ya existe</returns>
        public bool ExisteNumero(Decimal TopicNumber, Guid IdQuestion, Guid IdChoice)
        {
            //var question = db.Questions.Where(a => a.Id == IdAnswer).
            var answer = db.AnswerChoices.Where(s => s.IdQuestion == IdQuestion);

            
            foreach (AnswerChoice answerChoice in answer)
            {
                if (answerChoice.AnswerNumber == TopicNumber && answerChoice.Id != IdChoice)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

   
    }
}