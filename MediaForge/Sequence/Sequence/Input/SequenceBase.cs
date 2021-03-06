﻿using System;
using Windows.UI.Xaml;

namespace Sequence
{
    using Media;
    using Windows.ApplicationModel.DataTransfer;
    using Windows.Media.Editing;
    using Windows.Storage;

    /// <summary>
    /// Базовый класс последовательности.
    /// </summary>   
    public partial class SequenceBase
    {
        public void OnDropOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
        }

        public async void OnDrop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();
                if (items.Count > 0)
                {
                    var storageFile = items[0] as StorageFile;

                    if (storageFile.FileType == ".gif") {
                        Add(new SequenceAnimatedImage(storageFile)
                        {
                            Duration = new TimeSpan(0, 0, 1)
                        });
                    }
                    else if (storageFile.FileType == ".mp4")
                    {
                        Add(new SequenceFrameSet(storageFile)
                        {
                            Duration = new TimeSpan(0, 0, 1)
                        });

                        var mforge = MForge.MediaEngine.GetInstance();
                        MediaClip clip = await MediaClip.CreateFromFileAsync(storageFile);
                        mforge.Layers[0].AddClip(clip);
                    }
                    else
                    {
                        Add(new SequenceImage(storageFile)
                        {
                            Duration = new TimeSpan(0, 0, 1)
                        });
                    }
                }                
            }            
        }
    }


}
