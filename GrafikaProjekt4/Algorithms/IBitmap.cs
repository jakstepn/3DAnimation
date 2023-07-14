using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GKtoolkit
{
    public interface IBitmap
    {
        (byte r, byte g, byte b, byte a) GetColorValue(int x, int y);
    }
}
