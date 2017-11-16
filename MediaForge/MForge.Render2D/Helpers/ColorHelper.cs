using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace MForge.Render2D.Helpers
{
    public class MForgeColors
    {
        public static Color RandomColor(byte alpha = 255)
        {
            var rand = new Random();

            byte r = (byte)rand.Next(0, 255);
            byte g = (byte)rand.Next(0, 255);
            byte b = (byte)rand.Next(0, 255);

            return ColorHelper.FromArgb(alpha, r, g, b);
        }
    }
}
