using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataModel;
using Logs;
using MvcApp.Areas.Common.Models;

namespace MvcApp.Areas.Common
{
    public class RefInventoryController : Controller
    {
        private static ExtRef extRef;
        private LogsDataAccess logDA = new SysLogsDataAccess();

        public RefInventoryController()
        { extRef = new ExtRef("u8"); }

        public JsonResult referInventoryInfo(string id)
        {
            JsonResult r = new JsonResult();
            Inventory inventory = new Inventory();
            inventory = extRef.extInventory.getSingle(id);
            logDA.createLog(new LogModel() { cParams = id, cFunction = "referInventoryInfo", cModule = "RefInventory", cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(inventory) });
            r = Json(inventory, "application/json", JsonRequestBehavior.AllowGet);
            return r;
        }
        public ActionResult referInventory()
        {
            IList<InventoryClass> Model
                = extRef.extInventoryClass.getList( new InventoryClass() { isEnd = false, iGrade = 1 });
            SelectList sl = new SelectList(Model, dataValueField: "invClsCode", dataTextField: "invClsName");
            TempData["px"] = sl;
            return View();
        }
        public ActionResult referInventoryPP(string id)
        {
            List<InventoryClass> pp = extRef.extInventoryClass.getList(new InventoryClass() {  iGrade = 2, upInvClsCode = id  });
            var r = Json(pp, "application/json", JsonRequestBehavior.AllowGet);
            return r;
        }
        public ActionResult referInventorys(string id)
        {
            List<Inventory> Model = extRef.extInventory.getList(new Inventory() { cInvClassCode = id });
            var r = Json(Model, "application/json", JsonRequestBehavior.AllowGet);
            return r;
        }
        public ActionResult referSearchInventorys(string id)
        {
            List<Inventory> ModelA
                = extRef.extInventory.getList(new Inventory() { InvCode = id });
            List<Inventory> ModelB
                = extRef.extInventory.getList(new Inventory() { InvName = id });
            List<Inventory> ModelC
                = extRef.extInventory.getList(new Inventory() { InvStd = id });
            List<Inventory> ModelD
                = extRef.extInventory.getList(new Inventory() { cInvClassCode = id });
            List<Inventory> Model = new List<Inventory>();
            Model = ModelA.Union(ModelB).Union(ModelC).Union(ModelD).ToList();//去重

            var r = Json(Model, "application/json", JsonRequestBehavior.AllowGet);
            return r;
        }


    }
}
