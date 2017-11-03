using Microsoft.Graphics.Canvas.UI.Xaml;


namespace MForge.Render2D
{
    /// <summary> 2D рендер на базе Win2D </summary>
    public class Engine2D
    {
        private CanvasControl canvasControl;
        public Engine2D(CanvasControl canvas)
        {
            canvasControl = canvas;
        }
    }
}
