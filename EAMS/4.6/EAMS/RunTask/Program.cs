using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemBLL;
using SystemDB;
using DataDB;
using strategyLib;
using DataDB.ModelBase;
using DataDB.u8;

namespace RunTask
{
    class Program
    {
        static SystemBLL.LogsBLL logBll = new LogsBLL();
        static DataDB.u8.dbU8 dbu8 = new DataDB.u8.dbU8();
        static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string a in args)
            {
                sb.Append(a + " ");
            }

            //Console.WriteLine("参数准备:" + sb.ToString());
            //System.Threading.Thread.Sleep(1000 * 10);
            //Progress<LogsBLL> l = new Progress<LogsBLL>();            
            logBll.Add(new SystemDB.Logs()
            {
                iUserID = -999,
                cModule = "RunTask",
                cUserName = "SYSTEM",
                cParams = sb.ToString()
            });
            doTask(args);
            //Console.Write("按任意键退出");
            //Console.Read();
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="p">
        /// opeartype:操作
        /// vouchtype:单据类型
        /// vouchCode:单据编码
        /// vaildField:合法提示字段
        /// </param>
        /// RunTask Strategy Dispatch DP20150100045584 cMemo
        static void doTask(string [] p)
        {
            Dictionary<string, string> _params = new Dictionary<string, string>();
            string [] sp;
            foreach(string _p in p)
            {
                sp = _p.Split(':');
                _params.Add(sp[0], sp[1]);
            }
            switch (_params["opeartype"])
            {
                case "Strategy":
                    Strategy(_params);
                    break;
                default:
                    break;
            }
        }
        static void Strategy(Dictionary<string,string> _params){
            DataDB.ModelBase.IVouch ErpVouch = null;
            string strategyCode = string.Empty;
            string validField = _params["vaildField"];
            switch (_params["vouchtype"])
            {
                case "Dispatch":
                    ErpVouch = dbu8.getDispatch(_params["vouchCode"]);
                    //StrategyDispatch(_params["vouchCode"],validField);
                    break;
                case "SaleOrder":
                    ErpVouch = dbu8.getSaleOrder(_params["vouchCode"]);
                    break;
                default:
                    ErpVouch = null;
                    break;
            }

            if (ErpVouch != null 
                && ErpVouch.Main != null 
                && !string.IsNullOrEmpty(ErpVouch.Main.Code))
            {
                strategyCode = StrategyValid(ErpVouch);
                logBll.Add(new Logs(){
                    iUserID = -999,
                    cModule = "StrategyValid",
                    cUserName = "SYSTEM",
                    cReturn = strategyCode
                });
                //if (string.IsNullOrEmpty(strategyCode))
                //    dbu8.updateDispatchField(validField, "", ErpVouch.Main.Code);
                //else
                //    dbu8.updateDispatchField(validField, "非法!" + strategyCode + "!", ErpVouch.Main.Code);
            }
        }
        static string StrategyValid(DataDB.ModelBase.IVouch ErpVouch)
        {
            strategyDAL strategyDal = new strategyDAL();
            string strategyCode = strategyDal.isValid(ErpVouch);
            return strategyCode;
        }
        static void StrategyDispatch(string code, string vaildField)
        {

            DataDB.u8.dbU8 dbu8 = new DataDB.u8.dbU8();
            DataDB.ModelBase.Dispatch ErpVouch = dbu8.getDispatch(code);

            //strategyBLL strategyBll = new strategyBLL();
            strategyDAL strategyDal = new strategyDAL();
            //InventoryClassStandePriceRateBLL invStdPriceRate = new InventoryClassStandePriceRateBLL();
            string strategyCode = strategyDal.isValid(ErpVouch);
            #region v1.0
            //foreach (DispatchDetail dd in dispatch.Details)
            //{
            //    decimal p = (decimal)dd.iPrice;
            //    decimal rate = 1;
            //    //strategyParam = new Dispatch();
            //    //strategyParam.invClass = dbu8.getInvTopClass(dd.inventory.cInvClass);
            //    //strategyParam.invClass = dd.inventory.cInvClass;
            //    //strategyParam.Details.cinvAName = dd.inventory.cInvName;
            //    //strategyParam.cinvAStd = dd.inventory.cInvStd;
            //    //strategyParam.cDWCode = dispatch.Main.DWCode;
            //    //strategyParam.cVouchCode = dlcode;
            //    //strategyParam.cDCName = dbu8.getCustomer(strategyParam.cDWCode).District;
            //    //strategyParam.vDate = dispatch.Main.dDate;



            //    while (!string.IsNullOrEmpty(strategyParam.invClass))
            //    {
            //        var invConverts = invStdPriceRate.getInvConvert(strategyParam.invClass);
            //        if ((invConverts != null && invConverts.Count() > 0)) break;
            //        else
            //            strategyParam.invClass = dbu8.getInventoryClass(strategyParam.invClass).upClass;

            //    }
            //    rate = invStdPriceRate.getRate(strategyParam.invClass, strategyParam.invStd);
            //    strategyParam.vPrice = p/rate;

            //    strategyCode = strategyBll.isPriceVaild(strategyParam);
            //    if (!string.IsNullOrEmpty(strategyCode))
            //    {
            //        dbu8.updateDispatchField(vaildField, "非法!" + strategyCode + "!", dlcode);
            //        return;
            //    }
            //}
            #endregion
            if (string.IsNullOrEmpty(strategyCode))
                dbu8.updateDispatchField(vaildField, "", code);
            else
                dbu8.updateDispatchField(vaildField, "非法!" + strategyCode + "!", code);
        }
        static void StrategySaleOrder(string code, string vaildField)
        {

            DataDB.u8.dbU8 dbu8 = new DataDB.u8.dbU8();
            DataDB.ModelBase.SaleOrder ErpVouch = dbu8.getSaleOrder(code);

            //strategyBLL strategyBll = new strategyBLL();
            strategyDAL strategyDal = new strategyDAL();
            //InventoryClassStandePriceRateBLL invStdPriceRate = new InventoryClassStandePriceRateBLL();
            string strategyCode = strategyDal.isValid(ErpVouch);
                    if (string.IsNullOrEmpty(strategyCode))
                dbu8.updateDispatchField(vaildField, "", code);
            else
                dbu8.updateDispatchField(vaildField, "非法!" + strategyCode + "!", code);
        }

    }

}
