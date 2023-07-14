using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKtoolkit
{
    public class MotionTransformation
    {
        public Angle angle { get; }
        public Translation translation { get; }
        public Position centerPoint { get; }
        public Scale scale { get; }

        public MotionTransformation(Angle angle = null, Translation translation = null, Scale scale = null, Position centerPoint = null)
        {
            this.angle = angle ?? new Angle();
            this.translation = translation ?? new Translation();
            this.centerPoint = centerPoint ?? new Position(0,0,0);
            this.scale = scale ?? new Scale();
        }
    }
}
