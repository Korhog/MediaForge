using MediaCore.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCore.FrameSource
{
    public class FrameSet : FrameSourceBase
    {
        public FrameSet(FPS fps, int width, int height)
        {
            m_fps = fps;
            m_width = width;
            m_height = height;
        }
    }
}
