using System.Text;
using System.Collections.Generic;
using FluentData;

namespace DataAccess.U8
{
    public class u8Base:abstractDataAccess
    {
        public u8Base()
        {
            Context = erpContextBase.Context;
        }
        public static StringBuilder sqlcmd = new StringBuilder();
    }

    public abstract class u8<T> : u8Base, IDataAccess.IDbAccess<T>
    {
        public u8() : base() {; }
        public abstract List<T> getList(string code);
        public abstract T getSingle(string code);
        public abstract List<T> getList(T searchKey);
        public abstract void setField(string field, string val, T searchKey);
        public abstract void setField(string field, string val, string whereStr);
        public abstract object getField(string field, T searchKey);

    }
    public class u8ERP : IERP
    {
        public Istorager stor { get; set; }
        public string[] VouchType { get { return new string[] { "Dispatch", "SaleOrder" }; }  }
    }
}
