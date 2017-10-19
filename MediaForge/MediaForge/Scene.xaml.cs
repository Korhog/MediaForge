using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace MediaForge
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Scene : Page
    {
        public Scene()
        {
            this.InitializeComponent();
        }

        private void OnManipulation(object sender, ManipulationStartedRoutedEventArgs e)
        {
            var i = 1;
        }

        private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (e == null)
                return;

            var border = sender as Border;
            if (border == null)
                return;

            var grid = border.Parent as Grid;
            if (grid == null)
                return;

            var row = Grid.GetRow(border);
            var col = Grid.GetColumn(border);
            if (row == 2)
            {
                if (col == 0)
                {
                    Canvas.SetLeft(grid, Canvas.GetLeft(grid) + e.Delta.Translation.X);
                    var w = Content.ActualWidth - e.Delta.Translation.X;
                    Content.Width = w < 0 ? 0 : w;
                }
                else
                {
                    var w = Content.ActualWidth + e.Delta.Translation.X;
                    Content.Width = w < 0 ? 0 : w;
                }
            }

            if (col == 2)
            {
                if (row == 0)
                {
                    Canvas.SetTop(grid, Canvas.GetTop(grid) + e.Delta.Translation.Y);
                    var h = Content.ActualHeight - e.Delta.Translation.Y;
                    Content.Height = h < 0 ? 0 : h;
                }
                else
                {
                    var h = Content.ActualHeight + e.Delta.Translation.Y;
                    Content.Height = h < 0 ? 0 : h;
                }
            }


        }
    }
}
