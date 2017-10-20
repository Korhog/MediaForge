using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Sequence.UI
{

    public sealed class Sequensor : Control
    { 
        private SequenceControllerBase m_controller;
        private ScrollViewer m_time_scale_scroll;
        private ScrollViewer m_sequences_scroll;
        public SequenceControllerBase Controller {  get { return m_controller; } }


        public Sequensor()
        {
            this.DefaultStyleKey = typeof(Sequensor);
            m_controller = new SequenceControllerBase();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var sequences = GetTemplateChild("Sequences") as ItemsControl;
            var controls = GetTemplateChild("SequenceControls") as ItemsControl;
            var options = GetTemplateChild("SequenceOptions") as ItemsControl;

            var timeScale = GetTemplateChild("TimeScale") as TimeScaleControl;

            m_time_scale_scroll = GetTemplateChild("TimeScaleScroll") as ScrollViewer;
            m_sequences_scroll = GetTemplateChild("SequencesScroll") as ScrollViewer;

            m_sequences_scroll.ViewChanged += (sender, e) =>
            {
                var scroll = sender as ScrollViewer;
                m_time_scale_scroll.ChangeView(scroll.HorizontalOffset, null, null, true);
            }; 

            var add = GetTemplateChild("Add") as Button;
            add.Click += (sender, e) =>
            {
                m_controller.Create();
            };

            // timeScale.ItemsSource = new ObservableCollection<int>(new int[] { 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6});

            sequences.ItemsSource = m_controller.Sequences;
            controls.ItemsSource = m_controller.Sequences;
            options.ItemsSource = m_controller.Sequences;

            // Анимация

            var slider = GetTemplateChild("Slider") as Slider;

            SetupStoryBoard(slider);
        }

        public void SetupStoryBoard(Slider slider)
        {
            m_controller.SetSlider(slider);
        }

        public void Play()
        {
            m_controller.Play();
        }

        public void Pause()
        {
            m_controller.Play();
        }

        public void Stop()
        {
            m_controller.Play();
        }
    }
}
