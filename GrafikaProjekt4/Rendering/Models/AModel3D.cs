using GrafikaProjekt4;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GKtoolkit
{
    public abstract class AModel3D : IModel3D
    {
        protected BaseDimensions baseDimensions;
        public ICamera currentCamera { get; set; }

        public List<Triangle> triangles { get; }

        public List<Vertice> vertices { get; }

        //3D space vectors
        public CustomVector[] baseVectors { get; set; }
        // Center point in the 3D space
        public Position centerPoint { get; set; }
         
        public BitmapArray bitmapArray { get; set; }

        public IModel3D model => this;

        public CustomMatrix transformation { get; set; }

        protected AModel3D(BitmapArray bitmapArray)
        {
            triangles = new List<Triangle>();
            vertices = new List<Vertice>();
            this.bitmapArray = bitmapArray;
            SetBaseVectorsAndCenterPoint();
        }

        protected AModel3D()
        {
            triangles = new List<Triangle>();
            vertices = new List<Vertice>();
            SetBaseVectorsAndCenterPoint();
        }

        private void SetBaseVectorsAndCenterPoint()
        {
            centerPoint = new Position(0, 0, 0);

            baseVectors = new CustomVector[3];
            CustomVector xhat = new CustomVector(new Position(1, 0, 0), centerPoint).Normalized();
            CustomVector yhat = new CustomVector(new Position(0, 1, 0), centerPoint).Normalized();
            CustomVector zhat = new CustomVector(new Position(0, 0, 1), centerPoint).Normalized();
            baseVectors[0] = xhat;
            baseVectors[1] = yhat;
            baseVectors[2] = zhat;
        }

        public void SetBitmapArrayTo(BitmapArray bitmapArray)
        {
            this.bitmapArray = bitmapArray;
        }

        public void SetCameraReference(ICamera camera)
        {
            currentCamera = camera;
        }
        public void CheckWhatTrianglesToDraw(ICamera camera)
        {
            Parallel.For(0, triangles.Count, (i) =>
            {
                Position pos = triangles[i].vertices[0].position;
                CustomVector vec = new CustomVector(new double[4] {
                    pos.X - camera.position.X,
                    pos.Y - camera.position.Y,
                    pos.Z - camera.position.Z, 0 }, 4);

                // Dot product between normal and a vector
                triangles[i].shouldDraw = (
                    triangles[i].normalVector | vec.Normalized()) >= 0 ?
                    false : true;
            });
        }

        public BitmapArray GetArray(BitmapSize bitmapSize)
        {
            BitmapArray array = new BitmapArray(bitmapSize.length,
                bitmapSize.stride, bitmapSize.valuesPerPixel);
            array.Clean(Colors.White);

            return array;
        }

        public virtual void DrawOn(Action<int, int> putPixelAt)
        {
            foreach (Triangle triangle in triangles)
            {
                triangle.DrawOn(putPixelAt);
            }
        }

        public void SetupVerticesAndTriangles(Position position = null)
        {
            SetTrianglePoints();
            CreateTriangles();
        }

        public virtual void Transform()
        {

            for (int i = 0; i < 3; i++)
            {
                baseVectors[i] = (transformation * baseVectors[i]).Normalized();
            }
            centerPoint = transformation * centerPoint;
        }

        public void Project()
        {
            Parallel.For(0, triangles.Count, (i) => {
                foreach (Vertice vertice in triangles[i].vertices)
                {
                    vertice.Project();
                }
            });
        }

        protected Triangle CreateTriangle(Vertice point1, Vertice point2, Vertice point3, Color? c = null)
        {
            return new Triangle(new List<Vertice> { point1, point2, point3 }, this, c);
        }

        /// <summary>
        /// Sets points on the triangles
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        protected abstract void SetTrianglePoints();

        protected abstract void CreateTriangles();

        public abstract void InitModel();

        public abstract void SetNormalVectorsInVertices();

        public virtual void Fill(BitmapArray bitmapArray, List<LightSource> lights)
        {
            ColorValue color = new ColorValue(Colors.Blue);
            foreach (Triangle triangle in triangles)
            {
                triangle.Fill(bitmapArray, color, lights,
                    currentCamera.shading, currentCamera.ka, currentCamera.kd, currentCamera.ks);
            }
        }

        public void HighlightVertices(BitmapArray bitmapArray)
        {
            foreach (Triangle triangle in triangles)
            {
                triangle.HighlightVertices(bitmapArray);
            }
        }

        public virtual void SetDimensions(BaseDimensions dimensions)
        {
            baseDimensions = dimensions;
        }

        public BaseDimensions GetDimensions()
        {
            return baseDimensions;
        }

        public void SubscribeToCameraChange(AppViewModel model)
        {
            model.CameraChanged += OnCameraChanged;
        }
        private void OnCameraChanged(object sender, CameraChangedEventArgs args)
        {
            currentCamera = args.newCamera;
        }

        public void SetTransformation(MotionTransformation transformation)
        {
            this.transformation = Matrices.Transformation(transformation);
        }
    }
}
