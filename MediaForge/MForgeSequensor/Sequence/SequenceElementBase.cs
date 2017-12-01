using MForge.Sequensor.Sequence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MForge.Sequensor.Sequence
{
    public class SequenceElementBase : ISequenceElement
    {
        private IScene scene;
        public IScene Scene { get { return scene; } }

        public SequenceElementBase(IScene rootScene)
        {
            scene = rootScene;
        }

        public int FramesDuration { get; set; } = 20;
        public int StartFrame { get; set; } = 0;
        public int EndFrame { get { return StartFrame + FramesDuration; } }

        protected double frameScale = 5;
        public double FrameScale { get { return frameScale; } }

        public event UpdateScaleEvent OnScale;

        public void UpdateScale(double scale)
        {
            frameScale = scale;
            OnScale?.Invoke(frameScale);
        }
    }
}
