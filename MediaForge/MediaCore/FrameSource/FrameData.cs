using MediaCore.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MediaCore.FrameSource
{
    /// <summary> Базовый источник данных для рендера </summary>
    public abstract class FrameSourceBase : IFrameSource
    {
        protected int? m_width = null;
        public int Width { get { return m_width.HasValue ? m_width.Value : 0; } }

        protected int? m_height = null;
        public int Height { get { return m_height.HasValue ? m_height.Value : 0; } }

        protected TimeSpan m_duration = new TimeSpan();
        public TimeSpan Duration { get { return m_duration; } }

        protected FPS m_fps;
        public FPS FPS { get { return m_fps; } }

        List<Frame> m_frames;

        public FrameSourceBase()
        {
            m_fps = FPS.FPS30;
            m_frames = new List<Frame>();
        }

        /// <summary> Возвращает кадр по номеру </summary>
        public Frame this[int index]
        {
            get
            {
                if (index >= m_frames.Count)
                    return null;
                return m_frames[index];
            }
        }

        /// <summary> Возвращает кадр по времени </summary>
        public Frame this[TimeSpan time]
        {
            get
            {
                return m_frames
                    .Where(frame => frame.StartTime <= time && frame.StopTime > time)
                    .FirstOrDefault();
            }
        }

        public void AddFrame(Frame frame)
        {
            m_duration += frame.Duration;
            m_frames.Add(frame);
        }
    }
}
