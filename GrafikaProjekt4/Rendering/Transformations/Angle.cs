using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKtoolkit
{
    public class Angle
    {
        private double _xAngle;
        private double _yAngle;
        private double _zAngle;
        public double xAngle
        {
            get
            { return _xAngle % (Math.PI * 2); }
        }
        public double yAngle
        {
            get
            { return _yAngle % (Math.PI * 2); }
        }
        public double zAngle
        {
            get
            { return _zAngle % (Math.PI * 2); }
        }

        public Angle(double xAngle = 0, double yAngle = 0, double zAngle = 0)
        {
            _xAngle = xAngle;
            _yAngle = yAngle;
            _zAngle = zAngle;
        }
    }
}
