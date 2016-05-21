using System;
using System.Collections.Generic;

namespace DataDB.ModelBase
{
    public interface IContactBase
    {
        long id { get; set; }
        string phone { get; set; }
        string mobile { get; set; }
        string Email { get; set; }
        string PostCode { get; set; }
        string Address { get; set; }
        string shipAddress { get; set; }

    }
    public interface IContact
    {
        string code { get; set; }
        string DWCode { get; set; }
        IList<IIM> IMs { get; set; }
        string Memo { get; set; }
        IContactBase contact { get; set; }
    }
    public abstract class ContactBase : IPersonBase, IContact
    {
        public DateTime BrithDay
        { get; set; }
        public string code
        { get; set; }
        public IContactBase contact
        { get; set; }
        public List<IContactBase> Contacts
        { get; set; }
        public string DWCode
        { get; set; }
        public bool? gender
        { get; set; }
        public long id
        { get; set; }
        public string IDNo
        { get; set; }
        public IList<IIM> IMs
        { get; set; }
        public string Memo
        { get; set; }
        public string name
        { get; set; }
    }
}