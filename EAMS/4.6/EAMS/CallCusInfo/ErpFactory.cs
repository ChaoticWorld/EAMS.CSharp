using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CallInfo
{
    /// <summary>
    /// 为U8专写，后续版本改为ERP工厂模式
    /// </summary>
    [Serializable]
    public class ErpFactory
    {
        public static DataDB.u8.dbU8 dbu8 { get { return new DataDB.u8.dbU8(); } }
        public static DataAccess.EAI eai = new DataAccess.EAI();
        public ErpFactory() { eai.iErp = ERPFactory.ERPFactory.Create();
            //eai.iErp.stor.Customer.
        }
    }
}
