using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SourceQueryHandler.Models
{
    public class NextMap : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Map { get; set; }
        public string TimeLeft { get; set; }
        public bool EnableUpdate { get; set; } = true;

        public static string ParseTimeLeftResponse(string response)
        {
            if (response == null)
                return null;
                
            var result = response.Split('\n')[0].Replace("[SM]", "").Trim(' ').Split(' ').Last();

            return result;
        }

        public static string ParseNextMapResponse(string response)
        {
            if (response == null)
                return null;

            if (response.Contains("pendente"))
                return "Votação pendente";

            var result = response.Split('\n')[0].Replace("[SM]", "").Trim(' ');

            if (result.Contains(' '))
            {
                var arr = result.Split(' ');
                result = arr.Last();
            }
                
            return result;
        }
    }
}
