using GKtoolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace GKtoolkit
{
    public class Position : CustomVector
    {
        public Position(double[] position) : base(position, 4) { }

        public Position(int x, int y, int z, int w = 1) : base(new double[4] { x, y, z, w }, 4) { }
        public Position(CustomVector v, int w = 1) : base(new double[4] { v.X, v.Y, v.Z, w }, 4) { }
        public Position(CustomMatrix m, int w = 1) : base(new double[4] 
        { m.Get(0,0), m.Get(1, 0), m.Get(2, 0), w }, 4) { }

        public Position(double x, double y, double z, double w = 1.0) : base(new double[4] { x, y, z, w }, 4) { }

        public Position(double[,] array, MatrixSize size) : base(array, size) { }

        public Position(CustomVector position) : base(new double[4] {
            position.Get(0, 0), position.Get(1, 0), position.Get(2, 0), position.Get(3,0) }, 4)
        { }

        public Position(Vector3D position) : base(new double[4] {
            position.X, position.Y, position.Z, 1 }, 4)
        { }

        public Position() : base() { }

        public static Position operator *(CustomMatrix multiplied, Position multiplier)
        {
            CustomVector newVector = new CustomVector(new double[4] { multiplier.X, multiplier.Y, multiplier.Z, multiplier.W}, 4);
            CustomVector newPos = multiplied * newVector;
                return new Position(newPos.Get(0,0), newPos.Get(1,0), newPos.Get(2,0), newPos.Get(3, 0));
        }

        public static Position operator *(double scalar, Position multiplied)
        {
            CustomVector newVector = new CustomVector(new double[4] { multiplied.X, multiplied.Y, multiplied.Z, multiplied.W }, 4);
            CustomMatrix newPos = scalar * newVector;
            return new Position(newPos.Get(0, 0), newPos.Get(1, 0), newPos.Get(2, 0), newPos.Get(3, 0));
        }

        public static Position operator -(Position value, Position subtracted)
        {
            
            return new Position(value.X - subtracted.X, value.Y - subtracted.Y, value.Z - subtracted.Z);
        }

        public static Position operator +(Position value, Position added)
        {
            return new Position(value.X + added.X, value.Y + added.Y, value.Z + added.Z);
        }

        public static Position operator +(Position value, CustomVector added)
        {
            double[,] valueMatrix = value.matrix;
            double[,] subtractedMatrix = added.AsDoubleArray();
            double[,] resultMatrix = new double[value.size.rows, value.size.columns];

            for (int i = 0; i < value.size.rows; i++)
            {
                for (int j = 0; j < value.size.columns; j++)
                {
                    resultMatrix[i, j] = valueMatrix[i, j] + subtractedMatrix[i, j];
                }
            }

            return new Position(resultMatrix, new MatrixSize(value.size.rows, value.size.columns));
        }

        public double DistanceBetween(Position p)
        {
            return Math.Sqrt((p.X - X)* (p.X - X) + (p.Y - Y)* (p.Y - Y) + (p.Z - Z)* (p.Z - Z));
        }
    }
}
