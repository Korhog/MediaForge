using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MForge.Animator.BeginAnimations
{
    public class JumpAnimationBeginUp : JumpAnimationBeginBase
    {
        protected override void SetPosition()
        {
            position = new Vector2(100, -100);
            Velosity = new Vector2(0, 0);
            Canvas.SetTop(target, position.Y);
            Canvas.SetLeft(target, 200);
            done = false;
            ComputDuration();
        }



        protected override void Update(float delta, bool forward)
        {
            if (timespan.HasValue && startTime.HasValue)
            {
                var d = (timespan.Value - startTime.Value).Ticks / (float)TimeSpan.TicksPerSecond;

                d *= 5.0f;

                var pos = position.Y + (Gravity.Y * Math.Pow(d, 2)) / 2.0;

                if (pos >= 100)
                {
                    pos = 100;
                }

                Canvas.SetTop(target, pos);
            }
        }

        public override void ComputDuration()
        {
            base.ComputDuration();

            var deltaPos = 100 - position.Y;

            var aTime = Math.Sqrt((2 * deltaPos) / Gravity.Y) * (double)TimeSpan.TicksPerSecond / 5.0f;
            var time = TimeSpan.FromTicks((long)aTime);
        }
    }
}
