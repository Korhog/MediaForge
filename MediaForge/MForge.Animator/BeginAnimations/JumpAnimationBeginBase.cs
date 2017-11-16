using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MForge.Animator.BeginAnimations
{
    using System.Numerics;
    using Animations;
    using Windows.UI.Xaml;

    /// <summary>
    /// Анимация прыжка.
    /// </summary>
    public abstract class JumpAnimationBeginBase : IPhysicalAnimation
    {

        protected bool done = false;
        protected TimeSpan? duration = null;
        protected TimeSpan? startTime = null;

        protected FrameworkElement target = null;

        public Vector2 Force { get; set; } = new Vector2(0, 0);
        public Vector2 Gravity { get; set; } = new Vector2(0, 1500.0f);
        public Vector2 Velosity { get; set; } = new Vector2(0, 0);

        public bool IsDone { get; }

        public TimeSpan? Duration { get { return duration; } }

        public virtual void ComputDuration()
        {
            
        }

        public void Reset()
        {
            timespan = null;
            startTime = null;

            SetPosition();
            ComputDuration(); 
            done = false;
        }


        protected TimeSpan? timespan = null; 
        protected Vector2 position;

        public void Update(TimeSpan time)
        {
            if (target == null || done)
                return;

            if (timespan == null)
            {
                timespan = time;
                startTime = time;
                return;
            }

            if (time < startTime)
                return;

            var delta = Math.Abs(time.Ticks - timespan.Value.Ticks);           

            // Получаем масштаб времени
            var k = delta / (float)TimeSpan.TicksPerSecond;
            timespan = time;

            Update(k, time.Ticks >= timespan.Value.Ticks);
            
        }

        protected virtual void Update(float delta, bool forward)
        {
            
        }

        protected virtual void SetPosition()
        {
            
        }

        public void SetTarget(FrameworkElement frameworkElement)
        {
            target = frameworkElement;
        }
    }
}
