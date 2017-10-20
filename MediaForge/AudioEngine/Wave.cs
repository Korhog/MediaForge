using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.MediaProperties;
using Windows.Media.Transcoding;
using Windows.Storage;

namespace Audio.Core
{
    public class Wave
    {
        public async static Task<StorageFile> FromStorageFile(StorageFile source)
        {
            MediaTranscoder transcoder = new MediaTranscoder();
            MediaEncodingProfile profile = MediaEncodingProfile.CreateWav(AudioEncodingQuality.Medium);
            CancellationTokenSource cts = new CancellationTokenSource();
            
            string fileName = String.Format("TempFile_{0}.wav", Guid.NewGuid());
            StorageFile output = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(fileName);

            if (source == null || output == null)
            {
                return null;
            }
            try
            {
                var preparedTranscodeResult = await transcoder.PrepareFileTranscodeAsync(source, output, profile);
                if (preparedTranscodeResult.CanTranscode)
                {
                    var progress = new Progress<double>((percent) => { });
                    await preparedTranscodeResult.TranscodeAsync().AsTask(cts.Token, progress);
                }
                return output;
            }
            catch
            {
                return null;
            }
        }
    }
}
