using MForge.Sequensor.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace MForge.Sequensor.UIControls
{
    public sealed class Sequensor : Control
    {

        Scene currentScene = null;
        SequenceController controller;

        Slider slider;
        ListView items;
        public SequenceController Controller { get { return controller; } }

        public Sequensor()
        {
            this.DefaultStyleKey = typeof(Sequensor);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            items = GetTemplateChild("Sequences") as ListView;
            items.SizeChanged += (sender, e) =>
            {
                if (controller == null || currentScene == null)
                    return;

#warning убрать хардкод
                var frameScale = (e.NewSize.Width - 40) / currentScene.FrameDuration;
                foreach (var sequence in controller.Sequences)
                    sequence.UpdateScale(frameScale);
            };

            Button btn = GetTemplateChild("AddButton") as Button;
            MenuFlyout menu = btn.Flyout as MenuFlyout;

            foreach (var item in menu.Items)
            {
                item.Tapped += (sender, e) =>
                {
                    if ((sender as MenuFlyoutItem)?.Text == "Shape")
                    {
                        return;
                    }

                    if (controller == null || currentScene == null)
                        return;

                    var seq = new SequenceBase();
                    seq.Items.Add(new SequenceElementBase(currentScene));

                    var frameScale = (items.ActualWidth - 40) / currentScene.FrameDuration;
                    seq.UpdateScale(frameScale);

                    controller.Sequences.Add(seq);
                };

            }           


            // Получаем слайдер 
            slider = GetTemplateChild("DurationSlider") as Slider;
            slider.ValueChanged += SceneDurationChange;
        }

        public void SetScene(Scene scene)
        {
            currentScene = scene;
            if (scene == null)
                return;

            slider.Value = scene.FrameDuration;

            controller = scene.Sequensor;
            if (items != null)
            {
                items.ItemsSource = controller.Sequences;
#warning убрать хардкод
                var frameScale = (items.ActualWidth - 40) / scene.FrameDuration;
                foreach (var sequence in controller.Sequences)
                    sequence.UpdateScale(frameScale);
            }
        }

        void SceneDurationChange(object sender, RangeBaseValueChangedEventArgs e)
        {
            if ( controller == null || currentScene == null || e.OldValue == (int)e.NewValue )
                return;

            currentScene.FrameDuration = (int)e.NewValue;
#warning убрать хардкод
            var frameScale = (items.ActualWidth - 40) / currentScene.FrameDuration;

            foreach (var sequence in controller.Sequences)
                sequence.UpdateScale(frameScale);
        }
    }    
}
