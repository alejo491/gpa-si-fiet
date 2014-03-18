using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIFIET.Models;
using SIFIET.Models.ViewModels;

namespace SIFIET.Controllers
{
    public class ItemsController : Controller
    {
        private DbSIEPISContext db = new DbSIEPISContext();

        //
        // GET: /Items/

        /*  public ViewResult Index()
          {
              var items = db.Items.Include(i => i.Report);
              return View(items.ToList());
          }*/

        #region Listado General de los Items de un Reporte
        /// <summary>
        /// Lista los items de un reporte, (Items de Encuesta y de Base de Datos)
        /// </summary>
        /// <param name="id">identificador del reporte al cual pertenecen los Items a listar</param>
        /// <returns></returns>
        public ActionResult GeneralItems(Guid? id)
        {
            if (id != Guid.Empty && id != null)
            {
                var ListGeneralItems = new List<ItemReportViewModel>();
                var reporte = (Report)db.Reports.Find(id);
                var itemsurveys = db.ItemSurveys.Where(i => i.IdReport == id);
                var itemdata = db.ItemDatas.Where(i => i.IdReport == id);
                var ListItemData = itemdata.ToList();
                var ListItemSurveys = itemsurveys.ToList();
                ViewBag.reporte = reporte; 

                foreach(ItemSurvey itemS in ListItemSurveys)
                {
                    ItemReportViewModel objReport = new ItemReportViewModel();
                    objReport.Id = itemS.Id;
                    objReport.IdReporte = itemS.IdReport;
                    objReport.Reporte = itemS.Report.Description;
                    objReport.GraphicType = itemS.GraphicType;
                    objReport.ItemNumber = Convert.ToInt32(itemS.ItemNumber);
                    objReport.Sentence = itemS.Question;
                    objReport.DataNumber = 0;
                    ListGeneralItems.Add(objReport);
                }

                foreach (ItemData itemD in ListItemData)
                {
                    ItemReportViewModel objReport = new ItemReportViewModel();
                    objReport.Id = itemD.Id;
                    objReport.IdReporte = itemD.IdReport;
                    objReport.Reporte = itemD.Report.Description;
                    objReport.GraphicType = itemD.GraphicType;
                    objReport.ItemNumber = Convert.ToInt32(itemD.ItemNumber);
                    objReport.Sentence = itemD.Sentence;
                    objReport.DataNumber = 1;
                    ListGeneralItems.Add(objReport);
                }
                ViewBag.IdReport = id;
                var Listfinal = ListGeneralItems.OrderBy(ItemReportViewModel => ItemReportViewModel.ItemNumber).ToList();
                return View(Listfinal);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion


        //public ActionResult Index(Guid? id)
        //{
        //    if (id != Guid.Empty && id != null)
        //    {

        //        var ite = (Report)db.Reports.Find(id);

        //        if (ite != null)
        //        {
        //            var itemList = db.Items.Where(s => s.IdReport == ite.Id);
        //            ViewBag.Topic = ite;
        //            return View(itemList.ToList());
        //        }
        //        else
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //}

        ////
        //// GET: /Items/Details/5

        //public ViewResult Details(Guid id)
        //{
        //    Item item = db.Items.Find(id);
        //    ViewBag.iddet = item.IdReport;
        //    return View(item);
        //}

        ////
        //// GET: /Items/Create


        //public ActionResult Create(Guid? id)
        //{
        //    if (id != Guid.Empty && id != null)
        //    {
        //        var auxIte = (Report)db.Reports.Find(id);

        //        if (auxIte != null)
        //        {
        //            ViewBag.Topic = auxIte;
        //            return View();
        //        }
        //        else
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //}

        ///* public ActionResult Create()
        // {
        //     ViewBag.IdReport = new SelectList(db.Reports, "Id", "Description");
        //     return View();
        // }*/

        ////
        //// POST: /Items/Create

        //[HttpPost]
        //public ActionResult Create(Item item, Guid? id)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        item.Id = Guid.NewGuid();
        //        item.IdReport = new Guid("" + id);
        //        db.Items.Add(item);
        //        db.SaveChanges();
        //        TempData["Create"] = "Se registró correctamente el item !";
        //        return RedirectToAction("Index", new { id = id });
        //    }

        //    ViewBag.IdReport = new SelectList(db.Reports, "Id", "Description", item.IdReport);
        //    return View(item);
        //}

        ////
        //// GET: /Items/Edit/5

        //public ActionResult Edit(Guid id, Guid? idit)
        //{
        //    Item item = db.Items.Find(idit);
        //    ViewBag.idrep = id;
        //    return View(item);
        //}

        ////
        //// POST: /Items/Edit/5

        //[HttpPost]
        //public ActionResult Edit(Guid id, Guid idit, Item item)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        item.Id = idit;
        //        item.IdReport = id;
        //        db.Entry(item).State = EntityState.Modified;

        //        db.SaveChanges();

        //        TempData["Update"] = "Se actualizó correctamente el item !";
        //        return RedirectToAction("Index", new { id = id });
        //    }
        //    ViewBag.IdReport = new SelectList(db.Reports, "Id", "Description", item.IdReport);
        //    return View(item);
        //}

        ////
        //// GET: /Items/Delete/5

        //public ActionResult Delete(Guid id)
        //{
        //    Item item = db.Items.Find(id);
        //    ViewBag.iddel = item.IdReport;
        //    return View(item);
        //}

        ////
        //// POST: /Items/Delete/5

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(Guid id)
        //{
        //    Item item = db.Items.Find(id);
        //    ViewBag.iddel = item.IdReport;
        //    db.Items.Remove(item);
        //    db.SaveChanges();
        //    TempData["Delete"] = "Se eliminó correctamente el item !";
        //    return RedirectToAction("Index", new { id = item.IdReport });
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}