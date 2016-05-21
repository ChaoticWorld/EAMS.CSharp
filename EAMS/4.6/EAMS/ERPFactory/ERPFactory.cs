using System;
using System.IO;
using System.Reflection;
using DataAccess;
using DataAccess.U8;
using IDataAccess;
using DataModel;

namespace ERPFactory
{
    /// <summary>
    /// readme:
    /// ERP工厂;
    /// ErpName:ERP名，如U8,KIS
    /// Create(ERP名)，设置ERP对象,默认u8;
    /// </summary>
    public static class ERPFactory
    {
        public static string ErpName { get; private set; }
        private static Assembly _asse;

        public static IERP Create(string path = null, string erpName = "u8")
        {
            ErpName = erpName.ToLower();
            string assePath = string.IsNullOrEmpty(path) ? AppDomain.CurrentDomain.BaseDirectory : path;
            _asse = getErpAssembly(assePath.EndsWith("\\") || assePath.EndsWith("/") ? assePath : assePath + "\\");

            if (_asse != null)
            {
                string clsName = "DataAccess." + ErpName.ToUpper() + "." + ErpName.ToLower() + "ERP";
                return (IERP)_asse.CreateInstance(clsName);
            }
            else return null;
        }

        private static Assembly getErpAssembly(string path)
        {
            string DllFileName;
            if (!string.IsNullOrEmpty(path))
            {
                DllFileName = "DataAccess." + ErpName + ".dll";
                var binPaths = System.IO.Directory.GetFiles(path, DllFileName, System.IO.SearchOption.AllDirectories);
                if (binPaths != null && binPaths.Length > 0)
                    _asse = Assembly.Load(getDllStream(binPaths[0]));
                else _asse = null;
            }
            else _asse = null;
            //是否要做单例？
            return _asse;
        }
        private static byte[] getDllStream(string path)
        {
            MemoryStream memStream;
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                using (memStream = new MemoryStream())
                {
                    int res;
                    byte[] b = new byte[4096];
                    while ((res = stream.Read(b, 0, b.Length)) > 0)
                    {
                        memStream.Write(b, 0, b.Length);
                    }
                }
            }
            return memStream.ToArray();
        }

        public static Istorager createStoragerDB() {
            return new DB();
        }

        public static IDbAccess<Customer> createCustomer()
        {
            string clsName = "DataAccess." + ErpName.ToUpper() + "." + ErpName.ToLower() + "Customer";
            var c = _asse.CreateInstance(clsName,true) as IDbAccess<Customer>;
            return c;
        }
        public static IDbAccess<Vendor> createVendor()
        {
            IDbAccess<Vendor> r;
            string clsName = "DataAccess." + ErpName.ToUpper() + "." + ErpName.ToLower() + "Vendor";
            r = (IDbAccess<Vendor>)_asse.CreateInstance(clsName);
            return r;
        }
        public static IDbAccess<Inventory> createInventory()
        {
            IDbAccess<Inventory> r;
            string clsName = "DataAccess." + ErpName.ToUpper() + "." + ErpName.ToLower() + "Inventory";
            r = (IDbAccess<Inventory>)_asse.CreateInstance(clsName);
            return r;
        }
        public static IDbAccess<InventoryClass> createInvClass()
        {
            IDbAccess<InventoryClass> r;
            string clsName = "DataAccess." + ErpName.ToUpper() + "." + ErpName.ToLower() + "InventoryClass";
            r = (IDbAccess<InventoryClass>)_asse.CreateInstance(clsName);
            return r;
        }
        public static IDbAccess<District> createDistrict()
        {
            IDbAccess<District> r;
            string clsName = "DataAccess." + ErpName.ToUpper() + "." + ErpName.ToLower() + "District";
            r = (IDbAccess<District>)_asse.CreateInstance(clsName);
            return r;
        }
        public static IDbAccess<UnitBase> createUnit()
        {
            IDbAccess<UnitBase> r;
            string clsName = "DataAccess." + ErpName.ToUpper() + "." + ErpName.ToLower() + "UnitBase";
            r = (IDbAccess<UnitBase>)_asse.CreateInstance(clsName);
            return r;
        }
        public static IDbAccess<CurrentStock> createCurrentStock()
        {
            IDbAccess<CurrentStock> r;
            string clsName = "DataAccess." + ErpName.ToUpper() + "." + ErpName.ToLower() + "CurrentStock";
            r = (IDbAccess<CurrentStock>)_asse.CreateInstance(clsName);
            return r;
        }
        public static IDbAccess<Warehouse> createWarehouse()
        {
            IDbAccess<Warehouse> r;
            string clsName = "DataAccess." + ErpName.ToUpper() + "." + ErpName.ToLower() + "Warehouse";
            r = (IDbAccess<Warehouse>)_asse.CreateInstance(clsName);
            return r;
        }
        public static IDbAccess<Dispatch> createDispatch()
        {
            IDbAccess<Dispatch> r;
            string clsName = "DataAccess." + ErpName.ToUpper() + "." + ErpName.ToLower() + "Dispatch";
            r = (IDbAccess<Dispatch>)_asse.CreateInstance(clsName);
            return r;
        }
        public static IDbAccess<DispatchMain> createDispatchMain()
        {
            IDbAccess<DispatchMain> r;
            string clsName = "DataAccess." + ErpName.ToUpper() + "." + ErpName.ToLower() + "DispatchMain";
            r = (IDbAccess<DispatchMain>)_asse.CreateInstance(clsName);
            return r;
        }
        public static IDbAccess<DispatchDetail> createDispatchDetail()
        {
            IDbAccess<DispatchDetail> r;
            string clsName = "DataAccess." + ErpName.ToUpper() + "." + ErpName.ToLower() + "DispatchDetail";
            r = (IDbAccess<DispatchDetail>)_asse.CreateInstance(clsName);
            return r;
        }
        public static IDbAccess<SaleOrderMain> createSaleOrderMain()
        {
            IDbAccess<SaleOrderMain> r;
            string clsName = "DataAccess." + ErpName.ToUpper() + "." + ErpName.ToLower() + "SaleOrderMain";
            r = (IDbAccess<SaleOrderMain>)_asse.CreateInstance(clsName);
            return r;
        }
        public static IDbAccess<SaleOrderDetail> createSaleOrderDetail()
        {
            IDbAccess<SaleOrderDetail> r;
            string clsName = "DataAccess." + ErpName.ToUpper() + "." + ErpName.ToLower() + "SaleOrderDetail";
            r = (IDbAccess<SaleOrderDetail>)_asse.CreateInstance(clsName);
            return r;
        }
        public static IDbAccess<SaleOrder> createSaleOrder()
        {
            IDbAccess<SaleOrder> r;
            string clsName = "DataAccess." + ErpName.ToUpper() + "." + ErpName.ToLower() + "SaleOrder";
            r = (IDbAccess<SaleOrder>)_asse.CreateInstance(clsName);
            return r;
        }
        public static IMultiTableQuery createMultiTableQuery()
        {
            IMultiTableQuery r;
            string clsName = "DataAccess." + ErpName.ToUpper() + "." + ErpName.ToLower() + "MultiTableQuery";
            r = (IMultiTableQuery)_asse.CreateInstance(clsName);
            return r;
        }
        public static IDbAccess<Authen> createAuthen()
        {
            IDbAccess<Authen> r;
            string clsName = "DataAccess." + ErpName.ToUpper() + "." + ErpName.ToLower() + "Authen";
            r = (IDbAccess<Authen>)_asse.CreateInstance(clsName);
            return r;
        }
    }
}
