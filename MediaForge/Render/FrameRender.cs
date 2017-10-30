using Windows.Graphics.Imaging;

using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Render
{    
    using Render.Drawing;   
    /// <summary>
    /// Рендер кадра
    /// </summary>
    public class FrameRender
    {
        public static async Task<SoftwareBitmap> Render(RenderObjectBase[][] renderChain)
        {
            var sources = new List<RenderObjectBase>();

            foreach (var layer in renderChain)
            {
                foreach (var item in layer)
                {
                    sources.Add(item);
                }
            }

            var width = sources.Max(x => x.Source.PixelWidth);
            var height = sources.Max(x => x.Source.PixelHeight);

            CanvasDevice device = new CanvasDevice();
            CanvasRenderTarget render = new CanvasRenderTarget(device, 
                (float)width, 
                (float)height,
                96);

            using (var session = render.CreateDrawingSession())
            {
                CanvasBitmap cb;
                ICanvasImage cbi;

                // Проходим по слоям
                foreach (var layer in renderChain)
                {
                    foreach (var renderObject in layer)
                    {
                        cb = CanvasBitmap.CreateFromSoftwareBitmap(device, renderObject.Source);
                        cbi = new Transform2DEffect()
                        {
                            Source = cb,
                            TransformMatrix = renderObject.Transformaion
                        };

                        session.DrawImage(cbi);
                    }
                }
            }

            return SoftwareBitmap.Convert(
                await SoftwareBitmap.CreateCopyFromSurfaceAsync(render),
                BitmapPixelFormat.Bgra8,
                BitmapAlphaMode.Premultiplied
            );
        }        
    }
}
