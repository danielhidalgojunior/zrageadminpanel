using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHelper
{
    public interface IProgress
    {
        int Progress { get; set; }
        bool Done { get; set; }
    }
}
