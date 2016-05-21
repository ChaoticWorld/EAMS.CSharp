using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using SystemBLL;

namespace Petition
{
    public class QCMainDB
    {
        private AppDataPetitionsEntities qc = new AppDataPetitionsEntities();
        private string BaseQuery
            = "SELECT Id,cCode,cCreateMan,dCreateDate,cCusManager,cModifyMan,dModifyDate,cCusCode,cCusName,cCusPerson,dEffDate,dExpDate,cMemo,cResult,cSubject,cVouchCode,cClsFlag FROM QC_Main where 1 = 1 ";

        public int add(QC_Main m)
        {
            int r = -1;
            if (m.dCreateDate.HasValue)
                if (m.dCreateDate.Value.DayOfYear == DateTime.Now.DayOfYear)
                    m.dCreateDate = DateTime.Now;
                else m.dCreateDate = m.dCreateDate.Value;
            else m.dCreateDate = DateTime.Now;
            m.dModifyDate = m.dCreateDate;
            m.cCode = getNewCode(m.cCusManager, m.cClsFlag, m.dCreateDate.Value);
            qc.QC_Main.AddObject(m);
            qc.SaveChanges();
            r = m.Id;
            return r;
        }
        public QC_Main single(int id)
        {
            try
            {
                QC_Main r = qc.QC_Main.Single(m => m.Id == id);
                return r;
            }
            catch { return null; }
        }
        public int delete(int id)
        {
            QC_Main m = single(id);
            if (m != null)
                qc.QC_Main.DeleteObject(m);
            return qc.SaveChanges();
        }


        /// <summary>
        /// 更新数据,返回影响的记录数
        /// </summary>
        /// <param name="o">更新数据Object,数据对象</param>
        /// <returns>返回影响的记录数</returns>
        public int updata(QC_Main _m)
        {
            int r = -1;
            var upd = qc.QC_Main.Single(s => s.Id == _m.Id);
            //upd = _u;
            if (upd.cCusManager != _m.cCusManager || upd.cClsFlag != _m.cClsFlag || upd.dCreateDate != _m.dCreateDate)
            {
                upd.cCode = getNewCode(_m.cCusManager, _m.cClsFlag, _m.dCreateDate.Value);
            }
            else
                upd.cCode = string.IsNullOrEmpty(_m.cCode) ? upd.cCode : _m.cCode;
            upd.cCusCode = string.IsNullOrEmpty(_m.cCusCode) ? upd.cCusCode : _m.cCusCode;
            upd.cCusManager = string.IsNullOrEmpty(_m.cCusManager) ? upd.cCusManager : _m.cCusManager;
            upd.cCusName = string.IsNullOrEmpty(_m.cCusName) ? upd.cCusName : _m.cCusName;
            upd.cCusPerson = string.IsNullOrEmpty(_m.cCusPerson) ? upd.cCusPerson : _m.cCusPerson;
            upd.cModifyMan = string.IsNullOrEmpty(_m.cModifyMan) ? upd.cModifyMan : _m.cModifyMan;
            upd.dModifyDate = DateTime.Now;
            upd.dEffDate = _m.dEffDate;
            upd.dExpDate = _m.dExpDate;
            upd.cSubject = _m.cSubject;
            upd.cVouchCode = _m.cVouchCode;
            upd.cClsFlag = _m.cClsFlag;
            upd.dCreateDate = _m.dCreateDate;

            r = qc.SaveChanges();
            
            return r;
        }

        /// <summary>
        /// 验证指定字段的值是否存在,使用范围:注册,
        /// </summary>
        /// <param name="kv">单个键值对KeyValuePair[string,string]</param>
        /// <returns></returns>
        public bool Exist(KeyValuePair<string, string> _kv)
        {
            bool r = false;
            string QueryString = " and [" + _kv.Key + "] = '" + _kv.Value + "'";
            string Query = BaseQuery + QueryString;
            //以ExecuteStoryQuery方法实现查寻
            try {
                ObjectResult<QC_Main> Qr = qc.ExecuteStoreQuery<QC_Main>(Query);
             r = Qr.Count() > 0 ? true : false; }
            catch (Exception e) { var emsg = e.Message; r = false; }
            return r;
        }
        public bool Exist(QC_Main m)
        {
            bool r = Exist(new KeyValuePair<string, string>("Id", m.Id.ToString()));
            r = r || Exist(new KeyValuePair<string, string>("cCode", m.cCode.ToString()));
            return r;
        }
        
