using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GKtoolkit
{
    class SimpleCube : AModel3D
    {
        public List<Face> faces { get; }
        private CubeDimensions dimensions;

        public SimpleCube() : base()
        {
            faces = new List<Face>();
        }

        public SimpleCube(CubeDimensions dimensions) : base()
        {
            faces = new List<Face>();
            this.dimensions = dimensions;
        }

        public override void InitModel()
        {
            SetTrianglePoints();
            AddFaces();
            CreateTriangles();
        }

        protected override void CreateTriangles()
        {
            int i = 0;
            foreach(Face face in faces)
            {
                triangles.AddRange(face.GetTriangles(this, GetColor(i)));
                i++;
            }
        }

        public override void Fill(BitmapArray bitmapArray, List<LightSource> lights)
        {
            int i = 0;
            foreach (Face face in faces)
            {
                face.Fill(bitmapArray, lights,
                     currentCamera.ka ,currentCamera.kd, currentCamera.ks, currentCamera.shading);
                i++;
            }
        }

        private Color GetColor(int id)
        {
            switch (id)
            {
                case 0:
                    return Colors.Blue;
                case 1:
                    return Colors.Yellow;
                case 2:
                    return Colors.Orange;
                case 3:
                    return Colors.Brown;
                case 4:
                    return Colors.Aqua;
                case 5:
                    return Colors.Green;
                case 6:
                    return Colors.Red;
                default:
                    return Colors.Turquoise;
            }
        }

        protected override void SetTrianglePoints()
        {
            double size = baseDimensions.size;

            // Front
            vertices.Add(new Vertice(new Position(0, 0, 0), currentCamera, this));
            vertices.Add(new Vertice(new Position(0, size, 0), currentCamera, this));
            vertices.Add(new Vertice(new Position(size, size, 0), currentCamera, this));
            vertices.Add(new Vertice(new Position(size, 0, 0), currentCamera, this));

            // Back
            vertices.Add(new Vertice(new Position(0, 0, size), currentCamera, this));
            vertices.Add(new Vertice(new Position(0, size, size), currentCamera, this));
            vertices.Add(new Vertice(new Position(size, size, size), currentCamera, this));
            vertices.Add(new Vertice(new Position(size, 0, size), currentCamera, this));
        }

        public override void SetDimensions(BaseDimensions dimensions)
        {
            base.SetDimensions(dimensions);
            this.dimensions = new CubeDimensions(dimensions.size, dimensions.size,
                dimensions.distanceX, dimensions.distanceY);
        }

        private void AddFaces()
        {
            Face front, back, top, left, right, bottom;

            List<Vertice> frontFaceVertices = GetVertices(0);
            front = new Face(frontFaceVertices, dimensions.faceDimensions);

            List<Vertice> leftFaceVertices = GetVertices(1);
            left = new Face(leftFaceVertices, dimensions.faceDimensions);

            List<Vertice> rightFaceVertices = GetVertices(2);
            right = new Face(rightFaceVertices, dimensions.faceDimensions);

            List<Vertice> backFaceVertices = GetVertices(3);
            back = new Face(backFaceVertices, dimensions.faceDimensions);

            List<Vertice> topFaceVertices = GetVertices(4);
            top = new Face(topFaceVertices, dimensions.faceDimensions);

            List<Vertice> bottomFaceVertices = GetVertices(5);
            bottom = new Face(bottomFaceVertices, dimensions.faceDimensions);

            faces.Add(front);
            faces.Add(left);
            faces.Add(right);
            faces.Add(back);
            faces.Add(top);
            faces.Add(bottom);
        }

        private List<Vertice> GetVertices(int faceNum)
        {
            switch (faceNum)
            {
                case 0:
                    // Front
                    return new List<Vertice> { new Vertice(vertices[0]),
                        new Vertice(vertices[1]), new Vertice(vertices[3]), new Vertice(vertices[2]) };
                case 1:
                    // Left
                    return new List<Vertice> { new Vertice(vertices[4]),
                        new Vertice(vertices[5]), new Vertice(vertices[0]), new Vertice(vertices[1]) };
                case 2:
                    // Right
                    return new List<Vertice> { new Vertice(vertices[3]),
                        new Vertice(vertices[2]), new Vertice(vertices[7]), new Vertice(vertices[6]) };
                case 3:
                    // Back
                    return new List<Vertice> { new Vertice(vertices[7]),
                        new Vertice(vertices[6]), new Vertice(vertices[4]), new Vertice(vertices[5]) };
                case 4:
                    // Top
                    return new List<Vertice> { new Vertice(vertices[1]),
                        new Vertice(vertices[5]), new Vertice(vertices[2]), new Vertice(vertices[6]) };
                case 5:
                    // Bottom
                    return new List<Vertice> { new Vertice(vertices[7]),
                        new Vertice(vertices[4]), new Vertice(vertices[3]), new Vertice(vertices[0]) };
                default:
                    return null;
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
