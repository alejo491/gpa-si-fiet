using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIFIET.Models
{
    /// <summary>
    /// Clase que representa las opciones derespuesta
    /// </summary>
    public partial class AnswerChoice
    {
        /// <summary>
        /// contructor de la clase
        /// </summary>
        public AnswerChoice()
        {
            this.Answers = new List<Answer>();
        }
        /// <summary>
        /// Identificador de la opcion de respuesta
        /// </summary>
        [Display(Name = "Id Opción")]
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        public System.Guid Id { get; set; }
        /// <summary>
        /// identificador de la pregunta a la cual corresponde la opcion de respuesta
        /// </summary>
        [Display(Name = "Id Pregunta")]
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        public System.Guid IdQuestion { get; set; }
        /// <summary>
        /// Enunciado que tiene la opcion de respuesta
        /// </summary>
        [Display(Name = "Enunciado")]
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        [DataType(DataType.MultilineText)]
        public string Sentence { get; set; }
        /// <summary>
        /// Peso que se le asigna a esa opcion de respuesta en algun rango
        /// </summary>
        [Display(Name = "Valor Cuantitativo")]
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        [Range(1, double.MaxValue)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
        public decimal NumericValue { get; set; }
        /// <summary>
        /// Tipo de opcionde respuesta ya sea normal ,con texto corto o largo
        /// </summary>
        [Display(Name = "Tipo")]
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        public string Type { get; set; }
        /// <summary>
        /// orden en la que aparecera la opcion de respuesta dentro de la pregunta
        /// </summary>
        [Display(Name = "Número de Orden")]
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
        [Range(0, double.MaxValue, ErrorMessage = "Tiene que ser un numero positivo")]
        public decimal AnswerNumber { get; set; }
        /// <summary>
        /// Pregunta a la que correspponde la opcion de respuesta
        /// </summary>
        public virtual Question Question { get; set; }
        /// <summary>
        /// Coleccion de respuestas
        /// </summary>
        public virtual ICollection<Answer> Answers { get; set; }

    }
}
