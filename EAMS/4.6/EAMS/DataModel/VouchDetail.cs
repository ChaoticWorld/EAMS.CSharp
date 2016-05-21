using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModel
{
    public class VouchDetail : IVouchDetail
    {
        public long autoid { get; set; }
        public int Mid { get; set; }
        public int Did { get; set; }
        public Inventory inventory { get; set; }
        public Warehouse warehouse { get; set; }
        public Quantity quantity { get; set; }
        public double iPrice { get; set; }
        public double iSum { get; set; }
        public string Memo { get; set; }
        public virtual void getDetailField(string field, string code) { }
    }
}