using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModel
{
    public class District
    {
        public int DCID { get; set; }
        public string dcCode { get; set; }
        public string dcName { get; set; }
        public string upDCCode { get; set; }
        public int iGrade { get; set; }
        public bool isEnd { get; set; }
        public District upDC { get; set; }
    }
}