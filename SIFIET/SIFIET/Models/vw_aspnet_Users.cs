using System;
using System.Collections.Generic;

namespace SIFIET.Models
{
    public partial class vw_aspnet_Users
    {
        /// <summary>
        /// Identificador de la aplicacion
        /// </summary>
        public System.Guid ApplicationId { get; set; }

        /// <summary>
        /// Identificador del usuario
        /// </summary>
        public System.Guid UserId { get; set; }

        /// <summary>
        /// Nombre de usuario
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Nombrecorto del usuario
        /// </summary>
        public string LoweredUserName { get; set; }
        /// <summary>
        /// Alias
        /// </summary>
        public string MobileAlias { get; set; }
        /// <summary>
        /// Anónimo
        /// </summary>
        public bool IsAnonymous { get; set; }
        /// <summary>
        /// ültima actividad
        /// </summary>
        public System.DateTime LastActivityDate { get; set; }
    }
}
