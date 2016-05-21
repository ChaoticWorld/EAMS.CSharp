using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using IDataAccess;

namespace report
{
    public abstract class DataAccess<T> : abstractDataAccess, IdbCRUD<T>
    {
        public DataAccess() { Context = eamsAppDataContextBase.Context; }
        public abstract long Create(T t);
        public abstract T Single(long id);
        public abstract T Single(string key);
        public abstract int Update(T t);
        public abstract int Delete(long id);
        public abstract List<T> selects(T t);
    }
}