        /// <summary>
        /// 查询数据,返回List
        /// </summary>
        /// <param name="strWhere">为string类型的条件字符串; "it.iUserID >= 0";it为委托变量不要改变</param>
        /// <returns>返回IEnumerable可查询结果</returns>
        public List<QC_Main> list(string strWhere)
        {
            List<QC_Main> r = new List<QC_Main>();

            //以Context.ObjectQuery.Where(paramStr)方法实现
            //(参数)的写法:string paramStr = "it.iUserID >= 0";
            ObjectQuery<QC_Main> selected;
            if (!string.IsNullOrEmpty(strWhere))
                selected = qc.QC_Main.Where(strWhere);
            else
                selected = qc.QC_Main.Where("1=1");
            r = selected.ToList();
            return r;
        }

        public IQueryable<v_petitionList> filter (string key, string Memployee, string Mstartdate, string Mfinishdate
            , string Remployee, string Rstartdate, string Rfinishdate)
        {
            IQueryable<v_petitionList> selected = qc.v_petitionList;
            DateTime ms = new DateTime();
            DateTime mf = new DateTime();
            DateTime rs = new DateTime();
            DateTime rf = new DateTime();
            DateTime max = System.Data.SqlTypes.SqlDateTime.MaxValue.Value;
            DateTime min = System.Data.SqlTypes.SqlDateTime.MinValue.Value;

            //条件A:string.IsNullOrEmpty(startdate)?"_datetime_":startdate;确定startdate的字符串值
            //条件B:DateTime.TryParse(),如果为真s转换为startdate的日期型值,否则s为DateTime.MinValue
            if (DateTime.TryParse(string.IsNullOrEmpty(Mstartdate) ? "_datetime_" : Mstartdate, out ms))
                selected = selected.Where(q =>
                    (EntityFunctions.TruncateTime(ms) <= EntityFunctions.TruncateTime(q.dCreateDate ?? min))
                    || (EntityFunctions.TruncateTime(ms) <= EntityFunctions.TruncateTime(q.dModifyDate ?? min)));
            if (DateTime.TryParse(string.IsNullOrEmpty(Mfinishdate) ? "_datetime_" : Mfinishdate, out mf))
                selected = selected.Where(q =>
                    (EntityFunctions.TruncateTime(q.dCreateDate ?? min) <= EntityFunctions.TruncateTime(mf))
                    || (EntityFunctions.TruncateTime(q.dModifyDate ?? min) <= EntityFunctions.TruncateTime(mf)));
            if (DateTime.TryParse(string.IsNullOrEmpty(Rstartdate) ? "_datetime_" : Rstartdate, out rs))
                selected = selected.Where(q =>
                    (EntityFunctions.AddHours(rs, 0) <= EntityFunctions.TruncateTime(q.QC_SubmitDate ?? min))
                    || (EntityFunctions.TruncateTime(rs) <= EntityFunctions.TruncateTime(q.QC_ReplyDate ?? min))
                    );
            if (DateTime.TryParse(string.IsNullOrEmpty(Rfinishdate) ? "_datetime_" : Rfinishdate, out rf))
                selected = selected.Where(q =>
                    (EntityFunctions.TruncateTime(q.QC_SubmitDate ?? min) <= EntityFunctions.TruncateTime(rf))
                    || (EntityFunctions.TruncateTime(q.QC_ReplyDate ?? min) <= EntityFunctions.TruncateTime(rf))
                    );

            if (!string.IsNullOrEmpty(key))
                selected = selected.Where(q => q.cCode.Contains(key)
                            || q.cCusCode.Contains(key)
                            || q.cCusName.Contains(key)
                            || q.cCusPerson.Contains(key)
                            );
            if (!string.IsNullOrEmpty(Memployee))
            {
                selected = selected.Where(q => q.cCreateMan.Contains(Memployee)
                    || q.cCusManager.Contains(Memployee)
                    || q.cModifyMan.Contains(Memployee)
                    );
            }
            if (!string.IsNullOrEmpty(Remployee))
            {
                selected = selected.Where(q => q.submitMan.Contains(Remployee)
                    || q.replyMan.Contains(Remployee));
            }
            return selected;
        }

