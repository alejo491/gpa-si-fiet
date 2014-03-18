using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using SIFIET.Models;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using System.Net;
using SIFIET.Controllers;
using System.IO;
using System.Configuration;

namespace SIFIET.Controllers
{
    /// <summary>
    /// Controlador que gestiona el registro y la autenticación de los usuarios
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// Atributo que consulta la base de datos.
        /// </summary>
        private DbSIEPISContext db = new DbSIEPISContext();

        /// <summary>
        /// Metodo que permite registrarse o iniciar sesion usando una cuenta de Facebook
        /// </summary>
        /// <param name="model">usuario registrado en el sistema</param>
        /// <returns>vista en la cual se llenaran los demas campos del usuario</returns>
        [HttpPost]
        public ActionResult FacebookLogin(FacebookLoginModel model)
        {
            if (ModelState.IsValid)
            {
                Session["accessToken"] = model.accessToken;


                WebClient client = new WebClient();
                string JsonResult = client.DownloadString(string.Concat(
                       "https://graph.facebook.com/me?access_token=", model.accessToken));

                JObject jsonUserInfo = JObject.Parse(JsonResult);

                string username = jsonUserInfo.Value<string>("username");
                string contrasena = jsonUserInfo.Value<string>("email");
                string email = jsonUserInfo.Value<string>("email");

                // Intento de registrar al usuario
                MembershipCreateStatus createStatus;
                DbSIEPISContext db = new DbSIEPISContext();

                System.Web.Security.MembershipUserCollection uno = Membership.FindUsersByEmail(email);
                if (uno.Count == 0)
                {
                    var user = Membership.CreateUser(username, contrasena, email, null, null, true, null, out createStatus);
                    if (createStatus == MembershipCreateStatus.Success)
                    {
                        FormsAuthentication.SetAuthCookie(username, false);
                        Session["firstTime"] = true; //Para Wizard
                        return RedirectToAction("Index", "Verify");
                    }
                    else
                    {
                        ModelState.AddModelError("", ErrorCodeToString(createStatus));
                    }
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(username, true);
                    return RedirectToAction("Index", "Verify");
                }
            }
            return RedirectToAction("Index", "Verify");

        }

        /// <summary>
        /// Método que verifica si un usuario esta activo en el sistema
        /// </summary>
        /// <param name="model">Usuario registrado en el sistema</param>
        /// <returns>un entero entre 0 y 2, dependiendo el caso</returns>
        public int searchUser(LogOnModel model)
        {
            Guid g = System.Guid.Empty;
            foreach (var e in db.aspnet_Users)
            {
                if (e.UserName == model.UserName)
                {
                    g = e.UserId;
                }
            }
            var id = g;
            int data = 2;
            foreach (var w in db.Users)
            {
                if (w.Id == id)
                {
                    if (w.States == "false")
                    {
                        data = 0;
                    }
                    else { data = 1; }
                }
            }
            return data;
        }

