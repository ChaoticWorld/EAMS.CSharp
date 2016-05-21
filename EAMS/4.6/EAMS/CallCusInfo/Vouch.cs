using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using DataDB.u8;
using DataDB.ModelBase;

namespace CallInfo
{
    [Serializable]
    public class callVouchModel
    {
        [Display(Name = "显示最后交易记录数")]
        public int vouchLastNum { get; private set; }
        [Display(Name = "单位编码")]
        public string DWCode { get; set; }
        public List<Dispatch> dispatchLists { get; set; }
        public List<InWarehouse> inWarehouseLists { get; set; }
        public List<CurrentStockQ> currentStock { get; set; }

        public callVouchModel(int lstNum = 5) { 
        //后续版本，解决vouchLastNum,取数自系统设置或者数据库
            vouchLastNum = lstNum;
        }
        ~callVouchModel() { }
    }
    [Serializable]
    public class CallVouchInfo:ErpFactory
    {
        public callVouchModel CallVouchModel { get; set; }

        ~CallVouchInfo() { }
        public CallVouchInfo()
        {
            CallVouchModel = new callVouchModel();
        }

        public callVouchModel getDispatchModel(string dwcode,int num = 5)
        {
            callVouchModel r = new callVouchModel(num);
            r.dispatchLists = dbu8.getDispatch(dwcode, r.vouchLastNum);
            return r;
        }
    }
}
