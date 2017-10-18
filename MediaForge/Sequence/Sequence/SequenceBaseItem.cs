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
        bool m_reset = true;
        TransformationTarget m_target = TransformationTarget.Center;

        protected TimeSpan m_duration; // Продолжительность
        public TimeSpan Duration {
            get { return m_duration; }
            set { SetDuration(value); }
        }

        protected virtual void SetDuration(TimeSpan duration)
        {
            m_duration = duration;
            // Template.Width = m_parent.TimeSpanToDouble(m_duration);
            Template.Duration = string.Format(@"{0:hh\:mm\:ss\:ff}", Duration); 
        }

        protected TimeSpan m_time_shift; // смещение
        public TimeSpan TimeShift
        {
            get { return m_time_shift; }
            set { SetTimeShift(value); }
        }

        protected virtual void SetTimeShift(TimeSpan duration)
        {
            m_time_shift = duration;
            // Template.Width = m_parent.TimeSpanToDouble(m_duration);
            Template.TimeShift = string.Format(@"{0:hh\:mm\:ss\:ff}", TimeShift);
        }

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
            m_reset = true;
            var pointer = e.GetCurrentPoint(m_border);

            if (pointer.Position.X < 10)
            {
                m_target = TransformationTarget.LeftEdge;
                Template.DurationVisibility = Visibility.Visible;
            }
            else if (pointer.Position.X > Template.ActualWidth - 10)
            {
                m_target = TransformationTarget.RightEdge;
                Template.DurationVisibility = Visibility.Visible;
            }
            else
            {
                m_target = TransformationTarget.Center;
                Template.TimeShiftVisibility = Visibility.Visible;
            }

            Template.Width = Template.ActualWidth;                
            m_parent.SetDragItem(this);
        }

        protected void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            m_parent.SetDragItem(null, e);
            Template.DurationVisibility = Visibility.Collapsed;
            Template.TimeShiftVisibility = Visibility.Collapsed;            
        }

        public void Translate(double delta)
        {
            if (m_reset)
            {
                m_reset = false;
                return;
            }

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

            Duration = m_parent.DoubleToTimeSpan(Template.ActualWidth);
            TimeShift = m_parent.DoubleToTimeSpan(m_offset);
        }
    }
}
