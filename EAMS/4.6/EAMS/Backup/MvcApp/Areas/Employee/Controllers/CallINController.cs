using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemBLL;
using Webdiyer.WebControls.Mvc;
using CallInfo;

namespace MvcApp.Areas.Employee.Controllers
{
    //[AuthorizeEx(Roles="Employee")]
    public class CallINController : Controller
    {
        private static CallInfo.CallDWInfo ci;
        //
        // GET: /Employee/CallIN/

        public List<SelectListItem> setIEnumerableSelectListItem(Dictionary<string, string> d)
        {
            List<SelectListItem> Lists = new List<SelectListItem>();
            foreach (KeyValuePair<string, string> kv in d)
                Lists.Add(new SelectListItem { Value = kv.Key, Text = kv.Value });
            return Lists;
        }

        public ActionResult Index(string id = null)
        {
            //int t = -1;
            CallInfo.callRecordsBLL callrecordBll = new CallInfo.callRecordsBLL();
            CallInfo.Phone_Records rec = new CallInfo.Phone_Records();
            if (!string.IsNullOrEmpty(id))
            {
                //为呼叫中心(CoolAgent比高)Url字符串形式(t=13306408796&c=26270),提取来电号码
                if (id.StartsWith("t=")) id = id.Substring(2,id.IndexOf("&c=")-2);//从后一个位置的index-前辍长度
                //BUG必须改为正则表达式,+国际区号电话号码|区号电话号码（3位以上）;
                string regexPhoneString = "(^[+]?||^\\d?)\\d{4,16}";
                System.Text.RegularExpressions.Regex regexPhone = new System.Text.RegularExpressions.Regex(regexPhoneString,System.Text.RegularExpressions.RegexOptions.Compiled);
                if (regexPhone.Match(id, 0).Success)
                {
                    ci = new CallInfo.CallDWInfo(callid: id);
                    //filter(ref ci, "客户经理"); 
                    if (ci.CallDWModel.IsNull || ci.CallDWModel.cusInfo == null || ci.CallDWModel.cusInfo.Count < 1) return View();
                    ViewData["dwLists"] = setIEnumerableSelectListItem(ci.CallDWModel.DwList);
                    ViewData["contactLists"] = setIEnumerableSelectListItem(ci.CallDWModel.ContactList);
                    int callRecordId = -1;
                    if (ci.CallDWModel != null && !ci.CallDWModel.IsNull && ci.CallDWModel.conInfo.Count > 0)
                    {
                        rec = new CallInfo.Phone_Records();
                        var dw = ci.CallDWModel.cusInfo.First(cu => cu.cCusCode == cu.topCusCode);
                        rec.dwCode = dw.topCusCode;
                        rec.dwContact = dw.ContactName;
                        rec.dwName = dw.cCusName;
                        rec.callInOut = false;
                        rec.callDate = DateTime.Now;
                        rec.callPhone = id.Replace("-", "");

                        callRecordId = callrecordBll.add(rec);
                    }
                    TempData["callRecordId"] = callRecordId;
                    TempData["callPhone"] = id.Replace("-", ""); 
                    return View(ci.CallDWModel);
                }
                else
                {
                    //在URL地址中以[dwcode]作为ID获得Model,
                    //后期通过searchGo方法搜索获得参照结果，选择准确的DWCode,
                    //将DWCode传入searchResult(DwCode)获得Model,再转入Index(Model);
                    //下面方法可以直接从Url读取客户资料，不安全，禁用！
                    /* 禁用
                    string regexCusCode = "^\\[.*?\\]$";
                    if (System.Text.RegularExpressions.Regex.Match(id,regexCusCode).Success)
                    {
                        string key = id.Replace("[", "").Replace("]", "");
                        ci = new CallInfo.CallDWInfo(code:key);
                        SystemDB.User u = SystemBLL.UserBLL.getUser(int.Parse(User.Identity.Name));
                        if (u.RoleNames.Contains("客户经理") && !ci.CallDWModel.IsNull && ci.CallDWModel.cusInfo != null && ci.CallDWModel.cusInfo.Count > 0)
                            ci.CallDWModel.cusInfo.RemoveAll(e => e.employeeName != u.cUserName);
                        if (ci.CallDWModel.IsNull || ci.CallDWModel.cusInfo == null || ci.CallDWModel.cusInfo.Count < 1) return View();
                        ViewData["dwLists"] = setIEnumerableSelectListItem(ci.CallDWModel.DwList);
                        ViewData["contactLists"] = setIEnumerableSelectListItem(ci.CallDWModel.ContactList);

                        return View(ci.CallDWModel);
                    }
                    */
                    return View();                    
                }
            }
            else return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection f = null)
        {
            string key = f["keyCode"];
            ci = new CallInfo.CallDWInfo(code: key);
            //filter(ref ci, "客户经理");
            if (ci.CallDWModel.IsNull || ci.CallDWModel.cusInfo == null || ci.CallDWModel.cusInfo.Count < 1) return View();
            else
            {
                ViewData["dwLists"] = setIEnumerableSelectListItem(ci.CallDWModel.DwList);
                ViewData["contactLists"] = setIEnumerableSelectListItem(ci.CallDWModel.ContactList);

                TempData["callPhone"] = ci.CallDWModel.cusInfo.First().cPhone; 
                return View(ci.CallDWModel);
            }
        }

