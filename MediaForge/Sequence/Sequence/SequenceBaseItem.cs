using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;

namespace Sequence
{
    using System;
    using UI;

    public enum TransformationTarget
    {
        Center,
        LeftEdge,
        RightEdge
    }
    /// <summary>
    /// Базовый класс элемента последовательности.
    /// </summary>    
    public class SequenceBaseItem
    {
        TransformationTarget m_target = TransformationTarget.Center;

        TimeSpan m_lenght; // продолжительность
        TimeSpan m_start;  // время начала

        protected SequenceBase m_parent;
        protected FrameContainer m_border;
        public FrameContainer Template { get { return m_border; } }  
        public Thickness Margin { get { return GetMargin(); } }

        double m_offset = 0;

        Thickness GetMargin()
        {
            return new Thickness(m_offset, 0, 0, 0);
        }

        public SequenceBaseItem(SequenceBase parent)
        {
            m_parent = parent;

            m_border = new FrameContainer()
            {
                BorderThickness = new Thickness(4, 0, 4, 0),
                MinHeight = 50,
                MinWidth = 10,  
                AccentColor = parent.AccentColor
            };

            m_border.PointerPressed += OnPointerPressed;
            m_border.PointerReleased += OnPointerReleased;
        }

        protected void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var pointer = e.GetCurrentPoint(m_border);           

            if (pointer.Position.X < 10)
                m_target = TransformationTarget.LeftEdge;
            else if (pointer.Position.X > Template.ActualWidth - 10)
                m_target = TransformationTarget.RightEdge;
            else m_target = TransformationTarget.Center;

            Template.Width = Template.ActualWidth;                
            m_parent.SetDragItem(this);

            Duration d = new Duration(new System.TimeSpan(0, 0, 5));
            var r = d.TimeSpan.Seconds;
        }

        protected void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            m_parent.SetDragItem(null, e);
        }

        public void Translate(double delta)
        {

            switch (m_target)
            {
                case TransformationTarget.Center:
                    m_offset = m_offset + delta;
                    if (m_offset < 0) m_offset = 0;
                    Template.Margin = GetMargin();
                    break;
                case TransformationTarget.LeftEdge:
                    Template.Width -= delta;
                    m_offset = m_offset + delta;
                    if (m_offset < 0) m_offset = 0;
                    Template.Margin = GetMargin();
                    break;
                case TransformationTarget.RightEdge:
                    Template.Width += delta;
                    break;
            } 
        }
    }
}
