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
        }

        protected override void Update(float delta, bool forward)
        {
            if (forward)
            {
                Velosity += (Gravity * delta);
                position += Velosity * delta;

                if (position.Y > 100)
                {

                    position.Y = 100;
                    // done = true;
                    Velosity *= -0.3f;
                }
            }
            else
            {                
                position -= Velosity * delta;
                if (position.Y > 100)
                {
                    position.Y = 100;
                    // done = true;
                    Velosity /= -0.3f;
                }
                Velosity -= (Gravity * delta);
            }

            Canvas.SetTop(target, position.Y);
        }
    }
}
