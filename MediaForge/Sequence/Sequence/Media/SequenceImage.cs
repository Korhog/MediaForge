using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Sequence
{
    public class SequenceImage : SequenceBaseItem
    {
        protected Image m_image;
        public SequenceImage(SequenceBase parent, BitmapSource source) : base(parent)
        {
            m_image = new Image()
            {
                Height = 100,
                Source = source
            };

            Template.Content = m_image;
        }
    };
}
