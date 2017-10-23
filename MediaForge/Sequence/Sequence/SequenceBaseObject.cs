using System.Linq;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;

namespace Sequence
{
    using System;
    using System.Threading.Tasks;
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
    public partial class SequenceBaseObject
    {
        protected TimeSpan m_duration; // Продолжительность
        public TimeSpan Duration {
            get { return m_duration; }
            set { SetDuration(value, true); }
        }

        protected virtual void SetDuration(TimeSpan duration, bool external)
        {
            m_duration = duration;
            if (external)
                Template.Width = Helpers.TimeHelper.TimeSpanToDouble(m_duration);
            Template.Duration = string.Format(@"{0:hh\:mm\:ss\:ff}", Duration); 
        }

        protected TimeSpan m_time_shift; // смещение
        public TimeSpan TimeShift
        {
            get { return m_time_shift; }
            set { SetTimeShift(value); }
        }
        public TimeSpan StartTime { get; set; } // Абсолютное смещение

        protected virtual void SetTimeShift(TimeSpan duration)
        {
            m_time_shift = duration;
            // Template.Width = m_parent.TimeSpanToDouble(m_duration);
            Template.TimeShift = string.Format(@"{0:hh\:mm\:ss\:ff}", TimeShift);
        }

        protected FrameContainer m_border;
        public FrameContainer Template { get { return m_border; } }  
        public Thickness Margin { get { return GetMargin(); } }

        double m_offset = 0;

        Thickness GetMargin()
        {
            return new Thickness(m_offset, 0, 0, 0);
        }

        public SequenceBaseObject()
        {
            m_border = new FrameContainer()
            {
                ManipulationMode = ManipulationModes.TranslateX,
                BorderThickness = new Thickness(4, 0, 4, 0),
                MinHeight = 100,
                MinWidth = 10,
                AccentColor = Colors.PaleVioletRed
            };

            m_border.ManipulationStarted += OnManipulationStarted;
            m_border.ManipulationDelta += OnManipulationDelta;
            m_border.ManipulationCompleted += OnManipulationCompleted;
        }

        protected virtual void OnLoaded()
        {
            Loaded?.Invoke(this);
        }

        public virtual async Task Load()
        {
            OnLoaded();
        }
    }
}
