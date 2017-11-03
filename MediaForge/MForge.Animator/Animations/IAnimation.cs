using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace MForge.Animator.Animations
{    
    /// <summary> Анимация </summary>     
    public interface IAnimation
    {
        /// <summary> Обновление состояния анимации </summary>
        /// <param name="delta">Время предыдущего кадра</param>
        void Update(TimeSpan delta);
        /// <summary> Сброс состояния анимации </summary>
        void Reset();
        /// <summary> анимация закончена </summary>
        bool IsDone();
        /// <summary> анимация закончена </summary>
        TimeSpan? Duration { get; }
    }
}
