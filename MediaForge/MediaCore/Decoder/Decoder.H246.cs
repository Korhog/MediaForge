using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCore.Decoder
{
    using FrameSource;
    using Windows.Graphics.Imaging;
    using Windows.Media.Editing;
    using Windows.Storage;
    using Windows.Storage.Streams;
    using Windows.UI;
    using Windows.UI.Xaml.Media.Imaging;

    public class H246 : IDecoder
    {
        public async Task<IFrameSource> Encode(StorageFile source)
        {
            MediaClip clip = await MediaClip.CreateFromFileAsync(source);
            var props = clip.GetVideoEncodingProperties();
            var frameMilliseconts = 1000.0f / ((float)props.FrameRate.Numerator / (float)props.FrameRate.Denominator);
            var frameCount = (int)(clip.OriginalDuration.TotalMilliseconds / frameMilliseconts);

            var fps = MediaHelper.MillisecondsToFPS((long)frameMilliseconts);

            var frameSource = new FrameSet(fps, (int)props.Width, (int)props.Height);

            TimeSpan frameDuration = TimeSpan.FromMilliseconds((int)fps);
            TimeSpan frameStartTime = new TimeSpan();

            MediaComposition composition = new MediaComposition();            

            composition.Clips.Add(clip);

            for (int idx = 0; idx < frameCount; idx++)
            {
                var time = TimeSpan.FromMilliseconds(idx * frameMilliseconts);

                var frame = await composition.GetThumbnailAsync(
                    time, 
                    (int)props.Width, 
                    (int)props.Height,
                    VideoFramePrecision.NearestFrame
                );

                using (var stream = new InMemoryRandomAccessStream())
                {
                    await RandomAccessStream.CopyAsync(frame, stream);

                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                    stream.Seek(0);

                    SoftwareBitmap bitmap = SoftwareBitmap.Convert(
                        await decoder.GetSoftwareBitmapAsync(),
                        BitmapPixelFormat.Rgba16,
                        BitmapAlphaMode.Premultiplied);

                    frameSource.AddFrame(new Frame(bitmap) { Duration = frameDuration, StartTime = frameStartTime });
                    frameStartTime += frameDuration;
                }  
            }

            return frameSource;
        }

        public async Task<MediaComposition> GetMediaComposition(StorageFile source)
        {
            MediaClip clip = await MediaClip.CreateFromFileAsync(source);
       
            clip.VideoEffectDefinitions.Add(
                new Windows.Media.Effects.VideoEffectDefinition(typeof(MForge.VideoEffects.BlurEffect).FullName)
            );


            var props = clip.GetVideoEncodingProperties();
            var frameMilliseconts = 1000.0f / ((float)props.FrameRate.Numerator / (float)props.FrameRate.Denominator);
            var frameCount = (int)(clip.OriginalDuration.TotalMilliseconds / frameMilliseconts);

            var fps = MediaHelper.MillisecondsToFPS((long)frameMilliseconts);

            MediaComposition composition = new MediaComposition();
            MediaClip wait = MediaClip.CreateFromColor(Colors.Black, TimeSpan.FromSeconds(2));

            composition.Clips.Add(wait);
            composition.Clips.Add(clip);            

            return composition;
        }

        public static async Task<SoftwareBitmap> GetFrameFromTime(MediaComposition composition, TimeSpan time)
        {
            var frame = await composition.GetThumbnailAsync(
                time,
                400,
                300,
                VideoFramePrecision.NearestFrame
            );
            SoftwareBitmap bitmap;
            using (var stream = new InMemoryRandomAccessStream())
            {
                await RandomAccessStream.CopyAsync(frame, stream);

                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                stream.Seek(0);

                bitmap = SoftwareBitmap.Convert(
                    await decoder.GetSoftwareBitmapAsync(),
                    BitmapPixelFormat.Rgba16,
                    BitmapAlphaMode.Premultiplied);
                return bitmap;
            }
        }

        void T()
        {
            MediaOverlay over = new MediaOverlay(null);
        }
    }
}
