using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKtoolkit
{
    public class SphereSize : ModelSize
    {
        public double d1 { get; }
        public double d2 { get; }
        public double radius { get; }
        public double angleHorizontal { get; }
        public double angleVertical { get; }

        public SphereSize(double radius, double d1, double d2) : 
            base((int)(Math.PI / d1), (int)(2 * Math.PI / d2))
        {
            this.d1 = d1;
            this.d2 = d2;
            this.radius = radius;
            angleHorizontal = 2 * Math.PI;
            angleVertical = Math.PI;
        }
    }
}
