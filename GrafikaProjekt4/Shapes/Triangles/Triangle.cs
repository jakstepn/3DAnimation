using GKtoolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GKtoolkit
{
    public class Triangle : IDraw
    {
        public List<Vertice> vertices { get; }
        public List<Line> lines { get; }
        public bool shouldDraw { get; set; }
        public Color color { get; set; }

        private IObject3D parent;
        public CustomVector normalVector
        {
            get
            {
                Vertice v1, v2, v0;
                v0 = vertices[0];
                v1 = vertices[1];
                v2 = vertices[2];
                CustomVector vectorv0Tov2, vectorv0Tov1;
                vectorv0Tov1 = new CustomVector(v1.position, v0.position).Normalized();
                vectorv0Tov2 = new CustomVector(v2.position, v0.position).Normalized();
                return (vectorv0Tov2 * vectorv0Tov1).Normalized();
            }
        }

        public Triangle(List<Vertice> vertices, IObject3D parentModel, Color? color = null)
        {
            parent = parentModel;
            this.color = color ?? Colors.Red;
            this.vertices = vertices;
            lines = new List<Line>();
            shouldDraw = true;
        }
        public void Fill(BitmapArray bitmapArray, IBitmap source, List<LightSource> lights, int shading,
            double ka, double kd, double ks)
        {
            if (shouldDraw)
            {
                switch (shading)
                {
                    // Constant
                    case 0:
                        Color newColor = Shading.Intensity(ks, kd, ka, 5, vertices[0].position,
                        normalVector, lights, parent.currentCamera, color);
                        ScanLinia.Fill(this, bitmapArray, source, (pixelColor, vec, pos) =>
                        {
                            return newColor;
                        });
                        break;
                    // Gouraud
                    case 1:
                        ColorValue[] newColorArr = new ColorValue[3];

                        vertices[0].color = Shading.Intensity(ks, kd, ka, 5, vertices[0].position,
                        normalVector, lights, parent.currentCamera, color);

                        vertices[1].color = Shading.Intensity(ks, kd, ka, 5, vertices[1].position,
                        normalVector, lights, parent.currentCamera, color);

                        vertices[2].color = Shading.Intensity(ks, kd, ka, 5, vertices[2].position,
                        normalVector, lights, parent.currentCamera, color);

                        ScanLinia.Fill(this, bitmapArray, source, (pixelColor, vec, pos) =>
                        {
                            return pixelColor ?? Colors.Red;
                        }, true);
                        break;
                    // Phong
                    case 2:
                        ScanLinia.Fill(this, bitmapArray, source, (pixelColor, vec, pos) =>
                        {
                            return Shading.Intensity(ks, kd, ka, 5, pos,
                        vec, lights, parent.currentCamera, color);
                        }, phong: true);
                        break;
                    // None
                    default:
                        ScanLinia.Fill(this, bitmapArray, source, (pixelColor, vec, pos) =>
                        {
                            return color;
                        });
                        break;
                }
            }
        }
        public void HighlightVertices(BitmapArray bitmapArray)
        {
            bitmapArray.SetSquare((int)vertices[0].screenPosition.Y,
                (int)vertices[0].screenPosition.X, 5, color);
        }
        public void DrawOn(Action<int, int> putPixelAt)
        {
            if (shouldDraw)
            {
                Line l1, l2, l3;
                lines.Clear();
                l1 = new Line(vertices[0], vertices[1]);
                l1.DrawOn(putPixelAt);
                lines.Add(l1);

                l2 = new Line(vertices[1], vertices[2]);
                l2.DrawOn(putPixelAt);
                lines.Add(l2);

                l3 = new Line(vertices[2], vertices[0]);
                l3.DrawOn(putPixelAt);
                lines.Add(l3);
            }
        }
    }
}
