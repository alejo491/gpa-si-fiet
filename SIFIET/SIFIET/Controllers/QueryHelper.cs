using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using SIFIET.Models.ViewModels;

namespace SIFIET.Controllers
{
    /// <summary>
    /// Helper para realizar consultas manuales a la base de datos.
    /// </summary>
    public static class QueryHelper
    {

        #region Codigo, para generar tabla y grafico (chart pie) de los reportes

        /// <summary>
        /// Metodo que obtiene un dataset a partir de una consula
        /// </summary>
        /// <param name="sqlCommand">Comando SQL a ejecutar</param>
        /// <returns>Dataset con los resultados de la consulta</returns>
        public static DataSet GetDataSet(string sqlCommand)
        {
            
            DataSet ds = new DataSet();
            var strconn = ConfigurationManager.ConnectionStrings["DbSIEPISContext"].ToString();
            var myCon = new SqlConnection(strconn);
            myCon.Open();
            var myAda = new SqlDataAdapter(sqlCommand, myCon);
            myAda.Fill(ds);
            myCon.Close();
 
            return ds;
        }
    }

    /// <summary>
    /// Clase que convierte el dataset a xml
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Metodo que convierte el dataset a XML 
        /// </summary>
        /// <param name="ds">Dataset a convertir</param>
        /// <returns>String con el XML</returns>
        public static string ToXml(this DataSet ds)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(DataSet));
                    xmlSerializer.Serialize(streamWriter, ds);
                    return Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
        }
    }

    #endregion
    
}
