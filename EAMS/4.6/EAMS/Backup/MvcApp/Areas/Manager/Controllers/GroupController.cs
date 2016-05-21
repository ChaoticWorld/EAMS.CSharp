using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemDB;
using SystemBLL;

namespace MvcApp.Areas.Manager.Controllers
{
    [AuthorizeEx(Roles="Admin")]
    public class GroupController : Controller
    {
        //
        // GET: /Manager/Group/
        #region Index
        public ActionResult Index()
        {
            GroupsBLL groupBll = new GroupsBLL();
            IEnumerable<Group> groupList = groupBll.Lists("");
            if (Request.IsAjaxRequest())
                return PartialView("groupList", groupList);
            return View(groupList);
        }
        [HttpPost]
        public ActionResult Index(FormCollection f)
        {
            string gfk = (f.AllKeys.Contains("filterkey")) ? f["filterkey"] : null;
            GroupsBLL groupBll = new GroupsBLL();
            var gl = (IEnumerable<Group>)groupBll.Lists();
            IEnumerable<Group> groupList = gl.Where(w => w.groupName.Contains(gfk) || w.groupDescription.Contains(gfk));
            if (Request.IsAjaxRequest())
                return PartialView("groupList",groupList);
            return View(groupList);
        }
        [HttpPost]
        public ContentResult Save(FormCollection f) {
            Group g = new Group();

            g.groupid = (f.AllKeys.Contains("id")) ? int.Parse(f["id"]) : -1;
            g.groupName = (f.AllKeys.Contains("name")) ? f["name"] : "groupName-" + g.groupid.ToString();
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
        public ContentResult saveItem(Group group, bool isMulti)
        {
            int id = group.groupid;
            GroupsBLL gBll = new GroupsBLL();
            bool r = false;

            if (id < 0)
            {//新增
                r = !string.IsNullOrEmpty(gBll.Add(group));
            }
            else
            {//更新 
                r = gBll.Update(group) > 0;
            }
            if (r)
                return new ContentResult() { Content = isMulti ? "" : "保存成功!" };
            else return new ContentResult() { Content = isMulti ? "" : "保存失败" };
        }
        public ContentResult deleteItem(Group group, bool isMulti)
        {
            GroupsBLL gBll = new GroupsBLL();
            bool r = true;
            r &= (gBll.Delete(group.groupid) > 0);
            if (r)
                return new ContentResult() { Content = isMulti ? "" : "删除成功！" };
            else
                return new ContentResult() { Content = isMulti ? "" : "删除失败" };
        }

        #endregion
        #region GroupRefUser
        public ActionResult groupRefUser(int id)
        {
            GroupsBLL groupBll = new GroupsBLL();
            Group g; 
            List<User> uList = new List<User>();
            UserBLL uBll = new UserBLL();
            uList = uBll.Lists("").ToList();
            if (id > 0)
            {
                g = (Group)groupBll.Single(id);
                foreach (User u in g.Users)
                    uList.Remove(u);
            }
            else { g = new Group(); }
            ViewBag.UnSelUsers = uList;
            return View(g);
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
            dbUserGroupRefs dbugr = new dbUserGroupRefs();
            List<int> SeledUsers = new List<int>();
            foreach (string s in selUsers) { SeledUsers.Add(int.Parse(s)); }
            dbugr.saveGroup(groupid, SeledUsers, adminId);
            
            return RedirectToAction("Index");
        }

        #endregion
    }
}
