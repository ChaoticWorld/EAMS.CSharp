using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using DataDB.u8;
using DataDB.ModelBase;

namespace CallInfo
{
    [Serializable]
    public class callDWModel
    {
        private string _cid;
        [Display(Name="来电号码")]
        public string callid { get { return _cid; } set { _cid = value; } }
        public string dwCode { get; set; }
        //public List<string> dwCodeList { get; set; }
        public List<Customer> cusInfo { get; set; }
        public List<Contact> conInfo { get; set; }
        public List<Vendor> venInfo { get; set; }
        public bool IsNull { get; private set; }
        public void setCallIdNull(bool s)
        { IsNull = s; }
        /// <summary>
        /// 来电单位词典，key=code,value=name
        /// </summary>
        public Dictionary<string,string> DwList { get; set; }
        /// <summary>
        /// 来电联系人词典，key=code,value=name
        /// </summary>
        public Dictionary<string, string> ContactList { get; set; }

        public callDWModel() { IsNull = true; }
    }
    [Serializable]
    public class CallDWInfo:ErpFactory
    {
        private callDWModel _cm = new callDWModel();
        public callDWModel CallDWModel { get { return _cm; } private set { _cm = value; } }

        public CallDWInfo()
        {  }
        public CallDWInfo(string callid = null, string code = null, string key = null)
        {
            _cm = new callDWModel();
            if (!string.IsNullOrEmpty(callid))
                _cm = getModelwithCallId(cid: callid);
            else if (!string.IsNullOrEmpty(code))
                _cm = getModelwithDWCode(code: code);
            else if (!string.IsNullOrEmpty(key))
                _cm = getModelwithSearchKey(key: key);
        }
        ~CallDWInfo() { }

        public static callDWModel getModelwithDWCode(string code)
        {
            callDWModel r = new callDWModel();
            r.dwCode = code;
            List<Customer> _customers = null;
            List<Contact> _contacts = null;
            List<Vendor> _vendors = null;
            dbu8.getModelWithCode(code: r.dwCode, cusList: out _customers, conList: out _contacts, venList: out _vendors);
            r.cusInfo = _customers;
            r.conInfo = _contacts;
            r.venInfo = _vendors;
            //来电单位列表
            if (r.DwList == null) r.DwList = new Dictionary<string, string>();
            if (null != r.cusInfo)
                foreach (Customer c in r.cusInfo)
                    r.DwList.Add(c.cCusCode, c.cCusName);
            if (null != r.venInfo)
                foreach (Vendor v in r.venInfo)
                    r.DwList.Add(v.cVenCode, v.cVenName);

            //来电联系人列表
            if (r.ContactList == null) r.ContactList = new Dictionary<string, string>();
            if (null != r.conInfo)
                for (int i = 0; i < r.conInfo.Count; i++)
                    r.ContactList.Add(r.conInfo[i].code ?? "CONTACT" + i.ToString(), r.conInfo[i].name);
            if (r.conInfo == null && r.venInfo == null && r.cusInfo == null)
                r.setCallIdNull(true);
            else r.setCallIdNull(false);
            return r;
        }
        public static callDWModel getModelwithCallId(string cid)
        {
            callDWModel r = new callDWModel();
            r.callid = cid;
            List<Customer> _customers = null;
            List<Contact> _contacts = null;
            List<Vendor> _vendors = null;
            dbu8.getModelWithCallID(cid, cusList: out _customers, conList: out _contacts, venList: out _vendors);
            r.cusInfo = _customers;
            r.conInfo = _contacts;
            r.venInfo = _vendors;
            //来电单位列表
            if (r.DwList == null) r.DwList = new Dictionary<string, string>();
            if (null != r.cusInfo)
                foreach (Customer c in r.cusInfo)
                    r.DwList.Add(c.cCusCode, c.cCusName);
            if (null != r.venInfo)
                foreach (Vendor v in r.venInfo)
                    r.DwList.Add(v.cVenCode, v.cVenName);

            //来电联系人列表
            if (r.ContactList == null) r.ContactList = new Dictionary<string, string>();
            if (null != r.conInfo)
                for (int i = 0; i < r.conInfo.Count; i++)
                    r.ContactList.Add(r.conInfo[i].code ?? "CONTACT" + i.ToString(), r.conInfo[i].name);
            if (r.conInfo == null && r.venInfo == null && r.cusInfo == null)
                r.setCallIdNull(true);
            else r.setCallIdNull(false);
            return r;
        }
        public static callDWModel getModelwithSearchKey(string key)
        {
            callDWModel r = new callDWModel();
            List<Customer> _customers = null;
            List<Contact> _contacts = null;
            List<Vendor> _vendors = null;
            dbu8.getModelWithSearchKey(key:key, cusList: out _customers, conList: out _contacts, venList: out _vendors);
            r.cusInfo = _customers;
            r.conInfo = _contacts;
            r.venInfo = _vendors;
            //来电单位列表
            if (r.DwList == null) r.DwList = new Dictionary<string, string>();
            if (null != r.cusInfo)
                foreach (Customer c in r.cusInfo)
                    r.DwList.Add(c.cCusCode, c.cCusName);
            if (null != r.venInfo)
                foreach (Vendor v in r.venInfo)
                    r.DwList.Add(v.cVenCode, v.cVenName);

            //来电联系人列表
            if (r.ContactList == null) r.ContactList = new Dictionary<string, string>();
            if (null != r.conInfo)
                for (int i = 0; i < r.conInfo.Count; i++)
                    r.ContactList.Add(r.conInfo[i].code ?? "CONTACT" + i.ToString(), r.conInfo[i].name);
            if (r.conInfo == null && r.venInfo == null && r.cusInfo == null)
                r.setCallIdNull(true);
            else r.setCallIdNull(false);
            return r;
        }
    }
}
