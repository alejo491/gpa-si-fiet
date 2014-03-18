using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIFIET.Models;

namespace SIFIET.Controllers
{

    /// <summary>
    /// Controlador para crear items a partir de datos 
    /// </summary>
    public class ItemDataController : Controller
    {
        //
        // GET: /ItemData/
        
        /// <summary>
        /// Atributo que consulta la base de datos
        /// </summary>
        public readonly DbSIEPISContext db = new DbSIEPISContext();


        /// <summary>
        /// Metodo que crea un item de datos
        /// </summary>
        /// <param name="id">Id del reporte al que estara asociado el item</param>
        /// <returns>Retorna la vista de creacion del item</returns>
        public ActionResult Create(Guid id)
        {
            return View();
        }

        #region Eliminar Item de Base de Datos

        //
        // GET: /itemData/Delete/5
        /// <summary>
        /// Da la opcion de eliminar Un item de Base de Datos
        /// </summary>
        /// <param name="id">Identificador del item de Base de datos a eliminar</param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            ItemData itemdata = db.ItemDatas.Find(id);
            return View(itemdata);
        }
        #endregion
       
        /// <summary>
        /// Metodo post de la vista de crear item
        /// </summary>
        /// <param name="id">Id del reporte al que estara asociado el item</param>
        /// <param name="form">Valores que devuelve el formulario</param>
        /// <returns>Una reidreccion al listado de reportes si el item es correcto, o a la misma vista si no lo es</returns>
        [HttpPost]
        public ActionResult Create(Guid  id, FormCollection form)
        {
            ItemData item = new ItemData();
            item.Id = Guid.NewGuid();
            item.IdReport = id;
            item.SQLQuey = "SELECT DISTINCT * FROM Questions";
            item.GraphicType = "Ninguno";
            item.ItemNumber = 1;
            item.Sentence = "Hola";
            //db.ItemDatas.Add(item);
            //db.SaveChanges();
            var ListCampos = new List<Field>();
            var ListFiltros = new List<SIFIET.Models.Filter>();
            var ListGrupos = new List<GroupOption>();
            var Lcamposgroup = new List<Field>();

            var sL = new List<Field>();
            var sF = new List<SIFIET.Models.Filter>();
            var sG = new List<GroupOption>();
            
            int bandsql = 0;
            int bandcamp = 0;
            int bandfilt = 0;
            int bandAll = 0;
           // int bandCountAll = 0;
            int band = 0;
            string auxlogic = "";
            string auxsqlcampos = "";
            string auxsqlfiltros = "";
            string auxsqlgrupos = "";
            string SQL = "";
            foreach (String key in form)
            {
                var k = form[key];
                if (key == "AllFields" && k == "true,false")
                {
                   // SQL = "select * from ConsultaGeneral";
                    auxsqlcampos = "SELECT DISTINCT * FROM ConsultaGeneral ";
                    bandAll = 1;
                    bandsql++;
                }

              /*  if (key == "CountFields" && k == "true,false")
                {
                    //SQL = "select count(*) from ConsultaGeneral";
                    auxsqlcampos = "SELECT COUNT(*) FROM ConsultaGeneral ";
                    bandCountAll = 1;
                    bandsql++;
                }*/

                if (key.StartsWith("field"))
                {
                    if (key.StartsWith("field") && (k.Length != 0) && k != "Año" && k != "Mes" && k != "Dia" && k != "Suma" && k != "Minimo" && k != "Maximo" && k != "Contar" && k != "Promedio")
                    {
                        Field objcampo = new Field();
                        objcampo.Id = Guid.NewGuid();
                        objcampo.IdItemData = item.Id;
                        bandcamp = 1;
                        objcampo.FieldName = k;
                        ListCampos.Add(objcampo);
                        bandsql++;
                    }

                    if (key.StartsWith("fieldaction") && bandcamp == 1)
                    {
                        if ((k.Length == 0))
                        {
                            ListCampos.Last().FieldOperation = "";
                        }
                        if (k == "Suma")
                        {
                            ListCampos.Last().FieldOperation = "SUM";
                        }
                        if (k == "Minimo")
                        {
                            ListCampos.Last().FieldOperation = "MIN";
                        }
                        if (k == "Maximo")
                        {
                            ListCampos.Last().FieldOperation = "MAX";
                        }
                        if (k == "Contar")
                        {
                            ListCampos.Last().FieldOperation = "COUNT";
                        }
                        if (k == "Promedio")
                        {
                            ListCampos.Last().FieldOperation = "AVG";
                        }
                        if (k == "Año")
                        {
                            ListCampos.Last().FieldOperation = "YEAR";
                        }
                        if (k == "Mes")
                        {
                            ListCampos.Last().FieldOperation = "MONTH";
                        }
                        if (k == "Dia")
                        {
                            ListCampos.Last().FieldOperation = "DAY";
                        }
                        bandcamp = 0;
                        sL.Add(ListCampos.Last());
                        //db.SaveChanges();
                    }

                }

                if (key.StartsWith("searchfield") && (k.Length != 0))
                {
                    if (ListFiltros.Count == 0)
                    {
                        SIFIET.Models.Filter objfiltro = new SIFIET.Models.Filter();
                        objfiltro.Id = Guid.NewGuid();
                        objfiltro.IdItemData = item.Id;
                        bandfilt = 1;
                        objfiltro.FieldName = k;
                        objfiltro.LogicOperator = "WHERE";
                        ListFiltros.Add(objfiltro);
                        if (auxlogic != "")
                        {
                            auxlogic = "";
                        }
                    }
                    if(auxlogic != "Condicion" && auxlogic != "")
                    {
                        SIFIET.Models.Filter objfiltro = new SIFIET.Models.Filter();
                        objfiltro.Id = Guid.NewGuid();
                        objfiltro.IdItemData = item.Id;
                        bandfilt = 1;
                        if (ListFiltros.Count == 0)
                        {
                            objfiltro.LogicOperator = "WHERE";
                        }
                        else
                        {
                            objfiltro.LogicOperator = auxlogic;
                        }
                        objfiltro.FieldName = k;
                        ListFiltros.Add(objfiltro);
                        auxlogic = "";
                    }
                  
                }

                if (key.StartsWith("operator") && bandfilt == 1)
                {
                    if(k.Length == 0)
                    {
                        bandfilt = 0;
                        ListFiltros.RemoveAt(ListFiltros.Count-1);
                    }
                    if (k == "Mayor_o_igual_a")
                    {
                        ListFiltros.Last().Operator = ">=";
                    }
                    if (k == "Menor_o_igual_a")
                    {
                        ListFiltros.Last().Operator = "<=";
                    }
                    if (k == "Mayor_que")
                    {
                        ListFiltros.Last().Operator = ">";
                    }
                    if (k == "Menor_que")
                    {
                        ListFiltros.Last().Operator = "<";
                    }
                    if (k == "Igual_a")
                    {
                        ListFiltros.Last().Operator = "=";
                    }
                    if (k == "Diferente_de")
                    {
                        ListFiltros.Last().Operator = "<>";
                    }
                    if (k == "Like")
                    {
                        ListFiltros.Last().Operator = "LIKE";
                    }
                }

                if (key.StartsWith("criteria") && bandfilt == 1)
                {
                    if(k == "")
                    {
                        ListFiltros.RemoveAt(ListFiltros.Count - 1);
                        bandfilt = 0;
                    }
                    else
                    {
                        ListFiltros.Last().Value = k;
                        bandfilt = 0;
                        bandsql++;
                        sF.Add(ListFiltros.Last());
                        //db.SaveChanges();
                    }
                    
                }
                if (key.StartsWith("logic"))
                {
                    auxlogic = k;
                }
                if (key.StartsWith("group") && (k.Length != 0))
                {
                    GroupOption objgrupo = new GroupOption();
                    objgrupo.Id = Guid.NewGuid();
                    objgrupo.IdItemData = item.Id;
                    objgrupo.FieldName = k;
                    ListGrupos.Add(objgrupo);
                    bandsql++;
                    sG.Add(objgrupo);
                    //db.SaveChanges();
                }

            }

            foreach (SIFIET.Models.Filter filtro in ListFiltros)
            {
                auxsqlfiltros = auxsqlfiltros + filtro.LogicOperator + " " + filtro.FieldName + " " + filtro.Operator + " ";
                    if(filtro.Operator == "LIKE" || filtro.FieldName.StartsWith("Fecha"))
                    {
                        auxsqlfiltros = auxsqlfiltros + "'" + filtro.Value + "'" + " ";
                    }
                    else
                    {
                        auxsqlfiltros = auxsqlfiltros + filtro.Value + " ";
                    }
            }

            if (bandAll == 0)
            {
                foreach (GroupOption grupo in ListGrupos)
                {
                    if (auxsqlgrupos == "")
                    {
                        auxsqlgrupos = "GROUP BY ";
                    }
                    auxsqlgrupos = auxsqlgrupos + grupo.FieldName;
                    if (grupo.Id != ListGrupos.Last().Id)
                    {
                        auxsqlgrupos = auxsqlgrupos + "," + " ";
                    }
                }
            }

            if (bandsql == 0)
            {
                auxsqlcampos = "SELECT DISTINCT * FROM ConsultaGeneral ";
            }
            else
            {
                if (ListGrupos.Count == 0 && ListCampos.Count > 0 && bandAll == 0)
                {
                    auxsqlcampos = "SELECT DISTINCT ";
                    foreach (Field field in ListCampos)
                    {
                        if (field.FieldOperation != "")
                        {
                            band = band + 1;
                        }
                    }

                    if (band != 0)
                    {
                        foreach (Field field in ListCampos)
                        {
                            if (field.FieldOperation != "")
                            {
                                Lcamposgroup.Add(field);
                            }
                        }
                        foreach (Field campo in Lcamposgroup)
                        {
                            auxsqlcampos = auxsqlcampos + campo.FieldOperation + "(" + campo.FieldName + ")";
                            if (campo.Id != Lcamposgroup.Last().Id)
                            {
                                auxsqlcampos = auxsqlcampos + ", ";
                            }
                            else
                            {
                                auxsqlcampos = auxsqlcampos + " FROM ConsultaGeneral ";
                            }
                        }
                    }
                    if (band == 0)
                    {
                        foreach (Field campo in ListCampos)
                        {
                            auxsqlcampos = auxsqlcampos + campo.FieldName;
                            if (campo.Id != ListCampos.Last().Id)
                            {
                                auxsqlcampos = auxsqlcampos + ", ";
                            }
                            else
                            {
                                auxsqlcampos = auxsqlcampos + " FROM ConsultaGeneral ";
                            }
                        }
                    }
                }

                if (ListGrupos.Count > 0 && ListCampos.Count > 0 && bandAll == 0 )
                {
                    foreach (Field nuevofield in ListCampos)
                    {
                        foreach (GroupOption nuevogroup in ListGrupos)
                        {

                            if (nuevofield.FieldName == nuevogroup.FieldName )
                            {
                                Lcamposgroup.Add(nuevofield);
                            }
                            if (nuevofield.FieldName != nuevogroup.FieldName && nuevofield.FieldOperation != "")
                            {
                                Lcamposgroup.Add(nuevofield);
                            }
                        }
                    }
                    Lcamposgroup = Lcamposgroup.Distinct().ToList();
                    foreach (Field campo in Lcamposgroup)
                    {
                        if (campo.Id == Lcamposgroup.First().Id)
                        {
                            auxsqlcampos = "SELECT DISTINCT ";
                        }
                        if (campo.FieldOperation != "")
                        {
                            auxsqlcampos = auxsqlcampos + campo.FieldOperation + "(" + campo.FieldName + ")";
                        }
                        else
                        {
                            auxsqlcampos = auxsqlcampos + campo.FieldName;
                        }

                        if (campo.Id != Lcamposgroup.Last().Id)
                        {
                            auxsqlcampos = auxsqlcampos + ", ";
                        }
                        else
                        {
                            auxsqlcampos = auxsqlcampos + " FROM ConsultaGeneral ";
                        }
                    }
                    if (Lcamposgroup.Count == 0)
                    {
                        auxsqlcampos = "SELECT DISTINCT * FROM ConsultaGeneral ";
                        auxsqlgrupos = "";
                    }
                }                    
            }           

            SQL = auxsqlcampos + auxsqlfiltros + auxsqlgrupos;
            item.SQLQuey = SQL;
            item.GraphicType = form["GraphicType"];
            item.ItemNumber = int.Parse(form["ItemNumber"]);
            item.Sentence = form["Sentence"];
            try
            {
                var ds = QueryHelper.GetDataSet(SQL);
                var dt = ds.ToXml();
                db.ItemDatas.Add(item);
                db.SaveChanges();
                foreach (var field in sL)
                {
                    field.IdItemData = item.Id;
                    db.Fields.Add(field);

                }
                db.SaveChanges();

                foreach (var filter in sF)
                {
                    filter.IdItemData = item.Id;
                    db.Filters.Add(filter);

                }
                db.SaveChanges();

                foreach (var groupOption in sG)
                {
                    groupOption.IdItemData = item.Id;
                    db.GroupOptions.Add(groupOption);

                }

                db.SaveChanges();
                TempData["Success"] = "Se ha creado el Item correctamente";
                return RedirectToAction("GeneralItems", "Items", new { id = item.IdReport });
            }
            catch(Exception e)
            {
                TempData["error"] = "Criterio de consulta erroneos!";
                return View();
            }
              

        }

