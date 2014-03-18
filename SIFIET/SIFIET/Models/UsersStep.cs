using System;
using System.Collections.Generic;

namespace SIFIET.Models
{
    /// <summary>
    /// Clase que representa los pasos que el usuario ha completado del wizard
    /// </summary>
    public partial class UsersStep
    {
        /// <summary>
        /// Codigo del usuario
        /// </summary>
        public System.Guid UserId { get; set; }
        
        /// <summary>
        /// Codigo del paso
        /// </summary>
        public System.Guid IdSteps { get; set; }
        
        /// <summary>
        /// Bandera para verificar si el usuario ya completo el paso
        /// </summary>
        public string Ok { get; set; }

        /// <summary>
        /// Objeto de tipo paso
        /// </summary>
        public virtual Step Step { get; set; }

        /// <summary>
        /// Objeto de tipo usuario
        /// </summary>
        public virtual User User { get; set; }
    }
}
