using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml.Serialization;
using SIFIET.Models;
using SIFIET.Models.ViewModels;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using iTextSharp.text.html.simpleparser;

namespace SIFIET.Controllers
{
    /// <summary>
    /// Controlador de reportes
    /// </summary>
    public class ReportsController : Controller
    {
        private DbSIEPISContext db = new DbSIEPISContext();
   
        /// <summary>
        /// Carga el listado de reportes
        /// </summary>
        /// <returns>Vista con el listado de reportes</returns>
        public ViewResult Index()
        {
            var te = db.aspnet_Users.First(u => u.UserName == HttpContext.User.Identity.Name).UserId;
            var reports = db.Reports.Where(r => r.IdUser == te);
            return View(reports.ToList());
        }


        /// <summary>
        /// Permite ver en detalle 
        /// </summary>
        /// <param name="id">Codigo del reporte</param>
        /// <returns>Vista con los detalles del reporte</returns>
        public ViewResult Details(Guid id)
        {
            Report report = db.Reports.Find(id);
            var user = db.Users.Find(report.IdUser);

            // ViewBag.Re = id;

            ViewBag.nombre = user.FirstNames + " " + user.LastNames;

            return View(report);
        }

       
        /// <summary>
        /// Permite crear un reporte nuevo
        /// </summary>
        /// <returns>Vista para crear el reporte</returns>
        public ActionResult Create()
        {
            //ViewBag.IdUser = new SelectList(db.Users, "Id", "PhoneNumber");
            return View();
        }

       
        /// <summary>
        ///  Metodo Httpost de crear un reporte nuevo
        /// </summary>
        /// <param name="report"></param>
        /// <returns>Retorna el listado de reportes si no hay errores, si los hay devuelve la misma vista.</returns>
        [HttpPost]
        public ActionResult Create(Report report)
        {
            if (ModelState.IsValid)
            {
                report.IdUser = db.aspnet_Users.First(u => u.UserName == HttpContext.User.Identity.Name).UserId;
                report.ReportDate = DateTime.Now;
                report.Id = Guid.NewGuid();
                db.Reports.Add(report);
                db.SaveChanges();
                TempData["Create"] = "Se registró correctamente el reporte !";
                return RedirectToAction("Index");
            }

            return View(report);
        }

        
        /// <summary>
        /// Permite editar un reporte 
        /// </summary>
        /// <param name="id">Codigo del reporte</param>
        /// <returns>La vista para crear un reporte</returns>
        public ActionResult Edit(Guid id)
        {
            Report report = db.Reports.Find(id);

            return View(report);
        }


        /// <summary>
        ///  Metodo Httpost de editar reporte
        /// </summary>
        /// <param name="report">Objeto de tipo de reporte que trae los elementos de la vista</param>
        /// <returns>Retorna el listado de reportes si no hay errores, si los hay devuelve la misma vista.</returns>
        [HttpPost]
        public ActionResult Edit(Report report)
        {
            if (ModelState.IsValid)
            {
                report.IdUser = db.aspnet_Users.First(u => u.UserName == HttpContext.User.Identity.Name).UserId;
                report.ReportDate = DateTime.Now;
                db.Entry(report).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Update"] = "Se actualizó correctamente el reporte !";
                return RedirectToAction("Index");
            }
            ViewBag.IdUser = new SelectList(db.Users, "Id", "PhoneNumber", report.IdUser);
            return View(report);
        }

        
        /// <summary>
        /// Metodo para eliminar un reporte
        /// </summary>
        /// <param name="id">Codigo del reporte</param>
        /// <returns>La vista para eliminar un reporte</returns>
        public ActionResult Delete(Guid id)
        {
            Report report = db.Reports.Find(id);
            return View(report);
        }


        /// <summary>
        /// Metodo para confirmar la  eliminacion de un reporte
        /// </summary>
        /// <param name="id">Codigo del reporte</param>
        /// <returns>Retorna el listado de reportes</returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Report report = db.Reports.Find(id);
            db.Reports.Remove(report);
            db.SaveChanges();
            TempData["Delete"] = "Se eliminó correctamente el reporte !";
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Metodo dispose
        /// </summary>
        /// <param name="disposing">Bandera</param>
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Metodo que pasa los parametros para renderizar el reporte
        /// </summary>
        /// <param name="id">Codigo del reporte</param>
        /// <returns>Vista </returns>
        public ActionResult Preview(Guid id)
        {
            ViewBag.IdReport = id;
            return View();
        }
      
