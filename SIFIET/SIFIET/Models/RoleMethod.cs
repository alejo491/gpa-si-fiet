using System;
using System.Collections.Generic;

namespace SIFIET.Models
{
    /// <summary>
    /// Clase relacion entre roles y metodos
    /// </summary>
    public partial class RoleMethod
    {
        /// <summary>
        /// Codigo del rol
        /// </summary>
        public System.Guid IdRole { get; set; }
        
        /// <summary>
        /// Codigo de la accion
        /// </summary>
        public System.Guid IdAction { get; set; }

        /// <summary>
        /// Objeto de tipo rol
        /// </summary>
        public virtual aspnet_Roles aspnet_Roles { get; set; }
        
        /// <summary>
        /// Objeto de tipo metodo
        /// </summary>
        public virtual Method Method { get; set; }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="RoleId">Rol</param>
        /// <param name="ActionId">Metodo</param>
        public RoleMethod( Guid RoleId, Guid ActionId)
        {
            this.IdAction = ActionId;
            this.IdRole = RoleId;
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RoleMethod()
        {
            aspnet_Roles = new aspnet_Roles();
            Method = new Method();
        }
    }
}