        public List<QC_Main> getFiltedQCMainList(string key, string Memployee, string Mstartdate, string Mfinishdate
            , string Remployee, string Rstartdate, string Rfinishdate)
        {
            List<QC_Main> r = new List<QC_Main>();
            IQueryable<v_petitionList> selected = filter(key, Memployee, Mstartdate, Mfinishdate, Remployee, Rstartdate, Rfinishdate);
            
            if (selected != null && selected.Count() > 0)
            {
                var vl = selected.ToList();
                foreach (v_petitionList qv in selected.AsEnumerable())
                {
                    if (!r.Exists(re => re.Id == qv.Id))
                        r.Add(single(qv.Id));
                }
            }
            return r;
        }

        public List<v_petitionList> getFiltedvPetitionList(string key, string Memployee, string Mstartdate, string Mfinishdate
            , string Remployee, string Rstartdate, string Rfinishdate)
        {
            List<v_petitionList> r = new List<v_petitionList>();
            IQueryable<v_petitionList> selected = filter(key, Memployee, Mstartdate, Mfinishdate, Remployee, Rstartdate, Rfinishdate);

            if (selected != null && selected.Count() > 0)
            {
                r = selected.ToList();
            }
            return r;
        }

        /// <summary>
        /// 获得新签呈编号
        /// </summary>
        public string getNewCode(string empName,string clsFlag,DateTime date) {
            QCCodeIdentityDB ciDB = QCCodeIdentityDB.Create();
            string newCode = ciDB.getCode(empName, clsFlag, date);

            return newCode;
        }
    }

    public class QCDetailsDB
    {
        private AppDataPetitionsEntities qc = new AppDataPetitionsEntities();
        private string BaseQuery
            = "SELECT QC_Autoid,QC_RCID,QC_OrderCode,cInvCode,cInvName,cInvStd,iQuentity,iPrice,iSum,cMemo,bDaiShouKuan,QC_MainID,dSaleSumStartDate,dSaleSumFinishDate FROM QC_Details where 1 = 1 ";

        public int add(QC_Details m)
        {
            int r = -1;
            m.bDaiShouKuan = m.bDaiShouKuan ?? false;
            qc.QC_Details.AddObject(m);
            string s1 = qc.QC_DetailClass.ToTraceString();
            string s2 = qc.QC_Details.ToTraceString();
            qc.SaveChanges();
            r = m.QC_Autoid;
            return r;
        }
        public QC_Details single(int id)
        {
            try
            {
                QC_Details r = qc.QC_Details.Single(m => m.QC_Autoid == id);
                return r;
            }
            catch { return null; }
        }
        public int delete(int autoid)
        {
            QC_Details d = single(autoid);
            if (d != null)
                qc.QC_Details.DeleteObject(d);
            return qc.SaveChanges();
        }


        /// <summary>
        /// 更新数据,返回影响的记录数
        /// </summary>
        /// <param name="o">更新数据Object,数据对象</param>
        /// <returns>返回影响的记录数</returns>
        public int updata(QC_Details d)
        {
            QC_Details _d = (QC_Details)d;
            int r = -1;
            var upd = qc.QC_Details.Single(s => s.QC_Autoid == _d.QC_Autoid);
            //upd = _u;
            upd.bDaiShouKuan = _d.bDaiShouKuan ?? false;
            upd.cInvCode = _d.cInvCode;
            upd.cInvName = _d.cInvName;
            upd.cInvStd = _d.cInvStd;
            upd.cMemo = _d.cMemo;
            upd.iPrice = _d.iPrice;
            upd.iQuentity = _d.iQuentity;
            upd.iSum = _d.iSum;
            upd.QC_OrderCode = string.IsNullOrEmpty(_d.QC_OrderCode) ? upd.QC_OrderCode : _d.QC_OrderCode;
            //upd.QC_RCID = upd.QC_RCID;
            upd.dSaleSumFinishDate = _d.dSaleSumFinishDate;
            upd.dSaleSumStartDate = _d.dSaleSumStartDate;
            r = qc.SaveChanges();

            return r;
        }

        /// <summary>
        /// 验证指定字段的值是否存在,使用范围:注册,
        /// </summary>
        /// <param name="kv">单个键值对KeyValuePair[string,string]</param>
        /// <returns></returns>
        public bool Exist(KeyValuePair<string, string> _kv)
        {
            bool r = false;
            string QueryString = " and [" + _kv.Key + "] = '" + _kv.Value + "'";
            string Query = BaseQuery + QueryString;
            //以ExecuteStoryQuery方法实现查寻
            ObjectResult<QC_Details> Qr = qc.ExecuteStoreQuery<QC_Details>(Query);
            try { r = Qr.Count() > 0 ? true : false; }
            catch { r = false; }
            return r;
        }
        public bool Exist(object _m)
        {
            QC_Details m = (QC_Details)_m;
            bool r = Exist(new KeyValuePair<string, string>("QC_Autoid", m.QC_Autoid.ToString()));
            r = r && Exist(new KeyValuePair<string, string>("QC_MainID", m.QC_MainID.ToString()));
            return r;
        }
        public bool Exist(int autoid)
        {
            bool r = Exist(new KeyValuePair<string, string>("QC_Autoid", autoid.ToString()));
            return r;
        }

