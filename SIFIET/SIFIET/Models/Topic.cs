using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIFIET.Models
{
    /// <summary>
    /// Clase que representa los temas de una encuesta
    /// </summary>
    public partial class Topic
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public Topic()
        {
            this.Questions = new List<Question>();
            this.SurveysTopics = new List<SurveysTopic>();
        }

        /// <summary>
        /// Codigo del tema
        /// </summary>
        public System.Guid Id { get; set; }
        
        /// <summary>
        /// Descripcion del tema
        /// </summary>
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        [Display(Name="Descripción")]
        [MaxLength(50, ErrorMessage = "No puede tener más de 50 caracteres")]
        public string Description { get; set; }

        /// <summary>
        /// Coleccion de preguntas asociadas al tema
        /// </summary>
        public virtual ICollection<Question> Questions { get; set; }

        /// <summary>
        /// Coleccion de relaciones con encuestas
        /// </summary>
        public virtual ICollection<SurveysTopic> SurveysTopics { get; set; }
    }
}
