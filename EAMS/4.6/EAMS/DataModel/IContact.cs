using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public interface IContactInfoBase
    {
        long id { get; set; }
        string phone { get; set; }
        string mobile { get; set; }
        string Email { get; set; }
        string PostCode { get; set; }
        string Address { get; set; }
        string shipAddress { get; set; }
        bool isdefault { get; set; }
    }

    public class ContactInfoBase : IContactInfoBase
    {
        public string Address { get; set; }
        public string Email { get; set; }
        public long id { get; set; }
        public bool isdefault { get; set; }
        public string mobile { get; set; }
        public string phone { get; set; }
        public string PostCode { get; set; }
        public string shipAddress { get; set; }
    }
}
