using System;
namespace Attendance.Model
{  public partial class ClassPlanModel
    {
        public int autoid { get; set; }
        public Nullable<int> classId { get; set; }
        public int periodNo { get; set; }
        public string bTime { get; set; }
        public string eTime { get; set; }
        public string upTime { get; set; }
        public Nullable<System.DateTime> sdate { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual SchemeClassModel SchemeClass { get { return getSchemeClass(); } }
        private SchemeClassModel getSchemeClass()
        {
            SchemeClassModel r = new SchemeClassModel();
            DAL.SchemeClassDAL scDal = new DAL.SchemeClassDAL();
            r = scDal.Single(classId?? -1);
            return r;
        }
    }
}
