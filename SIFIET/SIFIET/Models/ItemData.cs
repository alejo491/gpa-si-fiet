using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIFIET.Models
{
    /// <summary>
    /// Almacena los items que traen informacion de los usuarios
    /// </summary>
    public partial class ItemData
    {
        /// <summary>
        /// Constructor de la Clase Item Data
        /// </summary>
        public ItemData()
        {
            this.Fields = new List<Field>();
            this.Filters = new List<Filter>();
            this.GroupOptions = new List<GroupOption>();
        }

        /// <summary>
        /// Codigo del ItemData
        /// </summary>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Codigo del Reporte
        /// </summary>
        public System.Guid IdReport { get; set; }
        
        /// <summary>
        /// Enunciado del item
        /// </summary>
        [Display(Name = "Enunciado")]
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        [MinLength(1, ErrorMessage = "No pueder tener mas de 300 caracteres")]
        public string Sentence { get; set; }


        /// <summary>
        /// Consulta SQL Armada
        /// </summary>
        public string SQLQuey { get; set; }

        [Display(Name = "Tipo de Gráfico")]
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
        /// Coleccion de Campos del item
        /// </summary>
        public virtual ICollection<Field> Fields { get; set; }

        /// <summary>
        /// Coleccion de Filtros
        /// </summary>
        public virtual ICollection<Filter> Filters { get; set; }
        
        /// <summary>
        /// Coleccion de objetos de Opciones de Grupo
        /// </summary>
        public virtual ICollection<GroupOption> GroupOptions { get; set; }
       
        /// <summary>
        /// Objeto del Reporte
        /// </summary>
        public virtual Report Report { get; set; }
    }
}
