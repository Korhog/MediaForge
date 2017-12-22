using System;
using MForge.MediaSource.Interfaces;
using Windows.Storage;

namespace MForge.MediaSource
{
    /// <summary> Генератор источников меда данных </summary>
    public abstract class MediaSourceBuilder
    {
        /// <summary> Создать из Uri </summary>
        public static IMediaSource CreateFromUri(Uri uri)
        {
            return null;
        }

        /// <summary> Создать из файла </summary>
        public static IMediaSource CreateFromStorageFile(StorageFile file)
        {
            return null;
        }
    }
}
