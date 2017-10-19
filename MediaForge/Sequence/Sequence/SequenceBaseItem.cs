using System.Linq;

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

        protected TimeSpan m_duration; // Продолжительность
        public TimeSpan Duration {
            get { return m_duration; }
            set { SetDuration(value, true); }
        }

        protected virtual void SetDuration(TimeSpan duration, bool external)
        {
            m_duration = duration;
            if (external)
                Template.Width = m_parent.TimeSpanToDouble(m_duration);
            Template.Duration = string.Format(@"{0:hh\:mm\:ss\:ff}", Duration); 
        }

        protected TimeSpan m_time_shift; // смещение
        public TimeSpan TimeShift
        {
            get { return m_time_shift; }
            set { SetTimeShift(value); }
        }

        protected TimeSpan m_absolute_time_shift; // смещение
        public TimeSpan AbsoluteTimeShift { get { return m_absolute_time_shift; } }

        protected virtual void SetTimeShift(TimeSpan duration)
        {
            m_time_shift = duration;
            // Template.Width = m_parent.TimeSpanToDouble(m_duration);
            Template.TimeShift = string.Format(@"{0:hh\:mm\:ss\:ff}", TimeShift);
        }

        public delegate void SequenceItemCommit(SequenceBaseItem sender);
        public event SequenceItemCommit Commit;

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
                ManipulationMode = ManipulationModes.TranslateX,
                BorderThickness = new Thickness(4, 0, 4, 0),
                MinHeight = 50,
                MinWidth = 10,  
                AccentColor = parent.AccentColor
            };

            m_border.ManipulationStarted += OnManipulationStarted;
            m_border.ManipulationDelta += OnManipulationDelta;
            m_border.ManipulationCompleted += OnManipulationCompleted;
        }

        // Манипуляции        
        private double m_begin_shift;
        private double m_begin_width;
        private double m_begin_position;
        private double m_current_position;

        public void OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            m_begin_shift = m_offset;
            m_begin_position = e.Position.X;
            m_current_position = m_begin_position;
            m_begin_width = Template.ActualWidth;

            m_target = e.Position.X < 20 ? 
                TransformationTarget.LeftEdge : 
                (
                    e.Position.X > Template.ActualWidth - 20 ? 
                        TransformationTarget.RightEdge : 
                        TransformationTarget.Center
                );

            Template.Width = Template.ActualWidth;
        }

        public void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            // с магнитом.

            int step = 0;
            m_current_position += e.Delta.Translation.X;

            double delta = 0;
            if (step > 0)
                delta = step * ((int)(m_current_position - m_begin_position) / step);
            else
                delta = m_current_position - m_begin_position;

            if (delta == 0)
                return;

            switch (m_target)
            {
                case TransformationTarget.Center:
                    m_offset = m_begin_shift + delta;
                    Template.Margin = GetMargin();
                    break;
                case TransformationTarget.LeftEdge:
                    if (m_begin_width - delta > 0)
                    {
                        Template.Width = m_begin_width - delta;
                        m_offset = m_begin_shift + delta;
                        Template.Margin = GetMargin();
                    }
                    break;
                case TransformationTarget.RightEdge:
                    Template.Width = m_begin_width + delta;
                    break;
            }

            
            SetDuration(m_parent.DoubleToTimeSpan(Template.Width), false);
            TimeShift = m_parent.DoubleToTimeSpan(m_offset);
        }

        public void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            Commit?.Invoke(this);
        }        
    }
}
