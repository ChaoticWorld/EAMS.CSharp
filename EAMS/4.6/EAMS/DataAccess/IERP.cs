using IDataAccess;
using DataModel;
using FluentData;

namespace DataAccess
{
    public interface IERP
    {
        Istorager stor { get; set; }
        string[] VouchType { get; } 
    }
    /// <summary>
    /// 存储-数据库
    /// </summary>
    public interface Istorager
    {
        IDbContext Context { get; }
        IDbAccess<Customer> Customer { get; set; }
        IDbAccess<Vendor> Vendor { get; set; }
        IDbAccess<District> District { get; set; }
        IDbAccess<Inventory> Inventory { get; set; }
        IDbAccess<InventoryClass> InventoryClass { get; set; }
        IDbAccess<Warehouse> Warehouse { get; set; }
        IDbAccess<CurrentStock> CurrentStock { get; set; }
        IDbAccess<Dispatch> Dispatch { get; set; }
        IDbAccess<DispatchMain> DispatchMain { get; set; }
        IDbAccess<DispatchDetail> DispatchDetail { get; set; }
        IDbAccess<SaleOrder> SaleOrder { get; set; }
        IDbAccess<SaleOrderMain> SaleOrderMain { get; set; }
        IDbAccess<SaleOrderDetail> SaleOrderDetail { get; set; }
        IMultiTableQuery MultiTableQuery { get; set; }
        IDbAccess<Authen> Authen { get; set; }
    }
}
