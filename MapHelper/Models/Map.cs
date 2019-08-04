using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHelper.Models
{
    public class Map
    {
        public string FullName { get; set; }
        public IEnumerable<string> DownloadableFiles
        {
            get
            {
                return null;
            }
        }
    }
}
