using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKtoolkit
{
    public class ModelSize
    {
        public int pointsHorizontally { get; }
        public int pointsVertically { get; }

        public ModelSize(int pointsVertically, int pointsHorizontally)
        {
            this.pointsVertically = pointsVertically;
            this.pointsHorizontally = pointsHorizontally;
        }
    }
}
