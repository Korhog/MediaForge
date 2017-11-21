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
    enum ManipulationMode
    {
        ScaleLeft,
        ScaleRight,
        Move
    }

    public sealed class SequenceElementControl : Control
    {
        ManipulationMode mode = UIControls.ManipulationMode.Move;
        ISequenceElement context;

        double frameScale = 5;
        double? beginX;
        double currentX;
        int beginFrame = 0;
        int frame = 0;

        int beginFrameSize = 20;
        int frameSize = 20;
        Border border;

        public SequenceElementControl()
        {
            this.DefaultStyleKey = typeof(SequenceElementControl);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            border = GetTemplateChild("Border") as Border;
            context = DataContext as ISequenceElement;
            context.OnScale += (scale) =>
            {
                frameScale = scale;
                SetBeginFrame(frame);
                SetFrameSize(frameSize);
            };

            frameScale = context.FrameScale;
            frame = context.StartFrame;
            frameSize = context.FramesDuration;

            SetBeginFrame(frame);
            SetFrameSize(frameSize);
        }

        protected override void OnManipulationStarting(ManipulationStartingRoutedEventArgs e)
        {
            base.OnManipulationStarting(e);
            beginX = null;
        }

        protected override void OnManipulationDelta(ManipulationDeltaRoutedEventArgs e)
        {
            base.OnManipulationDelta(e);
            if (!beginX.HasValue)
            {
                beginX = e.Position.X;
                beginFrame = frame;
                beginFrameSize = frameSize;
                currentX = beginX.Value;

                mode = e.Position.X < 8 ? UIControls.ManipulationMode.ScaleLeft : UIControls.ManipulationMode.Move;

                return;
            }

            currentX += e.Delta.Translation.X;

            var x = currentX - beginX.Value;
            var framesDelta = (int)(x / frameScale);

            if (beginFrame + framesDelta < 0)
            {
                framesDelta = -beginFrame;
            }

            switch (mode)
            {
                case UIControls.ManipulationMode.Move:
                    SetBeginFrame(beginFrame + framesDelta);
                    break;

                case UIControls.ManipulationMode.ScaleLeft:
                    SetBeginFrame(beginFrame + framesDelta);
                    SetFrameSize(beginFrameSize - framesDelta);
                    break;

                case UIControls.ManipulationMode.ScaleRight:
                    SetFrameSize(beginFrameSize + framesDelta);
                    break;
            }
                   
        }

        void SetBeginFrame(int newFrame)
        {
            context.StartFrame = newFrame;
            frame = newFrame;
            var left = frame * frameScale;
            border.Margin = new Thickness(left, 0, 0, 0);
        }

        void SetFrameSize(int frames)
        {
            frameSize = frames;
            context.FramesDuration = frames;

            var left = context.StartFrame * frameScale;
            var right = (context.StartFrame + context.FramesDuration) * frameScale;

            border.Width = right - left;
        }
    }
}
