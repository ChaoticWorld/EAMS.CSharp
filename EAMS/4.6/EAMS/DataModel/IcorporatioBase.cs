using System;
using System.Collections.Generic;

namespace DataModel
{
    public interface IcorporatioBase
    {
        string Code { get; set; }
        string Name { get; set; }
        List<ContactInfoBase> contactInfos { get; set; }
        DateTime corpCreateDay { get; set; }
        District district { get; set; }
        corporatioClass corpCls { get; set; }
    }
}