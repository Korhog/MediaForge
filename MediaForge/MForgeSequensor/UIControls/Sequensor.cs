using MForge.Sequensor.Sequence;
using MForge.Sequensor.Sequence.Interfaces;
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
    public delegate void SceneDurationChangedEvent(IScene scene);

    public sealed class Sequensor : Control
    {
        int borderSize = 40;

        public SceneDurationChangedEvent DurationChanged { get; set; }

        IScene currentScene = null;
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

                var frameScale = (e.NewSize.Width - borderSize) / currentScene.FrameDuration;
                foreach (var sequence in controller.Sequences)
                    sequence.UpdateScale(frameScale);
            };
            
            items.DragItemsStarting += (sender, e) =>
            {
                var item = e.Items.FirstOrDefault() as ISequence;
                if (item != null)
                    e.Data.Properties.Add("Context", item);
            };

            items.DragOver += (sender, e) => {
                e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
            };

            items.Drop += (sender, e) => {
                if (controller == null || currentScene == null)
                    return;

                var seq = new SequenceBase();
                seq.Items.Add(new SequenceElementBase(currentScene));

                var frameScale = (items.ActualWidth - 40) / currentScene.FrameDuration;
                seq.UpdateScale(frameScale);

                controller.Sequences.Add(seq);
            }; 
            // Получаем слайдер 
            slider = GetTemplateChild("DurationSlider") as Slider;
            slider.ValueChanged += SceneDurationChange;
        }

        public void SetScene(IScene scene)
        {
            currentScene = scene;
            if (scene == null)
                return;

            slider.Value = scene.FrameDuration;

            controller = scene.Sequensor;
            if (items != null)
            {
                items.ItemsSource = controller.Sequences;
                var frameScale = (items.ActualWidth - borderSize) / scene.FrameDuration;
                foreach (var sequence in controller.Sequences)
                    sequence.UpdateScale(frameScale);
            }
        }

        public void DeleteSequence(ISequence sequence)
        {
            currentScene.Sequences.Remove(sequence);
        }

        void SceneDurationChange(object sender, RangeBaseValueChangedEventArgs e)
        {
            if ( controller == null || currentScene == null || e.OldValue == (int)e.NewValue )
                return;            

            currentScene.FrameDuration = (int)e.NewValue;
            DurationChanged?.Invoke(currentScene);

            var frameScale = (items.ActualWidth - borderSize) / currentScene.FrameDuration;

            foreach (var sequence in controller.Sequences)
                sequence.UpdateScale(frameScale);
        }
    }    
}
