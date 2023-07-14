using GrafikaProjekt4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKtoolkit
{
    class EmptyModel : IModel3D
    {
        private IObject3D baseObject;
        public List<Triangle> triangles { get; }
        public List<Vertice> vertices => baseObject.vertices;
        public BitmapArray bitmapArray => baseObject.bitmapArray;
        public IModel3D model => this;

        public Position centerPoint {
            get
            {
                return baseObject.centerPoint;
            }
            set
            {
                baseObject.centerPoint = value;
            }
        }
        public CustomVector[] baseVectors 
        {
            get
            {
                return baseObject.baseVectors;
            }
            set
            {
                baseObject.baseVectors = value;
            }
        }

        public ICamera currentCamera 
        {
            get 
            {
                return baseObject.currentCamera;
            }
            set
            {
                baseObject.currentCamera = value;
            }
        }

        public CustomMatrix transformation { get; set; }

        public EmptyModel(IObject3D baseObject = null) 
        {
            this.baseObject = baseObject;
            triangles = new List<Triangle>();
        }

        public void CheckWhatTrianglesToDraw(ICamera camera)
        {
            return;
        }

        public void ClipToCameraView(ICamera camera)
        {
            return;
        }

        public void DrawOn(Action<int, int> putPixelAt)
        {
            baseObject.DrawOn(putPixelAt);
        }

        public void Fill(BitmapArray bitmapArray, List<LightSource> lights)
        {
            return;
        }

        public BaseDimensions GetDimensions()
        {
            return new BaseDimensions(0, 0, 0);
        }

        public void HighlightVertices(BitmapArray bitmapArray)
        {
            return;
        }

        public void SetBitmapArrayTo(BitmapArray bitmapArray)
        {
            baseObject.SetBitmapArrayTo(bitmapArray);
        }

        public void SetDimensions(BaseDimensions dimensions)
        {
            return;
        }

        public void Transform()
        {
            baseObject.Transform();
        }

        public void Project()
        {
            baseObject.Project();
        }

        public void SubscribeToCameraChange(AppViewModel model)
        {
            baseObject.SubscribeToCameraChange(model);
        }

        public void SetTransformation(MotionTransformation transformation)
        {
            baseObject.SetTransformation(transformation);
        }

        public void SetNormalVectorsInVertices()
        {
            return;
        }
    }
}
