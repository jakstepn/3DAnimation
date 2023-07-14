using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKtoolkit
{
    public class BitmapSize
    {
        public int width { get; }
        public int height { get; }
        public int valuesPerPixel { get; }
        public int stride { get; }
        public int length { get; }
        public BitmapSize(int width, int height, int valuesPerPixel = 3)
        {
            this.width = width;
            this.height = height;
            this.valuesPerPixel = valuesPerPixel;
            stride = width * valuesPerPixel;
            length = width * height * valuesPerPixel;
        }
    }
}
