using GrafikaProjekt4;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GKtoolkit
{
    public interface IShape : IDraw
    {
        List<Line> lines { get; }
        List<Vertice> vertices { get; }
    }
}
