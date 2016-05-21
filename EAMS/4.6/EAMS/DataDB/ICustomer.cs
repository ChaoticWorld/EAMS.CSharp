using System;
using System.Collections.Generic;

namespace DataDB.ModelBase
{
    public interface ICustomer
    {
        string cAddress { get; set; }
        string cCusAbbName { get; set; }
        string cCusCode { get; set; }
        string cCusName { get; set; }
        string cMobile { get; set; }
        string ContactName { get; set; }
        IList<IContact> Contacts { get; set; }
        string cPhone { get; set; }
        string cShipAddress { get; set; }
        string cusClass { get; set; }
        string District { get; set; }
        string EMail { get; set; }
        string employeeName { get; set; }
        IList<IEmployee> Employees { get; set; }
        IList<IIM> IMs { get; set; }
        string Memo { get; set; }
        ICustomer topCus { get; set; }
        string topCusCode { get; set; }
    }
}