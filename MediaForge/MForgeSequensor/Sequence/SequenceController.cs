using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MForge.Sequensor.Sequence
{
    public class SequenceController
    {
        private ObservableCollection<ISequence> sequences;
        public ObservableCollection<ISequence> Sequences { get { return sequences; } }

        public SequenceController()
        {
            sequences = new ObservableCollection<ISequence>();
        }
    }
}
