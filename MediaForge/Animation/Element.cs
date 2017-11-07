using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animation
{
    using System.Numerics;
    using MForge.Animator.Animations;
    using Windows.UI.Xaml.Shapes;
    using Windows.UI.Xaml.Media;
    using Windows.UI;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml;

    class Element : IPhysicalAnimation
    {
        bool done = false;
        float max = 55;

        Rectangle rect;
        public Rectangle Rect { get { return rect; } }

        public Element()
        {
            rect = new Rectangle()
            {
                Fill = new SolidColorBrush(Colors.Red),
                Width = 100,
                Height = 100
            };
        }

        Vector2 position;

        public Vector2 Force { get; set; } = new Vector2(0, 0);
        public Vector2 Gravity { get; set; } = new Vector2(0, 150.0f);
        public Vector2 Velosity { get; set; } = new Vector2(0, 0);

        public float MaxSpeed { get { return max; } }

        public TimeSpan? Duration { get { return null; } }

        public void Reset()
        {
            position = new Vector2(100, -100);
            Velosity = new Vector2(0, 0);
            Canvas.SetTop(rect, position.Y);
            Canvas.SetLeft(rect, 200);
            done = false;
        }

        public void Update(TimeSpan time)
        {
            var delta = time.Ticks;
            if (done)
                return;

            var k = (float)delta / (float)TimeSpan.TicksPerSecond;

            Velosity += (Gravity * k);
            position += Velosity * k;

            if (position.Y > 100)
            {

                position.Y = 100;
                // done = true;
                Velosity *= -0.3f;
            }

            Canvas.SetTop(rect, position.Y); 
        }

        public void ComputDuration()
        {
            /// Вычисление длительности анимации.
        }

        public void SetTarget(FrameworkElement frameworkElement)
        {
        }

        public bool IsDone { get { return done; } }
    }
}
