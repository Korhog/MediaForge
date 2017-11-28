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

            (GetTemplateChild("PART_LEFT_BTN") as Border).PointerPressed += (sender, e) => {
                mode = UIControls.ManipulationMode.ScaleLeft;
            };

            (GetTemplateChild("PART_RIGHT_BTN") as Border).PointerPressed += (sender, e) => {
                mode = UIControls.ManipulationMode.ScaleRight;
            };

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

        protected override void OnManipulationCompleted(ManipulationCompletedRoutedEventArgs e)
        {
            base.OnManipulationCompleted(e);
            mode = UIControls.ManipulationMode.Move;
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
                return;
            }

            currentX += e.Delta.Translation.X;

            var x = currentX - beginX.Value;
            var framesDelta = (int)(x / frameScale);

            switch (mode)
            {
                case UIControls.ManipulationMode.Move:
                    if (beginFrame + framesDelta < 0)
                    {
                        framesDelta = -beginFrame;
                    }
                    SetBeginFrame(beginFrame + framesDelta);
                    break;

                case UIControls.ManipulationMode.ScaleLeft:
                    if (beginFrame + framesDelta < 0)
                    {
                        framesDelta = -beginFrame;
                    }
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
            Margin = new Thickness(left, 0, 0, 0);
        }

        void SetFrameSize(int frames)
        {
            frameSize = frames;
            context.FramesDuration = frames;

            var left = context.StartFrame * frameScale;
            var right = (context.StartFrame + context.FramesDuration) * frameScale;

            border.Width = right - left;
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {            
            base.OnPointerPressed(e);
            e.Handled = true;
        }
    }
}
