using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace SIFIET.Models
{
    /// <summary>
    /// Clase que representa las encuestas
    /// </summary>
    public partial class Survey
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public Survey()
        {
            this.Exemplars = new List<Exemplar>();
            this.SurveysTopics = new List<SurveysTopic>();
        }

        /// <summary>
        /// Codigo de la encuesta
        /// </summary>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Nombre de la encuesta
        /// </summary>
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        [MaxLength(50, ErrorMessage = "No pueder tener mas de 50 caracteres")]
        public string Name { get; set; }


        /// <summary>
        /// Objetivo de la encuesta
        /// </summary>
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        [MaxLength(300, ErrorMessage = "No pueder tener mas de 300 caracteres")]
        [DataType(DataType.MultilineText)]
        public string Aim { get; set; }

        /// <summary>
        /// Coleccion de ejemplares de la encuesta
        /// </summary>
        public virtual ICollection<Exemplar> Exemplars { get; set; }

        /// <summary>
        /// Coleccion de objetos de la relacion entre Encuestas y temas
        /// </summary>
        public virtual ICollection<SurveysTopic> SurveysTopics { get; set; }
        
        
    }
}

