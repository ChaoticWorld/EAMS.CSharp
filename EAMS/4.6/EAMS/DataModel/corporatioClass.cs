using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModel
{
    public class corporatioClass
    {
        public int corpClsID { get; set; }
        public string corpClsCode { get; set; }
        public string corpClsName { get; set; }
        public string corpClsDescript { get; set; }
        public int upCorpClsID { get; set; }
        public int iGrade { get; set; }
        public bool isEnd { get; set; }
    }
}