using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIFIET.Models;

namespace SIFIET.Controllers
{
    public class ItemSurveysController : Controller
    {

        private DbSIEPISContext db = new DbSIEPISContext();
        
        #region Listar Items de Encuesta
        DbSIEPISContext _db = new DbSIEPISContext();
        /// <summary>
        /// Muestra el listado de Items de la encuesta a la que corresponda el id
        /// </summary>
        /// <param name="id">Identificador de la encuesta a la aque pertenecen los items</param>
        /// <returns></returns>
        public ActionResult Index(Guid id)
        {
            //var itemsurveys = db.ItemSurveys.Include(i => i.Report);
            var itemsurveys = db.ItemSurveys.Where(i=>i.IdReport==id);
            return View(itemsurveys.ToList());
        }
        #endregion

        #region Crear Item de Encuesta
        /// <summary>
        /// Da la opcion de crear un Item para un reporte desde una encuesta
        /// </summary>
        /// <param name="id">Identificador del Reporte</param>
        /// <returns></returns>
        public ActionResult Create(Guid id)
        {
            TempData["idr"] = id;
            return View();
        }
        #endregion

        #region Crear Item de Encuesta HttpPost
        /// <summary>
        /// Guarda el Item de encuesta para un reporte
        /// </summary>
        /// <param name="postedForm">Formulario donde esta contenida la informacion del Item a agrregar</param>
        /// <param name="id">Identificador del Reporte</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(FormCollection postedForm,Guid id)
        {
            var tam = postedForm.Count;
            if (tam > 4)
            {
                var conGrafico = postedForm[3];
                int numPagina = 0;
                try
                {
                    numPagina = Convert.ToInt32(postedForm[4]);
                    if (numPagina < 1) {
                        if (numPagina == 0)
                        {
                            numPagina = 1;

                        }
                        else {
                            numPagina = numPagina * -1;
                        }
                        

                    }
                }
                catch (Exception e)
                {
                    numPagina = 0;
                }
                Guid idSurvey = Guid.Empty;
                Guid idTopic = Guid.Empty;
                Guid idQuestion = Guid.Empty;
                string question = "";
                foreach (string v in postedForm)
                {
                    if (v.StartsWith("IdSurvey"))
                    {
                        idSurvey = new Guid(postedForm[v]);

                    }
                    if (v.StartsWith("IdTopic"))
                    {
                        idTopic = new Guid(postedForm[v]);
                    }
                    if (v.StartsWith("Question"))
                    {
                        idQuestion = new Guid(postedForm[v]);

                    }

                }
                string tema = ((Topic)db.Topics.First(t => t.Id == idTopic)).Description;
                string surveysName = ((Survey)db.Surveys.First(f => f.Id == idSurvey)).Name;
                question = ((Question)(db.Questions.First(st => st.Id == idQuestion))).Sentence;
                string topic = "" + db.Topics.Where(st => st.Id == idTopic);
                string tabla = "VistaModuloEncuesta";
                //String SQL = "" + "select * from " + tabla + " where surveysName='" + surveysName + "' and topicsDescription='" + tema + "' and questionsSentence='" + question + "'";
                String SQL = "" + "select * from (select (Convert(VARCHAR(2000), answerChoicesSentence)) as 'Opcion de respuesta', count(*) as Cantidad from  " + tabla + " where surveysName in('" + surveysName + "') and topicsDescription in('" + tema + "') and IdQuestion in('" + idQuestion + "') group by IdAnswer ,(Convert(VARCHAR(2000), answerChoicesSentence))) Q Order by Q.Cantidad ";
                ItemSurvey itenSurvey = new ItemSurvey();
                itenSurvey.Id = Guid.NewGuid();
                itenSurvey.IdReport = id;       //se recibe como parametro antes de entrar a crear. aqui se saca en ves de crearlo
                itenSurvey.Question = question;
                itenSurvey.GraphicType = conGrafico;
                itenSurvey.ItemNumber = numPagina;
                itenSurvey.Report = db.Reports.First(a => a.Id == itenSurvey.IdReport);
                itenSurvey.SQLQuey = SQL;

                if (numPagina != 0)
                {
                    db.ItemSurveys.Add(itenSurvey);
                    db.SaveChanges();
                    TempData["Success"] = "El Item se ha Creado  correctamente";
                    return RedirectToAction("../Items/GeneralItems", new { id = id });
                }
                else
                {
                    TempData["Success"] = "El Item se ha Creado !!!";
                }
             return View(itenSurvey);
            }
            //return View();

            return View();
            //return RedirectToAction("Index", new { id = id });
        }
        #endregion

