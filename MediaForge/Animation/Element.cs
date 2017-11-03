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

    class Element : IPhysicalAnimation
    {
        bool done = false;
        float max = 5;

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
        public Vector2 Gravity { get; set; } = new Vector2(0, 9.8f);
        public Vector2 Velosity { get; set; } = new Vector2(0, 0);

        public float MaxSpeed { get { return max; } }

        public TimeSpan? Duration { get { return null; } }

        public void Reset()
        {
            position = new Vector2(100, -100);
            Velosity = new Vector2(0, MaxSpeed);
            Canvas.SetTop(rect, position.Y);
            Canvas.SetLeft(rect, 200);
            done = false;
        }

        public void Update(TimeSpan delta)
        {
            Velosity += (Gravity / 5);
            if (Velosity.Length() > MaxSpeed)
                Velosity = new Vector2(0, MaxSpeed);

            position += Velosity;

            if (position.Y > 100)
            {
                position.Y = 100;
                Velosity *= -1f;
            }

            Canvas.SetTop(rect, position.Y); 
        }

        public bool IsDone() { return done; }
    }
}
