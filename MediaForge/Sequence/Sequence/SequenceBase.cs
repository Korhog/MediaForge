using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//

using System.Collections.ObjectModel;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml;

namespace Sequence
{
    /// <summary>
    /// Базовый класс последовательности.
    /// </summary>   

    public class SequenceBase
    {
        public Color AccentColor { get; set; }

        protected Point m_begin_position;
        protected SequenceBaseItem m_drag_item;
        public SequenceBaseItem DragItem { get { return m_drag_item; } }

        protected ObservableCollection<SequenceBaseItem> m_items;
        public ObservableCollection<SequenceBaseItem> Items { get { return m_items; } }

        public SequenceBase()
        {
            Random rand = new Random();

            AccentColor = ColorHelper.FromArgb(
                255,
                (byte)rand.Next(100, 200),
                (byte)rand.Next(100, 200),
                (byte)rand.Next(100, 200)
            );

            m_items = new ObservableCollection<SequenceBaseItem>();
        }

        public virtual void Add(SequenceBaseItem item)
        {

        }

        public void SetDragItem(SequenceBaseItem item, PointerRoutedEventArgs e = null)
        {
            m_drag_item = item;
        }
    }
}
