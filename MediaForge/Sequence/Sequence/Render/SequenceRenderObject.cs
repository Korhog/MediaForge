using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sequence.Render
{
    /// <summary>
    /// Класс, который рисуется.
    /// </summary>
    public abstract class SequenceRenderObject : SequenceBaseObject
    {
        protected Image m_render;
        public Image Render {  get { return m_render; } }

        public SequenceRenderObject() : base ()
        {
            m_render = new Image();            
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
    }
}
