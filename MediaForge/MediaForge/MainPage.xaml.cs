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

    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private AudioHelper wave;

        public MainPage()
        {
            this.InitializeComponent();

            wave = new AudioHelper();

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
            await wave.ChooseFile_Click(sender, e);

            Wav wavFile = new Wav(wave.currentFile.Path.ToString());
            var imgFile = new PlottingGraphImg(wavFile, 1000, 100);
            FileSavePicker fileSavePicker = new FileSavePicker();
            fileSavePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fileSavePicker.FileTypeChoices.Add("JPEG files", new List<string>() { ".jpg" });
            fileSavePicker.SuggestedFileName = "image";

            var outputFile = await fileSavePicker.PickSaveFileAsync();
            if (outputFile == null)
            {
                // The user cancelled the picking operation
                return;
            }
            await imgFile.SaveGraphicFile(outputFile);

        }
    }
}
