using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media;

namespace MediaCore.Types
{
    public class ImageFrame
    {
        public TimeSpan Duration { get; set; }
        public TimeSpan StartTime { get; set; }
        public SoftwareBitmap ImageSource { get; set; }
    }

    public class AnimatedImage
    {
        public double Width { get; set; }
        public double Height { get; set; }

        private ImageFrame[] m_frames;
        private TimeSpan m_duraion;
        public TimeSpan Duration { get { return m_duraion; } }

        public AnimatedImage(List<ImageFrame> sourceFrames, TimeSpan duraion)
        {
            m_frames = sourceFrames.ToArray();
            m_duraion = duraion;
        }

        public ImageFrame GetFrame(TimeSpan time)
        {
            return m_frames?.Where(x => time >= x.StartTime).LastOrDefault();
        }
    }
}
