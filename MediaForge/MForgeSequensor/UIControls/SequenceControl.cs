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
    public sealed class SequenceControl : Control
    {
        Border border;

        public SequenceControl()
        {
            this.DefaultStyleKey = typeof(SequenceControl);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            border = GetTemplateChild("PART_Border") as Border;
            var context = DataContext as ISequence;
            if (context != null)
            {
                context.OnScale += (scale) =>
                {
                    
                };                
            }
        }


    }
}
