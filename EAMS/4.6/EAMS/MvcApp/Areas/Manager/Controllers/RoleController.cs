using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApp.Models;
using UserInfo;
using UserBLL;
using OrganizationBase;

namespace MvcApp.Areas.Manager.Controllers
{
    public class RoleController : Controller
    {
        roleBLL roleBll = new roleBLL();
        roleRefBLL roleRefBll = new roleRefBLL();
        //
        // GET: /Manager/Role/
    [AuthorizeEx(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(roleBll.select(null));
        }

        //
        // GET: /Manager/Role/Details/5

        /// <summary>
        /// 分配角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeEx(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            TempData["RoleID"] = id;
            userBLL userBll= new userBLL();
            roleModel r = roleBll.single(id);
            TempData["RoleName"] = r.cRoleName;
            List<UserModel> selUsers = r.Users.ToList();
            List<UserModel> unselUsers = new List<UserModel>();
            var allUsers = userBll.selects(null).ToList();
            foreach (UserModel u in allUsers)
            {
                if (!selUsers.Exists(su => su.iUserId == u.iUserId))
                    unselUsers.Add(u);
            }
            ViewBag.SelectedUsers = selUsers;
            ViewBag.UnSelectUsers = unselUsers;
            return View();
        }

        [AuthorizeEx(Roles = "Admin")]
        [HttpPost]
        public ActionResult Details(FormCollection f)
        {
            int roleID = int.Parse(f["roleID"]);
            string selUsers = string.Empty;
            string UnselUsers = f.AllKeys.Contains("SelectedUserList") ? (f["UnSelectUserList"] != null ? f["UnSelectUserList"].ToString() : ",") : ",";
            var dbUsers = roleRefBll.select(new roleRefModel() { iRoleId = roleID }).Select(s=>s.iUserId).ToList().ConvertAll<string>(c=>c.ToString());
            int uid = -1;

            if (f.AllKeys.Contains("UnSelectUserList"))
            {
                selUsers = f["UnSelectUserList"] == null ? "," : f["UnSelectUserList"].ToString() + ",";
                foreach (string id in selUsers.Split(',').Intersect(dbUsers))
                    if (int.TryParse(id, out uid))
                    {
                        var roleRefs = roleRefBll.select(new roleRefModel() { iUserId = uid, iRoleId = roleID });
                        foreach (roleRefModel rfModel in roleRefs) roleRefBll.delete(rfModel.iURRefId);
                    }
            }

            if (f.AllKeys.Contains("SelectedUserList"))
            {
                selUsers = f["SelectedUserList"] == null ? "," : f["SelectedUserList"].ToString() + ",";
                foreach (string id in selUsers.Split(',').Except(dbUsers))
                    if (int.TryParse(id, out uid))
                        roleRefBll.create(new roleRefModel() { iRoleId = roleID, iUserId = uid });
            }
            

            return RedirectToAction("Index");
        }

        //
        // GET: /Manager/Role/Create

        [AuthorizeEx(Roles = "Admin")]
        public ActionResult Create()
        {
            return View(new roleModel());
        }

        //
        // POST: /Manager/Role/Create

        [AuthorizeEx(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(roleModel r)
        {
            // TODO: Add insert logic here
            if (ModelState.IsValid)
            {
                try
                {
                    if ((roleBll.create(r)) > 0)
                        return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message); return View();
                }
            }
            return View();          
        }

        //
        // GET: /Manager/Role/Edit/5

        [AuthorizeEx(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            roleModel r = roleBll.single(id);
            return View(r);
        }

        //
        // POST: /Manager/Role/Edit/5

        [AuthorizeEx(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(int id, roleModel r)
        {
            int rn = -1;
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add update logic here
                    rn = roleBll.update(r);
                    if (rn > 0)
                    {
                        Response.Write("<script type='JaveScript/text'>alert('保存成功，记录数：" + rn.ToString() + ".')</script>");
                        return RedirectToAction("Index"); }
                    else return View(r);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("无更新！", e.Message); return View(r);
                }
            }
            else
            {
                ModelState.AddModelError("输入有误！", "输入有误！"); return View(r);
            }
        }

        //
        // GET: /Manager/Role/Delete/5

        [AuthorizeEx(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            string rUrl = Request.UrlReferrer.AbsolutePath;
            
            int rn = -1;
            try
            {
                rn = roleBll.delete(id);
                if (rn > 0)
                {
                    Response.Write("<script type='JaveScript/text'>alert('删除成功，记录数：" + rn.ToString() + ".')</script>");
                    Redirect(rUrl);
                }
                else if (rn ==-2) ModelState.AddModelError("删除失败！", "有关联数据不能删除！");
                else ModelState.AddModelError("删除失败！", "删除失败！");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("删除失败！", e.Message); return Redirect(rUrl);
            }
            return Redirect(rUrl);
        }

        //
        // POST: /Manager/Role/Delete/5

        [AuthorizeEx(Roles = "Admin")]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult Exists(int uid, string roleName)
        {
            JsonResult r = new JsonResult();
            bool isExist = false;
            List<roleModel> roleList = roleBll.getOrganizations(uid).ToList();
            isExist = roleList.Exists(e => e.cRoleName == roleName);
            r.Data = isExist;
            return r;
        }
    }
}
