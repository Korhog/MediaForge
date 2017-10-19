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

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Sequence.UI
{
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

            m_border.DragOver += OnDropOver;
            m_border.Drop += OnDrop;
            m_border.PointerMoved += OnPointerMoved;
            m_border.PointerExited += OnPointerExited;
            m_border.PointerPressed += OnPointerPressed;
        }

        private void OnDropOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
        }

        private async void OnDrop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();
                if (items.Count > 0)
                {
                    var storageFile = items[0] as StorageFile;
                    var bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(await storageFile.OpenAsync(FileAccessMode.Read));
                    m_inner_sequence.Add(new SequenceImage(m_inner_sequence, bitmapImage) {
                        Duration = new TimeSpan(0, 0, 2)
                    });
                }
            }
            else if (e.DataView.Contains(StandardDataFormats.Text))
            {
                var text = await e.DataView.GetTextAsync();
                if (!string.IsNullOrEmpty(text))
                {
                    m_inner_sequence.Add(new SequenceBaseItem(m_inner_sequence)
                    {
                        Duration = new TimeSpan(0, 0, 2)
                    });
                }
            }
            else
            {
                var a = e.DataView.Properties;
                m_inner_sequence.Add(new SequenceBaseItem(m_inner_sequence) {
                    Duration = new TimeSpan(0, 0, 2)
                }); 
            }            
        }

        private void OnPointerMoved (object sender, PointerRoutedEventArgs e) {
            var pointer = e.GetCurrentPoint(m_border);
            m_inner_sequence.DragItem?.Translate(pointer.Position.X - m_begin.X);
            m_begin = pointer.Position;
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var pointer = e.GetCurrentPoint(m_border);
            m_begin = pointer.Position;
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            m_inner_sequence.SetDragItem(null);
        }
    }
}
