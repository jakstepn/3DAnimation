using GrafikaProjekt4;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace GKtoolkit
{
    public class Line : IShape
    { 
        private Vertice v1
        {
            get
            {
                return vertices[0];
            }
        }

        private Vertice v2
        {
            get
            {
                return vertices[1];
            }
        }

        public List<Line> lines { get; }

        public List<Vertice> vertices { get; }

        public Line(Vertice srcVertice, Vertice termVertice)
        {
            lines = new List<Line>();
            vertices = new List<Vertice>();

            vertices.Add(srcVertice);
            vertices.Add(termVertice);
            lines.Add(this);
        }

        public void DrawOn(Action<int, int> putPixelAt)
        {
            Bresenham.DrawLine((int)v1.screenPosition.X,
                (int)v2.screenPosition.X,
                (int)v1.screenPosition.Y,
                (int)v2.screenPosition.Y, putPixelAt);
        }

        public void DrawOnUsingModelSpace(Action<int, int> putPixelAt)
        {
            Bresenham.DrawLine((int)v1.position.X,
                (int)v2.position.X,
                (int)v1.position.Y,
                (int)v2.position.Y, putPixelAt);
        }

        public double Screendx()
        {
            return vertices[1].screenPosition.X - vertices[0].screenPosition.X;
        }

        public double Screendy()
        {
            return vertices[1].screenPosition.Y - vertices[0].screenPosition.Y;
        }
    }
}
