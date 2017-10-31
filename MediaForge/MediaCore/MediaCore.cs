using MediaCore.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCore
{
    public class MediaHelper
    {
        public static FPS MillisecondsToFPS(long frameTime)
        {
            var values = Enum
                .GetValues(typeof(FPS))
                .Cast<int>()
                .Select(x => new { value = x, delta = Math.Abs(frameTime - x) });

            var value = values.Where(x => x.delta == values.Min(y => y.delta)).FirstOrDefault();
            return (FPS)value.value;
        }
    }
}
