using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using SIFIET.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SIFIET.Controllers
{
    /// <summary>
    /// Controlador para el llenado de encuestas
    /// </summary>
    public class FillSurveyController : Controller
    {
        /// <summary>
        /// Atributo que consulta la base de datos.
        /// </summary>
        private DbSIEPISContext db = new DbSIEPISContext();
         
        #region Contestar Encuesta
        //
        // GET: /FillSurvey/
        /// <summary>
        /// Da la opcion de contestar una encuesta
        /// </summary>
        /// <param name="ids">identificador de la encuesta</param>
        /// <param name="Email">Email del encuestado</param>
        /// <returns></returns>
        public ActionResult Fill(Guid ids, string Email, int wizardStep = 0)
        {
            
            ViewBag.WizardStep = wizardStep;
            var survey = db.Surveys.Find(ids);
            Guid iduser;
            if (Email == null)
            {
                var users = db.aspnet_Users.Where(s => s.UserName == HttpContext.User.Identity.Name);
                if (users.Any())
                {
                    iduser = users.First().UserId;
                    var useremail = db.aspnet_Membership.First(x => x.UserId == iduser).Email;
                    var lsurveyed = db.Surveyeds.Where(x => x.Email == useremail);
                    if (lsurveyed.Any())
                    {
                        var surveyed = lsurveyed.First();
                        var lexemplar = db.Exemplars.Where(x=>x.IdSurveyed == surveyed.Id).Where(x=>x.IdSurveys == ids);
                        if (lexemplar.Any())
                        {
                            var exemplar = lexemplar.Any();
                            if (wizardStep == 0)
                            {
                                TempData["Error"] = "Usted ya ha diligenciado esta encuesta";
                                return RedirectToAction("Index", "Home", null);
                            }
                            else
                            {
                                TempData["Error"] = "Usted ya ha diligenciado esta encuesta";
                                return RedirectToAction("Confirm", "FillSurvey", null);
                            }

                        }
                    }
                    
                }
                
            }
            if (survey != null)
            {
                var surveystopics = db.SurveysTopics.Where(st => st.IdSurveys == survey.Id).OrderBy(st => st.TopicNumber);
                foreach (var surveysTopic in surveystopics.ToList())
                {
                    var topic = db.Topics.Find(surveysTopic.IdTopic);
                    //var user = db.aspnet_Users.Find(idu);
                    //var model = new AnswerViewModel(db.Questions.Where(s=>s.IdTopic == topic.Id));
                    if (topic != null)
                    {
                        var question = db.Questions.Where(s => s.IdTopic == topic.Id).OrderBy(s => s.QuestionNumber);
                        foreach (var question1 in question.ToList())
                        {
                            Question question2 = question1;
                            var answers = db.AnswerChoices.Where(a => a.IdQuestion == question2.Id).OrderBy(s => s.AnswerNumber);
                            question1.AnswerChoices = answers.ToList();
                        }

                        surveysTopic.Topic = topic;
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }


                }

                ViewBag.questions = surveystopics.ToList();
                return View();
            }

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Contestar Encuesta HttpPost
        /// <summary>
        /// Guarda la encuesta recibida en el Formulario
        /// </summary>
        /// <param name="ids">identificador de la encuesta</param>
        /// <param name="Email">Email del encuestado</param>
        /// <param name="postedForm">Formulario con la informacion que contesto el encuestado</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Fill(Guid ids, string Email, FormCollection postedForm, int wizardStep = 0 )
        {
            var w = wizardStep;
            var ListAnswerC = new List<string>();
            var ListVal = new List<string>();
            Guid iduser = Guid.Empty;
            var em = Email;
            if (em == null)
            {
                var users = db.aspnet_Users.Where(s => s.UserName == HttpContext.User.Identity.Name);
                if (users.Any())
                {
                    iduser = users.First().UserId;
                }

                if (iduser != Guid.Empty)
                {
                    var user = db.aspnet_Membership.Find(iduser);
                    em = user.Email;
                }
                else
                {
                    TempData["Error"] = "Usted no puede diligenciar esta encuesta";
                    return RedirectToAction("Index", "Home");
                }


            }
            var exemplar = SaveSurveyed(em, ids);
            if (exemplar == null)
            {
                TempData["Error"] = "No existe ese Email, no puede llenar la encuesta";
                return RedirectToAction("Index", "Home");
            }

            foreach (string v in postedForm)
            {
                if (v.StartsWith("tt"))
                {
                    var idanswC = v.Substring(2, (v.Length - 2));
                    Guid ias = new Guid(idanswC);
                    var text = postedForm[v];

                    if (ListAnswerC.Count > 0)
                    {
                        string idacexiste = ListAnswerC[0];
                        Guid idac = new Guid(idacexiste);
                        var objAnsC = db.AnswerChoices.First(s => s.Id == idac);
                        var idquexiste = objAnsC.IdQuestion;
                        var objAnsCactual = db.AnswerChoices.First(a => a.Id == ias);
                        var idqactual = objAnsCactual.IdQuestion;
                        if (idqactual != idquexiste)
                        {
                            //Aqui se hace el llamado a la funciòn para guardar
                            //saveAnswer(idquexiste, ListAnswerC, ListVal);
                            var ExemplarQuestion = SaveExemplarAnswers(exemplar.Id, idquexiste, ListAnswerC, ListVal);
                            ListAnswerC.Clear();
                            ListVal.Clear();
                            ListAnswerC.Add(idanswC);
                            if (text.Length > 0)
                            {
                                ListVal.Add(text);
                            }

                        }
                        else
                        {
                            ListAnswerC.Add(idanswC);
                            if (text.Length > 0)
                            {
                                ListVal.Add(text);
                            }

                        }
                    }
                    else
                    {
                        ListAnswerC.Add(idanswC);
                        if (text.Length > 0)
                        {
                            ListVal.Add(text);
                        }
                    }

                }

                if (v.StartsWith("un"))
                {
                    var idanswC = postedForm.GetValue(v).AttemptedValue;
                    var idquestion = v.Substring(2, (v.Length - 2));
                    Guid ias = new Guid(idanswC);
                    var objAnsCactual = db.AnswerChoices.First(a => a.Id == ias);
                    var idqactual = objAnsCactual.IdQuestion;
                    var text = objAnsCactual.Sentence;
                    if (ListAnswerC.Count > 0)
                    {
                        string idacexiste = ListAnswerC[0];
                        Guid idac = new Guid(idacexiste);
                        var objAnsC = db.AnswerChoices.First(s => s.Id == idac);
                        var idquexiste = objAnsC.IdQuestion;

                        if (idqactual != idquexiste)
                        {
                            //Aqui se hace el llamado a la funciòn para guardar
                            //saveAnswer(idquexiste, ListAnswerC, ListVal);
                            var ExemplarQuestion = SaveExemplarAnswers(exemplar.Id, idquexiste, ListAnswerC, ListVal);
                            ListAnswerC.Clear();
                            ListVal.Clear();
                            ListAnswerC.Add(idanswC);
                            ListVal.Add(text);
                        }
                        else
                        {
                            ListAnswerC.Add(idanswC);
                            ListVal.Add(text);
                        }
                    }
                    else
                    {
                        ListAnswerC.Add(idanswC);
                        ListVal.Add(text);
                    }
                }

                if (v.StartsWith("mu"))
                {

                    string idanswC = v.Substring(2, (v.Length - 2));
                    var k = postedForm[v];
                    if (k == "true,false")
                    {
                        Guid ias = new Guid(idanswC);
                        var objAnsCactual = db.AnswerChoices.First(a => a.Id == ias);
                        var idqactual = objAnsCactual.IdQuestion;
                        var text = objAnsCactual.Sentence;
                        if (ListAnswerC.Count > 0)
                        {
                            string idacexiste = ListAnswerC[0];
                            Guid idac = new Guid(idacexiste);
                            var objAnsC = db.AnswerChoices.First(s => s.Id == idac);
                            var idquexiste = objAnsC.IdQuestion;

                            if (idqactual != idquexiste)
                            {
                                //Aqui se hace el llamado a la funciòn para guardar
                                //saveAnswer(idquexiste, ListAnswerC, ListVal);
                                var ExemplarQuestion = SaveExemplarAnswers(exemplar.Id, idquexiste, ListAnswerC, ListVal);
                                ListAnswerC.Clear();
                                ListVal.Clear();
                                ListAnswerC.Add(idanswC);
                                ListVal.Add(text);
                            }
                            else
                            {
                                ListAnswerC.Add(idanswC);
                                ListVal.Add(text);
                            }
                        }
                        else
                        {
                            ListAnswerC.Add(idanswC);
                            ListVal.Add(text);
                        }
                    }

                }

            }

            if (ListAnswerC.Count > 0)
            {
                string idacexiste = ListAnswerC[0];
                Guid idac = new Guid(idacexiste);
                var objAnsC = db.AnswerChoices.First(s => s.Id == idac);
                var idquexiste = objAnsC.IdQuestion;
                //Aqui se hace el llamado a la funciòn para guardar
                //saveAnswer(idquexiste, ListAnswerC, ListVal);
                var ExemplarQuestion = SaveExemplarAnswers(exemplar.Id, idquexiste, ListAnswerC, ListVal);
                ListAnswerC.Clear();
                ListVal.Clear();
            }
            if (w==1)
            {
                TempData["Success"] = "¡Muchas gracias por llenar la encuesta!";
                return RedirectToAction("Confirm", "FillSurvey", null);
            }
            TempData["Success"] = "¡Muchas gracias por llenar la encuesta!";
            return RedirectToAction("Index", "Home");

        }

        public ActionResult Confirm()
        {
            return View();
        }

        #endregion

        #region Guaradar encuesta contestada
        /// <summary>
        /// Guarda el ejemplar de encuesta contestada
        /// </summary>
        /// <param name="Email">Email del Encuestado</param>
        /// <param name="id">identificador de la encuesta de la que se va a aguardar el ejemplar</param>
        /// <returns></returns>
        public Exemplar SaveSurveyed(string Email, Guid id)
        {
            var encuestado = db.Surveyeds.Where(s=>s.Email == Email);
            var temps = new Surveyed();
            var tempex = new Exemplar();
            if (encuestado.Any())
            {
                temps = encuestado.First();
            }
            else
            {
                var m = db.Bosses.Where(s => s.Email == Email);                             
                if (!m.Any())
                {
                        var u = db.aspnet_Membership.Where(a => a.Email == Email);
                        if (u.Count() != 0)
                        {

                            foreach (aspnet_Membership aux in u)
                            {
                                User u1 = db.Users.Find(aux.UserId);
                                if (u1 != null)
                                {
                                    temps.Id = Guid.NewGuid();
                                    temps.Name = u1.FirstNames + " " + u1.LastNames;
                                    temps.Email = Email;
                                    temps.Type = "";
                                }
                                else
                                {
                                    temps.Id = Guid.NewGuid();
                                    temps.Name = "";
                                    temps.Email = Email;
                                    temps.Type = "";
                                }
                            }

                            db.Surveyeds.Add(temps);
                            db.SaveChanges();
                        }
                        else
                        {
                    
                            return null;
                        }
                    }
                    else
                    {
                        var u = db.aspnet_Membership.Where(a => a.Email == Email);
                        foreach (aspnet_Membership aux in u)
                        {

                            Boss b1 = db.Bosses.Find(aux.UserId);
                            var tempb = new Surveyed();
                            if (b1 != null)
                            {
                                tempb.Name = b1.Name;
                                tempb.Email = Email;
                                tempb.Type = "Jefe";
                            }
                            else
                            {
                                tempb.Name = "";
                                tempb.Email = Email;
                                tempb.Type = "";
                            }
                }
                
            }

           
            }

            tempex.Id = Guid.NewGuid();
            tempex.ExemplarNumber = db.Exemplars.Count(s => s.IdSurveys == id) + 1;
            tempex.IdSurveyed = temps.Id;
            tempex.IdSurveys = id;
            if (db.Exemplars.Where(s => s.IdSurveyed == temps.Id).Any(s => s.IdSurveys == id))
            {
                return null;
            }
            db.Exemplars.Add(tempex);
            db.SaveChanges();
            return tempex;

        }
        #endregion

        #region Guradar Ejemplares de respuesta
        /// <summary>
        /// Gurada las opciones de respuesta escojidas por el encuestado
        /// </summary>
        /// <param name="idExemplar">identificador del ejemplar</param>
        /// <param name="idQuestion">identificador de la pregunta</param>
        /// <param name="opc">opciones de respuesta escojidas</param>
        /// <param name="valores">respuestas que dio a las opciones escojidas</param>
        /// <returns></returns>
        public Boolean SaveExemplarAnswers(Guid idExemplar, Guid idQuestion, List<string> opc, List<string> valores)
        {
            try
            {
                var tempexqu = new ExemplarsQuestion();
                tempexqu.IdExemplar = idExemplar;
                tempexqu.IdQuestion = idQuestion;

                db.ExemplarsQuestions.Add(tempexqu);
                db.SaveChanges();


                //Llenado de tabla Answers
                int i;
                for (i = 0; i < opc.Count; i++)
                {
                    var tempans = new Answer();
                    tempans.Id = Guid.NewGuid();
                    tempans.IdAnswer = new Guid(opc[i]);
                    tempans.IdQuestion = idQuestion;
                    tempans.IdExemplar = idExemplar;
                    tempans.TextValue = valores[i];
                    db.Answers.Add(tempans);
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }
        #endregion

    }
}
