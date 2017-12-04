using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MForge.Sequensor.Sequence.Interfaces
{
    public interface IScene
    {
        /// <summary> Размер сцены в кадрах </summary>
        int FrameDuration { get; set; }

        /// <summary> Последовательности сцены </summary>
        SequenceController Sequensor { get; }

        /// <summary> Последовательности сцены </summary>
        ObservableCollection<ISequence> Sequences { get; }
    }
}
