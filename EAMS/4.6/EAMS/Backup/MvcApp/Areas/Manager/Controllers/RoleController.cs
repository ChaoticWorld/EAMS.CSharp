using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemBLL;
using SystemDB;

namespace MvcApp.Areas.Manager.Controllers
{
    [AuthorizeEx(Roles = "Admin")]
    public class RoleController : Controller
    {
        //
        // GET: /Manager/Role/
        public ActionResult Index()
        {
            SystemBLL.RolesBLL r = new SystemBLL.RolesBLL();
            return View(r.Lists());
        }

        //
        // GET: /Manager/Role/Details/5

        /// <summary>
        /// 分配角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            TempData["RoleID"] = id;
            UserBLL Allu= new UserBLL();
            RolesBLL rb = new RolesBLL();
            Role r = rb.Single(id) as Role;
            TempData["RoleName"] = r.cRoleName;
            List<User> selUsers = r.Users;
            List<User> unselUsers = new List<SystemDB.User>();
            var allUsers = Allu.Lists("").ToList();
            foreach (User u in allUsers)
            {
                if (!selUsers.Exists(su => su.iUserId == u.iUserId))
                    unselUsers.Add(u);
            }
            ViewBag.SelectedUsers = selUsers;
            ViewBag.UnSelectUsers = unselUsers;
            return View();
        }

        [HttpPost]
        public ActionResult Details(FormCollection f)
        {
            int roleID = int.Parse(f["roleID"]);
            string selUsers = f["SelectedUserList"];
            List<int> selUsersID = new List<int>();
            foreach(string id in selUsers.Split(','))
                selUsersID.Add(int.Parse(id));

            dbUserRoleRefs urr = new dbUserRoleRefs();
            urr.saveRole(roleID, selUsersID);

            return RedirectToAction("Index");
        }

        //
        // GET: /Manager/Role/Create

        public ActionResult Create()
        {
            return View(new Role());
        }

        //
        // POST: /Manager/Role/Create

        [HttpPost]
        public ActionResult Create(Role r)
        {
            RolesBLL rb = new RolesBLL();
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(rb.Add(r)))
                        return RedirectToAction("Index");
                    else { ModelState.AddModelError("", rb.returnMsg); return View(); }
                }
                else { ModelState.AddModelError("", rb.returnMsg); return View(); }
            }
            catch
            {
                 ModelState.AddModelError("", rb.returnMsg); return View(); 
            }
        }

        //
        // GET: /Manager/Role/Edit/5

        public ActionResult Edit(int id)
        {
            Role r = (new RolesBLL()).Single(id) as Role;
            return View(r);
        }

        //
        // POST: /Manager/Role/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, Role r)
        {
            RolesBLL rb = new RolesBLL();
            int rn = -1;
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add update logic here
                    rn = rb.Update(r);
                    if (rn > 0)
                    {
                        Response.Write("<script type='JaveScript/text'>alert('保存成功，记录数：" + rn.ToString() + ".')</script>");
                        return RedirectToAction("Index"); }
                    else
                    {
                        ModelState.AddModelError("无更新！", rb.returnMsg); return View(r);
                    }
                }
                catch
                {
                    ModelState.AddModelError("无更新！", rb.returnMsg); return View(r);
                }
            }
            else
            {
                ModelState.AddModelError("输入有误！", rb.returnMsg); return View(r);
            }
        }

        //
        // GET: /Manager/Role/Delete/5

        public ActionResult Delete(int id)
        {
            string rUrl = Request.UrlReferrer.AbsolutePath;
            
            RolesBLL rb = new RolesBLL();
            int rn = -1;
            try
            {
                rn = rb.Delete(id);

                Response.Write("<script type='JaveScript/text'>alert('删除成功，记录数：" + rn.ToString() + ".')</script>");
                Redirect(rUrl);
            }
            catch (System.Data.OptimisticConcurrencyException e)
            {
                ModelState.AddModelError("删除失败！", e.Message); return Redirect(rUrl);
            }
            return Redirect(rUrl);
        }

        //
        // POST: /Manager/Role/Delete/5

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
    }
}
