using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public interface IOperatorBase
    {
        string CreateMan { get; set; }
        string ModifyMan { get; set; }
        DateTime CreateDate { get; set; }
        DateTime ModifyDate { get; set; }
    }
    public abstract class humanBase :IOperatorBase
    {
        public string Name { get; set; }
        public string Identity { get; set; }
        public string CreateMan { get; set; }
        public string ModifyMan { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }
    public class PersonBase : humanBase
    {
        public bool? gender { get; set; }
        public System.DateTime BrithDay { get; set; }
        public virtual List<ContactInfoBase> contactInfos { get; set; }
    }
    public class Employee :PersonBase
    {
        public string EmpId { get; set; }
        public override List<ContactInfoBase> contactInfos { get; set; }
    }
    
    public class Contact : PersonBase
    {
        public IcorporatioBase corpInfo { get; set; }
        public string corpCode { get; set; }
        public override List<ContactInfoBase> contactInfos { get; set; }
        public bool isDefault { get; set; }
    }
}
