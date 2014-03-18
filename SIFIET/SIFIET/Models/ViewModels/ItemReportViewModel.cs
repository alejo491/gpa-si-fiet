using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SIFIET.Models.ViewModels
{
    /// <summary>
    /// Modelo de vista para mostrar reportes
    /// </summary>
    public class ItemReportViewModel
    {
        /// <summary>
        /// Codigo del item
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Codigo del reporte
        /// </summary>
        public Guid IdReporte { get; set; }

        /// <summary>
        /// Enunciado del reporte
        /// </summary>
        public string Reporte { get; set; }   
     
        /// <summary>
        /// Enunciado del item
        /// </summary>
        public string Sentence { get; set; }

        /// <summary>
        /// Tipo de grafico del item
        /// </summary>
        public string GraphicType { get; set; }

        /// <summary>
        /// Numero del item
        /// </summary>
        public int ItemNumber { get; set; }

        /// <summary>
        /// Lista de datos del item
        /// </summary>
        public List<Dictionary<string, int>> DataList { get; set; }

        /// <summary>
        /// Numero de datos en el item
        /// </summary>
        public int DataNumber { get; set; }

        /// <summary>
        /// Tipo de item
        /// </summary>
        public bool Type { get; set; }

        /// <summary>
        /// Datos de la consulta
        /// </summary>
        public DataTable Table { get; set; }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public ItemReportViewModel()
        {
            Sentence = "";
            GraphicType = "";
            ItemNumber = 0;
            DataNumber = 0;
            DataList = new List<Dictionary<string, int>>();
            Table = new DataTable();
        }
    }
}