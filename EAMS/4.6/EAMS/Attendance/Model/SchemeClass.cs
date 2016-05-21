using System;
using System.Collections.Generic;

namespace Attendance.Model
{ public partial class SchemeClassModel
    {
        public SchemeClassModel()
        {  }
    
        public int classId { get; set; }
        public Nullable<int> schemeId { get; set; }
        public string className { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual List<ClassPlanModel> ClassPlans { get { return getClassPlans(); } }
        [Newtonsoft.Json.JsonIgnore]
        public virtual SchemeModel Scheme { get { return getScheme(); } }

        private List<ClassPlanModel> getClassPlans() {
            List<ClassPlanModel> r = new List<ClassPlanModel>();
            DAL.ClassPlanDAL cpDal = new DAL.ClassPlanDAL();
            r = cpDal.selects(new ClassPlanModel() { classId = classId });
            return r;
        }
        private SchemeModel getScheme()
        {
            SchemeModel r = new SchemeModel();
            DAL.SchemeDAL sDal = new DAL.SchemeDAL();
            r = sDal.Single(schemeId??-1);
            return r;
        }
    }
}
