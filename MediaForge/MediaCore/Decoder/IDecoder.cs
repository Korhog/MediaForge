using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCore.Decoder
{
    using FrameSource;
    using Windows.Storage;

    public interface IDecoder
    {
        
        Task<IFrameSource> Encode(StorageFile source);
    }
}
