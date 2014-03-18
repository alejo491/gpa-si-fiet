using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.IO;
using System.Configuration;

namespace SIFIET.Controllers
{
    /// <summary>
    /// Esta encargada de contruir el cuerpo del correo electrónico y
    /// de enviar un correo electrónico basado en unas configuraciones previas en el 
    /// web.config
    /// </summary>
    public static class SendSurveysEmailController
    {

        #region Construir cuerpo del correo
        /// <summary>
        /// Transforma la plantilla a una cadena, y cambia las etiquetas con los valores
        /// correspondientes para poder ser enviados como email
        /// </summary>
        /// <param name="userName">Contenido de la tag UserName</param>
        /// <param name="title">Contenido de la tag Title</param>
        /// <param name="url">Contenido de la tag Urk</param>
        /// <param name="description">Contenido de la tag Description</param>
        /// <returns>Cadena de caracteres con la estructura de la plantilla</returns>
        public static string PopulateBody(string userName, string title, string url, string description) {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Template/SurveyEmailTemplate.htm"))) {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{UserName}", userName);
            body = body.Replace("{Title}", title);
            body = body.Replace("{Url}", url);
            body = body.Replace("{Description}", description);
            return body;
        }
        #endregion

        #region Enviar email
        /// <summary>
        /// Enviar el html formateado en un correo
        /// </summary>
        /// <param name="recepientEmail">Email del destinatario</param>
        /// <param name="subject">Asunto del email</param>
        /// <param name="body">Cuerpo del email</param>
        public static void SendHtmlFormattedEmail(string recepientEmail, string subject, string body) {
            using (MailMessage mailMessage = new MailMessage()) {
                mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["UserName"]);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                mailMessage.To.Add(new MailAddress(recepientEmail));
                SmtpClient smtp = new SmtpClient();
                smtp.Host = ConfigurationManager.AppSettings["Host"];
                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = ConfigurationManager.AppSettings["UserName"];
                NetworkCred.Password = ConfigurationManager.AppSettings["Password"];
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
                smtp.Send(mailMessage);
            }
        }
        #endregion


    }
}
