using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GKtoolkit
{
    public class ColorValue : IBitmap
    {
        public Color color { get; set; }
        public ColorValue(Color color)
        {
            this.color = color;
        }

        public (byte r, byte g, byte b, byte a) GetColorValue(int x, int y)
        {
            return (color.R, color.G, color.B, 255);
        }
    }
}
