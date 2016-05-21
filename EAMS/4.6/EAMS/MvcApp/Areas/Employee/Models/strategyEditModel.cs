using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApp.Areas.Employee.Models
{
    public class strategyEditModel
    {
        public strategyLib.Main Main { get; set; }
        public Webdiyer.WebControls.Mvc.PagedList<strategyLib.Detail> Details { get; set; }
    }
}