using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKtoolkit
{
    public interface IDimensions
    {
        void SetDimensions(BaseDimensions dimensions);
        BaseDimensions GetDimensions();
    }
}
