using System;
using System.Collections.Generic;

namespace SIFIET.Models
{
    /// <summary>
    /// Representa la vista de la base de datos
    /// </summary>
    public partial class Comment
    {
        /// <summary>
        /// Identificador del Comment
        /// </summary>
        public System.Guid Id { get; set; }
        /// <summary>
        /// Identificador del post
        /// </summary>
        public System.Guid IdPost { get; set; }
        /// <summary>
        /// Identificador del usuario
        /// </summary>
        public System.Guid IdUser { get; set; }
        /// <summary>
        /// Contenido del comment
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Fecha de publicación del comentario
        /// </summary>
        public System.DateTime CommentDate { get; set; }
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
