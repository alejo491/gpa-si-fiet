using System;
using System.Collections.Generic;

namespace SIFIET.Models
{
    /// <summary>
    /// Clase que guarda las respuestas de las encuestas
    /// </summary>
    public partial class Answer
    {
        /// <summary>
        /// Identificador de la respuesta
        /// </summary>
        public System.Guid Id { get; set; }
        /// <summary>
        /// Identificador de la opcion de respuesta
        /// </summary>
        public System.Guid IdAnswer { get; set; }
        /// <summary>
        /// identificadoor de la respuesta a la cual pertenece la respuesta
        /// </summary>
        public System.Guid IdQuestion { get; set; }
        /// <summary>
        /// Identificador del ejemplar que representa una encuesta contestada
        /// </summary>
        public System.Guid IdExemplar { get; set; }
        /// <summary>
        /// valor que se contesto el encuestado
        /// </summary>
        public string TextValue { get; set; }
        /// <summary>
        /// opcion de respuesta a la que que se contesto essta respuesta
        /// </summary>
        public virtual AnswerChoice AnswerChoice { get; set; }
        /// <summary>
        /// Ejemplar de esa pregunta a la que pertenece la respuesta
        /// </summary>
        public virtual ExemplarsQuestion ExemplarsQuestion { get; set; }
    }
}
