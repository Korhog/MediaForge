using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MediaCore.Decoder
{
    using Microsoft.Graphics.Canvas;
    using System.Numerics;
    using Types;
    using Windows.Foundation;
    using Windows.Graphics.Imaging;
    using Windows.Storage.Streams;
    using Windows.UI;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Media.Imaging;

    public class Decoder
    {
        public static async Task<BitmapImage> GetTestBitmapImage()
        {
            CanvasDevice device = CanvasDevice.GetSharedDevice();
            CanvasRenderTarget renderTarget = new CanvasRenderTarget(
                device,
                200,
                200,
                96);

            BitmapImage bitmap = new BitmapImage();
            using (var session = renderTarget.CreateDrawingSession())
            {
                session.DrawEllipse(new Vector2(100, 100), 50, 50, Colors.Red, 4);
            }

            using (var stream = new InMemoryRandomAccessStream())
            {
                await renderTarget.SaveAsync(stream, CanvasBitmapFileFormat.Bmp);
                bitmap.SetSource(stream);
            }

            return bitmap;
        }

        public static async Task<SoftwareBitmap> DecodeSoftwareBitmap(StorageFile source)
        {
            var decoder = await BitmapDecoder.CreateAsync(await source.OpenAsync(FileAccessMode.Read));
            var result = SoftwareBitmap.Convert(
                await decoder.GetSoftwareBitmapAsync(),
                BitmapPixelFormat.Rgba16,
                BitmapAlphaMode.Premultiplied);

            return result;
        }

        public static async Task<AnimatedImage> DecodeAnimatedImage(StorageFile source)
        {
            return await DecodeAnimatedImage(source, CanvasBitmapFileFormat.Bmp);
        }

        public static async Task<AnimatedImage> DecodeAnimatedImage(StorageFile source, CanvasBitmapFileFormat format)
        {
            string[] props = new string[] { "/grctlext/Delay" };
            List<ImageFrame> frames = new List<ImageFrame>();

            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(
                BitmapDecoder.GifDecoderId,
                await source.OpenAsync(FileAccessMode.Read));

            CanvasDevice device = CanvasDevice.GetSharedDevice();
            CanvasRenderTarget renderTarget = new CanvasRenderTarget(
                device, 
                decoder.PixelWidth, 
                decoder.PixelHeight, 
                96);

            CanvasBitmap buffer;

            var count = decoder.FrameCount;
            TimeSpan delay;
            TimeSpan startTime = new TimeSpan();
            Rect rect = new Rect(0, 0, decoder.PixelWidth, decoder.PixelHeight);

            // Перебираем кадры.
            for (uint frameIdx = 0; frameIdx < decoder.FrameCount; frameIdx++)
            {
                var frame = await decoder.GetFrameAsync(frameIdx);
                
                // Получаем длительность кадра
                var propSet = await frame.BitmapProperties.GetPropertiesAsync(props);
                var prop = propSet.FirstOrDefault();
                delay = TimeSpan.FromMilliseconds(10 * (UInt16)prop.Value.Value);
                if (delay.Ticks == 0)
                    delay = TimeSpan.FromMilliseconds(100);                

                var softwareBitmap = SoftwareBitmap.Convert(
                    await frame.GetSoftwareBitmapAsync(), 
                    BitmapPixelFormat.Rgba16, 
                    BitmapAlphaMode.Premultiplied);

                // Создаем прадварительный буфер.
                buffer = CanvasBitmap.CreateFromSoftwareBitmap(device, softwareBitmap);
                using (var session = renderTarget.CreateDrawingSession())
                {
                    session.DrawImage(buffer, rect);
                }

                using (var stream = new InMemoryRandomAccessStream())
                {
                    await renderTarget.SaveAsync(stream, format);
                    BitmapImage bitmap = new BitmapImage();  
                    
                    bitmap.SetSource(stream);                    
                    
                    ImageFrame decodedFrame = new ImageFrame()
                    {
                        Duration = delay,
                        StartTime = startTime,
                        ImageSource = SoftwareBitmap.Convert(
                            await SoftwareBitmap.CreateCopyFromSurfaceAsync(renderTarget),
                            BitmapPixelFormat.Rgba16,
                            BitmapAlphaMode.Premultiplied)
                    };

                    frames.Add(decodedFrame);
                    startTime += delay;
                } 
            }
            return new AnimatedImage(frames, startTime)
            {
                Width = decoder.PixelWidth,
                Height = decoder.PixelHeight
            };
        }


    }    
}
