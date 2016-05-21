using System;
using System.Collections.Generic;
using FluentData;

namespace AttendanceDevice.HWATT.DAL
{
    public abstract class DAL<T>:DataAccess.abstractDataAccess,IDataAccess.IDbAccess<T>
    {
        public DAL()
        {
            Context = new DbContext().ConnectionStringName("AttendanceDevice", new SqlServerProvider());
        }
        public abstract List<T> getList(string code);
        public abstract T getSingle(string code);
        public abstract List<T> getList(T searchKey);
        public abstract void setField(string field, string val, T searchKey);
        public abstract void setField(string field, string val, string whereStr);
        public abstract object getField(string field, T searchKey);
    }
}
