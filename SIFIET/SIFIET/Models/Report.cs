using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIFIET.Models
{
    /// <summary>
    /// Clase que representa un reporte
    /// </summary>
    public partial class Report
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public Report()
        {
            this.ItemDatas = new List<ItemData>();
            this.Items = new List<Item>();
            this.ItemSurveys = new List<ItemSurvey>();
        }

        /// <summary>
        /// Codigo del reporte
        /// </summary>
        public System.Guid Id { get; set; }
        
        /// <summary>
        /// Codigo del usuario que genero el reporte
        /// </summary>
        public System.Guid IdUser { get; set; }

        /// <summary>
        /// Descripcion del reporte
        /// </summary>
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        public string Description { get; set; }

        /// <summary>
        /// Fecha del reporte
        /// </summary>
        public System.DateTime ReportDate { get; set; }
        
        /// <summary>
        /// Conjunto de item de datos
        /// </summary>
        public virtual ICollection<ItemData> ItemDatas { get; set; }

        /// <summary>
        /// Conjunto de items
        /// </summary>
        public virtual ICollection<Item> Items { get; set; }
        /// <summary>
        /// Conjunto de items de encuestas
        /// </summary>
        public virtual ICollection<ItemSurvey> ItemSurveys { get; set; }
        /// <summary>
        /// Usuario del reporte
        /// </summary>
        public virtual User User { get; set; }
    }
}
