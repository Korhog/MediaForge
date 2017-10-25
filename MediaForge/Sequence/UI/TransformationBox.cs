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
    using Render;


    public sealed class TransformationBox : ContentControl
    {
        CompositeTransform m_transform;
        public CompositeTransform Transform { get { return m_transform; } }

        public TransformationBox()
        {
            this.DefaultStyleKey = typeof(TransformationBox);
            ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
        }

        public delegate void CompositeTransformChangeEvent(TransformationBox sender);
        public event CompositeTransformChangeEvent CompositeTransformChange;



        public SequenceRenderObject Render { get; set; }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            m_transform = GetTemplateChild("TransformBox") as CompositeTransform;
            
            m_transform.CenterX = Render == null ? 0 : Render.Width / 2;
            m_transform.CenterY = Render == null ? 0 : Render.Height / 2;
        }

        protected override void OnManipulationDelta(ManipulationDeltaRoutedEventArgs e)
        {
            base.OnManipulationDelta(e);
            if (e == null || m_transform == null)
                return;

            if (Render == null)
                return;

            e.Handled = true;

            m_transform.ScaleX *= e.Delta.Scale;
            m_transform.ScaleY *= e.Delta.Scale;

            m_transform.Rotation += e.Delta.Rotation;

            m_transform.TranslateX += e.Delta.Translation.X;
            m_transform.TranslateY += e.Delta.Translation.Y;

            Render.Left = m_transform.TranslateX;
            Render.Top = m_transform.TranslateY;

            CompositeTransformChange?.Invoke(this);
        }

        protected override void OnPointerWheelChanged(PointerRoutedEventArgs e)
        {
            base.OnPointerWheelChanged(e);
            if (e == null || m_transform == null)
                return;

            if (Render == null)
                return;            

            e.Handled = true;
            var pointer = e.GetCurrentPoint(Parent as Canvas);

            if (e.KeyModifiers == Windows.System.VirtualKeyModifiers.Control)
            {
                var s = (1 + (float)pointer.Properties.MouseWheelDelta / 2400.0f);

                m_transform.ScaleX *= s;
                m_transform.ScaleY *= s;
                Render.Scale *= s;
            }

            CompositeTransformChange?.Invoke(this);
        }
    }
}
