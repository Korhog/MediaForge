using MediaCore.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCore.FrameSource
{
    /// <summary> Набор кадров </summary>
    public interface IFrameSource
    {
        /// <summary> Получить кадр по номеру кадра </summary>
        Frame this[int index] { get; }
        /// <summary> Получить кадр по времени кадра </summary>
        Frame this[TimeSpan time] { get; }
        /// <summary> Длительность ролика </summary>
        TimeSpan Duration { get; }
        /// <summary> FPS ролика </summary>
        FPS FPS { get; }
        /// <summary> Ширина ролика </summary>
        int Width { get; }        
        /// <summary> Высота ролика </summary>
        int Height { get; }
    }
}
