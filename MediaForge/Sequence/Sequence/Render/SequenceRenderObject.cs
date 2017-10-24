using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sequence.Render
{
    using Controls.Container.UI;
    using Windows.Graphics.Imaging;

    /// <summary>
    /// Класс, который рисуется.
    /// </summary>
    public abstract class SequenceRenderObject : SequenceBaseObject
    {        
        protected RenderBuffer m_render;

        public double Width { get; set; }
        public double Height { get; set; }

        public RenderBuffer Render {  get { return m_render; } }

        public SequenceRenderObject() : base ()
        {
            m_render = new RenderBuffer();            
        }

        public override SequenceUpdateResult Update(TimeSpan time)
        {
            var result = base.Update(time);
            UpdateRenderTarget(result, time);
            return result;            
        }

        protected virtual void UpdateRenderTarget(SequenceUpdateResult action, TimeSpan time)
        {

        }

        public virtual SoftwareBitmap GetRenderData(TimeSpan time)
        {
            return null;
        }
    }
}
