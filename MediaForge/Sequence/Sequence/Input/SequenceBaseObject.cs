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

    /// <summary>
    /// 
    /// </summary>  
    public partial class SequenceBaseObject
    {
        TransformationTarget m_target = TransformationTarget.Center;

        public delegate void SequenceItemCommit(SequenceBaseObject sender);
        public virtual event SequenceItemCommit Commit;

        public delegate void SequenceItemLoaded(SequenceBaseObject sender);
        public virtual event SequenceItemLoaded Loaded;

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
                    if (m_offset < 0)
                        m_offset = 0;

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

            
            SetDuration(Helpers.TimeHelper.DoubleToTimeSpan(Template.Width), false);
            TimeShift = Helpers.TimeHelper.DoubleToTimeSpan(m_offset);
        }

        public void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            Commit?.Invoke(this);
        }        
    }
}
