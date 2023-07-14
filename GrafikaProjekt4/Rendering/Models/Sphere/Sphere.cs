using GKtoolkit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GKtoolkit
{
    public class Sphere : AModel3D
    {
        private SphereSize size;
        public Position position;

        public Sphere() : base()
        {
        }

        public override void DrawOn(Action<int, int> putPixelAt)
        {
            base.DrawOn(putPixelAt);
        }

        /// <summary>
        /// Sets points on the triangles
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        protected override void SetTrianglePoints()
        {
            double x, y, z;

            for (double i = size.angleVertical; i >= -2*size.d1; i -= size.d1)
            {
                for (double j = size.angleHorizontal; j >= -size.d2; j -= size.d2)
                {
                    x = GetXParameter(i, j);
                    y = GetYParameter(i);
                    z = GetZParameter(i, j);
                    vertices.Add(new Vertice(new Position(x, y, z), currentCamera, this));
                }
            }
        }

        protected override void CreateTriangles()
        {
            for (int i = 0; i < size.pointsVertically + 1; i++)
            {
                for (int j = 0; j < size.pointsHorizontally; j++)
                {
                    Triangle tmp = CreateTriangle(
                        vertices[index(i + 1, j)], vertices[index(i, j + 1)], vertices[index(i, j)], Colors.Orange);
                    triangles.Add(tmp);

                    Triangle tmp2 = CreateTriangle(
                        vertices[index(i + 1, j)], vertices[index(i + 1, j + 1)], vertices[index(i, j + 1)], Colors.Orange);
                    triangles.Add(tmp2);
                }
            }
        }

        protected int index(int row, int column)
        {
            return column + row  * size.pointsHorizontally;
        }

        private double GetXParameter(double angle1, double angle2)
        {
            return size.radius * Math.Sin(angle1) * Math.Cos(angle2);
        }

        private double GetYParameter(double angle1)
        {
            return size.radius * Math.Cos(angle1);
        }

        private double GetZParameter(double angle1, double angle2)
        {
            return size.radius * Math.Sin(angle1) * Math.Sin(angle2);
        }

        public override void InitModel()
        {
            SetupVerticesAndTriangles();
        }
        public override void SetDimensions(BaseDimensions dimensions)
        {
            size = new SphereSize(dimensions.size,
                dimensions.distanceX, dimensions.distanceY);
        }
        public override void Fill(BitmapArray bitmapArray, List<LightSource> lights)
        {
            Parallel.For(0,triangles.Count, (i) => {
                triangles[i].Fill(bitmapArray, new ColorValue(triangles[i].color),
                    lights, currentCamera.shading, currentCamera.ka, currentCamera.kd, currentCamera.ks);
            });
        }

        public override void SetNormalVectorsInVertices()
        {
            Parallel.For(0, vertices.Count, (i) =>
            {
                vertices[i].normalVector = new CustomVector(vertices[i].position, centerPoint).Normalized();
            });
        }
    }
}