        public ActionResult searchGo(string id)
        {
            string searchKey = id;

            //模糊查询;电话，联系人，单位编码，单位名称,地址
            if (string.IsNullOrEmpty(id))
                ci = new CallInfo.CallDWInfo(key: "_AllCustomers_");
            else
                ci = new CallInfo.CallDWInfo(key: searchKey);
            //filter(ref ci, "客户经理");
            //是否有结果
            ViewData["hasResult"] = false;

            if (!ci.CallDWModel.IsNull && ci.CallDWModel.cusInfo !=null && ci.CallDWModel.cusInfo.Count>0)
                ViewData["hasResult"] = true;
            return View(ci.CallDWModel.cusInfo);
        }
        /// <summary>
        /// 过滤器,指定角色的客户档案.针对特别权限:客户经理只能看自己的客户
        /// </summary>
        /// <param name="ci"></param>
        /// <param name="roleName"></param>
        private void filter(ref CallDWInfo ci, string roleName)
        {
            SystemDB.User u = SystemBLL.UserBLL.getUser(int.Parse(User.Identity.Name));
            if (u.RoleNames.Contains(roleName) && !ci.CallDWModel.IsNull && ci.CallDWModel.cusInfo != null && ci.CallDWModel.cusInfo.Count > 0)
                ci.CallDWModel.cusInfo.RemoveAll(e => e.employeeName != u.cUserName);
        }

        public ActionResult cusInfoPage(string id, string Date)
        {
            DataDB.ModelBase.Customer c = new DataDB.ModelBase.Customer();
            c = ci.CallDWModel.cusInfo.First(item => item.cCusCode == id);
            return View(c);
        }

        public ActionResult saleRecordPage(string id)
        {
            CallInfo.CallVouchInfo cv = new CallInfo.CallVouchInfo();
            ViewData["dwcode"] = id;
            ViewData["lastNum"] = cv.CallVouchModel.vouchLastNum;
            return View();
        }

        [HttpPost]
        public ActionResult saleRecordTable(FormCollection f)
        {
            string id = f["dwcode"];
            int num = int.Parse(f["lstRecordNum"]);
            CallInfo.CallVouchInfo cv = new CallInfo.CallVouchInfo();
            cv.CallVouchModel = cv.getDispatchModel(id,num);
            return View(cv.CallVouchModel);
        }

        public ActionResult callRecordPage(string id,string param = "1")
        {
            string dwcode = id;
            ViewData["dwcode"]=id;
            CallInfo.callRecordsBLL callRecordBll = new CallInfo.callRecordsBLL();
            IEnumerable<CallInfo.Phone_Records> r =
                callRecordBll.list(id)
                    .OrderByDescending(o => o.callDate)
                    .OrderByDescending(o => o.dwContact);
            int p = -1;
            PagedList<Phone_Records> model;
            if (int.TryParse(param,out p))
             model = r.ToPagedList(p, 5);
            else 
             model = r.ToPagedList(1, 5);
            if (Request.IsAjaxRequest())
                return PartialView("callRecordTable", model);
            return View(model);
        }
        [HttpPost]
        public ActionResult callRecordPage(FormCollection f, string id, int param = 1)
        {
            string dwcode = null;
            dwcode = f.AllKeys.Contains("dwcode") ? f["dwcode"] : null;
            if (string.IsNullOrEmpty(dwcode))
                dwcode = string.IsNullOrEmpty(id) ? null : id;
            ViewData["dwcode"] = dwcode;
            CallInfo.callRecordsBLL callRecordBll = new CallInfo.callRecordsBLL();
            IEnumerable<CallInfo.Phone_Records> r =
                callRecordBll.list(dwcode)
                    .OrderByDescending(o => o.callDate)
                    .OrderByDescending(o => o.dwContact);
            var model = r.ToPagedList(param, 5);
            if (Request.IsAjaxRequest())
                return PartialView("callRecordTable", model);
            return View();
        }

        public ActionResult callRecordEdit(string id)
        {
            Phone_Records pr = new Phone_Records();
            CallInfo.callRecordsBLL crb = new callRecordsBLL();
            int _id = -1;
            if (int.TryParse(id, out _id))
            { if (_id > 0) pr = crb.single(_id); else pr.ID = -1; }
            else pr.ID = -1;
            return View(pr);
        }
        [HttpPost]
        public ActionResult callRecordEdit(FormCollection f)
        {
            Phone_Records pr = new Phone_Records();
            CallInfo.callRecordsBLL crb = new callRecordsBLL();
            pr.ID = f.AllKeys.Contains("ID") ? int.Parse(f["ID"]) : -1;
            pr.callDate = (f.AllKeys.Contains("callDate") ? DateTime.Parse(f["callDate"]) : DateTime.Now);
            pr.callInOut = f.AllKeys.Contains("callInOut") ? bool.Parse(f["callInOut"]) : false;
            pr.callPhone = f.AllKeys.Contains("callPhone") ? f["callPhone"] : null;
            pr.dwCode = f.AllKeys.Contains("dwCode") ? f["dwCode"] : null;
            pr.dwContact = f.AllKeys.Contains("dwContact") ? f["dwContact"] : null;
            pr.dwName = f.AllKeys.Contains("dwName") ? f["dwName"] : null;
            pr.callContent = (f.AllKeys.Contains("callContent")?f["callContent"]:null);
            
            if (pr.ID > 0) crb.update(pr);
            else crb.add(pr);
            return View(pr);
        }
    }
}