        /// <summary>
        /// 查询数据,返回List
        /// </summary>
        /// <param name="strWhere">为string类型的条件字符串; "it.iUserID >= 0";it为委托变量不要改变</param>
        /// <returns>返回IEnumerable可查询结果</returns>
        public List<QC_Details> list(string strWhere)
        {
            List<QC_Details> r = new List<QC_Details>();

            //以Context.ObjectQuery.Where(paramStr)方法实现
            //(参数)的写法:string paramStr = "it.iUserID >= 0";
            ObjectQuery<QC_Details> selected;
            if (!string.IsNullOrEmpty(strWhere))
                selected = qc.QC_Details.Where(strWhere);
            else
                selected = qc.QC_Details.Where("1=1");
            r = selected.ToList();
            return r;
        }
        public List<QC_Details> list(int mainid)
        {
            List<QC_Details> r = new List<QC_Details>();
            var e = from d in qc.QC_Details
                    where d.QC_MainID == mainid
                    select d;
            r = e.ToList();
            return r;
        }

        public bool isModifed(QC_Details d)
        {
            bool r = true;
            QC_Details s = single(d.QC_Autoid);
            r &= (s.QC_Autoid == d.QC_Autoid);
            r &= (s.QC_RCID == d.QC_RCID);
            r &= (s.QC_OrderCode == d.QC_OrderCode);
            r &= (s.QC_MainID == d.QC_MainID);
            r &= (s.cInvCode == d.cInvCode);
            r &= (s.cInvName == d.cInvName);
            r &= (s.cInvStd == d.cInvStd);
            r &= (s.cMemo == d.cMemo);
            r &= (s.iPrice == d.iPrice);
            r &= (s.iQuentity == d.iQuentity);
            r &= (s.iSum == d.iSum);
            r &= ((s.bDaiShouKuan ?? false) == (d.bDaiShouKuan ?? false));
            r &= (s.dSaleSumStartDate == d.dSaleSumStartDate);
            r &= (s.dSaleSumFinishDate == d.dSaleSumFinishDate);
            return !r;
        }

        public void addRange(IEnumerable<QC_Details> Dts)
        {
            foreach (QC_Details d in Dts)
                add(d);
        }
    }

    public class QCDetailClassDB
    {
        private AppDataPetitionsEntities qc = new AppDataPetitionsEntities();
        private string BaseQuery
            = "SELECT QC_RCName,QC_RCDescription,QC_RCIndex,QC_RCID FROM QC_DetailClass where 1 = 1 ";

        public int add(QC_DetailClass m)
        { 
            int r = -1;
            qc.QC_DetailClass.AddObject(m);
            qc.SaveChanges();
            r = m.QC_RCID;
            return r;
        }
        public QC_DetailClass single(int id)
        {
            try
            {
                QC_DetailClass r = qc.QC_DetailClass.Single(m => m.QC_RCID == id);
                return r;
            }
            catch { return null; }
        }
        public int delete(int id)
        {
            QC_DetailClass m = single(id);
            if (m != null)
                qc.QC_DetailClass.DeleteObject(m);
            return qc.SaveChanges();
        }


        /// <summary>
        /// 更新数据,返回影响的记录数
        /// </summary>
        /// <param name="o">更新数据Object,数据对象</param>
        /// <returns>返回影响的记录数</returns>
        public int updata(QC_DetailClass dc)
        {
            QC_DetailClass _dc = (QC_DetailClass)dc;
            int r = -1;
            var upd = qc.QC_DetailClass.Single(s => s.QC_RCID == _dc.QC_RCID);
            //upd = _u;
            upd.QC_RCName = string.IsNullOrEmpty(_dc.QC_RCName) ? upd.QC_RCName : _dc.QC_RCName;
            upd.QC_RCIndex =  _dc.QC_RCIndex;
            upd.QC_RCDescription = string.IsNullOrEmpty(_dc.QC_RCDescription) ? upd.QC_RCDescription : _dc.QC_RCDescription;

            r = qc.SaveChanges();

            return r;
        }

