using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace MForge.Animator.Animations
{
    using Objects;

    public interface IAnimationTransform : ITransform
    {
        /// <summary> Позиция объекта </summary>
        FrameworkElement Target { get; }
        /// <summary> Позиция объекта </summary>
        Vector2 Position { get; set; }
    }
}
