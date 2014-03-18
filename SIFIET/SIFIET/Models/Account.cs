using System;
using System.Collections.Generic;

namespace SIFIET.Models
{
    /// <summary>
    /// Clase que representa a la cuenta
    /// </summary>
    public partial class Account
    {
        /// <summary>
        /// Identificador de la cuenta
        /// </summary>
        public System.Guid Id { get; set; }
        /// <summary>
        /// Identificador de usuario
        /// </summary>
        public System.Guid IdUser { get; set; }
        /// <summary>
        /// Correo electrónico
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Tipo de cuenta
        /// </summary>
        public string AccountType { get; set; }
        /// <summary>
        /// usuario
        /// </summary>
        public virtual User User { get; set; }
    }
}