        /// <summary>
        /// 验证指定字段的值是否存在,使用范围:注册,
        /// </summary>
        /// <param name="kv">单个键值对KeyValuePair[string,string]</param>
        /// <returns></returns>
        public bool Exist(KeyValuePair<string, string> _kv)
        {
            bool r = false;
            string QueryString = " and [" + _kv.Key + "] = '" + _kv.Value + "'";
            string Query = BaseQuery + QueryString;
            //以ExecuteStoryQuery方法实现查寻
            ObjectResult<QC_DetailClass> Qr = qc.ExecuteStoreQuery<QC_DetailClass>(Query);
            try { r = Qr.Count() > 0 ? true : false; }
            catch { r = false; }
            return r;
        }
        public bool Exist(object _m)
        {
            QC_DetailClass m = (QC_DetailClass)_m;
            bool r = Exist(new KeyValuePair<string, string>("QC_RCID", m.QC_RCID.ToString()));
            return r;
        }

        /// <summary>
        /// 查询数据,返回List
        /// </summary>
        /// <param name="strWhere">为string类型的条件字符串; "it.iUserID >= 0";it为委托变量不要改变</param>
        /// <returns>返回IEnumerable可查询结果</returns>
        public List<QC_DetailClass> list(string strWhere)
        {
            List<QC_DetailClass> r = new List<QC_DetailClass>();

            //以Context.ObjectQuery.Where(paramStr)方法实现
            //(参数)的写法:string paramStr = "it.iUserID >= 0";
            ObjectQuery<QC_DetailClass> selected;
            if (!string.IsNullOrEmpty(strWhere))
                selected = qc.QC_DetailClass.Where(strWhere);
            else
                selected = qc.QC_DetailClass;
            r = selected.ToList();
            return r;
        }
    }

    public class QCReplysDB
    {
        private AppDataPetitionsEntities qc = new AppDataPetitionsEntities();
        private string BaseQuery
            = "SELECT AutoId,QC_MainID,QC_SubmitMan,QC_SubmitDate,QC_ReplyMan,QC_ReplyDate,QC_ReplyContent,QC_ReplyPass,QC_NextID FROM QC_Replys where 1 = 1 ";

        /// <summary>
        /// 增加记录，返回新记录的ID
        /// </summary>
        /// <param name="m">新记录实例</param>
        /// <returns>新记录的ID</returns>
        public int add(QC_Replys m)
        {
            int r = -1;
            int subManCode;
            int replyManCode;
            if (int.TryParse(m.QC_SubmitMan, out subManCode))
                m.submitMan = SystemBLL.UserBLL.getUser(subManCode).cUserName;
            if (int.TryParse(m.QC_ReplyMan, out replyManCode))
                m.replyMan = SystemBLL.UserBLL.getUser(replyManCode).cUserName;
            qc.QC_Replys.AddObject(m);
            qc.SaveChanges();
            r = m.AutoId;
            return r;
        }
        public QC_Replys single(int id)
        {
            try
            {
                QC_Replys r = qc.QC_Replys.Single(m => m.AutoId == id);
                return r;
            }
            catch { return null; }
        }
        public int delete(int id)
        {
            QC_Replys m = single(id);
            if (m != null)
                qc.QC_Replys.DeleteObject(m);
            return qc.SaveChanges();
        }


        /// <summary>
        /// 更新数据,返回影响的记录数
        /// </summary>
        /// <param name="o">更新数据Object,数据对象</param>
        /// <returns>返回影响的记录数</returns>
        public int updata(QC_Replys qr)
        {
            QC_Replys _r = (QC_Replys)qr;
            int r = -1;
            var upd = qc.QC_Replys.Single(s => s.AutoId == _r.AutoId);
            //upd = _u;
            upd.QC_MainID = upd.QC_MainID;
            upd.QC_NextID = _r.QC_NextID;
            upd.QC_ReplyContent = string.IsNullOrEmpty(_r.QC_ReplyContent) ? upd.QC_ReplyContent : _r.QC_ReplyContent;
            upd.QC_ReplyMan = string.IsNullOrEmpty(_r.QC_ReplyMan) ? upd.QC_ReplyMan : _r.QC_ReplyMan;
            upd.QC_ReplyPass = _r.QC_ReplyPass;
            upd.QC_ReplyDate = _r.QC_ReplyDate;

            int subManCode;
            int replyManCode;
            if (int.TryParse(_r.QC_SubmitMan, out subManCode))
                upd.submitMan = SystemBLL.UserBLL.getUser(subManCode).cUserName;
            if (int.TryParse(_r.QC_ReplyMan, out replyManCode))
                upd.replyMan = SystemBLL.UserBLL.getUser(replyManCode).cUserName;
            r = qc.SaveChanges();

            return r;
        }

