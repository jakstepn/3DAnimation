using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKtoolkit
{
    public class CameraChangedEventArgs : EventArgs
    {
        public Camera newCamera { get; set; }
    }
}
