using System;

namespace GKtoolkit
{
    public class Camera : ICamera
    {
        private Position _position;
        private Position _focus;
        private IObject3D _child;
        private CustomVector _up;
        private CustomVector _offset;
        private IObject3D _focusAt;

        public int shading { get; set; }
        public double ka { get; set; }
        public double kd { get; set; }
        public double ks { get; set; }

        private CustomVector toFocusVec;

        public Position position
        {
            get
            {
                if (_child != null)
                {
                    Position newpos =  new Position(
                    _child.centerPoint + _offset);
                    _focus = newpos + toFocusVec;
                    return newpos;
                }
                else
                {
                   return _position;
                }
            }
        }

        public Position focusPoint
        {
            get
            {
                if (_focusAt != null)
                {
                    return _focusAt.centerPoint;
                }
                else
                {
                    return _focus;
                }
            }
            set
            {
                _focus = value;
            }
        }

        public CustomVector toCamVector
        {
            get
            {
                return (position - focusPoint).Normalized();
            }
        }

        public CustomVector fromCamVector
        {
            get
            {
                return (focusPoint - position).Normalized();
            }
        }

        public CustomVector upVector
        {
            get
            {
                return _up.Normalized();
            }
        }

        public int ID { get; }

        public Camera(int id, IObject3D child = null, CustomVector offset = null,
            Position cameraPosition = null, Position cameraFocusPoint = null)
        {
            shading = -1;
            ka = 0;
            kd = 0;
            ks = 0;
            ID = id;
            _offset = offset;
            _child = child;
            if (child == null)
            {
                _position = cameraPosition ?? new Position(0, 0, 100);
            }
            else
            {
                if (offset != null)
                {
                    _position = child.centerPoint + offset;
                }
                else
                {
                    _position = child.centerPoint;
                }
            }
            _focus = cameraFocusPoint ?? new Position(0, 0, 0);
            _up = new CustomVector(new double[4] { 0, 1, 0, 0 }, 4);
        }

        public CustomVector VectorFromCamTo(Position target)
        {
            return target - position;
        }
        public void SetChild(IObject3D child, CustomVector offset = null)
        {
            _child = child;
            _offset = offset;
            if (offset != null)
            {
                toFocusVec = new CustomVector(focusPoint, child.centerPoint + offset);
                _position = child.centerPoint + offset;
            }
            else
            {
                _position = child.centerPoint;
            }
        }

        public void SetFocus(IObject3D object3d)
        {
            _focusAt = object3d;
        }

        public void Transform(MotionTransformation transformation)
        {
            _position = Matrices.Transformation(transformation) * position;
            TransformFocus(transformation);
        }

        public void TransformFocus(MotionTransformation transformation)
        {
            focusPoint = Matrices.Transformation(transformation) * focusPoint;
        }

        public Position Project(Position pos)
        {
            Position newPos = Matrices.PV(this) * pos;
            newPos = (1.0 / newPos.W) * newPos;
            newPos.W = 1;

            newPos.X = ScreenSize.screenX * (1 + newPos.X) / 2.0;
            newPos.Y = ScreenSize.screenY * (1 + newPos.Y) / 2.0;
            newPos.Z = (newPos.Z + 1) / 2.0;

            return newPos;
        }
    }
}