        /// <summary>
        /// 验证指定字段的值是否存在,使用范围:注册,
        /// </summary>
        /// <param name="kv">单个键值对KeyValuePair[string,string]</param>
        /// <returns></returns>
        public bool Exist(KeyValuePair<string, string> _kv)
        {
            bool r = false;
            string QueryString = " and [" + _kv.Key + "] = '" + _kv.Value + "'";
            string Query = BaseQuery + QueryString;
            //以ExecuteStoryQuery方法实现查寻
            ObjectResult<QC_Replys> Qr = qc.ExecuteStoreQuery<QC_Replys>(Query);
            try { r = Qr.Count() > 0 ? true : false; }
            catch { r = false; }
            return r;
        }
        public bool Exist(object _m)
        {
            QC_Replys m = (QC_Replys)_m;
            bool r = Exist(new KeyValuePair<string, string>("Autoid", m.AutoId.ToString()));
            r |= Exist(new KeyValuePair<string,string>("QC_MainID",m.QC_MainID.ToString()));
            return r;
        }

        /// <summary>
        /// 查询数据,返回List
        /// </summary>
        /// <param name="strWhere">为string类型的条件字符串; "it.iUserID >= 0";it为委托变量不要改变</param>
        /// <returns>返回IEnumerable可查询结果</returns>
        public List<QC_Replys> list(string strWhere)
        {
            List<QC_Replys> r = new List<QC_Replys>();

            //以Context.ObjectQuery.Where(paramStr)方法实现
            //(参数)的写法:string paramStr = "it.iUserID >= 0";
            ObjectQuery<QC_Replys> selected;
            if (!string.IsNullOrEmpty(strWhere))
                selected = qc.QC_Replys.Where(strWhere);
            else
                selected = qc.QC_Replys.Where("1=1");
            r = selected.ToList();
            return r;
        }

        public List<QC_Replys> list(int id)
        {
            List<QC_Replys> r = new List<QC_Replys>();
            var l = from rp in qc.QC_Replys
                    where rp.QC_MainID == id
                    orderby rp.AutoId descending
                    select rp;
            r = l.ToList();
            return r;
        }
    }

    public class QCtxtContentDB {
        private AppDataPetitionsEntities qc = new AppDataPetitionsEntities();
        private string BaseQuery
            = "SELECT autoid,id,plan,vouchCode FROM QC_txtContent where 1 = 1 ";

        /// <summary>
        /// 增加记录，返回新记录的ID
        /// </summary>
        /// <param name="m">新记录实例</param>
        /// <returns>新记录的ID</returns>
        public int add(QC_txtContent m)
        {
            int r = -1;
            
            qc.QC_txtContent.AddObject(m);
            qc.SaveChanges();
            r = m.autoid;
            return r;
        }
        public QC_txtContent single(int id)
        {
            try
            {
                QC_txtContent r = qc.QC_txtContent.Single(m => m.autoid == id);
                return r;
            }
            catch { return null; }
        }
        public int delete(int id)
        {
            QC_txtContent m = single(id);
            if (m != null)
                qc.QC_txtContent.DeleteObject(m);
            return qc.SaveChanges();
        }


        /// <summary>
        /// 更新数据,返回影响的记录数
        /// </summary>
        /// <param name="o">更新数据Object,数据对象</param>
        /// <returns>返回影响的记录数</returns>
        public int updata(QC_txtContent qr)
        {
            QC_txtContent _r = qr;
            int r = -1;
            var upd = qc.QC_txtContent.Single(s => s.autoid == _r.autoid);
            
            upd.id = upd.id;
            upd.plan = _r.plan;
            upd.order = _r.order;

            r = qc.SaveChanges();

            return r;
        }

