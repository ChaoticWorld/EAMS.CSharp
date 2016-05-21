using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModel
{
    public class CurrentStock
    {
        public Inventory inventory { get; set; }
        public Quantity quantity { get; set; }
        public Warehouse wareHouse { get; set; }
    }
}