        #region Detalles
       
        //
        // GET: /itemsurveys/Details/5
        /// <summary>
        /// Muestra en detalle la informacion del Iten de encuesta a que corresponda el identificador
        /// </summary>
        /// <param name="id">Identificador del Item de encuesta</param>
        /// <returns></returns>
        public ViewResult Details(Guid id)
        {
            ItemSurvey itemsurvey = db.ItemSurveys.Find(id);
            return View(itemsurvey);
        }
        #endregion

        #region Eliminar Item De encuesta

        //
        // GET: /itemsurveys/Delete/5
        /// <summary>
        /// Da la opcion de eliminar Un item de encuesta
        /// </summary>
        /// <param name="id">Identificador del item de encuesta a eliminar</param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            ItemSurvey itemsurvey = db.ItemSurveys.Find(id);
            return View(itemsurvey);
        }
        #endregion

        #region Eliminar Item de Encuesta HttpPost
        //
        // POST: /itemsurveys/Delete/5
        /// <summary>
        /// Elimina el Item de encuesta al aque corresponda el identificador
        /// </summary>
        /// <param name="id">Identificador del Item de encuesta a eliminar</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ItemSurvey itemsurvey = db.ItemSurveys.Find(id);
            db.ItemSurveys.Remove(itemsurvey);
            db.SaveChanges();
            TempData["Success"] = "Se ha Eliminado el Item correctamente";
            return RedirectToAction("../Items/GeneralItems", new { id = itemsurvey.IdReport });
           // return RedirectToAction("../Items/GeneralItems", new { id = id });
        }
        #endregion
      
        #region utilidades
       
        #region Listado de Encuestas
        /// <summary>
        /// Retorna el listado de Encuestas
        /// </summary>
        /// <returns></returns>
        public JsonResult SurveysList()
        {
            //List<String> surveys = new List<string>();
            //surveys.Add("Suma");
            //surveys.Add("Minimo");
            //surveys.Add("Maximo");
            //surveys.Add("Contar");
            //surveys.Add("Promedio");
            List<Survey> surveys = _db.Surveys.ToList();
            var d = new Dictionary<string, string>();
            foreach (var survey in surveys)
            {
                d.Add(survey.Id.ToString(),survey.Name);
            }        
            return Json(d, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Listado de Temas de una encuesta
        /// <summary>
        /// Retorna le listado de temas en una encuesta alque corresponda el id
        /// </summary>
        /// <param name="ids">identificador de la encuesta</param>
        /// <returns></returns>
        public JsonResult TopicsList(string ids)
        {
            var g=Guid.NewGuid();
            try
            {
               g = new Guid(ids);
               TempData["erro1"] = null;
            }
            catch {
                TempData["erro1"] = "Se ha Eliminado el Item correctamente";
            }
            var topics = _db.SurveysTopics.Where(s => s.IdSurveys == g).ToList();
            var list = new Dictionary<string, string>();
            foreach (var surveysTopic in topics)
            {
                var topic = _db.Topics.Find(surveysTopic.IdTopic);
                list.Add(topic.Id.ToString(), topic.Description);

            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Listado de preguntas
        /// <summary>
        /// retorna el listado de preguntas de un tema de la encuesta al que corresponda el id
        /// </summary>
        /// <param name="idt">identificador del tema</param>
        /// <returns></returns>
        public JsonResult QuestionsList(string idt)
        {
            var g = new Guid(idt);
            var questions = _db.Questions.Where(s => s.IdTopic == g);
            var list = new Dictionary<string, string>();
            foreach (var question in questions)
            {
                list.Add(question.Id.ToString(), question.Sentence);
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

       
        #endregion
    }
}