        /// <summary>
        /// 验证指定字段的值是否存在,使用范围:注册,
        /// </summary>
        /// <param name="kv">单个键值对KeyValuePair[string,string]</param>
        /// <returns></returns>
        public bool Exist(KeyValuePair<string, string> _kv)
        {
            bool r = false;
            string QueryString = " and [" + _kv.Key + "] = '" + _kv.Value + "'";
            string Query = BaseQuery + QueryString;
            //以ExecuteStoryQuery方法实现查寻
            ObjectResult<QC_txtContent> Qr = qc.ExecuteStoreQuery<QC_txtContent>(Query);
            try { r = Qr.Count() > 0 ? true : false; }
            catch { r = false; }
            return r;
        }
        public bool Exist(QC_txtContent _m)
        {
            QC_txtContent m = _m;
            bool r = Exist(new KeyValuePair<string, string>("autoid", m.autoid.ToString()));
            r |= Exist(new KeyValuePair<string,string>("id",m.id.ToString()));
            return r;
        }

        /// <summary>
        /// 查询数据,返回List
        /// </summary>
        /// <param name="strWhere">为string类型的条件字符串; "it.iUserID >= 0";it为委托变量不要改变</param>
        /// <returns>返回IEnumerable可查询结果</returns>
        public List<QC_txtContent> list(string strWhere)
        {
            List<QC_txtContent> r = new List<QC_txtContent>();

            //以Context.ObjectQuery.Where(paramStr)方法实现
            //(参数)的写法:string paramStr = "it.iUserID >= 0";
            ObjectQuery<QC_txtContent> selected;
            if (!string.IsNullOrEmpty(strWhere))
                selected = qc.QC_txtContent.Where(strWhere);
            else
                selected = qc.QC_txtContent.Where("1=1");
            r = selected.ToList();
            return r;
        }

