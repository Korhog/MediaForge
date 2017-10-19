using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Sequence.UI
{
    public sealed class TransformationBox : ContentControl
    {
        CompositeTransform m_transform;

        public TransformationBox()
        {
            this.DefaultStyleKey = typeof(TransformationBox);
            ManipulationMode = ManipulationModes.Scale | ManipulationModes.Rotate | ManipulationModes.TranslateX | ManipulationModes.TranslateY;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            m_transform = GetTemplateChild("TransformBox") as CompositeTransform;
        }

        protected override void OnManipulationDelta(ManipulationDeltaRoutedEventArgs e)
        {
            base.OnManipulationDelta(e);
            if (e == null || m_transform == null)
                return;

            m_transform.ScaleX *= e.Delta.Scale;
            m_transform.ScaleY *= e.Delta.Scale;

            m_transform.Rotation += e.Delta.Rotation;

            m_transform.TranslateX += e.Delta.Translation.X;
            m_transform.TranslateY += e.Delta.Translation.Y;
        }

        protected override void OnPointerWheelChanged(PointerRoutedEventArgs e)
        {
            base.OnPointerWheelChanged(e);
        }
    }
}
