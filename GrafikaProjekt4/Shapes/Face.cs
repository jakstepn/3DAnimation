using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GKtoolkit
{
    class Face
    {
        public List<Vertice> vertices { get; }
        public FaceDimensions dimensions { get; }
        private List<Triangle> triangles { get; }
        public Face(List<Vertice> vertices, FaceDimensions dimensions)
        {
            triangles = new List<Triangle>();
            this.vertices = vertices;
            this.dimensions = dimensions;
        }
        public void Transform(CustomMatrix transfromation)
        {
            foreach(Vertice vertice in vertices)
            {
                vertice.SetPosition(transfromation * vertice.position);
            }
        }

        public List<Triangle> GetTriangles(AModel3D parentModel, Color? color = null)
        {
            List<Triangle> triangles = new List<Triangle>();
            int verticesPerRow, verticesPerColumn;

            verticesPerRow = dimensions.amountOfVerticesRow;
            verticesPerColumn = dimensions.amountOfVerticesColumn;

            for (int i = 0; i < verticesPerColumn - 1; i++)
            {
                for (int j = 0; j < verticesPerRow - 1; j++)
                {
                    Triangle tmp = CreateTriangle(
                        vertices[index(i, j)], vertices[index(i + 1, j + 1)], vertices[index(i, j + 1)], parentModel, color);
                    triangles.Add(tmp);

                    Triangle tmp2 = CreateTriangle(
                        vertices[index(i, j)], vertices[index(i + 1, j)], vertices[index(i + 1, j + 1)], parentModel, color);
                    triangles.Add(tmp2);
                }
            }
            this.triangles.AddRange(triangles);
            return triangles;
        }

        public void Fill(BitmapArray bitmapArray, List<LightSource> lights,
            double kd, double ks, double ka, int shading)
        {
            foreach (Triangle triangle in triangles)
            {
                triangle.Fill(bitmapArray, new ColorValue(triangle.color), lights, shading, ka, kd, ks);
            }
        }

        private int index(int row, int column)
        {
            return column + row * dimensions.amountOfVerticesRow;
        }

        Triangle CreateTriangle(Vertice point1, Vertice point2, Vertice point3, AModel3D parentModel, Color? color)
        {
            return new Triangle(new List<Vertice> { point1, point2, point3 }, parentModel, color);
        }
    }
}
