using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;

namespace MediaCore.FrameSource
{
    public class Frame
    {
        private TimeSpan m_start_time = new TimeSpan();
        private void SetStartTime(TimeSpan time)
        {
            m_start_time = time;
            m_stop_time = time + Duration;
        }
        /// <summary> Время начала кадра </summary>
        public TimeSpan StartTime
        {
            get { return m_start_time; }
            set { SetStartTime(value); }
        }

        private TimeSpan m_stop_time = new TimeSpan();
        /// <summary> Время окончания кадра </summary>
        public TimeSpan StopTime { get { return m_stop_time; } }

        private TimeSpan m_duration = new TimeSpan();
        private void SetDuration(TimeSpan duration)
        {
            m_duration = duration;
            m_stop_time = StartTime + duration;
        }

        /// <summary> Длительность кадра </summary>
        public TimeSpan Duration
        {
            get { return m_duration; }
            set { SetDuration(value); }
        }
        SoftwareBitmap m_source;
        /// <summary> Рендер </summary>
        public SoftwareBitmap Source { get { return m_source; } }

        public Frame(SoftwareBitmap source)
        {
            m_source = source;
        }
    }
}
