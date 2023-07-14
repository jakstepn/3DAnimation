using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKtoolkit
{
    public interface IModel3D : IObject3D
    {
        List<Triangle> triangles { get; }
        void CheckWhatTrianglesToDraw(ICamera camera);
        void Fill(BitmapArray bitmapArray, List<LightSource> lights);
        void HighlightVertices(BitmapArray bitmapArray);
        void SetDimensions(BaseDimensions dimensions);
        void SetNormalVectorsInVertices();
        BaseDimensions GetDimensions();
    }
}
