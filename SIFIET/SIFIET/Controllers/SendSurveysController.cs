using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using SIFIET.Models;

namespace SIFIET.Controllers
{
   /// <summary>
   /// Controlador que se encarga de solicitar las consultas, de mostrar la vista de encuesta general, encuesta especificio y/o preview
   /// </summary>

    public class SendSurveysController : Controller
    {
        private DbSIEPISContext db = new DbSIEPISContext();

        #region Atributos
        private bool _egresados;
        private bool _jefeEgresados;
        private bool _ingenieriadeSistemas;
        private bool _ingenieriaElectronica;
        private bool _ingenieriaAutomatica;
        private bool _telematica;
        private string _fechaDesde;
        private string _fechaHasta;
        private string _asunto;
        private string _mensaje;
        private bool _continuar;
        private bool _continuar2 = false;
        public Regex Re;
       #endregion
        
        //
        // GET: /SendSurveys/
        /// <summary>
        /// Inicializa las variables necesarias, para usarse en la vista Send (Encuesta General o Especifica)
        /// </summary>
        /// <param name="id">GUID (Identificador) de la Encuesta</param>
        /// <returns>Una Vista de Enviar Encuesta</returns>
        
        public ActionResult Send(Guid id)
        {
            ViewBag.Seleccionado1 = true;
            ViewBag.Seleccionado2 = false;
            ViewBag.Seleccionado3 = false;
            ViewBag.Seleccionado4 = false;
            ViewBag.Seleccionado5 = false;
            ViewBag.Seleccionado6 = false;
            ViewBag.FechaDesde = "";
            ViewBag.FechaHasta = "";
            ViewBag.idSurvey = id;
            Re = new Regex("^(0?[1-9]|1[0-9]|2|2[0-9]|3[0-1])/(0?[1-9]|1[0-2])/(d{2}|d{4})$");

            return View();
           
        }

        /// <summary>
        /// Valida si el formulario de enviar encuesta general esta completo y correcto
        /// </summary>
        /// <param name="id">GUID (Identificador) de la Encuesta</param>
        /// <param name="form">Elementos de la página a evaluar (TODOS)</param>
        /// <returns>Retorna la acción de Redireccionar a otra Vista (Preview), si cumple con las condiciones de lo contrario se queda en la vista</returns>
        
