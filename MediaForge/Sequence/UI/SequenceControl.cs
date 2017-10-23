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

using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using AudioEngine;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Sequence.UI
{
    using Media;

    public sealed class SequenceControl : ItemsControl
    {
        private Border m_border;
        private Point m_begin;
        private SequenceBase m_inner_sequence;
        private ItemsControl m_items;

        public SequenceControl()
        {
            this.DefaultStyleKey = typeof(SequenceControl);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            m_inner_sequence = DataContext as SequenceBase;

            m_items = GetTemplateChild("Items") as ItemsControl;
            m_items.ItemsSource = m_inner_sequence.Items;

            m_border = GetTemplateChild("Border") as Border;

            m_border.DragOver += m_inner_sequence.OnDropOver;
            m_border.Drop += m_inner_sequence.OnDrop;
        }        
    }
}
