using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Media.Core;
using Sequence.Media;
using Windows.Graphics.Imaging;
using Microsoft.Graphics.Canvas;

using Render.Drawing;
using System.Numerics;

namespace Sequence
{
    using Project;
    using MediaCore.Encoder;
    using MediaCore.Types;
    using global::Render;
    using Sequence.Render;

    public enum SequenceControllerState
    {
        NONE,
        RESET,
        READY,
        PLAY,
        PAUSE
    }

    public class SequenceControllerBase : IFrameData
    {
        /*
         * Тут мы временно добаляем аудио дорожку до тех пор, пока не появится 
         * движок с использованием аудио графа
         */

        VideoProject m_project;

        MediaPlayer m_media_player;
        public void SetAudioTrack(StorageFile source)
        {
            m_media_player = new MediaPlayer();
            m_media_player.Source = MediaSource.CreateFromStorageFile(source);
        }

        private void PlayAudio(TimeSpan time)
        {
            if (m_media_player == null)
                return;

            m_media_player.PlaybackSession.Position = time;
            m_media_player.Play();
        }

        private void StopAudio()
        {
            if (m_media_player == null)
                return;
            m_media_player.Pause();
        }

        // конец

        protected Slider m_slider;

        protected Storyboard m_animator;
        protected SequenceControllerState m_controller_state = SequenceControllerState.NONE;

        public delegate void SequenceItemAdded(SequenceControllerBase sender, SequenceBaseObject item);
        public event SequenceItemAdded AddItem;

        public delegate void Draw();
        public event Draw OnDraw;

        ObservableCollection<SequenceBase> m_sequences;
        ObservableCollection<string> m_names;

        public ObservableCollection<SequenceBase> Sequences { get { return m_sequences; } }

        public void Create()
        {
            var sequence = new SequenceBase();

            sequence.AddItem += (sender, o) =>
            {
                m_animator.Stop();
                AddItem?.Invoke(this, o);
            };

            Sequences.Insert(0, sequence);
        }

        public SequenceControllerBase()
        {
            m_project = VideoProject.GetInstance();

            m_sequences = new ObservableCollection<SequenceBase>();
            m_names = new ObservableCollection<string>();

            m_animator = new Storyboard();

            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 600;
            da.Duration = new Duration(TimeSpan.FromSeconds(20));
            da.EnableDependentAnimation = true;
            m_animator.Children.Add(da);
            Storyboard.SetTargetProperty(m_animator, "Value");

            m_project.SettingsChanged += (project) =>
            {
                // Обновление настроек
                int frames = 20000 / (int)project.FPS;
                int mills = frames * (int)project.FPS;

                m_animator.Children.Clear();
                m_slider.Maximum = frames;

                var animation = new DoubleAnimation();

                animation.From = 0;
                animation.To = frames;

                animation.Duration = new Duration(TimeSpan.FromMilliseconds(mills));
                animation.EnableDependentAnimation = true;

                m_animator.Children.Add(da);
            };
        }

        public virtual void SetSlider(Slider slider)
        {
            if (slider == null)
            {
                if (m_slider != null)
                {
                    m_slider.ValueChanged -= OnFrame;
                }
                return;
            }

            if (slider == m_slider)
                return;

            m_slider = slider;
            m_slider.ValueChanged += OnFrame;
            Storyboard.SetTarget(m_animator, m_slider);
            m_controller_state = SequenceControllerState.RESET;
        }

        private int? m_current_frame = null;

        public void Play()
        {
            var animation = m_animator.Children[0] as DoubleAnimation;

            var time = new TimeSpan((long)(m_slider.Value / 30 * 10000000));
            switch (m_controller_state)
            {
                case SequenceControllerState.RESET:
                    m_controller_state = SequenceControllerState.PLAY;
                    m_animator.Begin();
                    PlayAudio(time);
                    break;
                case SequenceControllerState.READY:
                    m_controller_state = SequenceControllerState.PLAY;
                    m_animator.Begin();
                    PlayAudio(time);
                    break;
                case SequenceControllerState.PLAY:
                    var value = m_slider.Value;
                    m_controller_state = SequenceControllerState.PAUSE;
                    m_animator.Stop();
                    StopAudio();
                    m_slider.Value = value;
                    break;
                case SequenceControllerState.PAUSE:
                    animation.From = m_slider.Value;
                    m_controller_state = SequenceControllerState.PLAY;
                    m_animator.Begin();
                    PlayAudio(time);
                    break;
            }
        }

        RenderObjectBase[][] m_cache_frame = new RenderObjectBase[0][];
        public RenderObjectBase[][] CachedFrame { get { return m_cache_frame; } }

        protected virtual async void OnFrame(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (m_current_frame.HasValue && m_current_frame.Value != (int)m_slider.Value)
            {
                await GetRenderObjects();
                OnDraw?.Invoke();
            }
            m_current_frame = (int)m_slider.Value;
        }

        public async Task GetRenderObjects(int frame)
        {
            var fps = (int)VideoProject.GetInstance().FPS;
            var time = TimeSpan.FromMilliseconds(frame * fps);

            List<RenderObjectBase[]> source = new List<RenderObjectBase[]>();
            foreach (var sequence in m_sequences)
            {
                var items = sequence.Items.Where(x =>
                        x is SequenceRenderObject &&
                        time >= x.StartTime &&
                        time < x.StartTime + x.Duration
                    )
                    .Select(x => x as SequenceRenderObject)
                    .ToList();

                if (items.Any())
                {
                    List<RenderObjectBase> renders = new List<RenderObjectBase>();
                    foreach (var item in items)
                    {
                        renders.Add(new RenderObjectBase
                        {
                            Source = await item.GetRenderData(time),
                            Position = new Vector2((float)item.Left, (float)item.Top),
                            Scale = item.Scale
                        });
                    }
                    source.Add(renders.ToArray());
                }
            }

            lock (m_cache_frame)
            {
                m_cache_frame = source.ToArray();
            }
        }  

        public async Task GetRenderObjects()
        {
            var frame = (int)m_slider.Value;
            await GetRenderObjects(frame);
        }

        public async Task<ImageFrame> GetFrameToEncode(int idx)
        {
            await GetRenderObjects(idx);
            var softwareBitmap = await FrameRender.Render(m_cache_frame);
            return new ImageFrame()
            {
                ImageSource = softwareBitmap
            };
        }

        public int GetFramesCount()
        {
            var lastItems = m_sequences
                .Where(x => x.Items.Count > 0)
                .Select(x => x.Items.LastOrDefault())
                .ToList();


            int duration = lastItems.Count == 0 ? 0 : (int)lastItems.Select(x => x.StartTime + x.Duration).Select(x => x.TotalMilliseconds).Max();

            return duration / (int)m_project.FPS;      
        }
    }
}
