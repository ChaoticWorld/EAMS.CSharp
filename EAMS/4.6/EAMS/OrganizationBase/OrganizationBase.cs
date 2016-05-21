using System.Collections.Generic;
using DataAccess;
using FluentData;
using IDataAccess;

namespace OrganizationBase
{
    public abstract class Organization<T> : abstractDataAccess,IdbCRUD<T>,IgetOrganization<T>
    {
        public Organization(string tblName)
        {
            TableName = tblName;
            Context = eamsAppSystemContextBase.getContext();
        }
        protected abstract string WhereStr(T m);
        protected abstract void orgMapper(T m, IDataReader row);
        public abstract IList<T> getOrganization(long userid);
        public abstract long Create(T t);
        public abstract T Single(long id);
        public abstract T Single(string code);
        public abstract int Update(T t);
        public abstract int Delete(long id);
        public abstract List<T> selects(T t);
    }
    public interface IgetOrganization<T>
    {  IList<T> getOrganization(long userid); }
}
