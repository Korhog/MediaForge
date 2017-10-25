using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas.Text;
using Windows.UI;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Windows.Graphics.Imaging;
using System.Threading.Tasks;
using System.Numerics;

// Документацию по шаблону элемента "Диалоговое окно содержимого" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace MediaCore.Popup
{
    public sealed partial class TextCreationDialog : ContentDialog
    {
        public SoftwareBitmap Result { get; set; }

        private async Task<SoftwareBitmap> RenderText()
        {
            Rect bounds;
            var geometryRender = new CanvasRenderTarget(canvas, 400, 80);
            using (var geometrySession = geometryRender.CreateDrawingSession())
            {
                geometrySession.Clear(Color.FromArgb(0, 0, 0, 0));
                // Тут будут задаваться параметры
                var format = new CanvasTextFormat()
                {
                    FontSize = 24,
                    FontWeight = FontWeights.ExtraBlack,
                    HorizontalAlignment = CanvasHorizontalAlignment.Center,
                    VerticalAlignment = CanvasVerticalAlignment.Center,
                    WordWrapping = CanvasWordWrapping.Wrap,
                    FontFamily = "Showcard Gothic"
                };

                using (CanvasTextLayout textLayout = new CanvasTextLayout(geometrySession, Text.Text, format, 400, 80))
                {
                    CanvasGeometry geometry = CanvasGeometry.CreateText(textLayout);
                    CanvasStrokeStyle dashedStroke = new CanvasStrokeStyle()
                    {
                        DashStyle = CanvasDashStyle.Solid
                    };

                    bounds = geometry.ComputeStrokeBounds(2.0f);

                    geometrySession.DrawTextLayout(textLayout, 0, 0, Colors.Red);
                    geometrySession.DrawGeometry(geometry, Colors.White, 2.0f, dashedStroke);
                }
            }


            CanvasBitmap bitmap = CanvasBitmap.CreateFromDirect3D11Surface(canvas, geometryRender, 96);


            var resultRender = new CanvasRenderTarget(
                canvas, 
                (float)bounds.Width + 20, 
                (float)bounds.Height + 20
            );

            using (var resultSession = resultRender.CreateDrawingSession())
            {
                resultSession.Clear(Color.FromArgb(0, 0, 0, 0));             

                var shadow = new ShadowEffect
                {
                    Source = bitmap,
                    BlurAmount = 1.0f
                    
                };

                var transformMatrix = Matrix3x2.CreateTranslation(new Vector2(
                        10 - (float)bounds.Left,
                        10 - (float)bounds.Top
                    ));

                Transform2DEffect translate;
                translate = new Transform2DEffect
                {
                    Source = shadow,
                    TransformMatrix = transformMatrix
                };
                resultSession.DrawImage(translate);

                translate = new Transform2DEffect
                {
                    Source = bitmap,
                    TransformMatrix = transformMatrix
                };
                resultSession.DrawImage(translate);
            }

            return await SoftwareBitmap.CreateCopyFromSurfaceAsync(resultRender);
        }


        public TextCreationDialog()
        {
            this.InitializeComponent();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Result = await RenderText();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void canvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {

        }

        private void OnDraw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            var text = Text.Text;
            if (string.IsNullOrEmpty(text))
                return;

            var session = args.DrawingSession;
            session.Clear(sender.ClearColor);

            var textBitmap = new CanvasRenderTarget(sender, 400, 80);
            using (var drawingSession = textBitmap.CreateDrawingSession())
            {
                drawingSession.Clear(Color.FromArgb(0, 0, 0, 0));

                var format = new CanvasTextFormat()
                {
                    FontSize = 32,
                    FontWeight = FontWeights.ExtraBlack,
                    HorizontalAlignment = CanvasHorizontalAlignment.Center,
                    VerticalAlignment = CanvasVerticalAlignment.Center,
                    WordWrapping = CanvasWordWrapping.Wrap,
                    FontFamily = "Showcard Gothic"                    
                };

                using (CanvasTextLayout textLayout = new CanvasTextLayout(session, text, format, 400, 80))
                {
                    CanvasGeometry geometry = CanvasGeometry.CreateText(textLayout);

                    CanvasStrokeStyle dashedStroke = new CanvasStrokeStyle()
                    {
                        DashStyle = CanvasDashStyle.Solid
                    };

                    drawingSession.DrawTextLayout(textLayout, 0, 0, Colors.Red);
                    drawingSession.DrawGeometry(geometry, Colors.White, 2.0f, dashedStroke);

                }
            }

            var shadow = new ShadowEffect
            {
                Source = textBitmap,
                BlurAmount = 1
            };

            session.DrawImage(shadow);
            session.DrawImage(textBitmap);
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            canvas.Invalidate();
        }
    }
}
