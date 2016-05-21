using System;
using System.Collections.Generic;
using System.Text;
using DataModel;
using FluentData;
using IDataAccess;

namespace DataAccess.U8
{
    public class u8SaleOrder :u8<SaleOrder>
    {
        private SaleOrder _SaleOrder = new SaleOrder();
        private List<SaleOrder> _SaleOrders = new List<SaleOrder>();
        public override List<SaleOrder> getList(string whereStr)
        {
            List<SaleOrder> r = new List<SaleOrder>();
            string cmd = "" + whereStr;
            r = Context.Sql("").QueryMany<SaleOrder>();
            return r;
        }

        public override SaleOrder getSingle(string code)
        {
            u8SaleOrderMain u8dm = new u8SaleOrderMain();
            u8SaleOrderDetail u8dd = new u8SaleOrderDetail();
            _SaleOrder.Main = u8dm.getSingle(code);
            _SaleOrder.Details = u8dd.getList(new VouchDetail() { Mid = _SaleOrder.Main.Mid });
            _SaleOrder.Main.Je = 0;
            if (_SaleOrder.Details != null && _SaleOrder.Details.Count > 0)
                foreach (var dd in _SaleOrder.Details)
                {
                    _SaleOrder.Main.Je += Convert.ToDecimal(dd.iSum);
                }
            return _SaleOrder;
        }

        public override List<SaleOrder> getList(SaleOrder searchKey)
        {
            u8SaleOrderMain u8dm = new u8SaleOrderMain();
            u8SaleOrderDetail u8dd = new u8SaleOrderDetail();
            _SaleOrders = new List<SaleOrder>();
            if (searchKey != null) {
                if (searchKey.Main != null)
                {
                    var dms = u8dm.getList(searchKey.Main);
                    foreach (var dm in dms)
                    {
                        if (!_SaleOrders.Exists(e => e.Main.vouchCode == dm.vouchCode))
                        {
                            _SaleOrders.Add(getSingle(dm.vouchCode));
                        }
                    }
                }
                if (searchKey.Details != null)
                {
                    List<VouchMain> dms;
                    foreach (var kd in searchKey.Details)
                    { dms = u8dm.getList(new VouchMain() { Mid = kd.Mid });
                        foreach (var dm in dms)
                        {
                            if (!_SaleOrders.Exists(e => e.Main.vouchCode == dm.vouchCode))
                            {
                                _SaleOrders.Add(getSingle(dm.vouchCode));
                            }
                        }
                    }
                }
            }
            return _SaleOrders;
        }

        public override void setField(string field, string val, SaleOrder searchKey)
        {
            //不提供
        }

        public override void setField(string field, string val, string whereStr)
        {
            //不提供
        }
        public override object getField(string field, SaleOrder searchKey)
        {//不提供
            object r = null;
            return r;
        }
    }
}
