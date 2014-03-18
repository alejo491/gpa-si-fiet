using System;
using System.Collections.Generic;

namespace SIFIET.Models
{
    /// <summary>
    /// Representa la vista de la base de datos
    /// </summary>
    public partial class FilesPost
    {
        /// <summary>
        /// Identificador del post
        /// </summary>
        public System.Guid IdPost { get; set; }
        /// <summary>
        /// Identificador del archivo
        /// </summary>
        public System.Guid IdFile { get; set; }
        /// <summary>
        /// Principal
        /// </summary>
        public int Main { get; set; }
        /// <summary>
        /// Identificador del post
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Objeto de tipo File
        /// </summary>
        public virtual File File { get; set; }
        /// <summary>
        /// Objeto de tipo Post
        /// </summary>
        public virtual Post Post { get; set; }
    }
}
