using MForge.Engine.Video;
using System;
using System.Collections.Generic;
using Windows.Media.Core;
using Windows.Media.Editing;
using Windows.UI;

namespace MForge
{
    /// <summary> Движок для создания видео-проекта </summary>
    public partial class MediaEngine
    {
        public delegate void UpdateCompositionEvent();
        public event UpdateCompositionEvent UpdateComposition;

        private List<VideoLayer> videoLayers;
        public List<VideoLayer> Layers { get { return videoLayers; } }

        private MediaComposition mediaComposition;
        public MediaSource GetSource()
        {
            var mediaStreamSource = mediaComposition.GeneratePreviewMediaStreamSource(1280, 720);
            return MediaSource.CreateFromMediaStreamSource(mediaStreamSource);
        }

        private MediaEngine()
        {
            mediaComposition = new MediaComposition();
            // mediaComposition.Clips.Add(MediaClip.CreateFromColor(Colors.Black, TimeSpan.FromSeconds(5)));
            videoLayers = new List<VideoLayer>();
            CreateLayer();
        }

        private static MediaEngine instance;
        private static object sync = new object();

        public static MediaEngine GetInstance()
        {
            if (instance == null)
            {
                lock(sync)
                {
                    if (instance == null)
                    {
                        instance = new MediaEngine();
                    }
                }
            }
            return instance;
        }

        public VideoLayer CreateLayer()
        {
            var layer = new VideoLayer(mediaComposition);
            videoLayers.Add(layer);
            layer.AddMediaClip += (sender) =>
            {
                UpdateComposition?.Invoke();
            };

            return layer;
        }
    }
}
