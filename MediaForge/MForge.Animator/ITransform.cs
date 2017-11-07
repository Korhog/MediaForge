using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;

namespace MForge.Animator.Animations
{
    /// <summary>
    /// Трансформация объекта отрисовки
    /// </summary>
    public interface ITransform
    {
        /// <summary>
        /// Перемещение
        /// </summary>
        /// <param name="translate">дельта</param>
        /// <returns></returns>
        Vector2 Translate(Vector2 translate);
        
        /// <summary> Прямоугольник для отрисовки </summary>
        Rectangle Rect { get; }
    }
}
