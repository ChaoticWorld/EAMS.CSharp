using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModel
{
    public class VouchMain : IOperatorBase, IVouchMain
    {
        public int Mid { get; set; }
        public string vouchCode { get; set; }
        public IcorporatioBase corporatio { get; set; }
        public DateTime vouchDate { get; set; }

        public string CreateMan { get; set; }
        public string ModifyMan { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public string cShipAddress { get; set; }
        public string cSender { get; set; }
        public decimal Je { get; set; }
        public string Memo { get; set; }
        public string Maker { get; set; }
        public string Verifier { get; set; }
        public string vouchContact { get; set; }

        //public 物流
        //public 发票
        //public 收付款
        //public 附件
    }
}