using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//

using System.Collections.ObjectModel;
using Windows.Foundation;
using Windows.UI.Xaml.Input;

namespace Sequence
{
    /// <summary>
    /// Базовый класс последовательности.
    /// </summary>   

    public class SequenceBase
    {
        protected Point m_begin_position;
        protected SequenceBaseItem m_drag_item;
        public SequenceBaseItem DragItem { get { return m_drag_item; } }

        protected ObservableCollection<SequenceBaseItem> m_items;
        public ObservableCollection<SequenceBaseItem> Items { get { return m_items; } }

        public SequenceBase()
        {
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
