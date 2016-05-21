using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemBLL;
using Petition;
using Webdiyer.WebControls.Mvc;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;

namespace MvcApp.Areas.Employee
{
    [AuthorizeEx(Roles="Employee")]
    public class PetitionController : Controller
    {
        static LogsBLL logbll = new LogsBLL();
        SystemDB.User currUser;
        //签呈
        //
        // GET: /Employee/Petition/
        public List<QC_Main> getListQCM(bool isAjax,string key
            ,string Memployee,string MstartDate,string MfinishDate
            , string Remployee, string RstartDate, string RfinishDate)
        {
            List<QC_Main> rListQCM = new List<QC_Main>();
            
            QCMainDB qcm = new QCMainDB();
            QCReplysDB qcr = new QCReplysDB();
            rListQCM = qcm.getFiltedQCMainList(key, Memployee, MstartDate, MfinishDate, Remployee, RstartDate, RfinishDate);

            //此处插入员工可查看列表权限控制
            SystemDB.User u = SystemBLL.UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            if (u.RoleNames.Contains("客户经理"))
                rListQCM = rListQCM.FindAll(q => q.cCusManager == u.cUserName);

            rListQCM = rListQCM
                .OrderByDescending(d => d.result)
                .OrderByDescending(e => e.dModifyDate)
                .ToList();
            if (!isAjax)
            { //var unProcess = rListQCM;
            }
            string remark = string.Empty;
            string result = string.Empty;
            string resultPass = string.Empty;
            List<QC_Details> qcds = new List<QC_Details>();
            List<QC_Replys> qcrs = new List<QC_Replys>();
            foreach (QC_Main m in rListQCM)
            {
                remark = string.Empty;
                result = string.Empty;
                resultPass = string.Empty;
                QCMainText(m.Id, out remark, out result,out resultPass);
                m.remark = remark;
                m.resultTitle = result;
                m.result = resultPass;
                m.cModifyMan = string.IsNullOrEmpty(m.cModifyMan) ? m.cCreateMan : m.cModifyMan;
                m.cResult = resultPass;
            }
            return rListQCM;
        }
        public ActionResult Index(int id = 1)
        {
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Petition",
                cClass = "PetitionController",
                cFunction = "Index",
                cReturn = "Open Petition Default Page!"
            });
            //当前用户是否有[签呈明细分类管理]权限
            string roles =
            SystemBLL.UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name)).RoleNames;
            TempData["enableDetailsClassEdit"] = roles.Contains("PetitionDetailsClassEditer");
            List<QC_Main> lstQCM = getListQCM(true,null,null,null,null,null,null,null);

            var model = lstQCM.ToPagedList(id, 20);
            if (Request.IsAjaxRequest())
                return PartialView("List", model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(FormCollection f,int id = 1)
        {
            List<QC_Main> lstQCM = new List<QC_Main>();
            string key = f.AllKeys.Contains("searchKey") ? f["searchKey"].ToString() : null;
            string MstartDate = f.AllKeys.Contains("MstartDate") ? f["MstartDate"].ToString() : null;
            string MfinishDate = f.AllKeys.Contains("MfinishDate") ? f["MfinishDate"].ToString() : null;
            string Memployee = f.AllKeys.Contains("Memployee") ? f["Memployee"].ToString() : null;
            string RstartDate = f.AllKeys.Contains("RstartDate") ? f["RstartDate"].ToString() : null;
            string RfinishDate = f.AllKeys.Contains("RfinishDate") ? f["RfinishDate"].ToString() : null;
            string Remployee = f.AllKeys.Contains("Remployee") ? f["Remployee"].ToString() : null;
            
            lstQCM = getListQCM(false,key,Memployee,MstartDate,MfinishDate,Remployee,RstartDate,RfinishDate);
            foreach (QC_Main m in lstQCM)
            { m.cModifyMan = string.IsNullOrEmpty(m.cModifyMan) ? m.cCreateMan : m.cModifyMan; }
            
            int sizePage;
            if (f.AllKeys.Contains("sizePage"))
                int.TryParse(f["sizePage"].ToString(), out sizePage);
            else sizePage = 20;
            var model = lstQCM.ToPagedList(id, sizePage);
            if (Request.IsAjaxRequest()) 
                return PartialView("List", model);
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            QCMainDB qcm = new QCMainDB();
            QC_Main m = qcm.single(id);
            TempData["ID"] = id;
            if (m.QC_Replys.Count > 0)
            {
                TempData["deleteMsg"] = "已进入审批流程，不能删除！";
                TempData["canDelete"] = false;
            }
            else
            {
                TempData["deleteMsg"] = "未进入审批流程，可以删除！是否继续？";
                TempData["canDelete"] = true;
            }
            return View();
        }
        public JsonResult sureMainDelete(int id)
        {
            QCMainDB qcm = new QCMainDB();
            QCDetailsDB qcd = new QCDetailsDB();
            QCtxtContentDB txtc = new QCtxtContentDB();
            int di = 0; int mi = 0;

            List<QC_txtContent> tcs = txtc.list(id);
            if (tcs != null && tcs.Count > 0)
                foreach (QC_txtContent c in tcs)
                    di += txtc.delete(c.autoid);

            //List<QC_Details> details = qcd.list(id);
            //if (details != null && details.Count > 0)
            //    foreach (QC_Details d in details)
            //        di += qcd.delete(d.QC_Autoid);            
            mi = qcm.delete(id);
            
            var d1 = new { di = di.ToString(), mi = mi.ToString() };
            JsonResult r
                = new JsonResult() { Data = d1,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            
            //string script = "alert('删除内容" + di.ToString() + "条，删除签呈" + mi.ToString() + "条！');";
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Petition",
                cClass = "PetitionController",
                cFunction = "sureMainDelete",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(id),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(d1)
            });
            return r;
        }
        public ActionResult Replys(int id)
        {
            List<QC_Replys> r = new List<QC_Replys>();
            QCReplysDB qcr = new QCReplysDB();
            r = qcr.list(id);
            if (r.Count > 0)
                return View(r);
            else 
                return View(new List<QC_Replys>());
        }
        private SelectList getPetitionClass(string defaultFlag){
            QCClassDB qcc = QCClassDB.Create();
            var clss = qcc.list();
            SelectList sl = new SelectList(clss, "cClsFlag", "cClsName", defaultFlag);
            return sl;
        }
        public ActionResult Edit (int id){
            var Model = Models.petitionEditModel.createModel();
            QCMainDB qcm = new QCMainDB();
            QCtxtContentDB qct = new QCtxtContentDB();
            if (id > 0)
            {
                Model.isNew = false;
                Model.QCM = qcm.single(id);
                Model.QCtxt = qct.list(id);
                Model.isReply =
                    Model.QCM.QC_Replys.Where(
                                r => r.QC_ReplyPass.HasValue && r.QC_ReplyPass.Value
                                ).Count() > 0;
                Model.QCM.cResult = Model.isReply ? "同意" : "";
            }
            else {
                Model.isNew = true;
                SystemDB.User u = SystemBLL.UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            
                Model.QCM = new QC_Main()
                {
                    Id = -1,
                    cCode = "新签呈",
                    cCusManager = "",
                    cCusPerson = "",
                    cCusName = "",
                    cResult = "",
                    cCreateMan=u.cUserName,
                    dCreateDate=DateTime.Now,
                    cClsFlag = "A"
                };
            }

            TempData["PetitionClass"] = getPetitionClass(Model.QCM.cClsFlag);
            return View(Model);
        }
        #region ver20140327 弃用
        public ActionResult Edit_ver20140327(int id)
        {
            QC_Main m = new QC_Main();
            QCMainDB qcm = new QCMainDB();
            if (id > 0)
            {
                m = qcm.single(id);
                return View(m);
            }
            else return View(new QC_Main());
        }
        [HttpPost]
        public ActionResult Edit_ver20140327(QC_Main m)
        {
            QCMainDB qcm = new QCMainDB();

            int MainId = -1;
             //如果是新增的，m.Id <0
            if (m.Id < 0)
            {
                m.cCreateMan = SystemBLL.UserBLL.getUser(int.Parse(this.User.Identity.Name)).cUserName;
                MainId = qcm.add(m);
                return RedirectToAction("Edit", new { id = MainId });
            }
            else
            {
                m.cModifyMan = SystemBLL.UserBLL.getUser(int.Parse(this.User.Identity.Name)).cUserName;
                qcm.updata(m); 
                return RedirectToAction("Edit", new { id = m.Id });
            }
        }

        #endregion
        public ActionResult RecordList(int id)
        {
            if (id < 0)
                return View(new List<QC_Details>());
            else
            {
                QCMainDB qcm = new QCMainDB();
                QCReplysDB qcr = new QCReplysDB();
                QC_Main m = qcm.single(id);
                TempData["QC_MainID"] = id;
                TempData["isReplied"] = qcr.list(id).Exists(r=>r.QC_ReplyPass == true);
                return View(m.QC_Details.OrderBy(d=>d.QC_Autoid).ToList());
            }
        }

        public ActionResult RecordEdit(int id)
        {
            QCMainDB qcm = new QCMainDB();
            QC_Main m = qcm.single(id);
            ViewData["mid"] = m.Id;
            TempData["cusCode"] = m.cCusCode;
            TempData["cusName"] = m.cCusName;
            QCDetailsDB qcd = new QCDetailsDB();
            List<QC_Details> d = qcd.list(id).OrderBy(e=>e.QC_Autoid).ToList();
            QCDetailClassDB qcdc = new QCDetailClassDB();
            SelectList selRc = new SelectList(qcdc.list(""), "QC_RCID", "QC_RCName");
            TempData["SelectRC"] =selRc;
            return View(d);
        }
        [HttpPost]
        public ActionResult RecordEdit(List<QC_Details> DetailList,FormCollection f)
        {
            int mid = int.Parse(f["QC_MainID"]);
            QCDetailsDB qcd = new QCDetailsDB();
            DetailList.RemoveAt(0);
            IEnumerable<QC_Details> Dts = DetailList;

            //下面的Code执行修改和新增，删除在行删除时已经在数据库中操作.
            foreach (QC_Details d in Dts)
            {
                d.QC_MainID = mid;
                if (!qcd.Exist(d))
                    qcd.add(d);
                else
                    if (qcd.isModifed(d)) qcd.updata(d);
            }
            return RedirectToAction("RecordEdit", new { id = mid });
        }
        /// <summary>
        /// 参照销售发货单
        /// </summary>
        /// <param name="id">客户编码</param>
        /// <param name="param">发货单记录数</param>
        /// <returns></returns>
        public ActionResult referSaleDispatch(string id, string param)
        {
            DataDB.u8.dbU8 dbu8 = new DataDB.u8.dbU8();
            dbu8.existsDispatch(cusCode: id);
            int n;
            if (!int.TryParse(param, out n))
                n = 5;

            List<DataDB.ModelBase.Dispatch> DispatchModel = dbu8.getDispatch(cusCode: id, lastnum: n);

            return View(DispatchModel);
        }

        /// <summary>
        /// 参照销售订单
        /// </summary>
        /// <param name="id">客户编码</param>
        /// <param name="param">订单记录数</param>
        /// <returns></returns>
        public ActionResult referSaleOrder(string id,string param)
        {
            DataDB.u8.dbU8 dbu8 = new DataDB.u8.dbU8();
            dbu8.existsSaleOrder(cusCode: id);
            int n;
            if (!int.TryParse(param, out n))
                n = 5;

            List<DataDB.ModelBase.SaleOrder> SOModel = dbu8.getSaleOrder(cusCode: id, lastnum: n );
            
            return View(SOModel);
        }
        
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
            SelectList sl = new SelectList(Model,dataValueField:"code",dataTextField:"name");
            TempData["px"] = sl;
            return View();
        }
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
            List<DataDB.ModelBase.Inventory> Model = dbu8.getInventorys(ccode:pp);
            var r = Json(Model, "application/json", JsonRequestBehavior.AllowGet);

            return r;
        }
        public ActionResult referSearchInventorys(string id)
        {
            string key = id;
            key = key.Replace(" ","%");
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

        public void RecordItemDelete(int id)
        {
            QCDetailsDB qcd = new QCDetailsDB();
            if (qcd.Exist(id))
                qcd.delete(id);
        }
        #region ver20150302
        /// <summary>
        /// 客户信息以Json返回
        /// </summary>
        /// <param name="id">客户编码</param>
        /// <returns></returns>
        public ActionResult cusInfoJson(string id)
        {
            CallInfo.CallDWInfo ci = new CallInfo.CallDWInfo(code: id);
            DataDB.ModelBase.Customer c = new DataDB.ModelBase.Customer();
            try
            {
                c = ci.CallDWModel.cusInfo.First(item => item.cCusCode.ToLower().Equals(id.ToLower()));
                var rcus = Json(c, "application/json", JsonRequestBehavior.AllowGet);
                return rcus;
            }
            catch { return new JsonResult(); }
        }

        /// <summary>
        /// 保存签呈Head,返回签呈保存id
        /// </summary>
        /// <returns>保存签呈id</returns>
        [HttpPost]
        public ActionResult saveHead(QC_Main m) {
            JsonResult r = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            
            QCMainDB mdb = new QCMainDB();
            SystemDB.User u = SystemBLL.UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            int id = m.Id;
            if (m.Id < 0) {
                m.dCreateDate = m.dCreateDate.HasValue ? m.dCreateDate.Value : DateTime.Now;
                m.cCreateMan = u.cUserName;
                id = mdb.add(m);
                m.cCode = mdb.single(id).cCode;
            }
            else{
                m.dModifyDate = DateTime.Now;
                m.cModifyMan = u.cUserName;
                mdb.updata(m);
                m.cCode = mdb.single(id).cCode;
            }
            string rd = "{\"id\":\"" + id.ToString() + "\",\"code\":\"" + m.cCode+"\"}";
            r.Data = rd;
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Petition",
                cClass = "PetitionController",
                cFunction = "saveHead",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(m),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(rd)
            });
            return r;
        }
        /// <summary>
        /// 保存签呈Body,返回签呈保存id
        /// </summary>
        /// <returns>保存签呈id</returns>
        [HttpPost]
        public ActionResult saveBody(QC_txtContent [] Contents)
        {
            JsonResult r = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            QCtxtContentDB cdb = new QCtxtContentDB();
            var a = Request.Params;
            foreach(QC_txtContent c in Contents )
                if (c.autoid < 0) { if (!string.IsNullOrEmpty(c.plan.Trim())) cdb.add(c); }
                else
                {
                    if ((c.saveState != null && c.saveState.Equals("deleted"))
                    ||string.IsNullOrEmpty(c.plan.Trim()))
                        cdb.delete(c.autoid);
                    else cdb.updata(c);
                }
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Petition",
                cClass = "PetitionController",
                cFunction = "saveBody",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(Contents),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(r.Data)
            });
            return r;
        }
        [HttpPost]
        public ActionResult saveReply(int id)
        {            
            SystemDB.User u = SystemBLL.UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            QCReplysDB rdb = new QCReplysDB();
            QC_Replys r = new QC_Replys()
            {
                QC_MainID = id,
                AutoId = -1,
                QC_ReplyContent = "同意",
                QC_ReplyPass = true,
                QC_ReplyDate = DateTime.Now,
                QC_ReplyMan = u.iUserId.ToString()
            };
            rdb.add(r);
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Petition",
                cClass = "PetitionController",
                cFunction = "saveReply",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(id)
//                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(d1)
            });
            return null;
        }

        [HttpPost]
        public ActionResult saveClass(QC_Class [] clss)
        {
            bool ? br = null;
            JsonResult r = new JsonResult() { JsonRequestBehavior= JsonRequestBehavior.AllowGet, Data=false };
            
            QCClassDB qcc = QCClassDB.Create();
            foreach (QC_Class c in clss) {
                switch (c.saveState)
                {
                    case "Modified":
                        if (c.iClsAutoId > 0) br = qcc.Update(c) > 0;
                        else br = qcc.Add(c) > 0;
                        break;
                    case "Deleted":
                        qcc.Delete(c.iClsAutoId);
                        break;
                    default:
                        break;
                }
            }
            r.Data = br;// br.HasValue ? br.Value : false;
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Petition",
                cClass = "PetitionController",
                cFunction = "saveClass",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(clss),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(br)
            });
            return r;
        }
        [HttpPost]
        public ActionResult isClassUsed(int id)
        {
            JsonResult r = new JsonResult() { JsonRequestBehavior= JsonRequestBehavior.AllowGet, Data=false };
            QCClassDB qcc = QCClassDB.Create();
            string v = qcc.Signal(id).cClsFlag;
            string k = "cClsFlag";
            QCMainDB qcm = new QCMainDB();
            r.Data = qcm.Exist(new KeyValuePair<string, string>(k,v));
            return r;
        }
        #endregion
        /// <summary>
        /// 审批提交
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ReplySumbit(int id)
        {
            //审批人选择
            List<SystemDB.User> ulist = new List<SystemDB.User>();
            UserBLL ubll = new UserBLL();
            ulist = ubll.Lists("").ToList();
            ulist = ulist.FindAll(u=> !string.IsNullOrEmpty(u.RoleNames) && u.RoleNames.Contains("PetitionReplyDoer"));
            SelectList slEmployees = new SelectList(ulist, "iUserId", "cUserName");
            ViewData["ReplyEmployees"] = slEmployees;
            TempData["mid"] = id;
            return View();
        }
        [HttpPost]
        public JavaScriptResult ReplySumbit(int id,FormCollection f)
        {
            int aid = int.Parse(f["aid"].ToString());
            int mid = int.Parse(f["mid"].ToString());

            string man = (int.Parse(f["man"]) < 0) ? "" : f["man"].ToString();
            QC_Replys r = new QC_Replys() 
            { AutoId = aid, QC_MainID = mid, QC_SubmitMan = HttpContext.User.Identity.Name,
                QC_SubmitDate = DateTime.Now,
                QC_ReplyMan = man };
            QCReplysDB qcr = new QCReplysDB();
            if (!qcr.Exist(r))
                qcr.add(r);
            else
                qcr.updata(r);
            return new JavaScriptResult() { Script = "alert('提交成功！');" };
        }
        /// <summary>
        /// 审批记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ReplyRecords(int id)
        {
            List<QC_Replys> listReply = new List<QC_Replys>();
            QCReplysDB qcr = new QCReplysDB();

            //审批处理数据
            if (!qcr.Exist(new KeyValuePair<string, string>(key: "QC_MainID", value: id.ToString())))
            {//无审批记录，新的审批开始
                QC_Replys r = new QC_Replys()
                                {
                                    QC_MainID = id,
                                    QC_SubmitDate = DateTime.Now,
                                    QC_SubmitMan = HttpContext.User.Identity.Name
                                };
                //qcr.add(r);
                return new ContentResult() { Content="<div>无审批</div>" };
            }
            else
            {
                //已存在审批记录
                listReply = qcr.list("it.QC_MainID = " + id.ToString()).OrderByDescending(d => d.AutoId).ToList();
                return View(listReply);
            }
        }

        public ActionResult ReplyDo(int id)
        {
            //审批人选择
            List<SystemDB.User> ulist = new List<SystemDB.User>();
            UserBLL ubll = new UserBLL();
            ulist = ubll.Lists("").ToList(); 
            ulist = ulist.FindAll(u => !string.IsNullOrEmpty(u.RoleNames) && u.RoleNames.Contains("PetitionReplyDoer"));
            SelectList slEmployees = new SelectList(ulist, "iUserId", "cUserName");
            ViewData["ReplyEmployees"] = slEmployees;

            int autoid = id;
            QCReplysDB rdb = new QCReplysDB();
            QC_Replys r = new QC_Replys();
            r = rdb.single(autoid);
            return View(r);
        }
        [HttpPost]
        public JavaScriptResult ReplyDo(string id, FormCollection fc)
        {
            QCReplysDB rdb = new QCReplysDB();

            int mid = int.Parse(fc["mid"].ToString());
            int aid = int.Parse(fc["aid"].ToString());
            string c = (string.IsNullOrEmpty(fc["content"])) ? "" : fc["content"].ToString();
            bool p = bool.Parse(fc["pass"].ToString());
            QC_Replys r = new QC_Replys()
            {
                QC_MainID = mid,
                AutoId = aid,
                QC_ReplyContent = c,
                QC_ReplyPass = p,
                QC_ReplyDate = DateTime.Now
            };
        
            string nextman = (int.Parse(fc["nextman"])<0)?"":fc["nextman"].ToString();
            if (!string.IsNullOrEmpty(nextman))
            {
                QC_Replys next = new QC_Replys()
                {
                    QC_SubmitMan = HttpContext.User.Identity.Name,
                    QC_SubmitDate = DateTime.Now,
                    QC_MainID = r.QC_MainID,
                    QC_ReplyMan = nextman
                };
                r.QC_NextID = rdb.add(next);
            }
            if (!rdb.Exist(r))
                rdb.add(r);
            else
                rdb.updata(r);
            return new JavaScriptResult() { Script = "alert('提交成功！');" };
           
        }

        [HttpPost]
        public JsonResult Replying(string id) 
        {
            return Json(data:"true");
        }
        public JavaScriptResult DetailClassDelete(int id)
        {
            QCDetailClassDB dcdb = new QCDetailClassDB();
            string javaScript = string.Empty;
            if (dcdb.Exist(new KeyValuePair<string, string>("QC_RCID", id.ToString())))
            {
                dcdb.delete(id);
                javaScript = "删除成功！";
            }
            else
                javaScript = "未找到指定分类！";
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Petition",
                cClass = "PetitionController",
                cFunction = "DetailClassDelete",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(id),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(javaScript)
            });
            return new JavaScriptResult() { Script = "alert('"+javaScript+"');" };
        }
        public ActionResult DetailsClassEdit()
        {
            QCDetailClassDB dcdb = new QCDetailClassDB();
            List<QC_DetailClass> dcLists = dcdb.list("");
            return View(dcLists);
        }
        [HttpPost]
        public　ActionResult DetailsClassEdit(List<QC_DetailClass> _dcList, FormCollection f)
        {
            QCDetailClassDB dcdb = new QCDetailClassDB();
            _dcList.RemoveAt(0);
            List<QC_DetailClass> dcLists = _dcList;
            foreach (QC_DetailClass dc in dcLists)
            {
                if (dcdb.Exist(dc))
                    dcdb.updata(dc);
                else
                    dcdb.add(dc);
            }

            return RedirectToAction("DetailsClassEdit");
        }
        public void QCMainText(int id, out string remark, out string result, out string resultPass)
        {
            remark = string.Empty;
            result = string.Empty;
            resultPass = string.Empty;
            QC_Main m = new QC_Main();
            QCMainDB qcm = new QCMainDB();
            m = qcm.single(id);
            List<QC_Details> qcds = new List<QC_Details>();
            List<QC_Replys> qcrs = new List<QC_Replys>();
            List<QC_txtContent> qctc = new List<QC_txtContent>();

            if (null != m)
            {
                //排序

                qcds = m.QC_Details.OrderBy(d => d.QC_Autoid).ToList();
                qcrs = m.QC_Replys.OrderByDescending(r => r.QC_ReplyDate).ToList();
                qctc = m.QC_txtContent.OrderBy(d => d.order).ToList();

                #region 明细表 20150123 改版本弃用
                /* 明细表 ver20140327
                for (int i = 0; i < qcds.Count; i++)
                {
                    remark += i.ToString() + "." + qcds[i].QC_DetailClassReference.Value.QC_RCName + "-";
                    if (qcds[i].QC_DetailClassReference.Value.QC_RCName.Contains("累计销售"))
                        remark += ("起止日期:" + qcds[i].dSaleSumStartDate.Value.ToShortDateString()
                            + "到" + qcds[i].dSaleSumFinishDate.Value.ToShortDateString() + ";");
                    if (!qcds[i].bDaiShouKuan.Value)
                        remark += (string.IsNullOrEmpty(qcds[i].cInvName) ? "" : qcds[i].cInvName + ";")
                        + (string.IsNullOrEmpty(qcds[i].cInvStd) ? "" : qcds[i].cInvStd + ";")
                        + ((qcds[i].iQuentity == null) ? "" : "数量：" + qcds[i].iQuentity.Value.ToString("#0.00") + ";")
                        + ((qcds[i].iPrice == null) ? "" : "单价：" + qcds[i].iPrice.Value.ToString("#0.00") + ";")
                        + ((qcds[i].iSum == null) ? "" : "金额：" + qcds[i].iSum.Value.ToString("#0.00") + ";")
                        + "<br/>";
                    else remark += " 是。 <br/>";
                    if (!string.IsNullOrEmpty(qcds[i].cMemo))
                        remark += ("备注：" + qcds[i].cMemo + ";<br/>");
                }
                if (m.dEffDate.HasValue || m.dExpDate.HasValue)
                {
                    remark += ("生效起止日期："
                        + (m.dEffDate.HasValue ? m.dEffDate.Value.ToShortDateString() : "即日")
                        + "到" + (m.dExpDate.HasValue ? m.dExpDate.Value.ToShortDateString() : "--"));
                }
                */
                #endregion

                #region 使用文本编辑记录签呈内容 20150123
                remark = "签呈内容：\n";
                foreach (var tc in qctc.OrderBy(o=>o.order)) {
                    remark += (string.IsNullOrEmpty(tc.plan) ? "" :  tc.order.Value.ToString()+"." + tc.plan + "\n");
                }
                #endregion
                m.remark = remark;

                for (int i = 0; i < qcrs.Count; i++)
                {
                    result += i.ToString() + ".";
                    result += "批示内容：" + (string.IsNullOrEmpty(qcrs[i].QC_ReplyContent) ? "-" : qcrs[i].QC_ReplyContent) + ",";
                    result += "是否通过：" + (((qcrs[i].QC_ReplyPass.HasValue) ? (qcrs[i].QC_ReplyPass.Value ? "是" : "否") : "否") + ",");
                    result += "审批人：" + ((qcrs[i].QC_ReplyMan == null) ? "-" : SystemBLL.UserBLL.getUser(int.Parse(qcrs[i].QC_ReplyMan)).cUserName) + ",";
                    result += "审批日期：" + ((qcrs[i].QC_ReplyDate.HasValue) ? qcrs[i].QC_ReplyDate.Value.ToShortDateString() : "-");
                    result += "<br/>";
                }
                
                m.resultTitle = result;
                var resultlst = qcrs;
                if (null != resultlst && resultlst.Count > 0)
                {
                    QC_Replys r1 = resultlst.OrderByDescending(r => r.AutoId).First();
                    resultPass = r1.QC_ReplyPass.HasValue ? (r1.QC_ReplyPass.Value ? "同意" : "不同意") : "未处理";
                }
                else resultPass = "未提交";
                m.result = resultPass;
            }
        }

        public ActionResult EditClass()
        {
            QCClassDB qcc = QCClassDB.Create();
            IEnumerable<QC_Class> Model = qcc.list();
            return View(Model);
        }

        /// <summary>
        /// 单签呈打印
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Output(int id)
        {
            QC_Main m = new QC_Main();
            QCMainDB qcm = new QCMainDB();
            m = qcm.single(id);
            if (null != m)
            {
                string remark = string.Empty;
                string result = string.Empty;
                string resultPass = string.Empty;
                QCMainText(id, out remark, out result,out resultPass);
                m.remark = remark;
                m.resultTitle = result;
                m.result = resultPass;
                m.cModifyMan = string.IsNullOrEmpty(m.cModifyMan) ? m.cCreateMan : m.cModifyMan;
                m.cResult = resultPass;
            }
            return View(m);
        }
        
        /// <summary>
        /// 输出列表到excel
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ActionResult OutputList(string param)
        {
            Dictionary<string,string> Params = new Dictionary<string,string>();
            string[] kv = new string[2];
            if (!string.IsNullOrEmpty(param))
                foreach (string p in param.Split(';'))
                {
                    kv = p.Split(',');
                    Params.Add(kv[0], kv[1]);
                }
            string key = Params.ContainsKey("searchKey") ? Params["searchKey"].ToString() : null;
            string MstartDate = Params.ContainsKey("MstartDate") ? Params["MstartDate"].ToString() : null;
            string MfinishDate = Params.ContainsKey("MfinishDate") ? Params["MfinishDate"].ToString() : null;
            string Memployee = Params.ContainsKey("Memployee") ? Params["Memployee"].ToString() : null;
            string RstartDate = Params.ContainsKey("RstartDate") ? Params["RstartDate"].ToString() : null;
            string RfinishDate = Params.ContainsKey("RfinishDate") ? Params["RfinishDate"].ToString() : null;
            string Remployee = Params.ContainsKey("Remployee") ? Params["Remployee"].ToString() : null;
            
            QCMainDB qcm = new QCMainDB();
            //条件过滤结果
            List<v_petitionList> vPetitionList = qcm.getFiltedvPetitionList(key, Memployee, MstartDate, MfinishDate, Remployee, RstartDate, RfinishDate);

            return Download(vPetitionList);
        }

        public ActionResult Download(List<v_petitionList> list)
        {
            InitializeWorkbook();
            GenerateData(list); 
            var fileStream = new System.IO.MemoryStream();
            GetExcelStream().WriteTo(fileStream);
            //下一句很关键,否则输入0字节;
            fileStream.Position = 0;
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Petition",
                cClass = "PetitionController",
                cFunction = "Download",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(list)
//                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(d1)
            });
            return File(fileStream, "application/ms-excel", "petitionList"+DateTime.Now.DayOfYear.ToString()+".xls");
        }

        #region 生成Excel
        HSSFWorkbook hssfworkbook;
        MemoryStream GetExcelStream()
        { 
            //Write the stream data of workbook to the root directory
            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);
            return file;
        }
        void InitializeWorkbook()
        {
            hssfworkbook = new HSSFWorkbook();

            ////create a entry of DocumentSummaryInformation
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "EAMS GenerateData";
            hssfworkbook.DocumentSummaryInformation = dsi;

            ////create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "查寻签呈列表 PetitionList";
            si.ApplicationName = "EAMS";
            si.Title = "查寻签呈列表";
            hssfworkbook.SummaryInformation = si;
        }
        void getHeader(ref IRow header)
        {
            header.CreateCell(0).SetCellValue("序号");//Id
            header.CreateCell(1).SetCellValue("编码");//cCode
            header.CreateCell(2).SetCellValue("创建人");//cCreateMan
            header.CreateCell(3).SetCellValue("创建时间");//dCreateDate
            header.CreateCell(4).SetCellValue("客户经理");//cCusManager
            header.CreateCell(5).SetCellValue("修改人");//cModifyMan
            header.CreateCell(6).SetCellValue("修改日期");//dModifyDate
            header.CreateCell(7).SetCellValue("客户编码");//cCusCode
            header.CreateCell(8).SetCellValue("客户名称");//cCusName
            header.CreateCell(9).SetCellValue("客户联系人");//cCusPerson
            header.CreateCell(10).SetCellValue("失效日期");//dExpDate
            header.CreateCell(11).SetCellValue("生效日期");//dEffDate
            header.CreateCell(12).SetCellValue("主表备注");//mainMemo
            header.CreateCell(13).SetCellValue("项目分类号");//QC_RCID
            header.CreateCell(14).SetCellValue("单据编码");//QC_OrderCode
            header.CreateCell(15).SetCellValue("存货编码");//cInvCode
            header.CreateCell(16).SetCellValue("存货名称");//cInvName
            header.CreateCell(17).SetCellValue("存货型号");//cInvStd
            header.CreateCell(18).SetCellValue("数量");//iQuentity
            header.CreateCell(19).SetCellValue("单位");//iPrice
            header.CreateCell(20).SetCellValue("金额");//iSum
            header.CreateCell(21).SetCellValue("子表备注");//detailsMemo
            header.CreateCell(22).SetCellValue("是否代收款");//bDaiShouKuan
            header.CreateCell(23).SetCellValue("累计销售起始日期");//dSaleSumStartDate
            header.CreateCell(24).SetCellValue("累计销售截止日期");//dSaleSumFinishDate
            header.CreateCell(25).SetCellValue("提交人编码");//QC_SubmitMan
            header.CreateCell(26).SetCellValue("提交日期");//QC_SubmitDate
            header.CreateCell(27).SetCellValue("审批人编码");//QC_ReplyMan
            header.CreateCell(28).SetCellValue("审批日期");//QC_ReplyDate
            header.CreateCell(29).SetCellValue("审批内容");//QC_ReplyContent
            header.CreateCell(30).SetCellValue("是否通过");//QC_ReplyPass
            header.CreateCell(31).SetCellValue("下一审批ID");//QC_NextID
            header.CreateCell(32).SetCellValue("提交人");//submitMan
            header.CreateCell(33).SetCellValue("审批人");//replyMan
        }
        void setBodyRow(ref IRow row, v_petitionList once)
        {
            row.CreateCell(0).SetCellValue(once.Id);//序号
            row.CreateCell(1).SetCellValue(once.cCode);//编码
            row.CreateCell(2).SetCellValue(once.cCreateMan);//创建人
            row.CreateCell(3).SetCellValue(once.dCreateDate.Value.ToShortDateString());//创建时间
            row.CreateCell(4).SetCellValue(once.cCusManager);//客户经理
            row.CreateCell(5).SetCellValue(once.cModifyMan);//修改人
            row.CreateCell(6).SetCellValue(once.dModifyDate.Value.ToShortDateString());//修改日期
            row.CreateCell(7).SetCellValue(once.cCusCode);//客户编码
            row.CreateCell(8).SetCellValue(once.cCusName);//客户名称
            row.CreateCell(9).SetCellValue(once.cCusPerson);//客户联系人
            row.CreateCell(10).SetCellValue(once.dExpDate.HasValue ? once.dExpDate.Value.ToShortDateString() : null);//失效日期
            row.CreateCell(11).SetCellValue(once.dEffDate.HasValue ? once.dEffDate.Value.ToShortDateString() : "");//生效日期
            row.CreateCell(12).SetCellValue(once.mainMemo);//主表备注
            row.CreateCell(13).SetCellValue(once.QC_RCID.Value);//项目分类号
            row.CreateCell(14).SetCellValue(once.QC_OrderCode);//单据编码
            row.CreateCell(15).SetCellValue(once.cInvCode);//存货编码
            row.CreateCell(16).SetCellValue(once.cInvName);//存货名称
            row.CreateCell(17).SetCellValue(once.cInvStd);//存货型号
            row.CreateCell(18).SetCellValue((double)(once.iQuentity ?? 0));//数量
            row.CreateCell(19).SetCellValue((double)(once.iPrice ?? 0));//单位
            row.CreateCell(20).SetCellValue((double)(once.iSum ?? 0));//金额
            row.CreateCell(21).SetCellValue(once.detailsMemo);//子表备注
            row.CreateCell(22).SetCellValue(once.bDaiShouKuan ?? false);//是否代收款
            row.CreateCell(23).SetCellValue(once.dSaleSumStartDate.HasValue ? once.dSaleSumStartDate.Value.ToShortDateString() : "");//累计销售起始日期
            row.CreateCell(24).SetCellValue(once.dSaleSumFinishDate.HasValue ? once.dSaleSumFinishDate.Value.ToShortDateString() : "");//累计销售截止日期
            row.CreateCell(25).SetCellValue(once.QC_SubmitMan);//提交人编码
            row.CreateCell(26).SetCellValue(once.QC_SubmitDate.HasValue ? once.QC_SubmitDate.Value.ToShortDateString() : "");//提交日期
            row.CreateCell(27).SetCellValue(once.QC_ReplyMan);//审批人编码
            row.CreateCell(28).SetCellValue(once.QC_ReplyDate.HasValue ? once.QC_ReplyDate.Value.ToShortDateString() : "");//审批日期
            row.CreateCell(29).SetCellValue(once.QC_ReplyContent);//审批内容
            row.CreateCell(30).SetCellValue(once.QC_ReplyPass ?? false);//是否通过
            row.CreateCell(31).SetCellValue(once.QC_NextID ?? -1);//下一审批ID
            row.CreateCell(32).SetCellValue(once.submitMan);//提交人
            row.CreateCell(33).SetCellValue(once.replyMan);//审批人

        }
        void GenerateData(List<v_petitionList> lst)
        {
            ISheet sheet1 = hssfworkbook.CreateSheet("Sheet1");

            //header
            //sheet1.CreateRow(0).CreateCell(0).SetCellValue("This is a Sample");
            IRow rHeader=sheet1.CreateRow(0);
            getHeader(ref rHeader);

            
            //bodyer
            int x = 1;
            foreach (v_petitionList p in lst)
            {
                IRow row = sheet1.CreateRow(x++);
                setBodyRow(ref row, p);
            }
            //for (int i = 1; i <= 15; i++)
            //{
            //    IRow row = sheet1.CreateRow(i);
            //    for (int j = 0; j < 15; j++)
            //    {
            //        row.CreateCell(j).SetCellValue(x++);
            //    }
            //}
        }
        #endregion
    }
}
