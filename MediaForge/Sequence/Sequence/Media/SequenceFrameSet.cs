using MediaCore.FrameSource;
using Sequence.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.Editing;
using Windows.Storage;

namespace Sequence.Media
{
    public class SequenceFrameSet : SequenceRenderObject
    {
        StorageFile m_source;
        MediaComposition m_comp;
        IFrameSource m_frame_set;
        public SequenceFrameSet(StorageFile source)
        {
            m_source = source;
        }

        public override async Task<SoftwareBitmap> GetRenderData(TimeSpan time)
        {
            return await MediaCore.Decoder.H246.GetFrameFromTime(m_comp, time);
        }

        override public async Task Load()
        {
            var decoder = new MediaCore.Decoder.H246();

            m_comp = await decoder.GetMediaComposition(m_source);
            Width = 400;
            Height = 300;

            Duration = TimeSpan.FromSeconds(5);
            OnLoaded();
        }
    }
}
