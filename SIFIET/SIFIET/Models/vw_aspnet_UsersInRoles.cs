using System;
using System.Collections.Generic;

namespace SIFIET.Models
{
    public partial class vw_aspnet_UsersInRoles
    {
        /// <summary>
        /// Identificador de usuario
        /// </summary>
        public System.Guid UserId { get; set; }

        /// <summary>
        /// Identificador de rol
        /// </summary>
        public System.Guid RoleId { get; set; }
    }
}
