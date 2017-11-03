using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MForge.Animator.Animations
{
    public interface IPhysicalAnimation : IAnimation
    {
        /// <summary> Вектор силы </summary>
        Vector2 Force { get; set; }
        /// <summary> Вектор силы гравитации </summary>
        Vector2 Gravity { get; set; }
        /// <summary> Вектор ускорения </summary>
        Vector2 Velosity { get; set; }
        /// <summary> Максимальная скорость </summary>
    }
}
