using GKtoolkit;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace GKtoolkit
{
    public class ScanLinia
    {
        public static void Fill(Triangle shape, BitmapArray bitmap,
            IBitmap sourceBitmap, Func<Color?, CustomVector, Position, Color> shading, bool Ginterpolation = false, bool phong = false)
        {
            Vertice vmin, vmid, vmax;

            (Vertice vmin, Vertice vmid, Vertice vmax) GetVerticesFromTheSmallestY(List<Vertice> vertices)
            {
                Vertice vminp, vmidp, vmaxp;
                vminp = vertices[0]; // y1
                vmidp = vertices[1]; // y2
                vmaxp = vertices[2]; // y3

                Vertice tmp;
                if (vminp.screenPosition.Y > vmidp.screenPosition.Y)
                {
                    tmp = vminp;
                    vminp = vmidp;
                    vmidp = tmp;
                }
                if (vminp.screenPosition.Y > vmaxp.screenPosition.Y)
                {
                    tmp = vminp;
                    vminp = vmaxp;
                    vmaxp = tmp;
                }
                if (vmidp.screenPosition.Y > vmaxp.screenPosition.Y)
                {
                    tmp = vmidp;
                    vmidp = vmaxp;
                    vmaxp = tmp;
                }
                return (vminp, vmidp, vmaxp);
            }

            (vmin, vmid, vmax) = GetVerticesFromTheSmallestY(shape.vertices);

            Line line1 = new Line(vmin, vmid);
            Line line2 = new Line(vmin, vmax);
            Line line3 = new Line(vmid, vmax);

            // Line from the miny vertice to the midy vertice
            LinePointer minmidp = new LinePointer((int)vmin.screenPosition.X,
                (int)vmin.screenPosition.Y,
                GetDxDy(line1), vmin, vmid);

            // Line from the miny vertice to the maxy vertice
            LinePointer minmaxp = new LinePointer((int)vmin.screenPosition.X,
                (int)vmin.screenPosition.Y,
                GetDxDy(line2), vmin, vmax);

            // Line from the midy vertice to the maxy vertice
            LinePointer midmaxp = new LinePointer((int)vmid.screenPosition.X,
                (int)vmid.screenPosition.Y,
                GetDxDy(line3), vmid, vmax);

            double GetDxDy(Line line)
            {
                return line.Screendx() / line.Screendy();
            }

            int y = (int)vmin.screenPosition.Y,
                ymax = (int)vmax.screenPosition.Y,
                ymid = (int)vmid.screenPosition.Y;

            List<LinePointer> pointers = new List<LinePointer>();

            // Top line is horizontal
            pointers.AddRange(new List<LinePointer>() { minmidp, minmaxp });

            LinePointer lp1, lp2;


            while (y < ymax)
            {
                if (y == ymid)
                {
                    pointers.Clear();
                    pointers.Add(midmaxp);
                    pointers.Add(minmaxp);
                }

                lp1 = pointers[0];
                lp2 = pointers[1];

                if (lp1.x > lp2.x)
                {
                    LinePointer tmp = lp1;
                    lp1 = lp2;
                    lp2 = tmp;
                }

                int distance = (int)Math.Ceiling(lp2.x - lp1.x), xc;
                xc = (int)lp1.x;
                lp1.SetZValueAt(y);
                lp2.SetZValueAt(y);

                if (Ginterpolation)
                {
                    lp1.SetGouraud(y);
                    lp2.SetGouraud(y);
                }

                if(phong)
                {
                    lp1.SetNormal(y);
                    lp2.SetNormal(y);

                    lp1.SetPosition(y);
                    lp2.SetPosition(y);
                }

                if (distance > 0)
                {
                    for (int i = xc; i < distance + xc; i++)
                    {
                        double zval = GetZAt(distance, i - xc, lp1, lp2);
                        double zatI = bitmap.GetZAt(i, y);
                        if (zatI < zval)
                        {
                            bitmap.SetZAt(i, y, zval);

                            // Phong
                            if (phong)
                            {
                                CustomVector vec = GetNAt(distance, i - xc, lp1, lp2).Normalized();
                                Position pixpos = GetPosAt(distance, i - xc, lp1, lp2);
                                byte a, r, g, b;
                                (r, g, b, a) = sourceBitmap.GetColorValue(i, y);
                                Color c = Color.FromArgb(a, r, g, b);

                                bitmap.SetPixel(i, y, shading(c, vec, pixpos));
                            }
                            else if (!Ginterpolation)
                            {
                                byte a, r, g, b;
                                (r, g, b, a) = sourceBitmap.GetColorValue(i, y);
                                Color c = Color.FromArgb(a, r, g, b);

                                bitmap.SetPixel(i, y, shading(c, null, null));
                            }
                            // Gouraud
                            else
                            {
                                Color c = GetGAt(distance, i - xc, lp1, lp2);
                                bitmap.SetPixel(i, y, shading(c, null, null));
                            }
                        }
                    }
                }

                foreach (LinePointer p in pointers)
                {
                    p.x += p.dxdy;
                }
                y++;
            }

            double GetZAt(int length, int curXOnPath, LinePointer p1, LinePointer p2)
            {
                double val = curXOnPath / (double)length;
                return p2.z * val + (1 - val) * p1.z;
            }

            Color GetGAt(int length, int curXOnPath, LinePointer p1, LinePointer p2)
            {
                double val = curXOnPath / (double)length;
                byte red = (byte)(p2.gouraud.R * val + (1 - val) * p1.gouraud.R);
                byte green = (byte)(p2.gouraud.G * val + (1 - val) * p1.gouraud.G);
                byte blue = (byte)(p2.gouraud.B * val + (1 - val) * p1.gouraud.B);
                return Color.FromArgb(255, red, green, blue);
            }

            CustomVector GetNAt(int length, int curXOnPath, LinePointer p1, LinePointer p2)
            {
                double val = curXOnPath / (double)length;
                double xn = (byte)(p2.normal.X * val + (1 - val) * p1.normal.X);
                double yn = (byte)(p2.normal.Y * val + (1 - val) * p1.normal.Y);
                double zn = (byte)(p2.normal.Z * val + (1 - val) * p1.normal.Z);
                return new CustomVector(new double[4] { xn, yn, zn, 0 }, 4);
            }

            Position GetPosAt(int length, int curXOnPath, LinePointer p1, LinePointer p2)
            {
                double val = curXOnPath / (double)length;
                double xn = (byte)(p2.interpolatedPosition.X * val + 
                    (1 - val) * p1.interpolatedPosition.X);
                double yn = (byte)(p2.interpolatedPosition.Y * val +
                    (1 - val) * p1.interpolatedPosition.Y);
                double zn = (byte)(p2.interpolatedPosition.Z * val +
                    (1 - val) * p1.interpolatedPosition.Z);
                return new Position(xn, yn, zn);
            }
        }
    }
}