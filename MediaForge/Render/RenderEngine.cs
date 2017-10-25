using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render
{
    using Drawing;
    using Microsoft.Graphics.Canvas;
    using Microsoft.Graphics.Canvas.Effects;
    using System.Numerics;

    /// <summary>
    /// Класс рендер
    /// </summary>
    public class RenderEngine
    {
        protected CanvasControl m_canvas;
        protected RenderObjectBase[][] m_render_chain;

        public RenderEngine(CanvasControl canvas)
        {
            m_canvas = canvas;
            m_canvas.Draw += OnDraw;
        }

        public void Draw(RenderObjectBase[][] renderChain)
        {
            m_render_chain = renderChain;
            m_canvas?.Invalidate();
        }

        public void OnDraw(CanvasControl canvas, CanvasDrawEventArgs args)
        {
            if (m_canvas == null || m_render_chain == null)
                return;

            var session = args.DrawingSession;
            var device = session.Device;

            session.Clear(canvas.ClearColor);

            CanvasBitmap cb;
            ICanvasImage cbi;

            // Проходим по слоям
            foreach (var layer in m_render_chain)
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
    }
}
