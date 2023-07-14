using System.Threading.Tasks;
using System.Windows.Media;

namespace GKtoolkit
{
    public class BitmapArray : IBitmap
    {
        public byte[] array { get; }
        public double[] zbuffer { get; }
        public int height
        {
            get
            {
                return array.Length / stride;
            }
        }
        public int width
        {
            get
            {
                return stride / valuesPerColor;
            }
        }

        public int stride { get; }

        private int valuesPerColor { get; }

        public BitmapArray(int length, int stride, int amountOfValuesPerColor)
        {
            array = new byte[length];
            zbuffer = new double[length/amountOfValuesPerColor];
            this.stride = stride;
            valuesPerColor = amountOfValuesPerColor;
            CleanZBuffer();
        }

        public void Clean(Color color)
        {
            // Pixels per row
            Parallel.For(0, stride / valuesPerColor, (i) =>
              {
                // Buffer height
                for (int j = 0; j < array.Length / stride; j++)
                  {
                      SetPixel(i, j, color);
                  }
              });
            CleanZBuffer();
        }

        public int GetColorValueAt(int x, int y)
        {
            int color = array[index(x, y, 0)] << 16;
            color |= array[index(x, y, 1)] << 8;
            color |= array[index(x, y, 2)] << 0;
            return color;
        }
        public void SetPixel(int x, int y, byte r, byte g, byte b)
        {
            if (x < ScreenSize.screenX && y < ScreenSize.screenY &&
                   x >= 0 && y >= 0)
            {
                array[index(x, y, 2)] = r;
                array[index(x, y, 1)] = g;
                array[index(x, y, 0)] = b;
                array[index(x, y, 3)] = 255;
            }
        }

        public void SetPixel(int x, int y, Color c)
        {
            if (x < ScreenSize.screenX && y < ScreenSize.screenY &&
                x >= 0 && y >= 0)
            {
                array[index(x, y, 0)] = c.B;
                array[index(x, y, 1)] = c.G;
                array[index(x, y, 2)] = c.R;
                array[index(x, y, 3)] = 255;
            }
        }

        public void SetPixel(int x, int y, BitmapArray srcarray)
        {
            array[index(x, y, 0)] = srcarray.array[index(x, y, 0)];
            array[index(x, y, 1)] = srcarray.array[index(x, y, 1)];
            array[index(x, y, 2)] = srcarray.array[index(x, y, 2)];
            array[index(x, y, 3)] = 255;
        }

        public void SetPixel(int x, int y, IBitmap value)
        {
            (byte r, byte g, byte b, byte a) = value.GetColorValue(x, y);
            array[index(x, y, 0)] = b;
            array[index(x, y, 1)] = g;
            array[index(x, y, 2)] = r;
        }

        public void SetLine(int row, int column, int pixelLength, IBitmap value)
        {
            for (int i = 0; i < pixelLength; i++)
            {
                SetPixel(i + column, row, value);
            }
        }

        public void SetSquare(int row, int column, int radius, Color c)
        {
            for (int i = 0; i < radius; i++)
            {
                for (int j = 0; j < radius; j++)
                {
                    SetPixel(i + column - radius / 2, row - radius / 2 + j, c);
                }
            }
        }

        private void CleanZBuffer()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    zbuffer[zindex(i, j)] = double.NegativeInfinity + 1;
                }
            }
        }

        private int index(int x, int y, int offset)
        {
            return x * valuesPerColor + y * stride + offset;
        }

        private int zindex(int x, int y)
        {
            if (x >= 0 && y >=0 &&
                x < ScreenSize.screenX && y < ScreenSize.screenY)
            {
                return x + y * width;
            }
            else
            {
                return 0;
            }
        }

        public (byte r, byte g, byte b, byte a) GetColorValue(int x, int y)
        {
            byte r, g, b, a;
            b = array[index(x, y, 0)];
            g = array[index(x, y, 1)];
            r = array[index(x, y, 2)];
            a = 255;
            return (r, g, b, a);
        }

        public double GetZAt(int x, int y)
        {
            return zbuffer[zindex(x, y)];
        }

        public void SetZAt(int x,int y, double value)
        {
            zbuffer[zindex(x,y)] = value;
        }
    }
}
