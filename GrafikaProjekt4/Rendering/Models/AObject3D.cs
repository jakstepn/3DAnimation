using GrafikaProjekt4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKtoolkit
{
    public abstract class AObject3D : IObject3D
    {
        private BitmapArray _bitmapArray;
        public ICamera currentCamera { get; set; }
        public List<Vertice> vertices { get; }
        public CustomVector[] baseVectors { get; set; }
        public IModel3D model => new EmptyModel(this);
        public BitmapArray bitmapArray => _bitmapArray;
        public Position centerPoint { get; set; }
        public CustomMatrix transformation { get; set; }

        public AObject3D(BitmapArray bitmapArray, ICamera camera)
        {
            this.currentCamera = camera;
            vertices = new List<Vertice>();
            baseVectors = new CustomVector[3];
        }

        public void SetBitmapArrayTo(BitmapArray bitmapArray)
        {
            _bitmapArray = bitmapArray;
        }

        public abstract void Transform();
        public abstract void DrawOn(Action<int, int> putPixelAt);

        public void Project()
        {
            Parallel.For(0, vertices.Count, (i) => vertices[i].Project());
            centerPoint = currentCamera.Project(centerPoint);
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