        // GET: /Account/LogOn
        /// <summary>
        /// Método que carga la vista con el formulario que permite al usuario ingresar al sistema
        /// </summary>
        /// <returns>Vista que despliega el formulario para loguearse en el sistema</returns>
        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn
        /// <summary>
        /// Valida si los datos recibidos en el formulario pertenecen a un usuario e inicia sesion 
        /// </summary>
        /// <param name="model">Datos del usuario</param>
        /// <param name="returnUrl">Direccion</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    int userstate = searchUser(model);
                    if (userstate == 1 || userstate == 2)
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                             && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Verify");
                        }
                    }
                    else
                    {
                        TempData["Inactivo"] = "¡ El Usuario se encuentra Inactivo !";
                        TempData["info"] = " Pongase en contacto con el Administrador del Sistema";
                    }
                }
                else
                {
                    TempData["Invalidos"] = "El nombre de usuario o la contraseña especificados son incorrectos.";
                }
            }
            // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
            return View(model);
        }

        //
        // GET: /Account/LogOff
        /// <summary>
        /// Método que permite cerrar sesión
        /// </summary>
        /// <returns>Vista principal del Home de la aplicación</returns>
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            try
            {
                Session["userID"] = null;
            }
            catch (Exception e) { }
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register
        /// <summary>
        /// Método que carga la vista con el formulario que permite al usuario registrarse en el sistema
        /// </summary>
        /// <returns>Vista que despliega el formulario para registrarse en el sistema</returns>
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        /// <summary>
        /// Guarda los datos recibido en el formulario y crea el usuario 
        /// </summary>
        /// <param name="model">Datos del usuario</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Intento de registrar al usuario
                MembershipCreateStatus createStatus;
                System.Web.Security.MembershipUserCollection uno = Membership.FindUsersByEmail(model.Email);
                if (uno.Count == 0)
                {
                    var user = Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);
                    if (createStatus == MembershipCreateStatus.Success)
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                        Session["firstTime"] = true; //Para Wizard
                        return RedirectToAction("AsignarRol", "Verify");
                    }
                    else
                    {
                        ModelState.AddModelError("", ErrorCodeToString(createStatus));
                    }
                }
                else
                {
                    TempData["Error"] = "El Correo ya fue Registrado por otro Usuario";
                    model.Email = "";
                }
            }
            // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
            return View(model);
        }

        //
        // GET: /Account/ChangePassword
        /// <summary>
        /// Método que carga la vista con el formulario que permite al usuario cambiar su contraseña
        /// </summary>
        /// <returns>Vista que despliega el formulario para cambiar contraseña</returns>
        //[Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword
        /// <summary>
        /// Guarda los datos recibido en el formulario y actualiza la contraseña del usuario 
        /// </summary>
        /// <param name="model">Datos del usuario</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // ChangePassword iniciará una excepción en lugar de
                // devolver false en determinados escenarios de error.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    Guid g = System.Guid.Empty;
                    foreach (var e in db.aspnet_Users)
                    {
                        if (e.UserName == HttpContext.User.Identity.Name)
                        {
                            g = e.UserId;
                        }
                    }
                    TempData["NewPassword"] = "Su contraseña a sido cambiada satisfactoriamente.";
                    return RedirectToAction("Begin", "User", new { id = g });
                }
                else
                {
                    TempData["Error"] = "La contraseña actual es incorrecta o la nueva contraseña no es válida.";
                }
            }
            // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
            return View(model);
        }

        /// <summary>
        /// Método que carga la vista que permite al usuario recuperar su contraseña
        /// </summary>
        /// <returns>Vista para recuperar contraseña</returns>
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        //
        // POST: /Account/ChangePasswordSuccess
        /// <summary>
        /// Valida el correo digitado para cambiar la contraseña 
        /// </summary>
        /// <param name="model">Datos del usuario</param>
        /// <returns></returns>
        //[Authorize]
        [HttpPost]
        public ActionResult ChangePasswordSuccess(RegisterModel model)
        {
            Boolean aux = false;
            aspnet_Membership tem = new aspnet_Membership();
            string correo = "";
            string contra = "";
            Guid user = new Guid();

            foreach (var i in db.aspnet_Membership)
            {
                if (i.Email == model.Email)
                {
                    tem = i;
                    correo = i.Email;
                    user = i.UserId;
                    aux = true;
                }
            }
            if (aux == false)
            {
                TempData["Email"] = "El correo no ha sido registrado por ningún usuario en el sistema";
            }
            else 
            {
                aspnet_Users userr = db.aspnet_Users.Find(user);
                string usuario = userr.UserName.ToString();
                string subject = "SISTEMA CONTROL DE EGRESADOS UNICAUCA, recuperación de contraseña";
                MembershipUser userM = Membership.GetUser(usuario);
                String pass = userM.ResetPassword();
                userM.ChangePassword(tem.Password, pass);
                string body = "Hola: " + usuario + " su nueva contraseña es: " + pass + " le recomendamos cambiarla";
                SendHtmlFormattedEmail(correo, subject, body);              
                TempData["Cambiobn"] = "Se envió un correo a su cuenta: " + tem.Email + "  con la nueva contraseña. Verifique.";
                return RedirectToAction("LogOn");
            }
            return View(model);
        }

        #region Enviar email con contraseña nueva
        /// <summary>
        /// Envia una nueva contraseña al correo de un usuario en particular
        /// </summary>
        /// <param name="recepientEmail">Email del destinatario</param>
        /// <param name="subject">Asunto del email</param>
        /// <param name="body">Cuerpo del email</param>
        public static void SendHtmlFormattedEmail(string recepientEmail, string subject, string body)
        {
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress("siepisunicauca@gmail.com");
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                mailMessage.To.Add(new MailAddress(recepientEmail));
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = "siepisunicauca@gmail.com";
                NetworkCred.Password = "sistemapis";
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mailMessage);
            }
        }
        #endregion

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // Vaya a http://go.microsoft.com/fwlink/?LinkID=177550 para
            // obtener una lista completa de códigos de estado.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "El nombre de usuario ya existe. Escriba un nombre de usuario diferente.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Ya existe un nombre de usuario para esa dirección de correo electrónico. Escriba una dirección de correo electrónico diferente.";

                case MembershipCreateStatus.InvalidPassword:
                    return "La contraseña especificada no es válida. Escriba un valor de contraseña válido.";

                case MembershipCreateStatus.InvalidEmail:
                    return "La dirección de correo electrónico especificada no es válida. Compruebe el valor e inténtelo de nuevo.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "La respuesta de recuperación de la contraseña especificada no es válida. Compruebe el valor e inténtelo de nuevo.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "La pregunta de recuperación de la contraseña especificada no es válida. Compruebe el valor e inténtelo de nuevo.";

                case MembershipCreateStatus.InvalidUserName:
                    return "El nombre de usuario especificado no es válido. Compruebe el valor e inténtelo de nuevo.";

                case MembershipCreateStatus.ProviderError:
                    return "El proveedor de autenticación devolvió un error. Compruebe los datos especificados e inténtelo de nuevo. Si el problema continúa, póngase en contacto con el administrador del sistema.";

                case MembershipCreateStatus.UserRejected:
                    return "La solicitud de creación de usuario se ha cancelado. Compruebe los datos especificados e inténtelo de nuevo. Si el problema continúa, póngase en contacto con el administrador del sistema.";

                default:
                    return "Error desconocido. Compruebe los datos especificados e inténtelo de nuevo. Si el problema continúa, póngase en contacto con el administrador del sistema.";
            }
        }
        #endregion

       
    }
}
