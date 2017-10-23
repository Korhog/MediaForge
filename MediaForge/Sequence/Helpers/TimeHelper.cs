using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Helpers
{
    public class TimeHelper
    {
        public static double TimeSpanToDouble(TimeSpan timeSpan, double pixelsInSecond = 60)
        {
            return ((double)timeSpan.Ticks / 10000000.0) * pixelsInSecond;
        }

        public static TimeSpan DoubleToTimeSpan(double value, double pixelsInSecond = 60)
        {
            var ticks = (long)((value / pixelsInSecond) * 10000000);
            var timeSpan = new TimeSpan(ticks);
            return timeSpan;
        }
    }
}
