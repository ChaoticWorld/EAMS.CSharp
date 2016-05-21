using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemBLL;

namespace MvcApp.Areas.Common
{
    public class RefInventoryController : Controller
    {
        public JsonResult referInventoryInfo(string id)
        {
            JsonResult r = new JsonResult();
            DataDB.u8.dbU8 dbu8 = new DataDB.u8.dbU8();
            IEnumerable<DataDB.ModelBase.Inventory> invs = dbu8.getInventorys(code: id);
            DataDB.ModelBase.Inventory inv = new DataDB.ModelBase.Inventory();
            if (invs != null && invs.Count() > 0)
                inv = invs.First();
            r = Json(inv, "application/json", JsonRequestBehavior.AllowGet);
            return r;
        }
        public ActionResult referInventory()
        {
            DataDB.u8.dbU8 dbu8 = new DataDB.u8.dbU8();
            //DataDB.ModelBase.InventoryClass ic = new DataDB.ModelBase.InventoryClass();
            List<DataDB.ModelBase.InventoryClass> Model = dbu8.getInventoryClassList();
            SelectList sl = new SelectList(Model, dataValueField: "code", dataTextField: "name");
            TempData["px"] = sl;
            return View();
        }
        //public ActionResult referInventory(string key)
        //{
        //    DataDB.u8.dbU8 dbu8 = new DataDB.u8.dbU8();
        //    //DataDB.ModelBase.InventoryClass ic = new DataDB.ModelBase.InventoryClass();
        //    List<DataDB.ModelBase.InventoryClass> Model = dbu8.getInventoryClassList();
        //    SelectList sl = new SelectList(Model, dataValueField: "code", dataTextField: "name");
        //    TempData["px"] = sl;
        //    return View();
        //}
        public ActionResult referInventoryPP(string id)
        {
            string px = id;
            DataDB.u8.dbU8 dbu8 = new DataDB.u8.dbU8();
            //DataDB.ModelBase.InventoryClass ic = new DataDB.ModelBase.InventoryClass();
            DataDB.ModelBase.InventoryClass Model = dbu8.getInventoryClass(px);
            List<DataDB.ModelBase.InventoryClass> pp = dbu8.getInventoryClassList(Model.subClsCodes);
            var r = Json(pp, "application/json", JsonRequestBehavior.AllowGet);

            return r;
        }
        public ActionResult referInventorys(string id)
        {
            string pp = id;
            DataDB.u8.dbU8 dbu8 = new DataDB.u8.dbU8();
            //DataDB.ModelBase.InventoryClass ic = new DataDB.ModelBase.InventoryClass();
            List<DataDB.ModelBase.Inventory> Model = dbu8.getInventorys(ccode: pp);
            var r = Json(Model, "application/json", JsonRequestBehavior.AllowGet);

            return r;
        }
        public ActionResult referSearchInventorys(string id)
        {
            string key = id;
            key = key.Replace(" ", "%");
            DataDB.u8.dbU8 dbu8 = new DataDB.u8.dbU8();
            //DataDB.ModelBase.InventoryClass ic = new DataDB.ModelBase.InventoryClass();
            List<DataDB.ModelBase.Inventory> Model1 = dbu8.getInventorys(ccode: key);
            List<DataDB.ModelBase.Inventory> Model2 = dbu8.getInventorys(code: key);
            List<DataDB.ModelBase.Inventory> Model3 = dbu8.getInventorys(name: key);
            List<DataDB.ModelBase.Inventory> Model4 = dbu8.getInventorys(std: key);
            List<DataDB.ModelBase.Inventory> Model = new List<DataDB.ModelBase.Inventory>();
            if (Model1.Count > 0) Model.AddRange(Model1);
            if (Model2.Count > 0) Model.AddRange(Model2);
            if (Model3.Count > 0) Model.AddRange(Model3);
            if (Model4.Count > 0) Model.AddRange(Model4);
            var r = Json(Model, "application/json", JsonRequestBehavior.AllowGet);

            return r;
        }


    }
}
