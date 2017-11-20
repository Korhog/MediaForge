using MForge.Sequensor.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace MForge.Sequensor.UIControls
{
    public sealed class Sequensor : Control
    {
        SequenceController controller;
        ItemsControl items;
        public SequenceController Controller { get { return controller; } }

        public Sequensor()
        {
            this.DefaultStyleKey = typeof(Sequensor);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            items = GetTemplateChild("Sequences") as ItemsControl;
            items.SizeChanged += (sender, e) =>
            {
                if (controller == null)
                    return;

                var frameScale = e.NewSize.Width / 100;
                foreach (var sequence in controller.Sequences)
                    sequence.UpdateScale(frameScale);
            };

            Button btn = GetTemplateChild("AddButton") as Button;
            btn.Click += (sender, e) =>
            {
                if (controller == null) return;
                var seq = new SequenceBase();
                seq.Items.Add(new SequenceElementBase());

                controller.Sequences.Add(seq);
            };
        }

        public void SetScene(Scene scene)
        {
            if (scene == null)
                return;

            controller = scene.Sequensor;
            if (items != null)
            {
                items.ItemsSource = controller.Sequences;

                var frameScale = items.ActualWidth / scene.FrameDuration;
                foreach (var sequence in controller.Sequences)
                    sequence.UpdateScale(frameScale);
            }
        }
    }    
}
