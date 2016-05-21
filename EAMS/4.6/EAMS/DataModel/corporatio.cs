using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModel
{
    public class corporatioBase : humanBase, IcorporatioBase
    {
        public string Code { get; set; }
        //public string Name { get; set; }
        public System.DateTime corpCreateDay { get; set; }
        public virtual List<ContactInfoBase> contactInfos { get; set; }

        public District district { get; set; }
        public corporatioClass corpCls { get; set; }
        public List<Contact> contacts { get; set; }
    }
    public class Customer : corporatioBase
    { }
    public class Vendor : corporatioBase
    { }
}
