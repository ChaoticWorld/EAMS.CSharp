using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using DataAccess;
using report;

namespace cmdShell
{
    class Program
    {
        static void Main(string[] args)
        {
            //test 
            //testEAIErp();
            //testLog();
            //testUser();
            //testOrganization();
            //testAttendance();
            testReport();
        }
        private static void testProcess(string[] args)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i < args.Length; i++)
            {
                sb.Append(args[i] + " ");
            }

            Console.WriteLine("命令参数准备:" + sb.ToString());

            Process p = new Process();
            p.StartInfo.CreateNoWindow = false;
            p.StartInfo.FileName = args[0];
            p.StartInfo.Arguments = sb.ToString();
            p.StartInfo.UseShellExecute = false;

            p.Start();

            Console.WriteLine("程序退出.");
        }
        private static void testEAIErp()
        {
            DataAccess.EAI eai = new EAI();

            eai.iErp = ERPFactory.ERPFactory.Create(AppDomain.CurrentDomain.BaseDirectory);
            eai.iErp.stor = new DB();

            #region Customer Test
            eai.iErp.stor.Customer = ERPFactory.ERPFactory.createCustomer();
            var cus = eai.iErp.stor.Customer.getSingle("0531dfl");
            Console.Write(cus.Code + ";");
            Console.Write(cus.Name + ";");
            Console.Write(cus.CreateDate.ToString("yyyy-MM-dd") + ";");
            Console.WriteLine();
            Console.ReadLine();
            #endregion

            #region Inventory Test
            eai.iErp.stor.Inventory = ERPFactory.ERPFactory.createInventory();
            var inv = eai.iErp.stor.Inventory.getSingle("0101001");
            Console.Write(inv.InvCode + ";");
            Console.Write(inv.InvName + ";");
            Console.Write(inv.cInvClass + ";");
            if (inv.invClass != null)
            {
                DataModel.InventoryClass invcls = inv.invClass.First();
                while (invcls != null)
                {
                    Console.Write(" " + invcls.invClsName + ",");
                    invcls = invcls.upInvCls;
                }
            }
            Console.Write(";");
            Console.WriteLine();
            Console.ReadLine();
            #endregion

            #region InventoryClass Test
            InventoryClassTest(eai);
            #endregion

            #region CurrentStock Test
            eai.iErp.stor.CurrentStock = ERPFactory.ERPFactory.createCurrentStock();
            var cs = eai.iErp.stor.CurrentStock.getSingle("0101,0101001");
            Console.Write(cs.wareHouse.whName + ";");
            Console.Write(cs.inventory.InvName + ";");
            Console.Write(cs.quantity.iQuantity + ";");
            Console.Write(cs.quantity.iNum + ";");
            Console.WriteLine();
            Console.ReadLine();
            #endregion

            #region Dispatch Test
            eai.iErp.stor.Dispatch = ERPFactory.ERPFactory.createDispatch();
            var dl = eai.iErp.stor.Dispatch.getSingle("dp20150100044523");
            Console.Write(dl.Main.corporatio.Code + ";");
            Console.Write(dl.Main.corporatio.Name + ";");
            Console.Write(dl.Main.cShipAddress + ";");
            Console.Write(dl.Main.vouchDate.ToString("yyyy-MM-dd") + ";");
            Console.WriteLine();
            Console.WriteLine();
            foreach (var dd in dl.Details)
            {
                Console.Write(dd.warehouse.whName + ";");
                Console.Write(dd.inventory.InvName + ";");
                Console.Write(dd.quantity.iQuantity + ";");
                Console.Write(dd.quantity.iNum + ";");
                Console.Write(dd.iSum + ";");
                Console.WriteLine();
            }
            Console.ReadLine();

            #endregion

        }
        private static void InventoryClassTest(EAI eai)
        {
            eai.iErp.stor.InventoryClass = ERPFactory.ERPFactory.createInvClass();
            var invclss = eai.iErp.stor.InventoryClass.getList(new DataModel.InventoryClass() { isEnd = false, iGrade = 1 });
            if (invclss != null && invclss.Count > 0)
                foreach (var invcls in invclss)
                {
                    Console.Write(invcls.invClsCode + ";");
                    Console.Write(invcls.invClsName + ";");
                    Console.Write(invcls.iGrade.ToString() + ";");
                    Console.Write(invcls.isEnd + ";");
                    Console.WriteLine();
                }
            Console.ReadLine();
        }
        private static void testLog()
        {
            Logs.LogModel log1 = new Logs.LogModel() {
                //模拟数据
                cUserName = "dstTest",
                cReturn = "dstTest-log1",
                cParams = "log1"
            };
            Logs.LogModel log2 = new Logs.LogModel()
            {
                //模拟数据
                cUserName = "dstTest",
                cReturn = "dstTest-log2",
                cParams = "log2"
            };
            Logs.LogModel log3 = new Logs.LogModel()
            {
                //模拟数据
                cUserName = "dstTest"
            };

            List<Logs.LogModel> logs = new List<Logs.LogModel>();
            Logs.SysLogsDataAccess logsDA = new Logs.SysLogsDataAccess();
            logsDA.createLog(log1);
            logsDA.createLog(log2);
            logs.Add(logsDA.singleLog(33));
            logs.AddRange(logsDA.selectLogs(log3));

            foreach (Logs.LogModel lm in logs)
            {
                Console.Write("LogID:"+lm.iLogId.ToString()+";");
                Console.Write("UserID:"+lm.iUserID.ToString()+";");
                Console.Write("Return:"+lm.cReturn+";");
                Console.Write("Params:"+lm.cParams+";");
                Console.WriteLine();
            }
            Console.Write("要删除的日志id:");
            int logid = int.Parse(Console.ReadLine());
            logsDA.deleteLog(logid);
            logs.Clear();
            logs.Add(logsDA.singleLog(33));
            logs.AddRange(logsDA.selectLogs(log3));
            foreach (Logs.LogModel lm in logs)
            {
                Console.Write("LogID:" + lm.iLogId.ToString() + ";");
                Console.Write("UserID:" + lm.iUserID.ToString() + ";");
                Console.Write("Return:" + lm.cReturn + ";");
                Console.Write("Params:" + lm.cParams + ";");
                Console.WriteLine();
            }
            Console.ReadLine();
        }
        private static void testUser()
        {
            UserInfo.UserModel um = new UserInfo.UserModel();
            UserBLL.userBLL userBll = new UserBLL.userBLL();

            var authen =  userBll.Authentication("dst", "6fd742a61bd034804c00c49b18045020");
            Console.WriteLine("dst - Authened:"+authen.ToString());
            //认证
            um.cUserCode = "test1";
            um.cUserName = "test1";
            um.cUserEMail = "test1@eams.com";
            um.cUserMobile = "13312345678";
            um.cUserPassWord = "6fd742a61bd034804c00c49b18045020";
            try { userBll.Register(um); } catch (Exception e) { Console.WriteLine(e.Message); }//注册1
            um = new UserInfo.UserModel();
            um.cUserCode = "test2";
            um.cUserName = "test2";
            um.cUserEMail = "test2@eams.com";
            um.cUserMobile = "13387654321";
            um.cUserPassWord = "6fd742a61bd034804c00c49b18045020";
            try { userBll.Register(um); } catch (Exception e) { Console.WriteLine(e.Message); }//注册2
            var users = userBll.selects(new UserInfo.UserModel() { cUserCode = "test" });
            Console.WriteLine("UserLists:");//列表
            foreach (UserInfo.UserModel u in users)
                Console.WriteLine("Code:" + u.cUserCode + ";-" + u.cUserName + "," +
                    u.cUserEMail + "," + u.cUserMobile + ",UserID:" + u.iUserId.ToString());
            Console.WriteLine();
            Console.Write("User Update! import UserId:");
            int uid = int.Parse(Console.ReadLine());
            um = users.First(f=>f.iUserId == uid);
            um.cUserEMail = "testUpdate_" + um.cUserEMail;
            int upd = userBll.update(um);//更新
            Console.WriteLine("Update Result:" + upd.ToString());
            Console.Write("User Delete! import UserId:");
            uid = int.Parse(Console.ReadLine());
            int del = userBll.delete(uid);//删除
            Console.WriteLine("Delect Result:" + del.ToString());
        }
        private static void testOrganization()
        {
            //Console.WriteLine("Group:");
            //testGroup();
            //Console.ReadLine();
            Console.WriteLine("Role:");
            testRole();
            Console.ReadLine();
        }
        private static void testGroup()
        {
            int result = -1;
            OrganizationBase.groupDataAccess groupDA = new OrganizationBase.groupDataAccess();
            OrganizationBase.groupModel gm = new OrganizationBase.groupModel();
            List<OrganizationBase.groupModel> gms;
            gm.groupName = "NameTest1";gm.groupDescription = "DescriptionTest1";
            gm.groupId = (int)groupDA.Create(gm);//增加

            gms = groupDA.selects(null);//列表
            foreach (OrganizationBase.groupModel g in gms)
                Console.WriteLine("groupID:" + g.groupId + "; " + g.groupName + "," + g.groupDescription);

            gm.groupDescription = "updateTest-"+gm.groupDescription;
            result = groupDA.Update(gm);//更新
            Console.WriteLine("更新结果:"+result);
            gms = groupDA.selects(null);
            foreach (OrganizationBase.groupModel g in gms)
                Console.WriteLine("groupID:" + g.groupId + "; " + g.groupName + "," + g.groupDescription);

            Console.WriteLine("delectTest-Import GroupID:");
            int gid = int.Parse(Console.ReadLine());
            result = groupDA.Delete(gid);//删除
            Console.WriteLine("删除结果:" + result);
            gms = groupDA.selects(null);
            foreach (OrganizationBase.groupModel g in gms)
                Console.WriteLine("groupID:" + g.groupId + "; " + g.groupName + "," + g.groupDescription);

            Console.WriteLine("Refs:");
            gid = int.Parse(Console.ReadLine());
            OrganizationBase.groupRefDataAccess grDA = new OrganizationBase.groupRefDataAccess();
            var grs = grDA.selects(new OrganizationBase.groupRefModel() { groupId = gid });//列表
            foreach (OrganizationBase.groupRefModel grm in grs)
                Console.WriteLine("AutoID:" + grm.autoid + ";" + grm.UserId + "," + grm.groupId + "," + grm.isManager);
            grDA.Create(new OrganizationBase.groupRefModel() { groupId =3, UserId=36 });//增加

            grs = grDA.selects(new OrganizationBase.groupRefModel() { groupId = 3 });//列表
            foreach (OrganizationBase.groupRefModel grm in grs)
                Console.WriteLine("AutoID:" + grm.autoid + ";" + grm.UserId + "," + grm.groupId + "," + grm.isManager);
            OrganizationBase.groupRefModel gRefm = new OrganizationBase.groupRefModel();
            Console.WriteLine("Update-Import GourpRefID:");
            gid = int.Parse(Console.ReadLine());
            gRefm = grDA.Single(gid);
            gRefm.UserId = 37;//更新
            result = grDA.Update(gRefm);
            Console.WriteLine("更新结果:" + result);
            grs = grDA.selects(new OrganizationBase.groupRefModel() { groupId = 3 });//列表
            foreach (OrganizationBase.groupRefModel grm in grs)
                Console.WriteLine("AutoID:" + grm.autoid + ";" + grm.UserId + "," + grm.groupId + "," + grm.isManager);
            //删除
            Console.WriteLine("Delete-Import GourpRefID:");
            int grid = int.Parse(Console.ReadLine());
            result = grDA.Delete(grid);
            Console.WriteLine("删除结果:" + result);
            grs = grDA.selects(new OrganizationBase.groupRefModel() { groupId = 3 });//列表
            foreach (OrganizationBase.groupRefModel grm in grs)
                Console.WriteLine("AutoID:" + grm.autoid + ";" + grm.UserId + "," + grm.groupId + "," + grm.isManager);
        }
        private static void testRole()
        {
            int result = -1;
            OrganizationBase.roleDataAccess roleDA = new OrganizationBase.roleDataAccess();
            OrganizationBase.roleModel gm = new OrganizationBase.roleModel();
            List<OrganizationBase.roleModel> gms;
            gm.cRoleName = "NameTest1";
            gm.cRoleDescription = "DescriptionTest1";
            gm.iRoleId = (int)roleDA.Create(gm);//增加

            gms = roleDA.selects(null);//列表
            foreach (OrganizationBase.roleModel g in gms)
                Console.WriteLine("roleID:" + g.iRoleId + "; " + g.cRoleName + "," + g.cRoleDescription);

            gm.cRoleDescription = "updateTest-" + gm.cRoleDescription;
            roleDA.Update(gm);//更新
            Console.WriteLine("Update:-");
            gms = roleDA.selects(null);
            foreach (OrganizationBase.roleModel g in gms)
                Console.WriteLine("roleID:" + g.iRoleId + "; " + g.cRoleName + "," + g.cRoleDescription);

            Console.WriteLine("delectTest-Import GroupID:");
            int rid = int.Parse(Console.ReadLine());
            roleDA.Delete(rid);//删除
            gms = roleDA.selects(null);
            foreach (OrganizationBase.roleModel g in gms)
                Console.WriteLine("roleID:" + g.iRoleId + "; " + g.cRoleName + "," + g.cRoleDescription);



            Console.WriteLine("Refs:");
            rid = int.Parse(Console.ReadLine());
            OrganizationBase.roleRefDataAccess rrDA = new OrganizationBase.roleRefDataAccess();
            var grs = rrDA.selects(new OrganizationBase.roleRefModel() { iRoleId = rid });//列表
            foreach (OrganizationBase.roleRefModel grm in grs)
                Console.WriteLine("AutoID:" + grm.iURRefId + ";" + grm.iUserId + "," + grm.iRoleId );
            rrDA.Create(new OrganizationBase.roleRefModel() { iRoleId = 4, iUserId = 36 });//增加

            grs = rrDA.selects(new OrganizationBase.roleRefModel() { iRoleId = 4 });//列表
            foreach (OrganizationBase.roleRefModel grm in grs)
                Console.WriteLine("AutoID:" + grm.iURRefId + ";" + grm.iUserId + "," + grm.iRoleId);
            OrganizationBase.roleRefModel gRefm = new OrganizationBase.roleRefModel();
            Console.WriteLine("Update-Import RoleRefID:");
            rid = int.Parse(Console.ReadLine());
            gRefm = rrDA.Single(rid);
            gRefm.iUserId = 37;//更新
            result = rrDA.Update(gRefm);
            Console.WriteLine("更新结果:" + result);
            grs = rrDA.selects(new OrganizationBase.roleRefModel() { iRoleId = 4 });//列表
            foreach (OrganizationBase.roleRefModel grm in grs)
                Console.WriteLine("AutoID:" + grm.iURRefId + ";" + grm.iUserId + "," + grm.iRoleId );
            //删除
            Console.WriteLine("Delete-Import RoleRefID:");
            int grid = int.Parse(Console.ReadLine());
            result = rrDA.Delete(grid);
            Console.WriteLine("删除结果:" + result);
            grs = rrDA.selects(new OrganizationBase.roleRefModel() { iRoleId = 4 });//列表
            foreach (OrganizationBase.roleRefModel grm in grs)
                Console.WriteLine("AutoID:" + grm.iURRefId + ";" + grm.iUserId + "," + grm.iRoleId);
        }
        private static void testAttendance()
        {
            Console.WriteLine("考勤导入测试！");
            Attendance.AttendanceBLL aBll = new Attendance.AttendanceBLL(2);
            try
            {
                Console.WriteLine("参数-eid:8,begin:2015-10-8,end:2015-10-9.");
                aBll.DoAtte(8, new DateTime(2015, 10, 8), new DateTime(2015, 10, 9));
                var records = aBll.getRecords(8, new DateTime(2015, 10, 1), new DateTime(2015, 10, 12));
                foreach (Attendance.Model.RecordModel r in records)
                {
                    Console.Write("autoid:"+r.autoid+",");
                    Console.Write("sDate:" + r.sDate.Value.ToShortDateString() + ",");
                    Console.Write("periodNo:" + r.periodNo + ",");
                    Console.Write("dateType:" + r.dayName+ ",");
                    Console.Write("bAttTimeStr:" + r.bAttTimeStr+ ",");
                    Console.Write("eAttTimeStr:" + r.eAttTimeStr+ ",");
                    Console.WriteLine();
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            Console.ReadLine();
        }
        private static void testReport() {
            Report reportNew1 = new Report();
            ReportBLL reportBll = new ReportBLL();
            reportNew1.Name = "My First Report";
            reportNew1.Description = "My First Report";
            reportNew1.Title = "My First Report";
            reportNew1.QueryBase = " My First Report QueryBase";
            //reportNew1.css = "table { margin:auto }";
            //reportNew1.filter = " My First Report Filter";
            //reportNew1.footer = " My First Report Footer";

            reportBll.reportDA.Create(reportNew1);
            
        }

    }
}
