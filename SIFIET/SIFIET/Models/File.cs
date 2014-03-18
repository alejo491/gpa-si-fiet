using System;
using System.Collections.Generic;

namespace SIFIET.Models
{
    /// <summary>
    /// Representa la vista de la base de datos
    /// </summary>
    public partial class File
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public File()
        {
            this.FilesPosts = new List<FilesPost>();
        }
        /// <summary>
        /// Identificador del archivo
        /// </summary>
        public System.Guid Id { get; set; }
        /// <summary>
        /// Ruta del archivo
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Tipo de archivo
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Tamaño del archivo
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// Array de archivos
        /// </summary>
        public virtual ICollection<FilesPost> FilesPosts { get; set; }
    }
}
