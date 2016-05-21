using System;
using System.Collections.Generic;
namespace Attendance.Model
{ public partial class SchemeClassModel
    {
        public SchemeClassModel()
        {
            this.Attendance_ClassPlan = new HashSet<Attendance_ClassPlan>();
        }
    
        public int classId { get; set; }
        public Nullable<int> schemeId { get; set; }
        public string className { get; set; }
    
        public virtual ICollection<Attendance_ClassPlan> Attendance_ClassPlan { get; set; }
        public virtual Attendance_Scheme Attendance_Scheme { get; set; }
    }
}
