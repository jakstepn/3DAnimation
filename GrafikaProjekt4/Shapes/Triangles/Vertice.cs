using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace GKtoolkit
{
    public class Vertice
    {
        private Position _position;
        private Position _screenPosition;
        private IObject3D parentModel;

        public Position position
        {
            get
            {
                CustomMatrix baseCoordinates = _position.X * parentModel.baseVectors[0] +
                    _position.Y * parentModel.baseVectors[1] +
                    _position.Z * parentModel.baseVectors[2] + parentModel.centerPoint;

                return new Position(baseCoordinates.Get(0, 0),
                    baseCoordinates.Get(1, 0),
                    baseCoordinates.Get(2, 0));
            }
        }
        public Position screenPosition
        {
            get
            {
                return _screenPosition;
            }
        }

        public CustomVector normalVector {get;set;}

        public Color color { get; set; }

        public Vertice(Position position, ICamera currentCamera, IObject3D parent)
        {
            _position = position;
            parentModel = parent;
        }

        public Vertice(Point3D position, ICamera currentCamera, IObject3D parent)
        {
            _position = new Position(
                new double[4] { position.X, position.Y, position.Z, 1});
            parentModel = parent;
        }

        public Vertice(Vertice vertice)
        {
            _position = vertice._position;
            parentModel = vertice.parentModel;
        }

        public void SetPosition(Position newPosition)
        {
            _position = newPosition;
            Project();
        }

        public void Transform()
        {
            Project();
        }

        public void SetScreenPosition(double x, double y, double z)
        {
            _screenPosition = new Position(x, y, z);
        }

        public void Project()
        {
            _screenPosition = parentModel.currentCamera.Project(position);
        }
    }
}
