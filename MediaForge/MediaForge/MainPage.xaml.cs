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

    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            Sequensor.Controller.Create();
            Sequensor.Controller.AddItem += (sender, item) =>
            {
                if (item is Sequence.Render.SequenceRenderObject)
                {
                    W2D.Width = (item as Sequence.Render.SequenceRenderObject).Width;
                    W2D.Height = (item as Sequence.Render.SequenceRenderObject).Height;

                    canvas.Width = (item as Sequence.Render.SequenceRenderObject).Width;
                    canvas.Height = (item as Sequence.Render.SequenceRenderObject).Height;
                }

                var render = item as Sequence.Render.SequenceRenderObject;
                if (render != null)
                {
                    canvas.Children.Add(render.Render);
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

            var layers = Sequensor.Controller.GetRenderObjects();
            foreach(var layer in layers)
            {
                foreach(var render in layer)
                {
                    var cbi = CanvasBitmap.CreateFromSoftwareBitmap(
                        session.Device,
                        render
                    );

                    session.DrawImage(cbi);
                }                
            }
        }

        private void Render(object sender, RoutedEventArgs e)
        {
            W2D.Invalidate();
        }
    }
}
