using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    using MediaCore.Types;

    public class VideoProject
    {
        public delegate void SettingsChangedEvent(VideoProject sender);
        public event SettingsChangedEvent SettingsChanged;

        private static VideoProject m_instance;
        private static object sync = new object();

        protected FPS m_fps = FPS.FPS30;

        protected void SetFPS(FPS fps)
        {
            m_fps = fps;
            SettingsChanged?.Invoke(this);
        }

        public FPS FPS { get { return m_fps; } set { SetFPS(value); } }
       

        public static VideoProject GetInstance()
        { 
            if (m_instance == null)
            {
                lock (sync)
                {
                    if (m_instance == null)
                    {
                        m_instance = new VideoProject();
                    }
                }
            }

            return m_instance;
        }        
    }
}
