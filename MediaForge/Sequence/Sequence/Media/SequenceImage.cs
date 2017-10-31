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

    using MediaCore.Decoder;
    using MediaCore.Types;
    using Windows.Graphics.Imaging;

    public class SequenceImage : SequenceRenderObject
    {
        protected StorageFile m_source;
        protected BitmapImage m_bitmap;
        protected SoftwareBitmap m_render_source;

        public SequenceImage(StorageFile source) : base()
        {
            m_source = source;
        }

        public SequenceImage(SoftwareBitmap bitmap) : base()
        {
            m_source = null;
            m_render_source = SoftwareBitmap.Convert(
                bitmap,
                BitmapPixelFormat.Rgba16,
                BitmapAlphaMode.Premultiplied);            
        }

        override public async Task Load()
        {            
            await GetBitmap();
            OnLoaded();
        }

        protected virtual async Task GetBitmap()
        {
            Image image;
            SoftwareBitmapSource source;
            if (m_source == null)
            {
                if (m_render_source != null)
                {
                    source = new SoftwareBitmapSource();
                    await source.SetBitmapAsync(SoftwareBitmap.Convert(
                        m_render_source,
                        BitmapPixelFormat.Bgra8,
                        BitmapAlphaMode.Premultiplied
                    ));

                    image = new Image()
                    {
                        Height = 100,
                        Source = source
                    };

                    Width = m_render_source.PixelWidth;
                    Height = m_render_source.PixelHeight;

                    Template.Content = image;
                }
                return;
            }

            m_render_source = await Decoder.DecodeSoftwareBitmap(m_source);
            source = new SoftwareBitmapSource();
            await source.SetBitmapAsync(SoftwareBitmap.Convert(
                m_render_source,
                BitmapPixelFormat.Bgra8,
                BitmapAlphaMode.Premultiplied
            ));

            image = new Image()
            {
                Height = 100,
                Source = source
            };

            Width = m_render_source.PixelWidth;
            Height = m_render_source.PixelHeight;

            Template.Content = image;
        }

        public override async Task<SoftwareBitmap> GetRenderData(TimeSpan time)
        {
            return m_render_source;
        }
    };
}