        public List<QC_txtContent> list(int id)
        {
            List<QC_txtContent> r = new List<QC_txtContent>();
            var l = from rp in qc.QC_txtContent
                    where rp.id == id
                    orderby rp.autoid descending
                    select rp;
            r = l.ToList();
            return r;
        }
    }
    public class QCClassDB:IDB<QC_Class>
    {
        static AppDataPetitionsEntities qc = new AppDataPetitionsEntities();
        /// <summary>
        /// 返回列表
        /// </summary>
        /// <returns>返回列表</returns>
        public IEnumerable<QC_Class> list() {
            return qc.QC_Class.AsEnumerable();
        }
        /// <summary>
        /// 新增数据，返回记录Id
        /// </summary>
        /// <param name="t">id为-1的新记录数据</param>
        /// <returns>返回新记录Id</returns>
        public int Add(QC_Class t)
        {
            int newId = -1;
            qc.QC_Class.AddObject(t);
            qc.SaveChanges();
            newId = t.iClsAutoId;
            return newId;
        }
        /// <summary>
        /// 更新数据，返回更新数据的Id
        /// </summary>
        /// <param name="t">要更新的数据</param>
        /// <returns>返回更新数据的Id</returns>
        public int Update(QC_Class t)
        {
            int id = t.iClsAutoId;
            var qcc = qc.QC_Class.Single(s => s.iClsAutoId == id);
            qcc.iToDayMax = t.iToDayMax;
            qcc.dToDay = t.dToDay;
            qcc.cClsName = t.cClsName;
            qcc.cClsFlag = t.cClsFlag;

            qc.SaveChanges();
            return id;
        }
        /// <summary>
        /// 删除指定Id的数据记录
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var qcc = qc.QC_Class.Single(s => s.iClsAutoId == id);
            qc.QC_Class.DeleteObject(qcc);
            qc.SaveChanges();
        }
        /// <summary>
        /// 返回指定Id的单记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns>返回指定Id的单记录</returns>
        public QC_Class Signal(int id)
        {
            var qcc = qc.QC_Class.Single(s => s.iClsAutoId == id);
            return qcc;
        }
        public QC_Class Signal(string flag)
        {
            var qcc = qc.QC_Class.Single(s => s.cClsFlag == flag);
            return qcc;
        }
        /// <summary>
        /// 查寻参数对象中条件的数据
        /// </summary>
        /// <param name="t">包含条件的对象</param>
        /// <returns>返回查寻结果</returns>
        public IEnumerable<QC_Class> Find(QC_Class t)
        {
            IEnumerable<QC_Class> r = null;
            var f = qc.QC_Class.Where(qw => qw.iClsAutoId > 0);
            if (!string.IsNullOrEmpty(t.cClsName))
                f = f.Where(w => w.cClsFlag == t.cClsName);
            if (!string.IsNullOrEmpty(t.cClsFlag))
                f = f.Where(w => w.cClsFlag == t.cClsFlag);
            r = f.AsEnumerable();
            return r;
        }
        public static QCClassDB Create() {
            return new QCClassDB();
        }
    }

    public class QCCodeIdentityDB : IDB<QC_CodeIdentity>
    {
        static AppDataPetitionsEntities qc = new AppDataPetitionsEntities();
        /// <summary>
        /// 返回列表
        /// </summary>
        /// <returns>返回列表</returns>
        public IEnumerable<QC_CodeIdentity> list()
        {
            return qc.QC_CodeIdentity.AsEnumerable();
        }
        /// <summary>
        /// 新增数据，返回记录Id
        /// </summary>
        /// <param name="t">id为-1的新记录数据</param>
        /// <returns>返回新记录Id</returns>
        public int Add(QC_CodeIdentity t)
        {
            int newId = -1;
            qc.QC_CodeIdentity.AddObject(t);
            try
            {
                qc.SaveChanges();
            }
            catch (Exception e) {
                var em = e.Message;
                throw e;
            }
            newId = t.autoID;
            return newId;
        }
        /// <summary>
        /// 更新数据，返回更新数据的Id
        /// </summary>
        /// <param name="t">要更新的数据</param>
        /// <returns>返回更新数据的Id</returns>
        public int Update(QC_CodeIdentity t)
        {
            int id = t.autoID;
            var qcc = qc.QC_CodeIdentity.Single(s => s.autoID == id);
            qcc.iSN = t.iSN;
            try
            {
                qc.SaveChanges();
            }
            catch (Exception e)
            { var em = e.Message;
            throw e;
            }
            return id;
        }
        /// <summary>
        /// 删除指定Id的数据记录
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var qcc = qc.QC_CodeIdentity.Single(s => s.autoID == id);
            qc.QC_CodeIdentity.DeleteObject(qcc);
            qc.SaveChanges();
        }
        /// <summary>
        /// 返回指定Id的单记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns>返回指定Id的单记录</returns>
        public QC_CodeIdentity Signal(int id)
        {
            var qcc = qc.QC_CodeIdentity.Single(s => s.autoID == id);
            return qcc;
        }
        /// <summary>
        /// 查寻参数对象中条件的数据
        /// </summary>
        /// <param name="t">包含条件的对象</param>
        /// <returns>返回查寻结果</returns>
        public IEnumerable<QC_CodeIdentity> Find(QC_CodeIdentity t)
        {
            IEnumerable<QC_CodeIdentity> r = null;
            var f = qc.QC_CodeIdentity.Where(qw => qw.autoID > 0);
            if (!string.IsNullOrEmpty(t.cManager))
                f = f.Where(w => w.cManager == t.cManager);
            if (!string.IsNullOrEmpty(t.cClsFlag))
                f = f.Where(w => w.cClsFlag == t.cClsFlag);
            if (t.dDate.HasValue)
            {
                DateTime s, e,td;
                td = t.dDate.Value;
                s = td.Date; e = td.Date.AddDays(1);
                f = f.Where(w => s<=w.dDate && w.dDate<e);
            }
            r = f.AsEnumerable();
            return r;
        }
        public static QCCodeIdentityDB Create()
        {
            return new QCCodeIdentityDB();
        }
        public string getCode(string manager, string clsFlag, DateTime dt)
        {
            StringBuilder code = new StringBuilder();
            QC_CodeIdentity f = new QC_CodeIdentity()
            {
                cManager = manager,
                dDate = dt.Date,
                cClsFlag = clsFlag
            };
            IEnumerable<QC_CodeIdentity> Codes = Find(f);
            QC_CodeIdentity ci;
            int sn = -1;
            if (Codes != null && Codes.Count() > 0)
            {
                ci = Codes.First();
                sn = ci.iSN.Value+1;
                code.Append(ci.cManager);
                code.Append(ci.cClsFlag);
                code.Append(ci.dDate.Value.ToString("yyyyMMdd"));
                code.Append(sn.ToString("D3"));
                ci.iSN = sn;
                Update(ci);
            }
            else {
                f.iSN = 1;
                try
                {
                    Add(f);
                }
                catch (Exception e) { var em = e.Message; }
                code.Append(f.cManager);
                code.Append(f.cClsFlag);
                code.Append(f.dDate.Value.ToString("yyyyMMdd"));
                code.Append(f.iSN.Value.ToString("D3"));
            }
            return code.ToString();
        }
    }

}