        [HttpPost]
        public ActionResult Send(Guid id, FormCollection form)
        {
            ViewBag.idSurvey = id;
            var selected= new Dictionary<string,string>();
            _continuar = false;
            _continuar2 = false;
            TempData["Error3"] = "¡Es Obligatorio Colocar un Asunto!";
            TempData["Error4"] = "¡Es Obligatorio Colocar un Mensaje!";

            #region Validaciones de Seleccion de Destinatario

            foreach (string v in form)
            {
                var k = form[v];
                if (_continuar)
                {
                    TempData["Error1"] = "";
                    break;
                }

                switch (v)
                {
                    case "Dirigida":

                        if (k == "True")
                        {
                            ViewBag.Seleccionado1 = true;
                            /*
                            ViewBag.Seleccionado2 = false;
                          * */
                            _continuar = true;
                            _egresados = true;
                            _jefeEgresados = false;
                          
                        }
                        else
                        {
                           
                            ViewBag.Seleccionado1 = false;
                            /* ViewBag.Seleccionado2 = true;
                             */
                            _continuar = true;
                            _egresados = false;
                            _jefeEgresados = true;
                            
                        }
                        break;
                }
            }
            #endregion

            #region Validaciones de un Programa

            foreach (string variable in form)
            {
                var k = form[variable];
                switch (variable)
                {

                    case "IngSistemas":
                        _ingenieriadeSistemas = k.Contains("true");
                        if (k.Contains("true"))
                        {
                            ViewBag.Seleccionado3 = true;
                        }
                        else
                        {
                            ViewBag.Seleccionado3 = false;
                        }
                        break;
                    case "IngElectronica":
                        _ingenieriaElectronica = k.Contains("true");
                        if (k.Contains("true"))
                        {
                            ViewBag.Seleccionado4 = true;
                        }
                        else
                        {
                            ViewBag.Seleccionado4 = false;
                        }
                        break;
                    case "IngAutomatica":
                        _ingenieriaAutomatica = k.Contains("true");
                        if (k.Contains("true"))
                        {
                            ViewBag.Seleccionado5 = true;
                        }
                        else
                        {
                            ViewBag.Seleccionado5 = false;
                        }
                        break;
                    case "Telematica":
                        _telematica = k.Contains("true");
                        if (k.Contains("true"))
                        {
                            ViewBag.Seleccionado6 = true;
                        }
                        else
                        {
                            ViewBag.Seleccionado6 = false;
                        }
                        break;
                }
            }
            if (!(_ingenieriadeSistemas) && !(_ingenieriaAutomatica) && !(_ingenieriaElectronica) && !(_telematica))
            {
                TempData["Error2"] = "¡Es Obligatorio Seleccionar como Minimo un Programa!";
            }
            
            #endregion

            #region Se Valida la Fecha y se permite continuar si es Valida

            /*
             string []format = new string []{"yyyy-MM-dd HH:mm:ss"};
             string value = "2011-09-02 15:30:20";
             DateTime datetime;
             if (DateTime.TryParseExact(value, format, System.Globalization.CultureInfo.InvariantCulture,System.Globalization.DateTimeStyles.NoCurrentDateDefault  , out datetime))
             Console.WriteLine("Valido  : " + datetime);
             else
             Console.WriteLine("Invalido");
             */
            foreach (string variable in form)
            {
                var k = form[variable];
                switch (variable)
                {

                    case "txtFechaDesde":
                        ViewBag.FechaDesde = k;
                        _fechaDesde = k;
                        break;
                    case "txtFechaHasta":
                        ViewBag.FechaHasta = k;
                        _fechaHasta = k;
                        break;
                }
            }

            string[] format = new string[] { "yyyy-MM-dd" };
            string[] formatE = new string[] { "MM-dd-yyyy" };
            string[] formatV = new string[] { "dd-MM-yyyy" };

            DateTime fecha;
            if (_fechaDesde.Contains("/"))
            {
                _fechaDesde=_fechaDesde.Replace('/', '-');
            }
            if (_fechaHasta.Contains("/"))
            {
               _fechaHasta= _fechaHasta.Replace('/','-');
            }

            if (_fechaDesde !="")
            {
                if (DateTime.TryParseExact(_fechaDesde,format,System.Globalization.CultureInfo.InvariantCulture,System.Globalization.DateTimeStyles.NoCurrentDateDefault,out fecha))
                {
                    if (Convert.ToDateTime(_fechaDesde) > DateTime.Now)
                    {
                        TempData["Error1"] = "¡La Fecha Egresados Desde No pueden ser mayores al dia actual!";
                        _continuar = false;
                 
                    }
                }
                else if (DateTime.TryParseExact(_fechaDesde, formatE, System.Globalization.CultureInfo.InvariantCulture,
                                                System.Globalization.DateTimeStyles.NoCurrentDateDefault, out fecha))
                {
                    string fechaConFormato = string.Empty;

                    //formatea la fecha si viene en formato mm-dd-yyyy
                    fechaConFormato = Regex.Replace(_fechaDesde,
                    @"(?<mm>\d{1,2})-(?<dd>\d{1,2})\b-\b(?<yyyy>\d{4})",
                    "${yyyy}-${mm}-${dd}");
                    _fechaDesde = fechaConFormato;

                    if (Convert.ToDateTime(_fechaDesde) > DateTime.Now)
                    {
                        TempData["Error1"] = "¡La Fecha Egresados Desde No pueden ser mayores al dia actual!";
                        _continuar = false;

                    }

                }
                else if (DateTime.TryParseExact(_fechaDesde, formatV, System.Globalization.CultureInfo.InvariantCulture,
                                            System.Globalization.DateTimeStyles.NoCurrentDateDefault, out fecha))
                {
                    string fechaConFormato = string.Empty;

                    //formatea la fecha si viene en formato dd-mm-yyyy
                    fechaConFormato = Regex.Replace(_fechaDesde,
                    @"(?<dd>\d{1,2})\b-(?<mm>\d{1,2})-\b(?<yyyy>\d{4})",
                    "${yyyy}-${mm}-${dd}");
                    _fechaDesde = fechaConFormato;

                    if (Convert.ToDateTime(_fechaDesde) > DateTime.Now)
                    {
                        TempData["Error1"] = "¡La Fecha Egresados Desde No pueden ser mayores al dia actual!";
                        _continuar = false;

                    }

                }
                else
               {
                    TempData["Error1"] = "¡La Fecha No tiene un Formato Valido!";
                    _continuar = false;
                }
            }
            if (_fechaHasta != "")
            {
                if (DateTime.TryParseExact(_fechaHasta,format,System.Globalization.CultureInfo.InvariantCulture,System.Globalization.DateTimeStyles.NoCurrentDateDefault,out fecha))
                {
                    if (Convert.ToDateTime(_fechaHasta) > DateTime.Now)
                    {
                        TempData["Error1"] = "¡La Fecha Egresados Hasta No pueden ser mayores al dia actual!";
                        _continuar = false;
                    }
                    else
                    {
                        TempData["Error1"] = "";
                        _continuar = true;
                    }
                }
                else if (DateTime.TryParseExact(_fechaHasta, formatE, System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.NoCurrentDateDefault, out fecha))
                {
                    string fechaConFormato = string.Empty;

                    //formatea la fecha si viene en formato mm-dd-yyyy
                    fechaConFormato = Regex.Replace(_fechaHasta,
                    @"(?<mm>\d{1,2})-(?<dd>\d{1,2})\b-\b(?<yyyy>\d{4})",
                    "${yyyy}-${mm}-${dd}");
                    _fechaHasta = fechaConFormato;

                    if (Convert.ToDateTime(_fechaHasta) > DateTime.Now)
                    {
                        TempData["Error1"] = "¡La Fecha Egresados Hasta No pueden ser mayores al dia actual!";
                        _continuar = false;
                    }
                    else
                    {
                        TempData["Error1"] = "";
                        _continuar = true;
                    }

                }
                else if (DateTime.TryParseExact(_fechaHasta, formatV, System.Globalization.CultureInfo.InvariantCulture,
                                        System.Globalization.DateTimeStyles.NoCurrentDateDefault, out fecha))
                {
                    string fechaConFormato = string.Empty;

                    //formatea la fecha si viene en formato dd-mm-yyyy
                    fechaConFormato = Regex.Replace(_fechaHasta,
                    @"(?<dd>\d{1,2})\b-(?<mm>\d{1,2})-\b(?<yyyy>\d{4})",
                    "${yyyy}-${mm}-${dd}");
                    _fechaHasta = fechaConFormato;
                    
                    if (Convert.ToDateTime(_fechaHasta) > DateTime.Now)
                    {
                        TempData["Error1"] = "¡La Fecha Egresados Hasta No pueden ser mayores al dia actual!";
                        _continuar = false;
                    }
                    else
                    {
                        TempData["Error1"] = "";
                        _continuar = true;
                    }

                }
                else
                {
                    TempData["Error1"] = "¡La Fecha No tiene un Formato Valido!";
                    _continuar = false;
                }
            }
            if (_continuar)
            {
                if (_fechaDesde != "" && _fechaDesde != "")
                {
                    if (Convert.ToDateTime(_fechaDesde) > Convert.ToDateTime(_fechaHasta))
                    {
                        TempData["Error1"] = "¡La Fecha Desde no puede ser mayor a Fecha Hasta !";
                        _continuar = false;
                    }
                    else
                    {
                        _continuar = true;
                    }
                }
            }
            #endregion

            #region Validar si Asunto esta lleno

            foreach (string v in form)
            {
                var k = form[v];
                if (_continuar2)
                {
                    TempData["Error3"] = "";
                    break;
                }

                switch (v)
                {   
                    case "txtAsunto":
                        if (k == "")
                        {
                            TempData["Error3"] = "¡Es Obligatorio Colocar un Asunto!";
                            _continuar2 = false;
                        }
                        else
                        {
                            _continuar2 = true;
                            ViewBag.Asunto = k;
                            _asunto = k;
                        }
                        break;
                }
            }
            #endregion

            #region Validar si Mensaje esta lleno

            _continuar2 = false;
            foreach (string v in form)
            {
                var k = form[v];
                if (_continuar2)
                {
                    TempData["Error4"] = "";
                    break;
                }

                switch (v)
                {
                        case "txtMensaje":
                        if (k == "")
                        {
                            TempData["Error4"] = "¡Es Obligatorio Colocar un Mensaje!";
                            _continuar2 = false;
                        }
                        else
                        {
                            if (_asunto !=null)
                            {
                                _continuar2 = true;
                            }
                            ViewBag.Mensaje = k;
                            TempData["Error4"] = "";
                            _mensaje = k;
                        }
                        break;
                }
            }
            #endregion

            if (_continuar==true && _continuar2==true)
            {
                if (_jefeEgresados)
                {
                    #region Jefes de Egresados en Fechas Especificas y por Programas Seleccionados
                    if (_fechaDesde != "" && _fechaHasta != "")
                    {
                        DateTime fd = (Convert.ToDateTime(_fechaDesde));
                        DateTime fh = (Convert.ToDateTime(_fechaHasta));

                        if (_ingenieriadeSistemas)
                        {
                            var c = SendSurveysDbController.ListarEgresadosJefe("Ingenieria de Sistemas",fd,fh);
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                        else if (_ingenieriaAutomatica)
                        {
                            var c = SendSurveysDbController.ListarEgresadosJefe("Ingenieria Automatica", fd, fh);
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                        else if (_ingenieriaElectronica)
                        {
                            var c = SendSurveysDbController.ListarEgresadosJefe("Ingenieria de Electronica", fd, fh);
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                        else if (_telematica)
                        {
                            var c = SendSurveysDbController.ListarEgresadosJefe("Telematica", fd, fh);
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                    }
                    #endregion

                    #region Jefes de Egresados desde una Fecha, hasta la Actual, por Programas Seleccionados
                    else if (_fechaHasta == "" && _fechaDesde!="")
                    {
                        DateTime fd =(Convert.ToDateTime(_fechaDesde));

                        if (_ingenieriadeSistemas)
                        {
                            var c = SendSurveysDbController.ListarEgresadosJefe("Ingenieria de Sistemas",fd, DateTime.Now);
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                        else if (_ingenieriaAutomatica)
                        {
                            var c = SendSurveysDbController.ListarEgresadosJefe("Ingenieria Automatica", fd, DateTime.Now);
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                        else if (_ingenieriaElectronica)
                        {
                            var c = SendSurveysDbController.ListarEgresadosJefe("Ingenieria de Electronica", fd, DateTime.Now);
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                        else if (_telematica)
                        {
                            var c = SendSurveysDbController.ListarEgresadosJefe("Telematica", fd, DateTime.Now);
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                    }
                    #endregion

                    #region Jefes de Egresados Hasta una fecha, por Programas Seleccionados
                    else if (_fechaDesde=="" && _fechaHasta!="")
                    {
                        DateTime fh = Convert.ToDateTime(_fechaHasta);

                        if (_ingenieriadeSistemas)
                        {
                            var c = SendSurveysDbController.ListarEgresadosJefe("Ingenieria de Sistemas",
                                                                                fh);
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                        else if (_ingenieriaAutomatica)
                        {
                            var c = SendSurveysDbController.ListarEgresadosJefe("Ingenieria Automatica",
                                                                                fh);
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                        else if (_ingenieriaElectronica)
                        {
                            var c = SendSurveysDbController.ListarEgresadosJefe("Ingenieria de Electronica",
                                                                                fh);
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                        else if (_telematica)
                        {
                            var c = SendSurveysDbController.ListarEgresadosJefe("Telematica",
                                                                                fh);
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                    }
                    #endregion

                    #region Todos los Jefes de Egresados, por Programas Seleccionados
                    else if (_fechaDesde == "" && _fechaHasta != "")
                    {
                       if (_ingenieriadeSistemas)
                        {
                            var c = SendSurveysDbController.ListarEgresadosJefe("Ingenieria de Sistemas");
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                        else if (_ingenieriaAutomatica)
                        {
                            var c = SendSurveysDbController.ListarEgresadosJefe("Ingenieria Automatica");
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                        else if (_ingenieriaElectronica)
                        {
                            var c = SendSurveysDbController.ListarEgresadosJefe("Ingenieria de Electronica");
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                        else if (_telematica)
                        {
                            var c = SendSurveysDbController.ListarEgresadosJefe("Telematica");
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                    }
                    #endregion
                }

                if (_egresados)
                {
                    #region Egresados entre Fechas y Por Programas Seleccionados
                    if (_fechaDesde != "" && _fechaHasta != "")
                    {
                        DateTime fd = (Convert.ToDateTime(_fechaDesde));
                        DateTime fh = (Convert.ToDateTime(_fechaHasta));

                        if (_ingenieriadeSistemas)
                        {
                            var c = SendSurveysDbController.ListarEgresadosPrograma("Ingenieria de Sistemas", fd, fh);
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                        else if (_ingenieriaAutomatica)
                        {
                            var c = SendSurveysDbController.ListarEgresadosPrograma("Ingenieria Automatica",  fd, fh);
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                        else if (_ingenieriaElectronica)
                        {
                            var c = SendSurveysDbController.ListarEgresadosPrograma("Ingenieria de Electronica", fd, fh);
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                        else if (_telematica)
                        {
                            var c = SendSurveysDbController.ListarEgresadosPrograma("Telematica", fd, fh);
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                    }
                    #endregion

                    #region Egresados desde una Fecha Especifica hasta la actualidad por Programas Seleccionados
                    else if (_fechaHasta == ""&& _fechaDesde!="")
                    {
                        DateTime fd = (Convert.ToDateTime(_fechaDesde));

                        if (_ingenieriadeSistemas)
                        {
                            var c = SendSurveysDbController.ListarEgresadosPrograma("Ingenieria de Sistemas", fd, DateTime.Now);
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                        else if (_ingenieriaAutomatica)
                        {
                            var c = SendSurveysDbController.ListarEgresadosPrograma("Ingenieria Automatica", fd, DateTime.Now);
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                        else if (_ingenieriaElectronica)
                        {
                            var c = SendSurveysDbController.ListarEgresadosPrograma("Ingenieria de Electronica", fd, DateTime.Now);
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                        else if (_telematica)
                        {
                            var c = SendSurveysDbController.ListarEgresadosPrograma("Telematica", fd, DateTime.Now);
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                    }
                    #endregion

                    #region Todos los Egresados, por Programa Seleccionado
                    else if (_fechaDesde=="" && _fechaHasta!="")
                    {
                        DateTime fh = (Convert.ToDateTime(_fechaHasta));

                        if (_ingenieriadeSistemas)
                        {
                            Dictionary<string, string> c =
                                SendSurveysDbController.ListarEgresadosPrograma("Ingenieria de Sistemas",
                                                                                fh);
                           selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);

                        }
                        else if (_ingenieriaAutomatica)
                        {
                            var c = SendSurveysDbController.ListarEgresadosPrograma("Ingenieria Automatica",
                                                                                    fh);
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                        else if (_ingenieriaElectronica)
                        {
                            var c = SendSurveysDbController.ListarEgresadosPrograma("Ingenieria de Electronica",
                                                                                    fh);
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                        else if (_telematica)
                        {
                            var c = SendSurveysDbController.ListarEgresadosPrograma("Telematica",
                                                                                    fh);
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                    }
                    #endregion

                    #region Egresados entre Fechas y Por Programas Seleccionados

                    if (_fechaDesde == "" && _fechaHasta == "")
                    {
                        if (_ingenieriadeSistemas)
                        {
                            var c = SendSurveysDbController.ListarEgresadosPrograma("Ingenieria de Sistemas");
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                        else if (_ingenieriaAutomatica)
                        {
                            var c = SendSurveysDbController.ListarEgresadosPrograma("Ingenieria Automatica");
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                        else if (_ingenieriaElectronica)
                        {
                            var c = SendSurveysDbController.ListarEgresadosPrograma("Ingenieria de Electronica");
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                        else if (_telematica)
                        {
                            var c = SendSurveysDbController.ListarEgresadosPrograma("Telematica");
                            selected = selected.Concat(c).ToDictionary(x => x.Key, x => x.Value);
                        }
                    }
                    #endregion
                }

                //selected.Add("jaimealberto.jurado@gmail.com","Jaime Jurado");

                //Agregar validacion de que si no encuentra ningun usuario no pueda continuar
                //La Concatenacion no esta funcionando

                if (selected.Count == 0)
                {
                    TempData["ErrorDatos"] = "No se puede continuar, No Se encontraron Datos";
                    return View();
                }

                TempData["message"] = ViewBag.Mensaje;
                TempData["subject"] = ViewBag.Asunto;
                TempData["d"] = selected;

                return RedirectToAction("Preview", new {id});
            }
            return View();
        }

        /// <summary>
        /// Muestra una previsualización del correo electrónico a enviar. Sigue la plantilla SurveyEmailTemplate.htm
        /// ubicada en el directorio Templete. Previsualiza Asunto, link y cuerpo del correo electrónico
        /// </summary>
        /// <param name="id">Identificador de la encuesta a enviar</param>
        /// <returns>Una Vista con la previsualización del correo a enviar</returns>
        public ActionResult Preview(Guid id) {

            //var name = ((Dictionary<string, string>)TempData["d"])["jaimejn@unicauca.edu.co"];
            var survey = (Survey)db.Surveys.Find(id);
            ViewBag.Survey = survey;
            var message = (string)TempData["message"];
            var subject = (string)TempData["subject"];
            var usuarios = (Dictionary<string,string>)TempData["d"];
            TempData["d"] = usuarios;
            if (usuarios.Count > 0)
            {
                var item = usuarios.First();
                var url = Url.Action("Fill", "FillSurvey", new { @ids=id,@Email = item.Key });
                ViewData["body"] = SendSurveysEmailController.PopulateBody(item.Value, survey.Name, url, message);
                TempData["message"] = message;
                TempData["title"] = survey.Name;
                TempData["id"] = survey.Id;
                ViewBag.Subject = subject;
                TempData["subject"] = subject;
                return View();
            }
            else
            {
                var url = Url.Action("Fill", "FillSurvey", new { @ids=id,@Email = "localhost@localhost"});
                ViewData["body"] = SendSurveysEmailController.PopulateBody("UserName", survey.Name, url, message);
                TempData["message"] = message;
                TempData["title"] = survey.Name;
                TempData["id"] = survey.Id;
                ViewBag.Subject = subject;
                TempData["subject"] = subject;
                return View();
            }

            
        }

        [HttpPost]
        public ActionResult Preview(FormCollection form) {

            var recipients = (Dictionary<string, string>)TempData["d"];
            var message = (string)TempData["message"];
            var subject = (string)TempData["subject"];
            var title = (string)TempData["title"];
			var id = (Guid)TempData["id"];



            foreach (string item in recipients.Keys) {
                var restourl = Url.Action("Fill", "FillSurvey", new { @ids=id , @Email = item });
                var url = "http://localhost:54321" + restourl;
                var body = SendSurveysEmailController.PopulateBody(recipients[item], title, url, message);
                SendSurveysEmailController.SendHtmlFormattedEmail(item, subject, body);
            }

            return RedirectToAction("Index", "Surveys");

        }

        //
        // GET: /SendSurveys/

        public ActionResult SendSpecific(Guid id)
        {
            return View();
        }

        //
        // POST: /SendSurveys/SendSpecific
        [HttpPost]
        public ActionResult SendSpecific(Guid id, FormCollection form)
        {
            var selected = new Dictionary<string, string>();
            string nombreCompleto = "";
            TempData["ErrorD"] = "¡Es Obligatorio Seleccionar un Destinatario!";
            TempData["ErrorA"] = "¡Es Obligatorio Colocar un Asunto!";
            TempData["ErrorM"] = "¡Es Obligatorio Colocar un Mensaje!";

            #region Validar si Destinatario esta lleno

            _continuar = false;

            foreach (string variable in form)
            {
                var k = form[variable];
                if (_continuar)
                {
                    TempData["ErrorD"] = "";
                    break;
                }

                switch (variable)
                {
                    case "txtDestinatario":
                        if (k == "")
                        {
                            TempData["ErrorD"] = "¡Es Obligatorio Seleccionar un Destinatario!";
                            _continuar = false;
                        }
                        else
                        {
                            TempData["ErrorD"] = "";
                            nombreCompleto = k;
                        }
                        break;
                }
            }
            #endregion

            #region Validar si Asunto esta lleno

            foreach (string variable in form)
            {
                var k = form[variable];

                if (_continuar)
                {
                    TempData["ErrorA"] = "";
                    break;
                }

                switch (variable)
                {
                    case "txtAsunto":
                        if (k == "")
                        {
                            TempData["ErrorA"] = "¡Es Obligatorio Colocar un Asunto!";
                            _continuar = false;
                        }
                        else
                        {
                            if (nombreCompleto != "")
                            {
                                _continuar = true;
                            }
                            ViewBag.Asunto = k;
                            _asunto = k;
                        }
                        break;
                }
            }
            #endregion

            #region Validar si Mensaje esta lleno

            _continuar = false;

            foreach (string variable in form)
            {
                var k = form[variable];
                if (_continuar)
                {
                    TempData["ErrorM"] = "";
                    break;
                }

                switch (variable)
                {
                    case "txtMensaje":
                        if (k == "")
                        {
                            TempData["ErrorM"] = "¡Es Obligatorio Colocar un Mensaje!";
                            _continuar = false;
                        }
                        else
                        {
                            if (_asunto != null)
                            {
                                _continuar = true;
                            }
                            ViewBag.Mensaje = k;
                            TempData["ErrorM"] = "";
                            _mensaje = k;
                        }
                        break;
                }
            }
            #endregion

            if (_continuar)
            {
                var usuario = db.Users.Where(u => u.FirstNames + " " + u.LastNames == nombreCompleto);
                foreach (var user in usuario)
                {
                    var idUsuario = user.Id;
                    var memberShip = db.aspnet_Membership.Find(idUsuario);
                    selected.Add(memberShip.Email, nombreCompleto);
                }

                TempData["subject"] = _asunto;
                TempData["message"] = _mensaje;
                TempData["d"] = selected;

                return RedirectToAction("Preview", new { id });
            }

            return View();
            
        }

        public ActionResult Autocomplete(string term)
        {
            var items = (from u in db.Users select u.FirstNames + " " + u.LastNames).ToArray();
            var filteredItems = items.Where(
                item => item.StartsWith(term, StringComparison.InvariantCultureIgnoreCase));
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
    }
}
