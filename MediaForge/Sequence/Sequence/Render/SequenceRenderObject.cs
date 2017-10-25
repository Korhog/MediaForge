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
    using Microsoft.Graphics.Canvas;
    using Windows.Graphics.Imaging;

    /// <summary>
    /// Класс, который рисуется.
    /// </summary>
    public abstract class SequenceRenderObject : SequenceBaseObject
    {        
        protected RenderBuffer m_render;

        public double Width { get; set; }
        public double Height { get; set; }

        public double Left { get; set; }
        public double Top { get; set; }

        public float Scale { get; set; }
        public float Rotation { get; set; }




        public RenderBuffer Render {  get { return m_render; } }

        public SequenceRenderObject() : base ()
        {
            Left = 0;
            Top = 0;

            Scale = 1;

            Rotation = 0;

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
