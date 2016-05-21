using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CallInfo
{
    public class callRecordsBLL
    {
        AppDataPhoneEntities phoneEntities = new AppDataPhoneEntities();
        public callRecordsBLL() { }
        ~callRecordsBLL() { }

        public Phone_Records single(int id)
        {
            Phone_Records r = new Phone_Records();
            r = phoneEntities.Phone_Records.Single(s => s.ID == id);
            return r;
        }
        /// <summary>
        /// 来电后自动增加
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public int add(Phone_Records r)
        {
            r.callDate = r.callDate ?? DateTime.Now;
            phoneEntities.Phone_Records.AddObject(r);
            phoneEntities.SaveChanges();
            return r.ID;
        }

        public void update(Phone_Records r)
        {
            Phone_Records upd = phoneEntities.Phone_Records.Single(pr => pr.ID == r.ID);
            upd.callContent = string.IsNullOrEmpty(r.callContent) ? "" : r.callContent;
            phoneEntities.SaveChanges();
        }

        /// <summary>
        /// 返回指定客户编码的通话记录,并以日期排序(降序)
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public IEnumerable<Phone_Records> list(string code)
        {
            IEnumerable<Phone_Records> r = null;
            r = phoneEntities.Phone_Records.Where(pr => 
                pr.dwCode == code && !string.IsNullOrEmpty(pr.callContent))
                .OrderByDescending(pro=>pro.callDate);

            return r;
        }
    }
}
