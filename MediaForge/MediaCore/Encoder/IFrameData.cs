using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCore.Encoder
{
    using Types;

    public interface IFrameData
    {
        /// <summary> Получение конкретного кадра </summary>
        Task<ImageFrame> GetFrameToEncode(int idx);

        /// <summary> Количество кадров в клипе </summary>
        int GetFramesCount();
    }
}
