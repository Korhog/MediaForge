using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml.Controls;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Sequence.Media
{
    using Render;
    using Windows.Storage;

    public class SequenceImage : SequenceRenderObject
    {
        protected StorageFile m_source;
        protected BitmapImage m_bitmap;        

        public SequenceImage(StorageFile source) : base()
        {
            m_source = source;
        }

        override public async Task Load()
        {            
            await GetBitmap();
            OnLoaded();
        }

        protected virtual async Task GetBitmap()
        {
            m_bitmap = new BitmapImage() { AutoPlay = false };
            m_bitmap.SetSource(await m_source.OpenAsync(FileAccessMode.Read));

            var image = new Image()
            {
                Height = 100,
                Source = m_bitmap
            };

            m_render.SetImageSource(m_bitmap);
            Template.Content = image;
        } 
    };
}
