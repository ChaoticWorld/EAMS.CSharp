using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcApp.Models;
using UserInfo;
using UserBLL;
using Logs;
using OrganizationBase;

namespace MvcApp.Areas.Manager.Controllers
{
    public class AccountController : Controller
    {
        SysLogsDataAccess logonLog = new SysLogsDataAccess();
        userBLL userBll = new userBLL();
        UserModel currUser;
        //
        // GET: /Manager/Account/

        //MVC 窗体身份验证及角色权限管理示例
        //http://www.cnblogs.com/ldp615/archive/2010/10/27/asp-net-mvc-forms-authentication-roles-authorization-demo.html

        [AuthorizeEx(Roles = "Admin")]
        public ActionResult Index()
        {
            IList<UserModel> ums = userBll.selects(null).OrderBy(o=>o.cUserCode).ToList();
            return View(ums);
        }

        public ActionResult LogOff()
        {
            int uid = int.Parse(System.Web.HttpContext.Current.User.Identity.Name);
            currUser = userBll.single(uid);
            logonLog.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Account",
                cClass = "AccountController",
                cFunction = "LogOff",
                cReturn = "Log out!"
            });
            FormsAuthentication.SignOut();
            Session.Abandon();
            System.Web.HttpContext.Current.User = null;
            return Redirect(Request.ApplicationPath);
        }

        [AllowAnonymous]
        public ActionResult Logon()
        {
            var model = new LogonModel();
            if (AuthorizeExAttribute.Pass)
            {
                TempData["Msg"] = "登录成功";
            }
            else
            {
                TempData["Msg"] = "认证未通过！";
            }
            model.logoned = AuthorizeExAttribute.Pass;
            return View(model);
        }

        /// <summary>
        /// 登录,关键代码部分1+部分2:Global.asax.cs_MvcApplication_AuthorizeRequest
        /// </summary>
        /// <param name="u"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Logon(LogonModel logonModel)
        {
            string returnUrl = AuthorizeExAttribute.AuthorBeforeReturnURL;
            UserModel u = LogonModel.getAuthentication(logonModel);
            string Roles = string.Empty;
            if (ModelState.IsValid)
            {
                if (u != null)
                {
                    Roles = userBll.getStrRoles(u.iUserId);
                    HttpCookie authCookie = FormsAuthentication.GetAuthCookie(u.iUserId.ToString(), true);
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    FormsAuthenticationTicket newticket = new FormsAuthenticationTicket(
                        ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration,
                        ticket.IsPersistent, Roles //写入用户角色
                        );
                    authCookie.Value = FormsAuthentication.Encrypt(newticket);
                    Response.Cookies.Add(authCookie);
                    logonLog.createLog(new LogModel()
                    {
                        iUserID = u.iUserId,
                        cUserName = u.cUserName,
                        cModule = "Account",
                        cClass = "AccountController",
                        cFunction = "Logon",
                        cReturn = "Log on!"
                    });
                    logonModel.logoned = true;
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", "已登录，无返回地址！");
                        return Redirect(this.HttpContext.Request.ApplicationPath);
                    }
                }
                else
                    ModelState.AddModelError("", "用户名或密码不正确！");
            }
            return View(logonModel);

        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult LogonEx(LogonModel logonModel)
        {
            JsonResult r = new JsonResult();
            UserModel u = LogonModel.getAuthentication(logonModel);

            long uid = (u != null) ? u.iUserId : -1;
            r.Data = uid;
            return r;

        }

        [AuthorizeEx]
        public ActionResult Edit(int id)
        {
            UserModel um = userBll.single(id);
            TempData["returnUrl"] = Request.UrlReferrer.AbsolutePath;
            return View(um);
        }
        [AuthorizeEx]
        [HttpPost]
        public ActionResult Edit(UserModel u, FormCollection f)
        {
            string rUrl = f["RUrl"];
            if (ModelState.IsValid)
            {
                try
                {
                    userBll.update(u);
                    return Redirect(rUrl);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "保存失败！" + e.Message);
                    return View(u);
                }
            }
            else
            {
                ModelState.AddModelError("", "输入错误！");
                return View(u);
            }
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View(new RegisterModel());
        }
        [HttpPost]
        public ActionResult Register(RegisterModel newuser)
        {
            string addNewUserErrMsg = string.Empty;
            string newId = string.Empty;
            UserModel nuser = new UserModel();
            nuser.cUserCode = newuser.Code;
            nuser.cUserName = newuser.Name;
            nuser.cUserMobile = newuser.Mobile;
            nuser.cUserPassWord = newuser.pw;
            nuser.cUserEMail = newuser.Email;
            try
            {
                newId = userBll.Register(nuser).ToString();
            }
            catch (Exception e)
            {
                addNewUserErrMsg = e.Message;
            }
            currUser = userBll.single(int.Parse(System.Web.HttpContext.Current.User.Identity.Name));
            logonLog.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Account",
                cClass = "AccountController",
                cFunction = "Register",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(newuser),
                cException = addNewUserErrMsg,
                cReturn = string.IsNullOrEmpty(newId).ToString()
            });
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(newId))
                {
                    ViewData["registerMsg"] = "创建用户成功,请登录!";
                    return Redirect(HttpContext.Request.ApplicationPath);
                }
                else
                {
                    ViewData["registerMsg"] = "创建用户失败！" + addNewUserErrMsg;
                    return View(newuser);
                }
            }
            else return View(newuser);
        }
        [AuthorizeEx]
        public ActionResult ReSecurity(int id)
        { return View(); }
        [AuthorizeEx]
        [HttpPost]
        public ActionResult ReSecurity(ReSecurity rs)
        {
            UserModel u = userBll.single(rs.id);
            if (rs.oldpw == "_eamspasswordreset_")
            {
                u.cUserPassWord = rs.pw;
                userBll.update(u);
                return RedirectToAction("Edit", new { id = rs.id });
            }
            if (u.cUserPassWord == rs.oldpw)
            {
                u.cUserPassWord = rs.pw;
                userBll.update(u);
                return RedirectToAction("Edit", new { id = rs.id });
            }
            else { ModelState.AddModelError("", "旧密码不正确！"); return View(rs); }
        }

        [AuthorizeEx(Roles = "Admin")]
        public ActionResult Details(long id)
        {
            TempData["UserID"] = id;
            roleDataAccess roleDA = new roleDataAccess();
            UserModel u = userBll.single(id);
            TempData["UserName"] = u.cUserName;
            List<roleModel> selRoles = userBll.getRoles(id).ToList();
            List<roleModel> unselRoles = new List<roleModel>();
            var allRoles = roleDA.selects(null);
            foreach (roleModel r in allRoles)
            {
                if (!selRoles.Exists(sr => sr.iRoleId == r.iRoleId))
                    unselRoles.Add(r);
            }
            ViewBag.SelectedRoles = selRoles;
            ViewBag.UnSelectRoles = unselRoles;
            return View();
        }
        [HttpPost]
        public ActionResult Details(FormCollection f)
        {
            roleRefDataAccess roleRefDA = new roleRefDataAccess();
            int userID = int.Parse(f["UserID"]);
             int rid = -1;
            string unselRoles = string.Empty;
            var dbRoles = roleRefDA.getOrganization(userID).Select(s => s.iRoleId).ToList().ConvertAll<string>(c=>c.ToString()).AsEnumerable();

            if (f.AllKeys.Contains("UnSelectRoleList"))
            {
                unselRoles =
                   (f["UnSelectRoleList"] == null) ? ","
                    : f["UnSelectRoleList"].ToString() + ",";
                foreach (string id in unselRoles.Split(',').Intersect(dbRoles))
                {
                    if (int.TryParse(id, out rid))
                    {
                        var roleRefs = roleRefDA.selects(new roleRefModel() { iUserId = userID, iRoleId = rid });
                        foreach (roleRefModel rfModel in roleRefs) roleRefDA.Delete(rfModel.iURRefId);
                    }
                }
            }
            string selRoles = string.Empty;
            if (f.AllKeys.Contains("SelectedRoleList"))
            {
                selRoles =
                   (f["SelectedRoleList"] == null) ? ","
                    : f["SelectedRoleList"].ToString() + ",";

                foreach (string id in selRoles.Split(',').Except(dbRoles))
                    if (int.TryParse(id, out rid))
                        roleRefDA.Create(new roleRefModel() { iUserId = userID, iRoleId = rid });
            }

            return RedirectToAction("Index");
        }


        [AuthorizeEx(Roles = "Admin")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            int r = -1;
            r = userBll.delete(id);

            return Json(r);
        }
    }
}
