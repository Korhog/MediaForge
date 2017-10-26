using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Render.EffectSettings
{
    /// <summary>
    /// Параметры отрисовки эффекта тени
    /// </summary>
    public class ShadowEffectSettings
    {
        public bool Enabled { get; set; } = false;
        public float TranslationX { get; set; } = 3;
        public float TranslationY { get; set; } = 3;
        public float BlurRadius { get; set; } = 3;
        public Color ShadowColor { get; set; } = Colors.Black;
    }
}
