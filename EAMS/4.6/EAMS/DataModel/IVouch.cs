using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public interface IVouch
    {
        VouchMain Main { get; set; }
        IList<VouchDetail> Details { get; set; }
    }

    public abstract class Vouch : IVouch
    {
        public VouchMain Main { get; set; }
        public IList<VouchDetail> Details { get; set; }
    }
}
