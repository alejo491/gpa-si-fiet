using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIFIET.Models
{
    /// <summary>
    /// Clase que representa la Tesis
    /// </summary>
    public partial class Thesis
    {
        /// <summary>
        /// Identificador del Estudio
        /// </summary>
        public System.Guid IdStudies { get; set; }

        /// <summary>
        /// Título de la tesis
        /// </summary>
        [Display(Name = "Título")]
        [RegularExpression(@"[A-Za-zñÑáéíóúÁÉÍÓÚ\s]*", ErrorMessage = "El formato es incorrecto")]
        public string Title { get; set; }

        /// <summary>
        /// Descrioción de la tesis
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Estudio asignado a la tesis
        /// </summary>
        public virtual Study Study { get; set; }
    }
}
