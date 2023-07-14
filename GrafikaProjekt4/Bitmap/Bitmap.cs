using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GKtoolkit
{
    public class CustomBitmap
    {
        public WriteableBitmap writeableBitmap { get; }

        public CustomBitmap(int width, int height, int pW, int pH)
        {
            writeableBitmap = new WriteableBitmap(
                width,
                height,
                pW,
                pH,
                PixelFormats.Bgr32,
                null
                );
        }

        public void DrawPixel(int x, int y, Color color, int thickness = 0)
        {
            byte[] bitmapArray = new byte[writeableBitmap.PixelHeight * writeableBitmap.BackBufferStride];
            writeableBitmap.CopyPixels(bitmapArray, writeableBitmap.BackBufferStride, 0);
            int valuesPerPixel = writeableBitmap.BackBufferStride / writeableBitmap.PixelWidth;

            bitmapArray[index(x, y, 0, valuesPerPixel)] = color.B;
            bitmapArray[index(x, y, 1, valuesPerPixel)] = color.G;
            bitmapArray[index(x, y, 2, valuesPerPixel)] = color.R;
            for (int i = 1; i <= thickness; i++)
            {
                bitmapArray[index(x, y + i, 0, valuesPerPixel)] = color.B;
                bitmapArray[index(x, y + i, 1, valuesPerPixel)] = color.G;
                bitmapArray[index(x, y + i, 2, valuesPerPixel)] = color.R;

                bitmapArray[index(x, y - i, 0, valuesPerPixel)] = color.B;
                bitmapArray[index(x, y - i, 1, valuesPerPixel)] = color.G;
                bitmapArray[index(x, y - i, 2, valuesPerPixel)] = color.R;
            }
            writeableBitmap.WritePixels(new Int32Rect(0, 0, writeableBitmap.PixelWidth,
                writeableBitmap.PixelHeight), bitmapArray, writeableBitmap.BackBufferStride, 0);
        }

        private int index(int x, int y, int offset, int valuesPerPixel)
        {
            return (x + y * writeableBitmap.PixelWidth) * valuesPerPixel + offset;
        }
    }
}
