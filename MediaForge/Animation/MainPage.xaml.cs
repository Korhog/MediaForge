using MForge.Animator;
using MForge.Animator.Animations;
using MForge.Animator.BeginAnimations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace Animation
{

    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Storyboard storyboard;
        TimeSpan? time = null;
        JumpAnimationBeginUp anim;

        public MainPage()
        {
            this.InitializeComponent();

            Rectangle rect = new Rectangle
            {
                Width = 100,
                Height = 100,
                Fill = new SolidColorBrush(Colors.Blue)
            };

            canvas.Children.Add(rect);
            anim = new JumpAnimationBeginUp();
            anim.SetTarget(rect);

            storyboard = new Storyboard();

            slider.ValueChanged += (s, e) =>
            {
                var ticks = TimeSpan.TicksPerSecond * 5 * (slider.Value / slider.Maximum);
                var time = TimeSpan.FromTicks((long)ticks);
                anim.Update(time);
            };

            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 600;
            da.Duration = new Duration(TimeSpan.FromSeconds(20));
            da.EnableDependentAnimation = true;
            storyboard.Children.Add(da);
            Storyboard.SetTargetProperty(storyboard, "Value");
            Storyboard.SetTarget(storyboard, slider);

            IAnimation a;
            var animations = AnimationLib.GetAnimations();

            if (animations.Count > 0) {
                var t = animations[0].Type;
                var mi = typeof(AnimationLib).GetMethod("Create");
                var fooRef = mi.MakeGenericMethod(t);
                a = (IAnimation)fooRef.Invoke(null, null);
            }
        }

        private void OnReset(object sender, RoutedEventArgs e)
        {
            anim.Reset();
        }

        private void OnStep(object sender, RoutedEventArgs e)
        {
        }

        private void OnPlay(object sender, RoutedEventArgs e)
        {
            storyboard.Begin();
        }
    }
}
