using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKtoolkit
{
    public interface ICamera
    {
        Position position { get; }
        Position focusPoint { get; set; }
        CustomVector toCamVector { get; }
        double ka { get; set; }
        double kd { get; set; }
        double ks { get; set; }
        int shading { get; set; }
        CustomVector fromCamVector { get; }
        CustomVector upVector { get; }
        int ID { get; }
        CustomVector VectorFromCamTo(Position target);
        void SetChild(IObject3D child, CustomVector offset = null);
        void Transform(MotionTransformation transformation);
        void TransformFocus(MotionTransformation transformation);
        Position Project(Position pos);
    }
}
