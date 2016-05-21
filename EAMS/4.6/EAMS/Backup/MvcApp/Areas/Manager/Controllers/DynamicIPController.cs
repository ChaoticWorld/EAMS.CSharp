using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemBLL;
using DB.DynamicIP;

namespace MvcApp.Areas.Manager.Controllers
{
    public class DynamicIPController : Controller
    {

        DBBLL dbBll = new DBBLL();
        //
        // GET: /Default/

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RegisterIP(FormCollection f)
        {
            string Key = string.Empty;
            string IP = string.Empty;
            string savestate = "";
            if (f.AllKeys.Contains("Key") && !string.IsNullOrEmpty(f["Key"]))
                Key = f["Key"].ToString();
            if (f.AllKeys.Contains("ip") && !string.IsNullOrEmpty(f["ip"]))
                IP = f["ip"].ToString();
            savestate = dbBll.registerIP(Key, IP);

            JsonResult rJson = new JsonResult();
            rJson.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            rJson.MaxJsonLength = 64;
            rJson.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            rJson.Data = savestate;
            return rJson;
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult getIP(FormCollection f)
        {
            JsonResult rJson = new JsonResult();
            rJson.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            rJson.MaxJsonLength = int.MaxValue;
            string keys = f.AllKeys.Contains("Keys") ? f["Keys"] : "";
            keys += ",";
            vpn_Register vpn;
            List<vpn_Register> r = new List<vpn_Register>();
            string[] Keys = keys.Split(',');
            foreach (string k in Keys)
            {
                if ((!string.IsNullOrEmpty(k)) && dbBll.Exist(key: k))
                {
                    vpn = dbBll.signal(k);
                    r.Add(vpn);
                }
            }

            rJson.Data = r;
            return rJson;
        }
        [AllowAnonymous]
        public ActionResult test()
        {
            return View();
        }

        [AuthorizeEx(Roles = "Admin")]
        public ActionResult Index()
        {
            IEnumerable<vpn_Register> model = dbBll.filter();
            
            var t = model.ToList();
            return View(model);
            //return View();
        }

        [AuthorizeEx(Roles = "Admin")]
        [HttpPost]
        public JsonResult getUpdatedRow(int id) {
            JsonResult r = new JsonResult();
            r.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            r.MaxJsonLength = int.MaxValue;

            vpn_Register vpn = dbBll.signal(id);

            r.Data = vpn;
            return r;
        }

        [AuthorizeEx(Roles = "Admin")]
        [HttpPost]
        public ActionResult Update(FormCollection f)
        {//{ autoid: id, name: name, vpnid: vpnid, vpnpw: vpnpw, mac: mac, ip: ip, EncryptionType: EncryptionType }
            vpn_Register reg = new vpn_Register();
            if (f.AllKeys.Contains("name") && !string.IsNullOrEmpty(f["name"]))
                reg.Name = f["name"].ToString();
            if (f.AllKeys.Contains("vpnid") && !string.IsNullOrEmpty(f["vpnid"]))
                reg.vpnID = f["vpnid"].ToString();
            if (f.AllKeys.Contains("vpnpwd") && !string.IsNullOrEmpty(f["vpnpwd"]))
                reg.vpnPW = f["vpnpwd"].ToString();
            if (f.AllKeys.Contains("mac") && !string.IsNullOrEmpty(f["mac"]))
                reg.vpnMac = f["mac"].ToString();
            if (f.AllKeys.Contains("ip") && !string.IsNullOrEmpty(f["ip"]))
                reg.vpnIP = f["ip"].ToString();
            if (f.AllKeys.Contains("vpnEncryptionType") && !string.IsNullOrEmpty(f["vpnEncryptionType"]))
                reg.vpnEncryptionType = f["vpnEncryptionType"].ToString();
            if (f.AllKeys.Contains("autoid") && !string.IsNullOrEmpty(f["autoid"]))
                reg.autoid = int.Parse(f["autoid"].ToString());

            int savestate = -1;
            if (reg.autoid < 0)
                savestate = dbBll.add(reg);
            else
                savestate = dbBll.update(reg);

            JsonResult rJson = new JsonResult();
            rJson.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            rJson.MaxJsonLength = 64;
            rJson.Data = savestate;
            return rJson;
        }

        [AuthorizeEx(Roles = "Admin")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            int savestate = -1;
            savestate = dbBll.delete(id);

            JsonResult rJson = new JsonResult();
            rJson.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            rJson.MaxJsonLength = 64;
            rJson.Data = savestate;
            return rJson;
        }

    }

}
