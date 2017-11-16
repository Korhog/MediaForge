using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MForge.Sequensor.Sequence
{
    /// <summary> Последовательность с одним элементом </summary>
    public class SequenceSingleBase : SequenceBase
    {
        public SequenceSingleBase(ISequenceElement element) : base()
        {
            Items.Add(element);            
        }
    }
}
