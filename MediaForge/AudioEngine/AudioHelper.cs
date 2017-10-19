using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.MediaProperties;
using Windows.Media.Transcoding;
using Windows.Storage;
using Windows.UI.Xaml;

namespace AudioEngine
{
    public class AudioHelper
    {
        private StorageFile currentFile;

        // private PlottingGraphImg imgFile;

        public async Task ChooseFile_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.MusicLibrary;
            picker.FileTypeFilter.Add(".mp4");
            picker.FileTypeFilter.Add(".mp3");
            picker.FileTypeFilter.Add(".wav");
            picker.FileTypeFilter.Add(".m4a");

            StorageFile file = await picker.PickSingleFileAsync();
            await ConvertToWaveFile(file);
        }

        public async Task ConvertToWaveFile(StorageFile sourceFile)
        {
            MediaTranscoder transcoder = new MediaTranscoder();
            MediaEncodingProfile profile = MediaEncodingProfile.CreateWav(AudioEncodingQuality.Medium);
            CancellationTokenSource cts = new CancellationTokenSource();
            //Create temporary file in temporary folder
            string fileName = String.Format("TempFile_{0}.wav", Guid.NewGuid());
            StorageFile temporaryFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(fileName);
            currentFile = temporaryFile;
            if (sourceFile == null || temporaryFile == null)
            {
                return;
            }
            try
            {
                var preparedTranscodeResult = await transcoder.PrepareFileTranscodeAsync(sourceFile, temporaryFile, profile);
                if (preparedTranscodeResult.CanTranscode)
                {
                    var progress = new Progress<double>((percent) => {  });
                    await preparedTranscodeResult.TranscodeAsync().AsTask(cts.Token, progress);
                }
            }
            catch
            {

            }
        }             
    }
}
