using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace MediaForge
{
    using Sequence.UI;
    using Sequence.Media;
    using AudioEngine;
    using Microsoft.Graphics.Canvas;
    using Windows.UI;
    using Windows.UI.Xaml.Shapes;
    using Microsoft.Graphics.Canvas.Effects;
    using System.Numerics;
    using Project;
    using MediaCore.Encoder;

    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        bool m_need_init = true;

        public MainPage()
        {
            this.InitializeComponent();

            Sequensor.Controller.Create();
            Sequensor.Controller.AddItem += (sender, item) =>
            {
                if (m_need_init && item is Sequence.Render.SequenceRenderObject)
                {
                    var w = (item as Sequence.Render.SequenceRenderObject).Width;
                    var h = (item as Sequence.Render.SequenceRenderObject).Height;

                    W2D.Width = w == 0 ? 400 : w;
                    W2D.Height = h == 0 ? 300 : h;

                    canvas.Width = w == 0 ? 400 : w;
                    canvas.Height = h == 0 ? 300 : h;

                    m_need_init = false;

                    VideoProject.GetInstance().FPS = MediaCore.Types.FPS.FPS10;
                }

                var render = item as Sequence.Render.SequenceRenderObject;
                if (render != null)
                {
                    var rect = new TransformationBox()
                    {
                        Width = render.Width,
                        Height = render.Height,
                        Background = new SolidColorBrush(Colors.Transparent),
                        BorderBrush = new SolidColorBrush(Colors.Red),
                        BorderThickness = new Thickness(2),
                        Render = render
                    };

                    rect.CompositeTransformChange += (s) => { 
                        W2D.Invalidate();
                    };

                    canvas.Children.Add(rect);
                }
            };

            Sequensor.Controller.OnDraw += () =>
            {
                W2D.Invalidate();
            };
        }

        private void OnStart(object sender, RoutedEventArgs e)
        {
            Sequensor.Play();

        }

        private async void OnWave(object sender, RoutedEventArgs e)
        {
            var helpher = new AudioHelper();
            var wave = await helpher.GetWaveStorageFile(sender, e);
            Sequensor.Controller.SetAudioTrack(wave);
            
            Wav wavFile = new Wav(wave);
            var imgFile = new PlottingGraphImg(wavFile, 400, 100);

            var image = await imgFile.GetGraphicFile();

            var sequence = Sequensor.Controller.Sequences[0];
            var item = new Sequence.SequenceBaseObject();
            item.Template.Content = image;

            sequence.Add(item);            
        }

        private void OnDebugDraw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            var session = args.DrawingSession;
            session.Clear(Colors.DarkSlateBlue);

            var dpi = Windows.Graphics.Display.DisplayProperties.LogicalDpi;

            var awaiter = Sequensor.Controller.GetRenderObjects();
            awaiter.Wait();

            var layers = awaiter.Result;

            foreach (var layer in layers)
            {
                var l = layer;
                foreach(var render in layer)
                {
                    var cbi = CanvasBitmap.CreateFromSoftwareBitmap(session.Device, render.Source);
                    ICanvasImage image = new Transform2DEffect
                    {
                        Source = cbi,
                        TransformMatrix = render.Transformaion
                    };
                    session.DrawImage(image);
                }                
            }
        }

        private void Render(object sender, RoutedEventArgs e)
        {
            W2D.Invalidate();
        }

        private async void OnTextEditor(object sender, RoutedEventArgs e)
        {
            var dlg = new MediaCore.Popup.TextCreationDialog();
            await dlg.ShowAsync();

            var soft = dlg.Result;
            if (soft != null)
            {
                var sequence = Sequensor.Controller.Sequences.FirstOrDefault();
                sequence.Add(new SequenceImage(soft) { Duration = new TimeSpan(0, 0, 2) });
            }
        }

        private void OnCanvasShow(object sender, RoutedEventArgs e)
        {
            if (canvas.Visibility == Visibility.Visible)
                canvas.Visibility = Visibility.Collapsed;
            else
                canvas.Visibility = Visibility.Visible;
        }

        private async void OnEncode(object sender, RoutedEventArgs e)
        {
            await Encoder.EncodeGif(Sequensor.Controller);
        }
    }
}
