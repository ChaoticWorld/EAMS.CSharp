using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace DB.DynamicIP
{
    public class DBBLL
    {
        //public enum SaveState { success = 1, fail = -1, unModify = -2 };
        //public SaveState saveState { get; set; }
        Entities vpnEntry = new Entities();
       
        
        public int add(vpn_Register reg)
        {
            reg.createDate = DateTime.Now;
            reg.KEY = createKey(reg.Name, reg.vpnID, reg.vpnPW, reg.vpnMac);
            reg.Name = string.IsNullOrEmpty(reg.Name) ? " " : reg.Name;
            reg.vpnID = string.IsNullOrEmpty(reg.vpnID) ? " " : reg.vpnID;
            vpnEntry.vpn_Register.Add(reg);

            vpnEntry.SaveChanges();
            return reg.autoid;
        }
        public int update(vpn_Register reg)
        {
            int r = -1;
            var entryUpdate = vpnEntry.vpn_Register.Single(s => s.autoid == reg.autoid);
            entryUpdate.Name = reg.Name;
            entryUpdate.vpnID = reg.vpnID;
            entryUpdate.vpnIP = reg.vpnIP;
            entryUpdate.vpnPW = reg.vpnPW;
            entryUpdate.vpnEncryptionType = reg.vpnEncryptionType;
            entryUpdate.enable = reg.enable;
            entryUpdate.vpnMac = reg.vpnMac;
            entryUpdate.modifyDate = DateTime.Now;
            entryUpdate.KEY = createKey(entryUpdate.Name, entryUpdate.vpnID, entryUpdate.vpnPW, entryUpdate.vpnMac);

            int sc = vpnEntry.SaveChanges();
            r = (sc > 0) ? reg.autoid : 0;
            return r;
        }

        /// <summary>
        /// 注册新IP地址,返回值:成功>0,失败<=0
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="IP"></param>
        /// <returns></returns>
        public string registerIP(string Key,string IP)
        {
            string r = "";
            if (Exist(key: Key))
            {
                var entryUpdate = vpnEntry.vpn_Register.Single(s => s.KEY == Key);
                if (entryUpdate.vpnIP == IP) return "IP地址未变更!";
                entryUpdate.vpnIP = IP;
                entryUpdate.modifyDate = DateTime.Now;

                if (vpnEntry.SaveChanges() > 0) r = "成功!";
            }
            else r = "key不存在!";
            return r;
        }

        public int delete(int autoid)
        {
            int r = -1;
            var entryDelete = vpnEntry.vpn_Register.Single(s => s.autoid == autoid);
            vpnEntry.vpn_Register.Remove(entryDelete);
            r = vpnEntry.SaveChanges();
            return r;
        }
        public IEnumerable<vpn_Register> filter(string name = null,string key = null)
        {
            var f = vpnEntry.vpn_Register.AsQueryable();
            if (!string.IsNullOrEmpty(name))
                f = f.Where(fw => fw.Name == name);
            if (!string.IsNullOrEmpty(key))
                f = f.Where(fw => fw.KEY == key);
            return f.AsEnumerable();
        }
        public bool Exist(string name = null, string key = null)
        {
            bool r = false;
            r = filter(name, key).Count() > 0;
            return r;
        }
        public string createKey(string name ,string id,string pwd ,string mac ) 
        {
            string strKey = string.Empty;
            if (string.IsNullOrEmpty(name)
                && string.IsNullOrEmpty(id)
                && string.IsNullOrEmpty(pwd)
                && string.IsNullOrEmpty(mac)) return null;
            StringBuilder r = new StringBuilder();
            r.Append(id);
            r.Append(name);
            r.Append(pwd);
            r.Append(mac);
            strKey = MD5_Crypto.MD5(r.ToString());//加密
            return strKey;
        }

        public vpn_Register signal(int autoid)
        {
            //throw new NotImplementedException();
            return vpnEntry.vpn_Register.Single(s=>s.autoid == autoid);
        }

        public vpn_Register signal(string key)
        {
            //throw new NotImplementedException();
            return vpnEntry.vpn_Register.Single(s => s.KEY == key);
        }
    }
}
