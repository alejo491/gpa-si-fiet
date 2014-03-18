using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SIFIET.Models
{
    /// <summary>
    /// Clase que representa la relacion entre encuesta y tema
    /// </summary>
    public partial class SurveysTopic
    {
        /// <summary>
        /// Codigo de la encuesta
        /// </summary>
        public System.Guid IdSurveys { get; set; }

        /// <summary>
        /// Codigo del tema
        /// </summary>
        [Display(Name = "Tema")]
        public System.Guid IdTopic { get; set; }

        /// <summary>
        /// Numero del tema en la encuesta
        /// </summary>
        [Display(Name = "Orden")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
        [Range(1, double.MaxValue, ErrorMessage = "Tiene que ser un numero positivo")]
        public decimal TopicNumber { get; set; }
        
        /// <summary>
        /// Objeto encuesta asociado a este objeto
        /// </summary>
        public virtual Survey Survey { get; set; }

        /// <summary>
        /// Objeto tema asociado a este objeto
        /// </summary>
        public virtual Topic Topic { get; set; }
    }
}
