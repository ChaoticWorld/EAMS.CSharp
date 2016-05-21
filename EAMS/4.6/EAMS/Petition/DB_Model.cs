using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Petition
{
    public partial class QC_Main
    {
        [Display(Name="内容摘要")]
        public string remark { get; set; }
        [Display(Name="处理过程")]
        public string resultTitle { get; set; }
        [Display(Name="处理结果")]
        public string result { get; set; }
        public QC_Main() { this.Id = -1; }
    }
    public  partial class  QC_Details
    { }
    public partial class  QC_DetailClass
    { }
    public partial class  QC_Replys
    {
    }
    public partial class QC_Class
    {
        public string saveState { get; set; }
    }
    public partial class QC_txtContent {
        public string saveState { get; set; }
    }
    public partial class QC_CodeIdentity
    {
        public string Code { get; set; }
    }
}
