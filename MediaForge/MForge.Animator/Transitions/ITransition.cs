using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace MForge.Animator.Transitions
{
    //
    public interface ITransition
    {
        /// <summary> Размер  </summary>
        Rect RenderSize { get; }
        Rect ScreenSize { get; }
    }
}
