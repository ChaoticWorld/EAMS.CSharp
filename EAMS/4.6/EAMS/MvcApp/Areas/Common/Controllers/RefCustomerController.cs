using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Logs;
using DataModel;
using MvcApp.Areas.Common.Models;

namespace MvcApp.Areas.Common.Controllers
{
    public class RefCustomerController : Controller
    {
        // GET: Common/RefCustomer
        private static ExtRef extRef;
        private SysLogsDataAccess logDA = new SysLogsDataAccess();

        public RefCustomerController()
        { extRef = new ExtRef("u8");   }

        /// <summary>
        /// 客户信息以Json返回
        /// </summary>
        /// <param name="id">客户编码</param>
        /// <returns></returns>
        public ActionResult cusInfoJson(string id)
        {
            JsonResult r;
            DataModel.Customer customer = new DataModel.Customer();
            customer = extRef.extCustomer.getSingle(id);
            if (customer != null)
                r = Json(customer, "application/json", JsonRequestBehavior.AllowGet);
            else r = new JsonResult() { ContentType = "application/json", JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = null };
            logDA.createLog(new LogModel() { cParams = id, cFunction = "cusInfoJson", cModule = "RefCustomer", cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(customer) });
            return r;
        }
        public ActionResult searchGo(string id)
        {            
            List<Customer> cuss = new List<Customer>();
            var contact = new ContactInfoBase() { isdefault = true };
            var contactMan = new Contact() { isDefault = true };
            if (!string.IsNullOrEmpty(id))
            {
                var cus1 = extRef.extCustomer.getList(new Customer() { Code = id });//客户编码
                var cus2 = extRef.extCustomer.getList(new Customer() { Name = id });//客户名称
                contact.Address = id;
                var cus3 = extRef.extCustomer.getList(new Customer() { contactInfos = new List<ContactInfoBase>() { contact } });//客户地址
                contact.Address = string.Empty;
                contact.shipAddress = id;
                var cus4 = extRef.extCustomer.getList(new Customer() { contactInfos = new List<ContactInfoBase>() { contact } });//客户发货地址
                contact.shipAddress = string.Empty;
                contactMan.Name = id;
                var cus5 = extRef.extCustomer.getList(new Customer() { contacts = new List<Contact>() { contactMan } });//联系人
                cuss = cus1.Union(cus2).Union(cus3).Union(cus4).Union(cus5).ToList();
            }
            else
                cuss = extRef.extCustomer.getList(whereStr: string.Empty);
            var cussJsonStr = (cuss == null) ? "" : Newtonsoft.Json.JsonConvert.SerializeObject(cuss);
            logDA.createLog(new LogModel() { cParams = id, cFunction = "searchGo", cModule = "RefCustomer", cReturn = cussJsonStr.Substring(0, cussJsonStr.Length > 2000 ? 2000 : cussJsonStr.Length) });
            //是否有结果
            ViewData["hasResult"] = false;
            if (cuss != null && cuss.Count > 0) ViewData["hasResult"] = true;
            return View(cuss);
        }
    }
}