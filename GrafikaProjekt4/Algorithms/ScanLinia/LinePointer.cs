using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GKtoolkit
{
    class LinePointer
    {
        private Vertice start;
        private Vertice end;

        public double x { get; set; }
        public int y { get; set; }
        public double dxdy { get; }
        public double z { get; set; }
        public Position interpolatedPosition { get; set; }
        public Color gouraud { get; set; }
        public CustomVector normal { get; set; }
        public LinePointer(int x, int y, double dxdy, Vertice start, Vertice end)
        {
            z = 0;
            this.x = x;
            this.y = y;
            this.dxdy = dxdy;
            this.start = start;
            this.end = end;
        }
        public void SetZValueAt(int curY)
        {
            int stX, stY, enX, enY;
            stX = (int)start.screenPosition.X;
            stY = (int)start.screenPosition.Y;

            enX = (int)end.screenPosition.X;
            enY = (int)end.screenPosition.Y;

            double dist = Math.Sqrt(
                (x - stX) * (x - stX) + (curY - stY)*(curY - stY));
            double length = Math.Sqrt(
                (enX - stX) * (enX - stX) + (enY - stY)*(enY - stY));

            double val = dist / length;
            z = val * end.screenPosition.Z + (1 - val) * start.screenPosition.Z;
        }
        public void SetGouraud(int curY)
        {
            int stX, stY, enX, enY;
            stX = (int)start.screenPosition.X;
            stY = (int)start.screenPosition.Y;

            enX = (int)end.screenPosition.X;
            enY = (int)end.screenPosition.Y;

            double dist = Math.Sqrt(
                (x - stX) * (x - stX) + (curY - stY) * (curY - stY));
            double length = Math.Sqrt(
                (enX - stX) * (enX - stX) + (enY - stY) * (enY - stY));

            double val = dist / length;
            byte red = (byte)(val * end.color.R + (1 - val) * start.color.R);
            byte green = (byte)(val * end.color.G + (1 - val) * start.color.G);
            byte blue = (byte)(val * end.color.B + (1 - val) * start.color.B);
            gouraud = Color.FromArgb(255, red, green, blue);
        }

        public void SetNormal(int curY)
        {
            int stX, stY, enX, enY;
            stX = (int)start.screenPosition.X;
            stY = (int)start.screenPosition.Y;

            enX = (int)end.screenPosition.X;
            enY = (int)end.screenPosition.Y;

            double dist = Math.Sqrt(
                (x - stX) * (x - stX) + (curY - stY) * (curY - stY));
            double length = Math.Sqrt(
                (enX - stX) * (enX - stX) + (enY - stY) * (enY - stY));

            double val = dist / length;
            double xn = (byte)(val * end.normalVector.X + (1 - val) * start.normalVector.X);
            double yn = (byte)(val * end.normalVector.Y + (1 - val) * start.normalVector.Y);
            double zn = (byte)(val * end.normalVector.Z + (1 - val) * start.normalVector.Z);
            normal = new CustomVector(new double[4] { xn, yn, zn, 0 }, 4).Normalized();
        }

        public void SetPosition(int curY)
        {
            int stX, stY, enX, enY;
            stX = (int)start.screenPosition.X;
            stY = (int)start.screenPosition.Y;

            enX = (int)end.screenPosition.X;
            enY = (int)end.screenPosition.Y;

            double dist = Math.Sqrt(
                (x - stX) * (x - stX) + (curY - stY) * (curY - stY));
            double length = Math.Sqrt(
                (enX - stX) * (enX - stX) + (enY - stY) * (enY - stY));

            double val = dist / length;
            double xn = (byte)(val * end.position.X + (1 - val) * start.position.X);
            double yn = (byte)(val * end.position.Y + (1 - val) * start.position.Y);
            double zn = (byte)(val * end.position.Z + (1 - val) * start.position.Z);
            interpolatedPosition = new Position(xn, yn, zn);
        }
    }
}
