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

namespace Controls.Container.UI
{
    // Класс для частой смены контента.
    public sealed class RenderBuffer : Control
    {
        ImageSource m_source;

        Image m_buffer_a;
        Image m_buffer_b;

        Image m_current_buffer = null;

        public RenderBuffer()
        {
            this.DefaultStyleKey = typeof(RenderBuffer);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            m_buffer_a = GetTemplateChild("BufferOne") as Image;
            m_buffer_b = GetTemplateChild("BufferTwo") as Image;

            SetImageSource(m_source);
        }

        public void SetImageSource(ImageSource source)
        {
            m_source = source;

            if (m_buffer_a == null || m_buffer_b == null)
                return;

            if (m_current_buffer == null)
            {
                m_current_buffer = m_buffer_a;
                m_buffer_a.Source = source;
                return;
            }

            if (m_current_buffer == m_buffer_a)
            {
                m_buffer_b.Source = source;
                Canvas.SetZIndex(m_buffer_b, 1);
                Canvas.SetZIndex(m_buffer_a, 0);
                m_current_buffer = m_buffer_b;
            }
            else
            {
                m_buffer_a.Source = source;
                Canvas.SetZIndex(m_buffer_a, 1);
                Canvas.SetZIndex(m_buffer_b, 0);
                m_current_buffer = m_buffer_a;
            }        
        }
    }
}
