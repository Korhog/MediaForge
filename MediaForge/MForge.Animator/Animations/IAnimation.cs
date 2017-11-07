using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace MForge.Animator.Animations
{    
    /// <summary> Анимация </summary>     
    public interface IAnimation
    {
        /// <summary> Обновление состояния анимации </summary>
        /// <param name="delta">Время предыдущего кадра</param>
        void Update(TimeSpan time);
        
        /// <summary> Сброс состояния анимации </summary>
        void Reset();
        
        /// <summary> анимация закончена </summary>
        bool IsDone { get; }
        
        /// <summary> длительность анимации </summary>
        TimeSpan? Duration { get; }
        
        /// <summary> вычисление длительности анимации </summary>
        void ComputDuration();

        /// <summary> Целевой объект </summary>
        void SetTarget(FrameworkElement frameworkElement);
    }
}