        /// <summary>
        /// Metodo que renderiza la vista para ver un reporte graficamente.
        /// </summary>
        /// <param name="id">Codigo del reporte que se va  mostrar</param>
        /// <returns>Una vista parcial que se renderiza dentro de la vista de preview</returns>
        public ActionResult RenderReport(Guid id)
        {
            var items = new List<ItemReportViewModel>();
            var report = db.Reports.Find(id);
            ViewBag.ReportName = report.Description;
            ViewBag.ReportDate = report.ReportDate;

            if (report != null)
            {
                var itemsurveys = db.ItemSurveys.Where(item => item.IdReport == id).ToList();
                foreach (var itemsurvey in itemsurveys)
                {
                    string selectquery = itemsurvey.SQLQuey;
                    var dataa = QueryHelper.GetDataSet(selectquery);
                    var dataSet = dataa.ToXml();
                    var ds = new DataSet();
                    var stream = new StringReader(dataSet);
                    ds.ReadXml(stream);
                    DataTable dt = ds.Tables[0];
                    var item = new ItemReportViewModel();
                    var d = new Dictionary<string, int>();
                    int n = 0;
                    //int acum = dt.Rows.Cast<DataRow>().Sum(auxRow => Int32.Parse(auxRow.ItemArray[1].ToString()));
                    foreach (DataRow row in dt.Rows)
                    {
                        int temp = Int32.Parse(row.ItemArray[1].ToString());
                        d.Add(row.ItemArray[0].ToString(), temp);
                        n += temp;
                    }
                    item.DataNumber = n;
                    item.GraphicType = itemsurvey.GraphicType;
                    item.ItemNumber = (int) itemsurvey.ItemNumber;
                    item.Sentence = itemsurvey.Question;
                    item.Type = true;
                    item.Table = dt;
                    item.DataList.Add(d);
                    items.Add(item);
                }

                var itemsDatas = db.ItemDatas.Where(it => it.IdReport == id).ToList();
                foreach (var itemsData in itemsDatas)
                {
                    string selectquery = itemsData.SQLQuey;
                    var dataa = QueryHelper.GetDataSet(selectquery);
                    var dataSet = dataa.ToXml();
                    var ds = new DataSet();
                    var stream = new StringReader(dataSet);
                    ds.ReadXml(stream);
                    DataTable dt = ds.Tables[0];
                    var item = new ItemReportViewModel();
                    var d = new Dictionary<string, int>();
                    //int n = 0;
                    var labels = new List<string>();
                    var vals = new List<List<int>>();
                    if (dt.Rows.Count >= 2)
                    {
                        int m = dt.Rows.Count;
                        int n = dt.Rows[0].ItemArray.Length;
                        var valores = new int[m,n];
                        var fila = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            int indice = 0;
                            string label = "";

                            foreach (var ob in row.ItemArray)
                            {

                                int value = 0;
                                if (int.TryParse(ob.ToString(), out value))
                                {
                                    valores[fila, indice] = value;
                                    indice++;
                                }
                                else
                                {
                                    label += ob.ToString() + " ";
                                }
                                
                            }
                            fila++;
                            labels.Add(label);
                        }


                        var l = new List<Dictionary<string, int>>();

                       // foreach (var label in labels)
                        //{
                            for (int i = 0; i < n; i++)
                            {
                                var d1 = new Dictionary<string, int>();
                                for (int j = 0; j < m; j++)
                                {
                                    d1.Add(labels[j], valores[j, i]);

                                }
                                l.Add(d1);
                            }
                        //}

                        item.DataNumber = 0;
                        item.GraphicType = itemsData.GraphicType;
                        item.ItemNumber = (int) itemsData.ItemNumber;
                        item.Sentence = itemsData.Sentence;
                        item.DataList = l;
                        item.Type = false;
                        item.Table = dt;

                    }
                    else
                    {
                        item.DataNumber = 0;
                        item.GraphicType = itemsData.GraphicType;
                        item.ItemNumber = (int) itemsData.ItemNumber;
                        item.Sentence = itemsData.Sentence;
                        item.DataList = new List<Dictionary<string, int>>();
                        item.Type = false;
                        item.Table = dt;
                    }

                    items.Add(item);
                }

            }

            var il = items.OrderBy(ite => ite.ItemNumber).ToList();
            ViewBag.Items = il;
            /*var itemSurvey = db.ItemSurveys.Find(id);
            
            string selectquery = itemSurvey.SQLQuey;
            var dataa = GetDataSet(selectquery);            
            var dataSet = dataa.ToXml();
            var ds = new DataSet();
            var stream = new StringReader(dataSet);
            ds.ReadXml(stream);
            DataTable dt = ds.Tables[0];*/
            //Dictionary<string,int> d = new Dictionary<string,int>();

            /*foreach (DataRow row in dt.Rows)
            {
                d.Add(row.ItemArray[0].ToString(),Int32.Parse(row.ItemArray[1].ToString()));
            }
            foreach (var i in d)
            {
                KeyValuePair<string, int> k = i;
            }*/
            /*ViewBag.Question = itemSurvey.Question;
            ViewBag.graphicsvalue = d;
            ViewBag.TipoGrafico = itemSurvey.GraphicType;
            
            // dataSet.ReadXmlSchema(Server.MapPath("data.xsd"));
            //dataSet.ReadXml(Server.MapPath("data.xml"));
            ViewBag.datos = dataSet;*/
            return View();
        }
   

    }
}