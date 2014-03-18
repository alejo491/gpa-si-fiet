using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using SIFIET.Models;

namespace SIFIET.Controllers
{ 
    /*Controlador Finalizado, Documentado*/
    /// <summary>
    /// Controlador Preguntas de las Encuestas
    /// </summary>
    public class QuestionsController : Controller
    {
        private DbSIEPISContext db = new DbSIEPISContext();

        /// <summary>
        /// Esta funcion se llama para mostrar las Preguntas (Questions) asociadas a un Tema (Topic)
        /// </summary>
        /// <param name="id"> GUID (Identificador) del Tema</param>
        /// <returns>Listado de Preguntas Asociadas a un Tema</returns>
        public ActionResult Index(Guid? id)
        {
            if (id != Guid.Empty && id != null)
            {

                var topic = db.Topics.Find(id);

                if (topic != null)
                {
                    var questionsTopics = db.Questions.Include(s => s.Topic).Where(s => s.IdTopic == id).OrderBy(s=>s.QuestionNumber);
                    ViewBag.Topic = topic;
                    return View(questionsTopics.ToList());
                }
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Vista que muestra La información, que pertenece a una Pregunta
        /// </summary>
        /// <param name="id">GUID del la pregunta</param>
        /// <returns>Una (ViewResult) Vista que contiene la información de la Pregunta </returns>

        public ViewResult Details(Guid id)
        {
            Question question = db.Questions.Find(id);
            var auxTopic = db.Topics.Find(question.IdTopic);
            ViewBag.Topic = auxTopic;
            return View(question);
        }

        /// <summary>
        /// Vista que Asocia una Pregunta a un Tema 
        /// </summary>
        /// <param name="id">Identificador del Tema</param>
        /// <returns>ActionResult de la Vista y un Formulario para asociar, la Pregunta al Tema</returns>

        public ActionResult Create(Guid? id)
        {
            if (id != Guid.Empty && id != null)
            {
                var auxTopic = db.Topics.Find(id);

                if (auxTopic != null)
                {
                    ViewBag.Topic = auxTopic;
                    return View();
                }
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Questions/Create

        /// <summary>
        /// Se llama luego que el usuario oprime el boton Crear Pregunta, si ocurre un error redirecciona al usuario a 
        /// una pagina de error.
        /// </summary>
        /// <param name="id">GUID (Identificador del Tema)</param>
        /// <param name="question">Objeto que se crea en el formulario</param>
        /// <returns>ActionResult Redirecciona al listado de Preguntas de un Tema Especifico</returns>
        
        [HttpPost]
        public ActionResult Create(Guid? id, Question question)
        {
          
            if (ModelState.IsValid)
            {
                question.Id = Guid.NewGuid();
                question.IdTopic = new Guid("" + id);
                var topic = db.Topics.Find(id); 
                bool existeNumero = ExisteNumero(question.QuestionNumber, question.IdTopic, question.Id);

                if (topic != null)
                {
                    ViewBag.Topic = topic;
                }
                TempData["Error2"] = "";
                if (existeNumero)
                {
                    
                    db.Questions.Add(question);
                    db.SaveChanges();
                    TempData["Success"] = "Se ha creado la pregunta correctamente";
                    var aux = new AnswerChoice();
                    if (question.Type == "Larga" || question.Type == "Corta")
                    {
                        aux.Id = Guid.NewGuid();
                        aux.IdQuestion = question.Id;                       
                        aux.AnswerNumber = 1;
                        aux.NumericValue = 1;
                        aux.Sentence = "Rta";
                        aux.Type = "Normal";
                        //aux.Question = question;
                        db.AnswerChoices.Add(aux);
                        //question.AnswerChoices.Add(aux);
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index", new {id = id});
               }

                TempData["Error2"] = "Este número ya ha sido asignado a otra pregunta";
                return View();
            }

           // ViewBag.IdTopic = new SelectList(db.Topics, "Id", "Description", question.IdTopic);
            return View(question);
        }
 
        /// <summary>
        /// Se llama cuando el usuario en el listado presiona editar sobre una Pregunta
        /// </summary>
        /// <param name="idT">GUID (Identificador) del Tema</param>
        /// <param name="idQ">GUID (Identificador) de la Pregunta</param>
        /// <returns>Retorna una vista con la informacion editable de la Pregunta</returns>

        public ActionResult Edit(Guid? idT, Guid? idQ)
        {
         
            if (idT != Guid.Empty && idT != null && idQ != Guid.Empty && idQ != null)
            {
                Question question = db.Questions.Find(idQ);
                var auxTopic = db.Topics.Find(idT);

                if (question != null || auxTopic != null)
                {
                    ViewBag.Topic = auxTopic;
                    return View(question);
                }
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Questions/Edit/

        /// <summary>
        /// Se llama despues de que el usuario oprime el boton Guardar
        /// </summary>
        /// <param name="idT">GUID (Identificador) del Tema</param>
        /// <param name="idQ">GUID (Identificador) de la Pregunta</param>
        /// <param name="question">Objeto Modificado de la Pregunta</param>
        /// <returns>Redirecciona al listado de Preguntas, luego de que el objeto se modifique correctamente</returns>

        [HttpPost]
        public ActionResult Edit(Guid idT, Guid idQ, Question question)
        {
            if (ModelState.IsValid)
            {
                //var question2_ = db.Questions.Find(idQ);
                question.IdTopic = idT;
                //question.Type = question2_.Type;
                
               // question.Id = idQ;
                db.Entry(question).State = EntityState.Modified;

                bool existeNumero = ExisteNumero(question.QuestionNumber, question.IdTopic, question.Id);
                TempData["Error2"] = "";
                
                if (!existeNumero)
                {
                    var question_ = db.Questions.Find(idQ);
                    var auxTopic = db.Topics.Find(idT);
                    ViewBag.Topic = auxTopic;

                    if (question_ != null || auxTopic != null)
                    {
                        //ViewBag.IdTopic = new SelectList(db.Topics, "Id", "Description", question.IdTopic);
 
                        TempData["Error2"] = "Este número ya ha sido asignado a otra pregunta";
                        return View(question);
                    }
                   
                }
                else
                {
                    db.SaveChanges();
                    TempData["Success"] = "Se ha editado la pregunta correctamente";
                    return RedirectToAction("Index", new { id = idT }); 
                }
            }

            return View(question);
        }


        /// <summary>
        /// Elimina la Pregunta de un Tema Especifico
        /// </summary>
        /// <param name="id">GUID (Identificador) de la Pregunta</param>
        /// <returns>Redirecciona a la Vista de Confirmar Operacion</returns>
        
        public ActionResult Delete(Guid id)
        {
           
            Question question = db.Questions.Find(id);
            if (question != null)
            {
                var auxTopic = db.Topics.Find(question.IdTopic);
                ViewBag.Topic = auxTopic; 
                return View(question);
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Questions/Delete/

        /// <summary>
        /// Se llama después de que el Usuario confirme eliminar la Pregunta
        /// </summary>
        /// <param name="id">GUID (Identificador) de la Pregunta</param>
        /// <returns>Redirecciona al listado de Preguntas, después de borrar la Pregunta</returns>

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Question question = db.Questions.Find(id);
            var auxTopic = db.Topics.Find(question.IdTopic);
            ViewBag.Topic = auxTopic;
            db.Questions.Remove(question);
            db.SaveChanges();
            TempData["Success"] = "Se ha eliminado la pregunta correctamente";
            return RedirectToAction("Index", new {id=auxTopic.Id});
        }

        /// <summary>
        /// Método que se ejecuta antes de cargar una vista
        /// </summary>
        /// <param name="disposing">Recibe si destruye o no un Objeto</param>

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
 
        /// <summary>
        /// Metodo que retorna una validacion si el número de pregunta que se le va asignar a la Pregunta es único 
        /// </summary>
        /// <param name="questionNumber">Numero de la Pregunta</param>
        /// <param name="idTopic">GUID (Identificador) del  Tema</param>
        /// <param name="idQuestion">GUID (Identificador) de la Pregunta</param>
        /// <returns>Verdadero (True) si el numero no existe, Falso (False) si el numero ya existe</returns>
        
        public bool ExisteNumero(Decimal questionNumber, Guid idTopic, Guid idQuestion)
        {
            //var question = db.Questions.Where(a => a.Id == IdAnswer).
            var q = db.Questions.Where(s => s.IdTopic == idTopic);

            foreach (Question question in q)
            {
                if (question.QuestionNumber == questionNumber && question.Id != idQuestion)
                {
                    return false;
                }
            }
            return true;
        }
    }
}