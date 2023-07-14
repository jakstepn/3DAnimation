using GrafikaProjekt4;
using System;
using System.Windows.Media;

namespace GKtoolkit
{
    public class Bresenham
    {
        public static void DrawLine(int x1, int x2, int y1, int y2,
            Action<int, int> putPixelAt)
        {
            int d, dx, dy, incrNE, incrE, incrX, incrY, x = x1, y = y1;

            // Choose direction
            incrX = x1 < x2 ? 1 : -1;
            incrY = y1 < y2 ? 1 : -1;
            dx = incrX * (x2 - x1);
            dy = incrY * (y2 - y1);

            putPixelAt(x, y);


            // Choose axis
            if (dx > dy)
            {
                d = 2 * dy - dx;
                incrE = 2 * dy;
                incrNE = (dy - dx) * 2;

                while (x != x2)
                {
                    if (d < 0)
                    {
                        d += incrE;
                        x += incrX;
                    }
                    else
                    {
                        d += incrNE;
                        x += incrX;
                        y += incrY;
                    }
                    putPixelAt(x, y);
                }
            }
            else
            {
                d = 2 * dx - dy;
                incrE = 2 * dx;
                incrNE = (dx - dy) * 2;

                while (y != y2)
                {
                    if (d < 0)
                    {
                        d += incrE;
                        y += incrY;
                    }
                    else
                    {
                        d += incrNE;
                        x += incrX;
                        y += incrY;
                    }
                    putPixelAt(x, y);
                }
            }
        }
    }
}
