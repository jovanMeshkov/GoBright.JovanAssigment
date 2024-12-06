using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AssigmentBackend.Business.Utilities
{
    public class DateTimeInterval
    {
        public DateTime Start { get; init; }
        public DateTime End { get; init; }
        public DateTimeInterval()
        {

        }

        public DateTimeInterval(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public bool Overlap(DateTimeInterval interval)
        {
            return Start < interval.End && End > interval.Start;
        }

        public bool Overlap((DateTime start, DateTime end) interval)
        {
            var i = new DateTimeInterval(interval.start, interval.end);

            return Overlap(i);
        }
    }
}
