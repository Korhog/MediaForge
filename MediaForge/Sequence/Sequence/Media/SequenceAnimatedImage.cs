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

    public class AnimatedImageFrame
    {
        public TimeSpan StartTime { get; set; }
        public ImageSource ImageSource { get; set; }
    }

    public class SequenceAnimatedImage : SequenceImage
    {
        AnimatedImageFrame[] m_frames = null;

        public SequenceAnimatedImage(StorageFile source) : base(source)
        {

        }

        override public async Task Load()
        {
            await GetBitmap();
            await GetFrames();
            OnLoaded();
        }

        protected virtual async Task GetFrames()
        {
            string[] props = new string[] { "/grctlext/Delay" };

            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(
                BitmapDecoder.GifDecoderId,
                await m_source.OpenAsync(FileAccessMode.Read));

            var count = decoder.FrameCount;           
            m_frames = new AnimatedImageFrame[count];

            TimeSpan startTime = new TimeSpan();
            for (uint i = 0; i < count; i++)
            {
                var frame = await decoder.GetFrameAsync(i);

                var prop = await frame.BitmapProperties.GetPropertiesAsync(props);
                TimeSpan delay = TimeSpan.FromMilliseconds(10 * (UInt16)prop.FirstOrDefault().Value.Value);

                var source = new SoftwareBitmapSource();
                await source.SetBitmapAsync(await frame.GetSoftwareBitmapAsync(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Premultiplied
                    )
                );

                m_frames[i] = new AnimatedImageFrame()
                {
                    StartTime = startTime,
                    ImageSource = source
                };

                startTime += delay;             
            }

            Duration = startTime;
        }

        protected override void UpdateRenderTarget(SequenceUpdateResult action, TimeSpan time)
        {
            base.UpdateRenderTarget(action, time);
            if (m_frames == null)
                return;
            // получаем кадр
            var localTime = time - StartTime;
            var frame = m_frames.Where(x => x.StartTime <= localTime).LastOrDefault();
            if (frame == null || m_render.Source == frame.ImageSource)
                return;

            m_render.Source = frame.ImageSource;
        }
    }
}
