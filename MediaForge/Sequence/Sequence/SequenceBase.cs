﻿using System;
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
        public int m_current_idx = -1;
        public Color AccentColor { get; set; }

        public delegate void SequenceItemAdded(SequenceBase sender, SequenceBaseItem item);
        public event SequenceItemAdded AddItem;

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
            var v = DoubleToTimeSpan(120);
            var w = TimeSpanToDouble(v);
        }

        public virtual void Add(SequenceBaseItem item)
        {
            Items.Add(item);
            AddItem?.Invoke(this, item);
        }

        public void SetDragItem(SequenceBaseItem item, PointerRoutedEventArgs e = null)
        {
            m_drag_item = item;
        }

        public double TimeSpanToDouble(TimeSpan timeSpan)
        {
            return (timeSpan.Ticks / 10000000) * 60;
        }

        public TimeSpan DoubleToTimeSpan(double value)
        {
            var ticks = (long)((value / 60) * 10000000);
            var timeSpan = new TimeSpan(ticks);            
            return timeSpan;
        }

        public SequenceBaseItem First()
        {
            if (Items.Count == 0)
                return null;

            m_current_idx = 0;
            return Items[m_current_idx];
            
        }

        public SequenceBaseItem Next()
        {
            if (m_current_idx == -1)
                return First();


            if (m_current_idx + 1 >= Items.Count)
                return null;

            m_current_idx++;
            return Items[m_current_idx];
        }

        public SequenceBaseItem Current()
        {
            return null;
        }
    }
}
