using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence
{
    public class SequenceControllerBase
    {
        ObservableCollection<SequenceBase> m_sequences;
        ObservableCollection<string> m_names;

        public ObservableCollection<SequenceBase> Sequences { get { return m_sequences; } }

        public SequenceControllerBase()
        {
            m_sequences = new ObservableCollection<SequenceBase>();
            m_names = new ObservableCollection<string>();
        }        
    }
}
