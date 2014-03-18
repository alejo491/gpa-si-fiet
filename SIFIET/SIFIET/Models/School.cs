using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIFIET.Models
{
    /// <summary>
    /// Clase que representa las instituciones
    /// </summary>
    public partial class School
    {
        /// <summary>
        /// Constructor de la Clase
        /// </summary>
        public School()
        {
            this.Studies = new List<Study>();
        }

        /// <summary>
        /// Identificador de la Institución
        /// </summary>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Nombre de la Institución
        /// </summary>
        [DisplayName("Nombre de la Institución")]
        [RegularExpression(@"[A-Za-zñÑáéíóúÁÉÍÓÚ\s]*", ErrorMessage = "El formato es incorrecto")]
        public string Name { get; set; }

        /// <summary>
        /// Colección de estudios de la institución
        /// </summary>
        public virtual ICollection<Study> Studies { get; set; }
    }
}
