using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using System.Web.Routing;
using System.Web.Security;
using SystemBLL;
using SystemDB;
using MvcApp.Areas.Manager.Models;

namespace MvcApp.Areas.Manager.Controllers
{
    public class AccountController : Controller
    {

        static LogsBLL logbll = new LogsBLL();
        SystemDB.User currUser;
        //
        // GET: /Manager/Account/

        //MVC 窗体身份验证及角色权限管理示例
        //http://www.cnblogs.com/ldp615/archive/2010/10/27/asp-net-mvc-forms-authentication-roles-authorization-demo.html
        
        [AuthorizeEx(Roles="Admin")]
        public ActionResult Index()
        {
            UserBLL u = new UserBLL();
            return View(u.Lists(""));
        }

        public ActionResult LogOff()
        {
            //UserAuthen.RemoveAuthorizeCookie(this.HttpContext);
            FormsAuthentication.SignOut();
            Session.Abandon();
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Account",
                cClass = "AccountController",
                cFunction = "LogOff",
                cReturn = "Log out!"
            });
            return Redirect(Request.ApplicationPath);
        }

        [AllowAnonymous]
        public ActionResult Logon()
        {
            
            if (!AuthorizeExAttribute.Pass) TempData["Msg"] = "认证未通过！";
            else TempData["Msg"] = "";
            return View(new LogonModel()); 
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
            SystemDB.User u = LogonModel.getUser(logonModel);
            int userID =-1;
            SystemDB.User _user = null;
            SystemDB.dbUser dbuser = new SystemDB.dbUser();

            if (ModelState.IsValid)
            {
                userID = dbuser.Authentication(u);
                if (userID >= 0)
                    _user = (SystemDB.User)dbuser.single(userID);
                if (_user != null)
                {
                    HttpCookie authCookie = FormsAuthentication.GetAuthCookie(_user.iUserId.ToString(), true);
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    FormsAuthenticationTicket newticket = new FormsAuthenticationTicket(
                        ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration,
                        ticket.IsPersistent, _user.RoleNames //写入用户角色
                        );
                    authCookie.Value = FormsAuthentication.Encrypt(newticket);
                    Response.Cookies.Add(authCookie);
                    //currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
                    logbll.Add(new SystemDB.Logs()
                    {
                        iUserID = _user.iUserId,
                        cUserName = _user.cUserName,
                        cModule = "Account",
                        cClass = "AccountController",
                        cFunction = "Logon",
                        cReturn = "Log on!"
                    });
                    if (!String.IsNullOrEmpty(returnUrl)) {                       
                        return Redirect(returnUrl); 
                    }
                    else
                    {
                        ModelState.AddModelError("", "权限不正确！");
                        return Redirect(this.HttpContext.Request.ApplicationPath);
                    }
                }
                else
                    ModelState.AddModelError("", "用户名或密码不正确！");
            }
            return View(logonModel);
 
        }
        [AuthorizeEx]
        public ActionResult Edit(int id)
        {
            SystemBLL.UserBLL ub = new UserBLL();
            SystemDB.User u = ub.Single(id) as SystemDB.User;
            TempData["returnUrl"] = Request.UrlReferrer.AbsolutePath;
            return View(u);
        }
        [AuthorizeEx]
        [HttpPost]
        public ActionResult Edit(SystemDB.User u,FormCollection f)
        {
            string rUrl = f["RUrl"];
            UserBLL ub = new UserBLL();
            if (ModelState.IsValid)
            {
                try
                {
                    ub.Update(u);
                    return Redirect(rUrl);
                }
                catch (System.Data.OptimisticConcurrencyException e)
                {
                    ModelState.AddModelError("", "保存失败！"+e.Message);
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
            SystemDB.dbUser dbu = new SystemDB.dbUser();
            SystemDB.User nuser = new SystemDB.User();
            nuser.cUserCode = newuser.Code;
            nuser.cUserName = newuser.Name;
            nuser.cUserMobile = newuser.Mobile;
            nuser.cUserPassWord = newuser.pw;
            nuser.cUserEMail = newuser.Email;
            try
            {
                newId = dbu.add(nuser);
            }
            catch (Exception e)
            {
                addNewUserErrMsg = e.Message;
            }
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
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
                    ViewData["registerMsg"] = "创建用户失败！"+addNewUserErrMsg;
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
            //User u = new User() { iUserId = int.Parse(this.HttpContext.User.Identity.Name), cUserPassWord = rs.pw };
            UserBLL ub = new UserBLL();
            User u = ub.Single(rs.id) as User;
            if (rs.oldpw == "_eamspasswordreset_")
            { u.cUserPassWord = rs.pw; ub.Update(u); return RedirectToAction("Edit", new { id = rs.id }); }
            if (u.cUserPassWord == rs.oldpw)
            {
                u.cUserPassWord = rs.pw;
                ub.Update(u);
                return RedirectToAction("Edit", new { id = rs.id });
            }
            else { ModelState.AddModelError("", "旧密码不正确！"); return View(rs); }
        }

        [AuthorizeEx(Roles="Admin")]
        public ActionResult Details(int id)
        {
            TempData["UserID"] = id;
            UserBLL ub = new UserBLL();
            RolesBLL allrb = new RolesBLL();
            SystemDB.User u = ub.Single(id) as User;
            TempData["UserName"] = u.cUserName;
            List<Role> selRoles = u.Roles;
            List<Role> unselRoles = new List<SystemDB.Role>();
            var allRoles = allrb.Lists().ToList();
            foreach (Role r in allRoles)
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
            int userID = int.Parse(f["UserID"]);
            string selRoles = string.Empty;
            if (f.AllKeys.Contains("SelectedRoleList"))
            {
                selRoles =
                   (f["SelectedRoleList"] == null) ? ","
                    : f["SelectedRoleList"].ToString() + ",";
            }
            else selRoles = ",";
            List<int> selRolesID = new List<int>();
            int rid = -1;
            foreach (string id in selRoles.Split(','))
                if (int.TryParse(id, out rid))
                    selRolesID.Add(rid);
            
            dbUserRoleRefs urr = new dbUserRoleRefs();
            urr.saveUser(userID, selRolesID);

            return RedirectToAction("Index");
        }

        
        [AuthorizeEx(Roles="Admin")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            int r = -1;
            UserBLL ub = new UserBLL();
            if (ub.preDelete(id))
                r = -2;
            else
                r = ub.Delete(id);
            return Json(r);
        }
    }
}
