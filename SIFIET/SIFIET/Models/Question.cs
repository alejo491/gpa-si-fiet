using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SIFIET.Models
{
    /// <summary>
    /// Clase que representa una pregunta
    /// </summary>
    public partial class Question
    {
        /// <summary>
        /// Constructor de Question
        /// </summary>
        public Question()
        {
            this.AnswerChoices = new List<AnswerChoice>();
            this.ExemplarsQuestions = new List<ExemplarsQuestion>();
        }
        /// <summary>
        /// Muestra Informacion en español y el mensaje en caso de que sea un campo obligatorio
        /// </summary>
        
        [Display(Name = "Id Pregunta")]
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        public System.Guid Id { get; set; }


        /// <summary>
        /// Muestra Informacion en español y el mensaje en caso de que sea un campo obligatorio
        /// </summary>
        
        [Display(Name = "Id Tema")]
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        public System.Guid IdTopic { get; set; }

        /// <summary>
        /// Muestra Informacion en español y el mensaje en caso de que sea un campo obligatorio
        /// </summary>
        
        [Display(Name = "Tipo de Pregunta")]
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        public string Type { get; set; }

        /// <summary>
        /// Muestra Informacion en español y el mensaje en caso de que sea un campo obligatorio
        /// </summary>
        
        [Display(Name = "Enunciado de la Pregunta")]
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        [DataType(DataType.MultilineText)]
        public string Sentence { get; set; }

        /// <summary>
        /// Muestra Informacion en español y el mensaje en caso de que sea un campo obligatorio
        /// </summary>
        
        [Display(Name = "Orden de Pregunta")]
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
        [Range(1, double.MaxValue, ErrorMessage = "Tiene que ser un numero positivo")]
        public decimal QuestionNumber { get; set; }

        /// <summary>
        /// Muestra Informacion en español y el mensaje en caso de que sea un campo obligatorio
        /// </summary>
        /// 
        [Display(Name = "¿Es Obligatorio La Pregunta?")]
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]        
        public decimal IsObligatory { get; set; }
        public virtual ICollection<AnswerChoice> AnswerChoices { get; set; }
        public virtual ICollection<ExemplarsQuestion> ExemplarsQuestions { get; set; }
        public virtual Topic Topic { get; set; }
    }
}
