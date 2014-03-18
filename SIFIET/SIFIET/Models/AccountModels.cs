using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace SIFIET.Models
{
    /// <summary>
    /// Clase que representa el cambio de contraseña
    /// </summary>
    public class ChangePasswordModel
    {
        /// <summary>
        /// Contraseña actual
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña actual")]
        public string OldPassword { get; set; }

        /// <summary>
        /// Nueva Contraseña 
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Confirmar Contraseña 
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar la nueva contraseña")]
        [Compare("NewPassword", ErrorMessage = "La nueva contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
    /// <summary>
    /// Acceso al token de Facebook
    /// </summary>
    public class FacebookLoginModel
    {
        public string accessToken { get; set; }
    }

    /// <summary>
    /// Clase que representa el inicio de sesión de usuario
    /// </summary>
    public class LogOnModel
    {
        /// <summary>
        /// Nombre de usuario
        /// </summary>
        [Required]
        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        /// <summary>
        /// Contraseña 
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        /// <summary>
        /// Chequeo del usuario que inicia sesión
        /// </summary>
        [Display(Name = "Recordar mi cuenta")]
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// Clase que representa el registro del usuario
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// Nombre
        /// </summary>
        [Required]
        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        /// <summary>
        /// Correo
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Dirección de correo electrónico")]
        [RegularExpression(@"^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$", ErrorMessage = " No tiene el formato de un correo electrónico")]
        public string Email { get; set; }

        /// <summary>
        /// Contraseña
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        /// <summary>
        /// Confirmar Contraseña 
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}
