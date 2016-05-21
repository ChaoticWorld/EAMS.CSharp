using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApp.Areas.Employee.Models
{
    public class petitionEditModel
    {
        public Petition.QC_Main QCM { get; set; }
        public List<Petition.QC_txtContent> QCtxt { get; set; }
        public bool isNew { get; set; }
        public bool isReply { get; set; }

        public static petitionEditModel createModel() {
            return new petitionEditModel() {
                QCM = null, QCtxt = null, isNew = true, isReply = false }; 
        }

    }
}