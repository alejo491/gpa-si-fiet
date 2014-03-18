using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SIFIET.Models
{
    /// <summary>
    /// Clase que representa un Item
    /// </summary>
    public partial class Item
    {
        /// <summary>
        /// Codigo del Item
        /// </summary>
        public System.Guid Id { get; set; }
        /// <summary>
        /// Codigo del Reporte
        /// </summary>
        public System.Guid IdReport { get; set; }

        /// <summary>
        /// Nombre de la Tabla
        /// </summary>
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        public string TableName { get; set; }

        /// <summary>
        /// Nombre del Campo
        /// </summary>
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        public string FieldName { get; set; }

        /// <summary>
        /// Numero de Pagina
        /// </summary>
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
        public decimal PageNumber { get; set; }

        /// <summary>
        /// Objeto del Modelo Reporte
        /// </summary>
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        public virtual Report Report { get; set; }
    }
}
