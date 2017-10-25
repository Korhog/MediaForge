using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;

namespace Render.Drawing
{
    public class RenderObjectBase
    {
        public Vector2 Position { get; set; }
        public float Scale { get; set; }
        public SoftwareBitmap Source { get; set; }

        public Matrix3x2 Transformaion { get { return GetTransformaion(); } }

        protected Matrix3x2 GetTransformaion()
        {
            Matrix3x2 transformaion = Matrix3x2.Identity;

            transformaion *= Matrix3x2.CreateScale(
                Scale, 
                new Vector2(
                    Source.PixelWidth / 2, 
                    Source.PixelHeight / 2
                ));

            transformaion *= Matrix3x2.CreateTranslation(Position);

            return transformaion;
        }

        public RenderObjectBase()
        {
            Position = new Vector2();
            Scale = 1;
        }
    }
}
