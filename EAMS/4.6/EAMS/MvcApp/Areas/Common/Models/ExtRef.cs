using IDataAccess;
using DataModel;
using DataAccess.U8;

namespace MvcApp.Areas.Common.Models
{
    public class ExtRef
    {
        private DataAccess.EAI eai = new DataAccess.EAI();

        private IDbAccess<Customer> _extCustomer;
        private IDbAccess<Inventory> _extInventory;
        private IDbAccess<InventoryClass> _extInventoryClass;
        private IDbAccess<District> _extDistrict;
        public IDbAccess<Customer> extCustomer { get { return _extCustomer; } }
        public IDbAccess<Inventory> extInventory { get { return _extInventory; } }
        public IDbAccess<InventoryClass> extInventoryClass { get { return _extInventoryClass; } }
        public IDbAccess<District> extDistrict { get { return _extDistrict; } }
        /// <summary>
        /// 外部系统（如ERP）,访问器
        /// </summary>
        /// <param name="ExtName">支持的外部系统名称</param>
        public ExtRef(string ExtName)
        {
	    ////工厂模式，反射
            eai.iErp = ERPFactory.ERPFactory.Create(path:null,erpName: ExtName);
            _extCustomer = ERPFactory.ERPFactory.createCustomer();
            _extInventory = ERPFactory.ERPFactory.createInventory();
            _extInventoryClass = ERPFactory.ERPFactory.createInvClass();
            _extDistrict = ERPFactory.ERPFactory.createDistrict();

	    ////直接引用
            //switch (ExtName)
            //{
            //    case "u8":
            //        _extCustomer = new u8Customer();
            //        _extInventory = new u8Inventory();
            //        _extInventoryClass = new u8InventoryClass();
            //        _extDistrict = new u8District();
            //        break; 
            //    default:
            //        break;
            //}
        }
    }
}