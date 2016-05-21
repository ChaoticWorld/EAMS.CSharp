using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModel
{
    public class Inventory
    {
        public string InvCode { get; set; }
        public string InvName { get; set; }
        public string InvStd { get; set; }
        public string cUnit { get; set; }
        public string cUnitCode { get; set; }
        public string cInvClass { get; set; }
        public string cInvClassCode { get; set; }
        public string Memo { get; set; }
        public List<InventoryClass> invClass { get; set; }
        public List<UnitBase> Units { get; set; }
    }
}