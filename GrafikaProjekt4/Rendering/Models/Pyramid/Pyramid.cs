using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GKtoolkit
{
    class Pyramid : AModel3D
    {
        public Pyramid() : base()
        {

        }

        public override void InitModel()
        {
            SetTrianglePoints();
            CreateTriangles();
        }

        protected override void CreateTriangles()
        {
            triangles.Add(new Triangle(
                new List<Vertice>() { new Vertice(vertices[0]),
                new Vertice(vertices[1]), new Vertice(vertices[2]) }, this, Colors.Red));
            triangles.Add(new Triangle(
                new List<Vertice>() { new Vertice(vertices[2]),
                new Vertice(vertices[1]), new Vertice(vertices[3]) }, this, Colors.Blue));
            triangles.Add(new Triangle(
                new List<Vertice>() { new Vertice(vertices[3]),
                new Vertice(vertices[1]), new Vertice(vertices[4]) }, this, Colors.Yellow));
            triangles.Add(new Triangle(
                new List<Vertice>() { new Vertice(vertices[4]),
                new Vertice(vertices[1]), new Vertice(vertices[0]) }, this, Colors.Green));

            // Bottom
            triangles.Add(new Triangle(
                new List<Vertice>() { new Vertice(vertices[4]),
                new Vertice(vertices[0]), new Vertice(vertices[2]) }, this, Colors.Purple));
            triangles.Add(new Triangle(
                new List<Vertice>() { new Vertice(vertices[2]),
                new Vertice(vertices[3]), new Vertice(vertices[4]) }, this, Colors.Orange));
        }

        protected override void SetTrianglePoints()
        {
            // bottom left # 0
            vertices.Add(new Vertice(new Position(-baseDimensions.size/2, 0, baseDimensions.size/2), currentCamera, this));
            // top # 1
            vertices.Add(new Vertice(new Position(0, baseDimensions.size / 2, 0), currentCamera, this));
            // bottom right # 2
            vertices.Add(new Vertice(new Position(baseDimensions.size/2, 0, baseDimensions.size/2), currentCamera, this));
            // bottom right back # 3
            vertices.Add(new Vertice(new Position(baseDimensions.size/2, 0, -baseDimensions.size/2), currentCamera, this));
            // bottom right back # 4
            vertices.Add(new Vertice(new Position(-baseDimensions.size/2, 0, -baseDimensions.size/2), currentCamera, this));
        }

        public override void Fill(BitmapArray bitmapArray, List<LightSource> lights)
        {
            foreach (Triangle triangle in triangles)
            {
                triangle.Fill(bitmapArray, new ColorValue(triangle.color), lights,
                    currentCamera.shading, currentCamera.ka, currentCamera.kd, currentCamera.ks);
            }
        }

        public override void SetNormalVectorsInVertices()
        {
            Parallel.For(0, triangles.Count, (i) =>
            {
                foreach (Vertice vertice in triangles[i].vertices)
                {
                    vertice.normalVector = triangles[i].normalVector;
                }
            });
        }
    }
}
