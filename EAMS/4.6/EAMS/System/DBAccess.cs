using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data;
using System.Data.Objects;
using DBAccessBase;
using FluentData;
using DataAccess;
namespace SystemDB
{
    public class sysDB : DBAccessBase.DBAccessBase
    {
        public static AppSystemEntities appSystemEntity = new AppSystemEntities();
        public static IDbContext Context = eamsAppSystemContextBase.getContext();
    }
}
