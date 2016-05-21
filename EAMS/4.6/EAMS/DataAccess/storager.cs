using System;
using IDataAccess;
using DataModel;
using FluentData;

namespace DataAccess
{
    public class DB : Istorager
    {
        public IDbAccess<Customer> Customer { get; set; }
        public IDbAccess<Vendor> Vendor { get; set; }
        public IDbAccess<District> District { get; set; }
        public IDbAccess<Inventory> Inventory { get; set; }
        public IDbAccess<InventoryClass> InventoryClass { get; set; }
        public IDbAccess<Warehouse> Warehouse { get; set; }
        public IDbAccess<CurrentStock> CurrentStock { get; set; }
        public IDbAccess<Dispatch> Dispatch { get; set; }
        public IDbAccess<DispatchMain> DispatchMain { get; set; }
        public IDbAccess<DispatchDetail> DispatchDetail { get; set; }
        public IDbAccess<SaleOrder> SaleOrder { get; set; }
        public IDbAccess<SaleOrderMain> SaleOrderMain { get; set; }
        public IDbAccess<SaleOrderDetail> SaleOrderDetail { get; set; }
        public IMultiTableQuery MultiTableQuery { get; set; }
        public IDbContext Context { get { return erpContextBase.Context; } }
        public IDbAccess<Authen> Authen { get; set; }
    }
}
