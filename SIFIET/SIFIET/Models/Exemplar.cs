using System;
using System.Collections.Generic;

namespace SIFIET.Models
{
    /// <summary>
    /// Clase que representa la relacion entre Encuestados y encuestas
    /// </summary>
    public partial class Exemplar
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public Exemplar()
        {
            this.ExemplarsQuestions = new List<ExemplarsQuestion>();
        }

        /// <summary>
        /// Codigo del ejemplar
        /// </summary>
        public System.Guid Id { get; set; }
        
        /// <summary>
        /// Codigo de la encuesta
        /// </summary>
        public System.Guid IdSurveys { get; set; }
        
        /// <summary>
        /// Numero de ejemplar
        /// </summary>
        public decimal ExemplarNumber { get; set; }
        
        /// <summary>
        /// Codigo del encuestado
        /// </summary>
        public System.Guid IdSurveyed { get; set; }
        
        /// <summary>
        /// Objeto de tipo encuesta
        /// </summary>
        public virtual Survey Survey { get; set; }
        
        /// <summary>
        /// Objeto de tipo encuestado
        /// </summary>
        public virtual Surveyed Surveyed { get; set; }
        
        /// <summary>
        /// Coleccion de ejemplares por pregunta
        /// </summary>
        public virtual ICollection<ExemplarsQuestion> ExemplarsQuestions { get; set; }
    }
}
