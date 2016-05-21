using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModel
{
    public class InventoryClass
    {
        public int invClsID { get; set; }
        public string invClsCode { get; set; }
        public string invClsName { get; set; }
        public string invClsDescript { get; set; }
        public bool isDefault { get; set; }
        public int upInvClsID { get; set; }
        public string upInvClsCode { get; set; }
        public int iGrade { get; set; }
        public Nullable<bool> isEnd { get; set; } = null;
        public InventoryClass upInvCls { get; set; }
    }
}