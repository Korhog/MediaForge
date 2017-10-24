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

namespace Sequence
{
    public enum SequenceControllerState
    {
        NONE,
        RESET,
        READY,
        PLAY,
        PAUSE
    }

    public class SequenceControllerBase
    {
        /*
         * Тут мы временно добаляем аудио дорожку до тех пор, пока не появится 
         * движок с использованием аудио графа
         */

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
                AddItem?.Invoke(this, o);
            };

            Sequences.Add(sequence);
        }
        public SequenceControllerBase()
        {
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

        protected virtual void OnFrame(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (e.NewValue > e.OldValue)
            {
                FrameForward(sender, e);
            }
            else
            {
                // FrameBackward(sender, e);
                FrameForward(sender, e);
            }
        }  

        public SoftwareBitmap[][] GetRenderObjects()
        {
            var time = new TimeSpan((long)(m_slider.Value / 30 * 10000000));
            var layers = m_sequences
                .OrderByDescending(x => m_sequences.IndexOf(x))
                .Select(x => x.Items
                    .Where(y => y is SequenceAnimatedImage && y.StartTime < time && y.StartTime + y.Duration >= time)
                    .Select(y => y as SequenceAnimatedImage)
                    .Select(y => y.GetRenderData(time)).ToArray())
                    .ToArray();
            return layers;
        }
        
        protected virtual void FrameForward(object sender, RangeBaseValueChangedEventArgs e)
        {
            /*
            // Получаем список всех активных рендер объектов   
            var time = new TimeSpan((long)(m_slider.Value / 30 * 10000000));
            var layers = m_sequences
                .OrderByDescending(x => m_sequences.IndexOf(x))
                .Select(x => x.Items.Where(y => y is Render.SequenceRenderObject && y.StartTime < time && y.StartTime + y.Duration <= time)
                .ToArray()).ToArray();
            
            foreach (var sequence in Sequences)
            {
                sequence.Frame(time);            
            }
            */

            OnDraw?.Invoke();
        }

        protected virtual void FrameBackward(object sender, RangeBaseValueChangedEventArgs e)
        {

        }

        
    }
}
