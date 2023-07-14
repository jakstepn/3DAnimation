using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKtoolkit
{
    public class Scale
    {
        private double _xAxis;
        private double _yAxis;
        private double _zAxis;
        public double xAxis
        {
            get
            { return _xAxis; }
        }
        public double yAxis
        {
            get
            { return _yAxis; }
        }
        public double zAxis
        {
            get
            { return _zAxis; }
        }

        public Scale(double xAxis = 1, double yAxis = 1, double zAxis = 1)
        {
            _xAxis = xAxis;
            _yAxis = yAxis;
            _zAxis = zAxis;
        }
    }
}
