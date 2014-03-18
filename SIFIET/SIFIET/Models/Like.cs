using System;
using System.Collections.Generic;

namespace SIFIET.Models
{
    /// <summary>
    /// Representa la vista de la base de datos
    /// </summary>
    public partial class Like
    {
        /// <summary>
        /// Identificador del like
        /// </summary>
        public System.Guid Id { get; set; }
        /// <summary>
        /// Identificador del post
        /// </summary>
        public System.Guid Id_Post { get; set; }
        // <summary>
        /// Identificador del usuario
        /// </summary>
        public System.Guid Id_User { get; set; }
        /// <summary>
        /// Objeto de tipo Post
        /// </summary>
        public virtual Post Post { get; set; }
        /// <summary>
        /// Objeto de tipo User
        /// </summary>
        public virtual User User { get; set; }
    }
}
