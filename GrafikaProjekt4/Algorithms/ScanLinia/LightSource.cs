using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;

namespace GKtoolkit
{
    public class LightSource : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Color color { get; set; }

        private Position _screenPos;

        public double X
        {
            get
            {
                return position.X;
            }
            set
            {
                position.X = value;
                OnPropertyChanged(nameof(X));
            }
        }

        public double Y
        {
            get
            {
                return position.Y;
            }
            set
            {
                position.Y = value;
                OnPropertyChanged(nameof(Y));
            }
        }

        public double Z
        {
            get
            {
                return position.Z;
            }
            set
            {
                position.Z = value;
                OnPropertyChanged(nameof(Z));
            }
        }

        public Position position { get; set; }

        public LightSource(double x, double y, double z)
        {
            position = new Position(x,y,z);
            color = Colors.White;
        }

        protected void OnPropertyChanged(string cMode = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(cMode));
        }

        public void SetPosition(double x, double y, double z)
        {
            position = new Position(x, y, z);
        }

        public void SetPosition(Position position)
        {
            this.position = position;
        }

        public double DistanceTo(Position p)
        {
            return Math.Sqrt((p.X - X) * (p.X - X) + (p.Y - Y) * (p.Y - Y) + (p.Z - Z) * (p.Z - Z));
        }
    }
}
