using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MForge.Sequensor.Sequence
{
    public delegate void UpdateScaleEvent(double frameScale);
    /// <summary> Элемент последовательности </summary>
    public interface ISequenceElement
    {
        /// <summary> Длительность элемента в кадрах </summary>
        int FramesDuration { get; set; }

        /// <summary> Начальный кадр </summary>
        int StartFrame { get; set; }

        /// <summary> Конечный кадр </summary>
        int EndFrame { get; }

        double FrameScale { get; }

        void UpdateScale(double scale);

        event UpdateScaleEvent OnScale;
    }
}
