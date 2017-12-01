using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MForge.Sequensor.Sequence.Interfaces
{
    public interface IScene
    {
        /// <summary> Размер сцены в кадрах </summary>
        int FrameDuration { get; set; }
    }
}
