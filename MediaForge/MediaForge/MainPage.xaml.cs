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
    using AudioEngine;
    using Windows.Storage.Pickers;
    using Windows.Media.Playback;
    using Windows.Media.Core;

    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();            

            Sequensor.Controller.Create();
            Sequensor.Controller.AddItem += (sender, o) =>
            {
                var img = o as Sequence.SequenceImage;
                if (img == null) return;
                
                canvas.Children.Add(new TransformationBox() { Content = img.Image });
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
            var item = new Sequence.SequenceBaseItem();
            item.Template.Content = image;

            sequence.Add(item);            
        }
    }
}
