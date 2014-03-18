using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIFIET.Models
{
    /// <summary>
    /// Clase que representa los estudios
    /// </summary>
    public partial class Study
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public Study()
        {
            this.Electives = new List<Elective>();
        }

        /// <summary>
        /// Identificador de la institución
        /// </summary>
        [DisplayName("Institución")]
        public System.Guid IdSchool { get; set; }

        /// <summary>
        /// NombrePrograma
        /// </summary>
        [DisplayName("Programa")]
        public System.String Programs { get; set; }

        /// <summary>
        /// Identificador del Usuario
        /// </summary>
        public System.Guid IdUser { get; set; }

        /// <summary>
        /// Título
        /// </summary>
        [DisplayName("Título")]
        public string Grade { get; set; }

        /// <summary>
        /// Fecha de Inicio
        /// </summary>
        [DisplayName("Fecha de Inicio")]
        public System.DateTime StartDate { get; set; }

        /// <summary>
        /// Fecha de Finalización
        /// </summary>
        [DisplayName("Fecha de Finalización")]
        public System.DateTime EndDate { get; set; }

        /// <summary>
        /// Identificador del estudio
        /// </summary>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Institución 
        /// </summary>
        public virtual School School { get; set; }

        /// <summary>
        /// usuario
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Tesis
        /// </summary>
        public virtual Thesis Thesis { get; set; }

        /// <summary>
        /// Colección de electivas del estudio
        /// </summary>
        public virtual ICollection<Elective> Electives { get; set; }
    }
}
