using System;
using System.Collections.Generic;

namespace SIFIET.Models
{
    /// <summary>
    /// Guarda los metodos de una clase
    /// </summary>
    public partial class Method
    {
        /// <summary>
        /// Constructor de la Clase
        /// </summary>
        public Method()
        {
            this.RoleMethods = new List<RoleMethod>();
        }

        /// <summary>
        /// Codigo del Metodo
        /// </summary>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Nombre del Metodo
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Nombre Completo del Metodo
        /// </summary>
        public string FullName { get; set; }
        
        /// <summary>
        /// Codigo del Controlador
        /// </summary>
        public System.Guid IdController { get; set; }
        
        /// <summary>
        /// Coleccion de objetos de la relacion de RoleMethods y Method
        /// </summary>
        public virtual ICollection<RoleMethod> RoleMethods { get; set; }
        
        /// <summary>
        /// Objeto de tipo SecureController
        /// </summary>
        public virtual SecureController SecureController { get; set; }
    }
}
