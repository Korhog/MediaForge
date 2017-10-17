using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Sequence.UI
{
    public sealed class FrameContainer : Control
    {
        public FrameContainer()
        {
            this.DefaultStyleKey = typeof(FrameContainer);
        }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content",
            typeof(UIElement),
            typeof(FrameContainer),
            new PropertyMetadata(null)
        );

        public UIElement Content
        {
            get { return (UIElement)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty AccentColorProperty = DependencyProperty.Register(
            "AccentColor",
            typeof(Color),
            typeof(FrameContainer),
            new PropertyMetadata(null)
        );

        public Color AccentColor
        {
            get { return (Color)GetValue(AccentColorProperty); }
            set { SetValue(AccentColorProperty, value); }
        }
    }
}
