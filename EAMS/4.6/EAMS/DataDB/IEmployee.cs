using System;
using System.Collections.Generic;

namespace DataDB.ModelBase
{
    public interface IEmployee
    {
        string EmpId { get; set; }
    }
    public interface IPersonBase
    {
        long id { get; set; }
        string name { get; set; }
        bool? gender { get; set; }
        string IDNo { get; set; }
        System.DateTime BrithDay { get; set; }
        List<IContactBase> Contacts { get;set;}
    }
}