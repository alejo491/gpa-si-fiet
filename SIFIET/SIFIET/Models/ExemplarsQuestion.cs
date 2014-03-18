using System;
using System.Collections.Generic;

namespace SIFIET.Models
{
    /// <summary>
    /// Representa la relacion entre los ejemplares y las preguntas
    /// </summary>
    public partial class ExemplarsQuestion
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public ExemplarsQuestion()
        {
            this.Answers = new List<Answer>();
        }

        /// <summary>
        /// Codigo de la pregunta
        /// </summary>
        public System.Guid IdQuestion { get; set; }
        
        /// <summary>
        /// Codigo del ejemplar
        /// </summary>
        public System.Guid IdExemplar { get; set; }

        /// <summary>
        /// Objeto de tipo respuesta
        /// </summary>
        public virtual ICollection<Answer> Answers { get; set; }


        /// <summary>
        /// Objeto de tipo ejemplat
        /// </summary>
        public virtual Exemplar Exemplar { get; set; }

        /// <summary>
        /// Objeto de tipo pregunta
        /// </summary>
        public virtual Question Question { get; set; }
    }
}
