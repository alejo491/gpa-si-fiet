using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace SIFIET.Models
{
    /// <summary>
    ///  Son los items que se sacan a apartir de una encuesta
    /// </summary>
    public partial class ItemSurvey
    {
        /// <summary>
        /// Codigo del Item
        /// </summary>
        public System.Guid Id { get; set; }
        
        /// <summary>
        /// Codigo del Reporte
        /// </summary>
        public System.Guid IdReport { get; set; }

        /**************campos agregados para validacion**********************/
        /// <summary>
        /// Codigo de la Encuesta
        /// </summary>
        [Display(Name = "Encuesta")]
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        public System.Guid IdSurvey { get; set; }


        /// <summary>
        /// Codigo del Tema
        /// </summary>
        [Display(Name = "Tema")]
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        public System.Guid IdTopic { get; set; }
        /**************************************/


        /// <summary>
        /// Objeto del modelo de Pregunta
        /// </summary>
        [Display(Name = "Pregunta")]
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        public string Question { get; set; }

        /// <summary>
        /// Tipo de Grafico (Barras, Pastel, sin grafico)
        /// </summary>
        [Display(Name = "Tipo de Gràfico")]
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        public string GraphicType { get; set; }

        /// <summary>
        /// Numero del Item
        /// </summary>
        [Display(Name = "Numero de Item")]
        [Required(ErrorMessage = " ¡El campo es obligatorio y debe ser Numerico!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
        [Range(1, double.MaxValue, ErrorMessage = "Tiene que ser un numero positivo")]
        public decimal ItemNumber { get; set; }

        /// <summary>
        /// Consulta SQL
        /// </summary>
        public string SQLQuey { get; set; }
        
        /// <summary>
        /// Onjeto Reportes del modelo
        /// </summary>
        public virtual Report Report { get; set; }
    }
}
