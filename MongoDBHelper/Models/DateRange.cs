using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBHelper.Models
{
    public class DateRange : INotifyPropertyChanged
    {
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public TimeSpan? Period => End - Start;
        public TimeSpan? TimeLeft => End - DateTime.Now;
        public bool Expired => TimeLeft.Value.TotalSeconds <= 0;

        public event PropertyChangedEventHandler PropertyChanged;
        public DateRange()
        {
            Start = DateTime.Now;
            End = DateTime.Now.AddHours(45);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (!TimeLeft.HasValue)
                return null;

            var time = TimeLeft.Value;

            if (time.TotalSeconds <= 0)
                return "Expired";

            if (time.Days != 0)
                sb.Append($"{time.Days}d ");

            if(time.Hours != 0)
                sb.Append($"{time.Hours}h ");

            if (time.Minutes != 0)
                sb.Append($"{time.Minutes}m ");

            return sb.ToString();
        }
    }
}
