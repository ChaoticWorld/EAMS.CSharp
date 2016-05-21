using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcApp.Models;
using OrganizationBase;
using UserInfo;
using UserBLL;

namespace MvcApp.Areas.Manager.Controllers
{
    [AuthorizeEx(Roles="Admin")]
    public class GroupController : Controller
    {
        groupBLL gBll = new groupBLL();
        groupRefBLL grBll = new groupRefBLL();
        //
        // GET: /Manager/Group/
        #region Index
        public ActionResult Index()
        {
            IList<groupModel> groupList = gBll.select(null);
            if (Request.IsAjaxRequest())
                return PartialView("groupList", groupList);
            return View(groupList);
        }
        [HttpPost]
        public ActionResult Index(FormCollection f)
        {
            string gfk = (f.AllKeys.Contains("filterkey")) ? f["filterkey"] : null;
            var gl = gBll.select(null);
            IEnumerable<groupModel> groupList = gl.Where(w => w.groupName.Contains(gfk) || w.groupDescription.Contains(gfk));
            if (Request.IsAjaxRequest())
                return PartialView("groupList",groupList);
            return View(groupList);
        }
        [HttpPost]
        public ContentResult Save(FormCollection f) {
            groupModel g = new groupModel();

            g.groupId = (f.AllKeys.Contains("id")) ? int.Parse(f["id"]) : -1;
            g.groupName = (f.AllKeys.Contains("name")) ? f["name"] : "groupName-" + g.groupId.ToString();
            g.groupDescription = (f.AllKeys.Contains("description")) ? f["description"] : "Description-" + g.groupName;
            
            string Operation = f["Operation"].ToString();
            switch (Operation)
            {
                case "SaveRow":
                    return saveItem(g, false);
                case "DeleteRow":
                    return deleteItem(g, false);
                case "SaveALL":
                    return saveItem(g, true);
                case "DeleteALL":
                    return deleteItem(g, true);
                default:
                    return new ContentResult() { Content="default" };
            }
        }
        public ContentResult saveItem(groupModel group, bool isMulti)
        {
            int id = group.groupId;
            bool r = false;

            if (id < 0)
            {//新增
                r = gBll.create(group) > 0;
            }
            else
            {//更新 
                r = gBll.update(group) > 0;
            }
            if (r)
                return new ContentResult() { Content = isMulti ? "" : "保存成功!" };
            else return new ContentResult() { Content = isMulti ? "" : "保存失败" };
        }
        public ContentResult deleteItem(groupModel group, bool isMulti)
        {
            bool r = true;
            r &= (gBll.delete(group.groupId) > 0);
            if (r)
                return new ContentResult() { Content = isMulti ? "" : "删除成功！" };
            else
                return new ContentResult() { Content = isMulti ? "" : "删除失败" };
        }

        #endregion

        #region GroupRefUser
        public ActionResult groupRefUser(int id)
        {
            groupModel gm = new groupModel();

            List<UserModel> uList = new List<UserModel>();
            userBLL uBll = new userBLL();
            uList = uBll.selects(null).ToList();

            if (id > 0 )
            {
                gm = gBll.single(id);
                if (gm != null)
                {
                    foreach (int uid in gm.Users.Select(s=>s.iUserId))
                    {
                        if (uList.Exists(e => e.iUserId == uid))
                            uList.RemoveAll(r => r.iUserId == uid);
                        ViewBag.UnSelUsers = uList;                       
                    }
                    return View(gm);
                }
            }
            return View(new groupModel());
        }
        [HttpPost]
        public ActionResult groupRefUser(FormCollection f)
        {
            int groupid = -1;
            if (f.AllKeys.Contains("id") && !string.IsNullOrEmpty(f["id"]))
                if (!int.TryParse(f["id"], out groupid))
                    groupid = -1;
            int adminId = -1;
            if (f.AllKeys.Contains("AdminId") && !string.IsNullOrEmpty(f["AdminId"]))
                if (!int.TryParse(f["AdminId"], out adminId)) adminId = -1;

            string[] selUsers = (f.AllKeys.Contains("SeledUserList") && !string.IsNullOrEmpty(f["SeledUserList"])) ? f["SeledUserList"].Split(',') : null;

            List<groupRefModel> grModels = new List<groupRefModel>();

            int uid = -1;
            foreach (string s in selUsers) {
                if (int.TryParse(s, out uid))
                    grModels.Add(new groupRefModel() {
                        groupId = groupid, UserId = uid, isManager = (uid == adminId) });
            }

            grBll.Update(grModels);
            
            return RedirectToAction("Index");
        }
        #endregion
    }
}
