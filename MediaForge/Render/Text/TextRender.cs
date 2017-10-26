namespace Render.Text
{
    using EffectSettings;

    using Microsoft.Graphics.Canvas;
    using Microsoft.Graphics.Canvas.Effects;
    using Microsoft.Graphics.Canvas.Geometry;
    using Microsoft.Graphics.Canvas.Text;
    using Microsoft.Graphics.Canvas.UI.Xaml;

    using System;
    using System.Numerics;
    using System.Threading.Tasks;

    using Windows.Foundation;
    using Windows.Graphics.Imaging;
    using Windows.UI;
    using Windows.UI.Xaml.Media;
    
    /// <summary>
    /// Параметры обводки текста
    /// </summary>
    public class OutlineTextSettings
    {
        public bool Enabled { get; set; } = false;
        public CanvasDashStyle DashStyle { get; set; } = CanvasDashStyle.Solid;
        public float StrokeWidth { get; set; } = 1.0f;
        public Color StrokeColor { get; set; } = Colors.White;
    }


    /// <summary>
    /// Параметры отрисовки текста
    /// </summary>
    public class TextRenderSettings
    {
        /// <summary>Размер текста</summary>
        public float FontSize { get; set; } = 24;

        /// <summary>Размер текста</summary>
        public FontFamily FontFamily { get; set; } = new FontFamily("Segoe UI");

        /// <summary>Цвет текста </summary>
        public Color TextColor { get; set; } = Colors.Black;

        /// <summary>Текст</summary>
        public string Text { get; set; } = "Sample";

        /// <summary>Параметры отрисовки текста</summary>
        public ShadowEffectSettings ShadowEffectSettings { get; set; } = new ShadowEffectSettings();

        /// <summary>Параметры обводки текста</summary>
        public OutlineTextSettings OutlineTextSettings { get; set; } = new OutlineTextSettings();
    }

    /// <summary>
    /// Рисовалка текста.
    /// </summary>
    public class TextRender
    {
        public static CanvasBitmap Render(ICanvasResourceCreatorWithDpi resourceCreator, TextRenderSettings settings, bool clip = false)
        {
            return Render(resourceCreator, settings, new Size(400, 80), clip);
        }  
        
        public static CanvasBitmap Render(ICanvasResourceCreatorWithDpi resourceCreator, TextRenderSettings settings, Size renderSize, bool clip)
        {
            Rect geometryBounds;
            ICanvasResourceCreator device;
            CanvasRenderTarget geometryRender;
            float dpi = 96;
            if (resourceCreator == null)
            {
                device = new CanvasDevice();
                geometryRender = new CanvasRenderTarget(device, (float)renderSize.Width, (float)renderSize.Height, dpi);
            }
            else
            {
                device = resourceCreator;
                dpi = resourceCreator.Dpi;
                geometryRender = new CanvasRenderTarget(resourceCreator, (float)renderSize.Width, (float)renderSize.Height);
            } 
            
            // получаем сессию для рисования текста
            using (var geometrySession = geometryRender.CreateDrawingSession())
            {
                geometrySession.Clear(Color.FromArgb(0, 0, 0, 0)); // прозрачность

                var format = new CanvasTextFormat()
                {
                    FontSize = settings.FontSize,
                    FontFamily = settings.FontFamily.Source,
                    HorizontalAlignment = CanvasHorizontalAlignment.Center,
                    VerticalAlignment = CanvasVerticalAlignment.Center,
                };

                // создаем тектовый слой
                using (CanvasTextLayout textLayout = new CanvasTextLayout(device, settings.Text, format, (float)renderSize.Width, (float)renderSize.Height))
                {
                    geometrySession.DrawTextLayout(textLayout, 0, 0, settings.TextColor);

                    CanvasGeometry geometry = CanvasGeometry.CreateText(textLayout);
                    CanvasStrokeStyle stroke = new CanvasStrokeStyle()
                    {
                        DashStyle = settings.OutlineTextSettings.DashStyle
                    };

                    geometryBounds = geometry.ComputeStrokeBounds(settings.OutlineTextSettings.Enabled ? settings.OutlineTextSettings.StrokeWidth : 0.0f);

                    if (settings.OutlineTextSettings.Enabled)
                    {
                        geometrySession.DrawGeometry(
                            geometry, 
                            settings.OutlineTextSettings.StrokeColor, 
                            settings.OutlineTextSettings.StrokeWidth, 
                            stroke);
                    }
                }                
            }

            CanvasBitmap bitmap = CanvasBitmap.CreateFromDirect3D11Surface(device, geometryRender, dpi);


            var computedWidth = (float)renderSize.Width;
            var computedHeight = (float)renderSize.Height;

            Matrix3x2 clipMatrix = Matrix3x2.Identity;
            // Если требуется обрезка
            if (clip)
            {
                var shadowOffset = settings.ShadowEffectSettings.BlurRadius * 2;
                computedWidth = (float)Math.Floor(geometryBounds.Width + shadowOffset + Math.Abs(settings.ShadowEffectSettings.TranslationX));
                computedHeight = (float)Math.Floor(geometryBounds.Height + shadowOffset + Math.Abs(settings.ShadowEffectSettings.TranslationY));

                var offsetX = -(float)Math.Truncate(geometryBounds.Left);
                var offsetY = -(float)Math.Truncate(geometryBounds.Top);

                clipMatrix = Matrix3x2.CreateTranslation(new Vector2(offsetX, offsetY));
            }
            CanvasRenderTarget resultRender;
            if (resourceCreator == null)
            {
                resultRender = new CanvasRenderTarget(device, computedWidth, computedHeight, dpi);
            }
            else
            {
                resultRender = new CanvasRenderTarget(resourceCreator, computedWidth, computedHeight);
            }

            // получаем сессию для рисования текста
            using (var resultSession = resultRender.CreateDrawingSession())
            {
                resultSession.Clear(Color.FromArgb(0, 0, 0, 0));

                var shadow = new ShadowEffect
                {
                    Source = bitmap,
                    BlurAmount = settings.ShadowEffectSettings.BlurRadius
                };

                Transform2DEffect shadowTranslation = new Transform2DEffect
                {
                    Source = shadow,
                    TransformMatrix = clipMatrix * Matrix3x2.CreateTranslation(new Vector2(
                        settings.ShadowEffectSettings.TranslationX,
                        settings.ShadowEffectSettings.TranslationY
                        ))
                };
                resultSession.DrawImage(shadowTranslation);

                Transform2DEffect imageTranslation = new Transform2DEffect
                {
                    Source = bitmap,
                    TransformMatrix = clipMatrix
                };

                resultSession.DrawImage(imageTranslation);
            }

            return CanvasBitmap.CreateFromDirect3D11Surface(device, resultRender);
        }

        public static async Task<SoftwareBitmap> RenderToSoftwareBitmap(ICanvasResourceCreatorWithDpi resourceCreator, TextRenderSettings settings)
        {
            var cbi = Render(resourceCreator, settings, true);
            CanvasRenderTarget renderTarget = new CanvasRenderTarget(resourceCreator, cbi.Size);
            using (var session = renderTarget.CreateDrawingSession())
            {
                session.DrawImage(cbi);
            }

            return SoftwareBitmap.Convert(
                await SoftwareBitmap.CreateCopyFromSurfaceAsync(renderTarget),
                BitmapPixelFormat.Bgra8,
                BitmapAlphaMode.Premultiplied
            );
        }
    }
}
