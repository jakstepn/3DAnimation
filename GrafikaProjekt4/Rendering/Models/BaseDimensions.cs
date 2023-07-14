using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKtoolkit
{
    public class BaseDimensions : IDimensions
    {
        private double _s;
        private double _x;
        private double _y;
        public double size => _s;

        // Distance between vertices on a given axis
        public double distanceX => _x;
        public double distanceY => _y;

        public BaseDimensions(double size,
            double distanceBetweenVerticesX,
            double distanceBetweenVerticesY)
        {
            _s = size;
            _x = distanceBetweenVerticesX;
            _y = distanceBetweenVerticesY;
        }

        public BaseDimensions GetDimensions()
        {
            return this;
        }

        public void SetDimensions(BaseDimensions dimensions)
        {
            _s = dimensions.size;
            _x = dimensions.distanceX;
            _y = dimensions.distanceY;
        }
    }
}
