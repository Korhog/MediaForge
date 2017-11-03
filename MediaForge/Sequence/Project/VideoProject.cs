using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    using MediaCore.Types;
    using Windows.UI.Popups;
    using Windows.UI.Xaml.Controls;

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

        protected int m_width = 320;
        public int Width { get { return m_width; } }

        protected int m_height = 240;
        public int Heigth { get { return m_height; } }

        public async Task SetSettings(int width, int height, FPS fps)
        {
            if (width <= 0 || height <= 0)
                return;

            bool change = !(fps == m_fps && width == m_width && height == m_height);

            if (change)
            {
                var msg = @"Параметры медиафайла отличаются от параметров проекта, изменить проект?";

                var dialog = new MessageDialog(msg);

                dialog.Commands.Add(new UICommand("Да") { Id = 0 });
                dialog.Commands.Add(new UICommand("Нет") { Id = 1 });

                var command = await dialog.ShowAsync();
                if ((int)command.Id == 0)
                {
                    m_fps = fps;
                    m_width = width;
                    m_height = height;

                    SettingsChanged?.Invoke(this);
                }
            }
        }       

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
