using System;

namespace Sequence.Media
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Windows.Graphics.Imaging;
    using Windows.Storage;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Media.Imaging;

    using MediaCore.Decoder;
    using MediaCore.Types;

    public class SequenceAnimatedImage : SequenceImage
    {
        private AnimatedImage m_image;

        public SequenceAnimatedImage(StorageFile source) : base(source)
        {
            m_fixed = true;
        }

        override public async Task Load()
        {
            await GetBitmap();
            await DecodeFrames();
            OnLoaded();
        }

        protected virtual async Task DecodeFrames()
        {
            m_image = await Decoder.DecodeAnimatedImage(m_source, Microsoft.Graphics.Canvas.CanvasBitmapFileFormat.Gif);

            Width = m_image.Width;
            Height = m_image.Height;

            Duration = m_image.Duration;
        }

        protected override void UpdateRenderTarget(SequenceUpdateResult action, TimeSpan time)
        {
            base.UpdateRenderTarget(action, time);
            if (m_image == null)
                return;
            // получаем кадр
            var localTime = time - StartTime;
            var frame = m_image.GetFrame(localTime);
            if (frame == null)
                return;

            m_render.SetImageSource(frame.ImageSource);
        }

        public override SoftwareBitmap GetRenderData(TimeSpan time)
        {
            var localTime = time - StartTime;
            return m_image.GetFrame(localTime).Render;            
        }
    }
}
