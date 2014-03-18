using System;
using System.Collections.Generic;

namespace SIFIET.Models
{
    /// <summary>
    /// Clase que representa los roles asignados a los usuarios
    /// </summary>
    public partial class aspnet_UsersInRoles
    {
        /// <summary>
        /// Identificador del usuario al que se le asignara el rol
        /// </summary>
        public System.Guid UserId { get; set; }
        /// <summary>
        /// rol que se le asigna al usuario
        /// </summary>
        public System.Guid RoleId { get; set; }
       /// <summary>
       /// Coleccion de Roles
       /// </summary>
        public virtual aspnet_Roles aspnet_Roles { get; set; }
        /// <summary>
        /// Coleccion de usuarios
        /// </summary>
        public virtual aspnet_Users aspnet_Users { get; set; }
        /// <summary>
        /// constructor parametrzado
        /// </summary>
        /// <param name="UserId">identificador del usuario</param>
        /// <param name="RoleId">identificador del rol</param>
        public aspnet_UsersInRoles(Guid UserId, Guid RoleId)
        {
            this.UserId = UserId;
            this.RoleId = RoleId;
            
        }
        /// <summary>
        /// constructor por defecto
        /// </summary>
        public aspnet_UsersInRoles()
        {
            aspnet_Roles = new aspnet_Roles();
            aspnet_Users = new aspnet_Users();
        }
    }
}
