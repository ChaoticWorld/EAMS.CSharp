using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDataAccess;
using DataAccess;
using FluentData;

namespace Attendance.DAL
{
    public abstract class DAL<T> : abstractDataAccess, IdbCRUD<T>
    {
        public DAL(string tableName)
        {
            Context = eamsAppDataContextBase.Context;
            TableName = tableName;
        }
        protected abstract string WhereStr(T t);
        protected abstract void Mapper(T m, IDataReader row);
        public abstract long Create(T t);
        public abstract int Delete(long id);
        public abstract List<T> selects(T t);
        public abstract T Single(string key);
        public abstract T Single(long id);
        public abstract int Update(T t);
    }

}
