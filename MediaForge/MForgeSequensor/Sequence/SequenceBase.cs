using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MForge.Sequensor.Sequence
{
    public class SequenceBase : ISequence
    {
        protected ObservableCollection<ISequenceElement> items;

        public event UpdateScaleEvent OnScale;

        public ObservableCollection<ISequenceElement> Items { get { return items; } }

        public SequenceBase()
        {
            items = new ObservableCollection<ISequenceElement>();
        }

        public void Update(int frameIdx)
        {

        }

        public void UpdateScale(double frameScale)
        {
            foreach (var item in Items)
                item.UpdateScale(frameScale);
            OnScale?.Invoke(frameScale);
        }
    }
}
