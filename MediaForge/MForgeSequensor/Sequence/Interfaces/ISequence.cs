using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MForge.Sequensor.Sequence
{
    public interface ISequence
    {
        event UpdateScaleEvent OnScale;

        /// <summary>
        /// Обновление элементов последовательности
        /// </summary>
        /// <param name="clipTime"> Время клипа </param>
        void Update(int frameIdx);

        ObservableCollection<ISequenceElement> Items { get; }

        /// <summary>
        /// Обновление размера кадра.
        /// </summary>
        /// <param name="frameScale"></param>
        void UpdateScale(double frameScale);
    }
}