        #region Eliminar Item de Base de Datos HttpPost
        //
        // POST: /itemdata/Delete/5
        /// <summary>
        /// Elimina el Item de Base de Datos al que corresponda el identificador
        /// </summary>
        /// <param name="id">Identificador del Item de Base de Datos a eliminar</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ItemData itemdata = db.ItemDatas.Find(id);
            db.ItemDatas.Remove(itemdata);
            db.SaveChanges();
            TempData["Success"] = "Se ha Eliminado el Item correctamente";
            return RedirectToAction("GeneralItems", "Items", new { id = itemdata.IdReport });
        }
        #endregion

        #region Ajax

        /// <summary>
        /// Metodo que retorna el listado de operadores logicos
        /// </summary>
        /// <returns>Listado de operadores logicos</returns>
        public JsonResult ListLogic()
        {
            List<String> list = new List<string>();
            list.Add("AND");
            list.Add("OR");
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }     

        /// <summary>
        /// Retorna lista de funciones deacuerdo al tipo de dato de un campo
        /// </summary>
        /// <param name="field">Nombre del campo</param>
        /// <param name="campo">Valor del campo</param>
        /// <returns></returns>
        public JsonResult ListActions(string field, string campo)
        {

            var assembly = new AssemblyHelper();
            var result = assembly.GetFieldsType();
            Dictionary<string, string> d;
            if (Session["Dictionary"] == null)
            {
                d = new Dictionary<string, string>();
                Session["Dictionary"] = d;

            }
            else
            {
                d = (Dictionary<string, string>)Session["Dictionary"];
            }

            List<String> list = new List<string>();
            if (field != "Seleccione una opcion")
            {

                if (result[field] == "System.decimal" || result[field] == "Decimal" || result[field] == "decimal")
                {

                    list.Add("Suma");
                    list.Add("Minimo");
                    list.Add("Maximo");
                    list.Add("Contar");
                    list.Add("Promedio");
                }
                else
                {
                    var cadena = "System.Nullable`1[System.DateTime]".Substring(18, 15);
                    if (result[field] == @"System.DateTime")
                    {
                        list.Add("Año");
                        list.Add("Mes");
                        list.Add("Dia");
                        list.Add("Contar");
                    }
                    else
                    {
                        if (result[field].Length >= 15)
                        {
                            if (result[field].Substring(18, 15) == cadena)
                            {
                                list.Add("Año");
                                list.Add("Mes");
                                list.Add("Dia");
                                list.Add("Contar");
                            }
                            else
                            {
                                list.Add("Contar");
                            }
                        }
                        else
                        {
                            list.Add("Contar");
                        }
                    }

                }

                if (d.ContainsKey(campo))
                {
                    d[campo] = field;
                    Session["Dictionary"] = d;
                }
                else
                {
                    d.Add(campo, field);
                    Session["Dictionary"] = d;
                }
            }
            else
            {
                if (d.ContainsKey(campo))
                {
                    d.Remove(campo);
                    Session["Dictionary"] = d;
                }
            }
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retorna la lista de operadores segun sea el tipo de dato de un campo
        /// </summary>
        /// <param name="field">Nombre del campo</param>
        /// <param name="campo">Valor del campo</param>
        /// <returns>Retorna la lista de operadores</returns>
        public JsonResult ListOperators(string field, string campo)
        {
            List<String> list = new List<string>();
            var assembly = new AssemblyHelper();
            var result = assembly.GetFieldsType();
            if (result[field] == "System.string" || result[field] == "string" || result[field] == "System.String" || result[field] == "String")
            {
                list.Add("Like");
            }
            else
            {
                list.Add(@"Mayor_o_igual_a");
                list.Add(@"Menor_o_igual_a");
                list.Add(@"Mayor_que");
                list.Add(@"Menor_que");
                list.Add(@"Igual_a");
                list.Add(@"Diferente_de");
            }

            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Metodo que lista los campos que se van a mostrar en la busqueda
        /// </summary>
        /// <returns>Lista de los campos que se van a mostrar en la busqueda</returns>
        public JsonResult ListSearchFields()
        {
            var list = new List<string>();
            var assembly = new AssemblyHelper();
            var result = assembly.GetFieldsType();
            foreach (var key in result)
            {
                list.Add(key.Key);
            }
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lista los campos que se van a mostrar en la opcion de grupos
        /// </summary>
        /// <returns>Lista de campos</returns>
        public JsonResult ListGroupFields()
        {
            var list = new List<string>();
            var assembly = new AssemblyHelper();
            var result = assembly.GetFieldsType();
            foreach (var key in result)
            {
                list.Add(key.Key);
            }

            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Lista todos los campos que se pueden mostrar en la parte de select
        /// </summary>
        /// <returns>Lista de todos los campos</returns>
        public JsonResult ListFields()
        {
            var assembly = new AssemblyHelper();
            var result = assembly.GetFieldsType();
            var list = new List<string>();
            foreach (var opc in result)
            {
                list.Add(opc.Key);
            }

            /*list.Add("Campo1");
            list.Add("Campo2");
            list.Add("Campo3");
            list.Add("Campo4");*/
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        #endregion



    }



}
