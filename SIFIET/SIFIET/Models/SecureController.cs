using System;
using System.Collections.Generic;

namespace SIFIET.Models
{
    /// <summary>
    /// Representa los controladores de la aplicacion que se van a asegurar
    /// </summary>
    public partial class SecureController
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public SecureController()
        {
            this.Methods = new List<Method>();
        }

        /// <summary>
        /// Codigo del controlador
        /// </summary>
        public System.Guid Id { get; set; }
        /// <summary>
        /// Nombre del controlador
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Conjunto de metodos del controlador
        /// </summary>
        public virtual ICollection<Method> Methods { get; set; }
    }
}
