using System;
using System.Collections.Generic;

namespace Attendance.Model
{
    public partial class SchemeModel
    {
        public SchemeModel()
        { }
    
        public int schemeID { get; set; }
        public string schemeName { get; set; }
        public string schemeDescription { get; set; }
        public Nullable<int> periods { get; set; }
        public Nullable<int> classs { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<SchemeClassModel> SchemeClasss { get { return getSchemeClasss(); } }
        private ICollection<SchemeClassModel> getSchemeClasss() {
            List<SchemeClassModel> r = new List<SchemeClassModel>();
            DAL.SchemeClassDAL scDal = new DAL.SchemeClassDAL();
            r = scDal.selects(new SchemeClassModel() { schemeId = schemeID });
            return r;
        }
    }
}
