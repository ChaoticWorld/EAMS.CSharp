using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModel
{
    public class UnitBase
    {
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public int UnitID { get; set; }
        public int upUnitID { get; set; }
        public bool isDefault { get; set; }
        public int iGrade { get; set; }
        public bool isEnd { get; set; }
        public double rate { get; set; }
    }
}