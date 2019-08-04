using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBHelper
{
    public static class StringExtensions
    {
        public static string NormalizeCollectionName(this string name)
        {
            return name.ToLower().Replace("model", "") + 's';
        }
    }
}
