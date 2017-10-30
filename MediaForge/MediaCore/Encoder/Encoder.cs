using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;

namespace MediaCore.Encoder
{
    public struct EncodeResult
    {
        public bool Success;
    }

    public struct EncoderOptions
    {

    }

    public class Encoder
    {
        public static async Task<EncodeResult> EncodeGif(IFrameData frameData)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();

            savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            savePicker.FileTypeChoices.Add("GIF", new List<string>() { ".gif" });
            savePicker.SuggestedFileName = "MyGif";

            var file = await savePicker.PickSaveFileAsync();
            if (file == null)
                return new EncodeResult { Success = false };

            using (var memoryStream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.GifEncoderId, memoryStream);
                var propSet = await encoder.BitmapProperties.GetPropertiesAsync(new string[] { "/grctlext/Delay" });

                var props = new BitmapPropertySet();
                props.Add("/grctlext/Delay", new BitmapTypedValue((UInt16)10, Windows.Foundation.PropertyType.UInt16));

                var count = frameData.GetFramesCount();
                for (var frameIdx = 0; frameIdx < count; frameIdx++)
                {
                    var frame = await frameData.GetFrameToEncode(frameIdx);
                    encoder.SetSoftwareBitmap(frame.ImageSource);

                    await encoder.BitmapProperties.SetPropertiesAsync(props);
                    if (frameIdx + 1 < count)
                        await encoder.GoToNextFrameAsync();
                }

                await encoder.FlushAsync();
            }

            return new EncodeResult { Success = true };
        }
    }
}
