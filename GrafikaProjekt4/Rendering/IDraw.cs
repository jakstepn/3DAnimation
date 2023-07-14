using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GKtoolkit
{
    public interface IDraw
    {
        void DrawOn(Action<int, int> putPixelAt);
    }
}
