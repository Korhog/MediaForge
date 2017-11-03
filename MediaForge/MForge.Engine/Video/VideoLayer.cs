using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Editing;

namespace MForge.Engine.Video
{
    public class VideoLayer
    {
        public delegate void AddMediaClipEvent(VideoLayer sender);
        public event AddMediaClipEvent AddMediaClip;

        private MediaOverlayLayer mediaOverlayLayer;
        public VideoLayer(MediaComposition composition)
        {
            mediaOverlayLayer = new MediaOverlayLayer();
            composition.OverlayLayers.Add(mediaOverlayLayer);
        }

        public void AddClip(MediaClip clip)
        {
            var aspect = clip.GetVideoEncodingProperties().PixelAspectRatio;
            MediaOverlay overlay = new MediaOverlay(clip, new Windows.Foundation.Rect(20, 20, 400, 400), 1);
            mediaOverlayLayer.Overlays.Add(overlay);
            AddMediaClip?.Invoke(this);
        }
    }
}
