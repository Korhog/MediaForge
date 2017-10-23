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
    using Media;
    /// <summary>
    /// Базовый класс последовательности.
    /// </summary>   
    public partial class SequenceBase
    {
        public int m_current_idx = -1;
        public Color AccentColor { get; set; }

        public delegate void SequenceItemAdded(SequenceBase sender, SequenceBaseObject item);
        public event SequenceItemAdded AddItem;

        protected SequenceBaseObject m_drag_item;
        public SequenceBaseObject DragItem { get { return m_drag_item; } }

        protected ObservableCollection<SequenceBaseObject> m_items;
        public ObservableCollection<SequenceBaseObject> Items { get { return m_items; } }

        public SequenceBase()
        {
            Random rand = new Random();

            AccentColor = ColorHelper.FromArgb(
                255,
                (byte)rand.Next(100, 200),
                (byte)rand.Next(100, 200),
                (byte)rand.Next(100, 200)
            );

            m_items = new ObservableCollection<SequenceBaseObject>();
        }

        public async virtual void Add(SequenceBaseObject item)
        {
            Items.Add(item);
            item.Commit += OnItemCommit;

            item.Loaded += (sender) =>
            {
                AddItem?.Invoke(this, sender);
            };

            await item.Load();
        }

        public void OnItemCommit(SequenceBaseObject sender)
        {
            foreach(var item in Items)
            {
                TimeSpan time = new TimeSpan(Items
                    .Where(x => Items.IndexOf(x) < Items.IndexOf(item))
                    .Sum(x => x.TimeShift.Ticks + x.Duration.Ticks));

                item.StartTime = time + item.TimeShift;
            }
        }        

        public void Frame(TimeSpan time)
        {
            foreach (var item in Items.Where(x => x is Render.SequenceRenderObject).Select(x => x as Render.SequenceRenderObject))
                item.Update(time);
        }
    }
}
