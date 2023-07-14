using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKtoolkit
{
    public class Translation
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

        public Translation(double xAxis = 0, double yAxis = 0, double zAxis = 0)
        {
            _xAxis = xAxis;
            _yAxis = yAxis;
            _zAxis = zAxis;
        }
    }
}
