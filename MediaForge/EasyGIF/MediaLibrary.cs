using MForge.Render2D.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGIF
{
    public class MediaLibrary
    {
        ObservableCollection<IRenderSource> items;
        public ObservableCollection<IRenderSource> Items { get { return items; } }

        public MediaLibrary()
        {
            items = new ObservableCollection<IRenderSource>();
        }

        public void CreateImage()
        {
            var image = new RenderSourceBase();
            items.Add(image);
        }
    }
}
