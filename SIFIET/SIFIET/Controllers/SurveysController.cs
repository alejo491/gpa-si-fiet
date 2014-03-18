using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SIFIET.Models;

namespace SIFIET.Controllers
{
    /// <summary>
    /// Controlador que se encarga de crear una encuesta, la cual tiene enlazados temas, preguntas y respuestas que la conforman
    /// </summary>
    
    public class SurveysController : Controller
    {
        private DbSIEPISContext db = new DbSIEPISContext();

        /// <summary>
        /// Muestra el listado de las encuestas existentes
        /// </summary>
        /// <returns>Vista con el listado de encuestas</returns>

        public ViewResult Index()
        {
            return View(db.Surveys.ToList());
        }

        /// <summary>
        /// Permite ver en detalle las encuestas
        /// </summary>
        /// <param name="id">Codigo de la encuesta</param>
        /// <returns>Vista con los detalles de la encuesta</returns>

        public ViewResult Details(Guid id)
        {
            Survey survey = db.Surveys.Find(id);
            return View(survey);
        }

        /// <summary>
        /// Permite crear una encuesta nueva
        /// </summary>
        /// <returns>Vista para crear la encuesta</returns>

        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        ///  Metodo Httpost de crear una encuesta nueva
        /// </summary>
        /// <param name="survey">Objeto que se crea en el formulario</param>
        /// <returns>Retorna el listado de encuestas si no hay errores, si los hay devuelve la misma vista.</returns>

        [HttpPost]
        public ActionResult Create(Survey survey)
        {
            if (ModelState.IsValid)
            {
                survey.Id = Guid.NewGuid();
                db.Surveys.Add(survey);
                db.SaveChanges();
                TempData["Sucess"] = "Se registró correctamente la encuesta "+survey.Name+"!";

                return RedirectToAction("Index");  
            }

            return View(survey);
        }

        /// <summary>
        /// Permite editar una encuesta 
        /// </summary>
        /// <param name="id">Codigo de la encuesta</param>
        /// <returns>Retorna la vista para crear una encuesta</returns>
 
        public ActionResult Edit(Guid id)
        {
            Survey survey = db.Surveys.Find(id);
            return View(survey);
        }

        /// <summary>
        ///  Metodo Httpost de editar encuesta
        /// </summary>
        /// <param name="report">Objeto de tipo de encuesta que trae los elementos de la vista</param>
        /// <returns>Retorna el listado de encuestas si no hay errores, si los hay devuelve la misma vista.</returns>

        [HttpPost]
        public ActionResult Edit(Survey survey)
        {
            if (ModelState.IsValid)
            {
                db.Entry(survey).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Update"] = "Se actualizó correctamente la encuesta " + survey.Name + "!";
                return RedirectToAction("Index");
            }
            return View(survey);
        }

        /// <summary>
        /// Metodo para eliminar una encuesta
        /// </summary>
        /// <param name="id">Codigo de la encuesta</param>
        /// <returns>La vista para eliminar una encuesta</returns>
 
        public ActionResult Delete(Guid id)
        {
            Survey survey = db.Surveys.Find(id);
            return View(survey);
        }

        /// <summary>
        /// Metodo para confirmar la  eliminacion de una encuesta
        /// </summary>
        /// <param name="id">Codigo de la encuesta</param>
        /// <returns>Retorna el listado de encuestas</returns>

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {            
            Survey survey = db.Surveys.Find(id);
            db.Surveys.Remove(survey);
            db.SaveChanges();
            TempData["Remove"] = "Se eliminó correctamente la encuesta " + survey.Name + "!";
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Método que se ejecuta antes de cargar una encuesta
        /// </summary>
        /// <param name="disposing">Recibe si destruye o no un Objeto</param>
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult CreateReport(Guid id)
        {
            Survey survey = db.Surveys.Find(id);
            List<SurveysTopic> topics = db.SurveysTopics.Where(x => x.IdSurveys == id).OrderBy(x=>x.TopicNumber).ToList();
            int cont = 1;
            Report r = new Report();
            r.Id = Guid.NewGuid();
            r.IdUser = db.aspnet_Users.First(u => u.UserName == User.Identity.Name).UserId;
            r.Description = "Reporte de encuesta";
            r.ReportDate = DateTime.Now;
            db.Reports.Add(r);
            db.SaveChanges();
            foreach (var surveysTopic in topics)
            {
                SurveysTopic topic = surveysTopic;
                List<Question> questions = db.Questions.Where(q => q.IdTopic == topic.IdTopic).OrderBy(q=>q.QuestionNumber).ToList();
                foreach (var question in questions)
                {
                    Guid idSurvey = topic.IdSurveys;
                    Guid idTopic = topic.IdTopic;
                    Guid idQuestion = question.Id;
                    string squestion = "";
                    
                    string tema = ((Topic)db.Topics.First(t => t.Id == idTopic)).Description;
                    string surveysName = ((Survey)db.Surveys.First(f => f.Id == idSurvey)).Name;
                    squestion = ((Question)(db.Questions.First(st => st.Id == idQuestion))).Sentence;
                    //string topics = "" + db.Topics.Where(st => st.Id == idTopic);
                    string tabla = "VistaModuloEncuesta";
                    //String SQL = "" + "select * from " + tabla + " where surveysName='" + surveysName + "' and topicsDescription='" + tema + "' and questionsSentence='" + question + "'";
                    String SQL = "" + "select * from (select (Convert(VARCHAR(2000), answerChoicesSentence)) as 'Opcion de respuesta', count(*) as Cantidad from  " + tabla + " where surveysName in('" + surveysName + "') and topicsDescription in('" + tema + "') and IdQuestion in('" + idQuestion + "') group by IdAnswer ,(Convert(VARCHAR(2000), answerChoicesSentence))) Q Order by Q.Cantidad ";
                    ItemSurvey itenSurvey = new ItemSurvey();
                    itenSurvey.Id = Guid.NewGuid();
                    itenSurvey.IdReport = r.Id;       //se recibe como parametro antes de entrar a crear. aqui se saca en ves de crearlo
                    itenSurvey.Question = squestion;
                    itenSurvey.GraphicType = "Pastel";
                    itenSurvey.ItemNumber = cont;
                    itenSurvey.Report = db.Reports.First(a => a.Id == itenSurvey.IdReport);
                    itenSurvey.SQLQuey = SQL;
                    db.ItemSurveys.Add(itenSurvey);
                    db.SaveChanges();
                    cont++;
                    //TempData["Success"] = "El Item se ha Creado  correctamente";                 
                    
                }
            }

            return RedirectToAction("Preview", "Reports", new {id = r.Id});

        }
    }
}