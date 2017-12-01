using MForge.Sequensor.Sequence.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MForge.Sequensor.Sequence
{
    public class Scene : IScene
    {
        private SequenceController sequensor;
        public SequenceController Sequensor { get { return sequensor; } }

        private TimeSpan duration;
        /// <summary> Длительность сцены </summary>
        public TimeSpan Duration { get; }

        /// <summary> Длительность сцены в кадрах </summary>
        public int FrameDuration { get; set; } = 50;

        public Scene()
        {
            sequensor = new SequenceController();
        }

        public ObservableCollection<ISequence> Sequences { get { return sequensor.Sequences; } }
    }
}
