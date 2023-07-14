using GrafikaProjekt4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKtoolkit
{
    public interface IObject3D : IDraw
    {
        BitmapArray bitmapArray { get; }
        IModel3D model { get; }
        // Center point in the 3D space
        Position centerPoint { get; set; }
         ICamera currentCamera { get; set; }
        // 3D space vectors
        CustomVector[] baseVectors { get; set; }
        CustomMatrix transformation { get; set; }
        List<Vertice> vertices { get; }
        void Transform();
        void SetBitmapArrayTo(BitmapArray bitmapArray);
        void Project();
        void SubscribeToCameraChange(AppViewModel model);
        void SetTransformation(MotionTransformation transformation);
    }
}
