using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Media.Core;
using System.Linq;
using Windows.Storage;
using System.Collections.Generic;
using Windows.Media.Playback;
using System;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace VideoToGif
{    
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();


        }

        MediaPlayer _mediaPlayer;

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Uri manifestUri = new Uri("http://amssamples.streaming.mediaservices.windows.net/49b57c87-f5f3-48b3-ba22-c55cfdffa9cb/Sintel.ism/manifest(format=m3u8-aapl)");
            var source = MediaSource.CreateFromUri(manifestUri);
            await source.OpenAsync();

            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.Source = source;
            _mediaPlayer.Play();
        }
    }
}
