using System;
using System.Windows;
using System.Windows.Media.Media3D;

namespace GKtoolkit
{
    public class CustomVector : CustomMatrix
    {
        public double X
        {
            get
            {
                return Get(0, 0);
            }
            set
            {
                Set(0, 0, value);
            }
        }

        public double Y
        {
            get
            {
                return Get(1, 0);
            }
            set
            {
                Set(1, 0, value);
            }
        }

        public double Z
        {
            get
            {
                return Get(2, 0);
            }
            set
            {
                Set(2, 0, value);
            }
        }

        public double W
        {
            get
            {
                return Get(3, 0);
            }
            set
            {
                Set(3, 0, value);
            }
        }

        public CustomVector(int height, int width) : base(height, width) { }
        public CustomVector(int[,] array, MatrixSize matrixSize) : base(array, matrixSize) { }
        public CustomVector(float[,] array, MatrixSize matrixSize) : base(array, matrixSize) { }
        public CustomVector(double[,] array, MatrixSize matrixSize) : base(array, matrixSize) { }

        public CustomVector(int rows = 3)
        {
            size = new MatrixSize(rows, 1);
            matrix = new double[rows, 1];
            for (int i = 0; i < rows; i++)
            {
                matrix[i, 0] = 0;
            }
            FillWithArray(matrix);
        }

        public CustomVector(int[] array, int rows)
        {
            size = new MatrixSize(rows, 1);
            matrix = new double[rows, 1];
            FillWithArray(array);
        }
        public CustomVector(double[] array, int rows)
        {
            size = new MatrixSize(rows, 1);
            matrix = new double[rows, 1];
            FillWithArray(array);
        }
        public CustomVector(float[] array, int rows)
        {
            size = new MatrixSize(rows, 1);
            matrix = new double[rows, 1];
            FillWithArray(array);
        }

        /// <summary>
        /// Vector between two points in 3D space (4D vector with a last value 0)
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        public CustomVector(Position target, Position source)
        {
            Position tmp = target - source;
            size = new MatrixSize(4, 1);
            matrix = new double[4, 1] { { tmp.X }, { tmp.Y }, { tmp.Z }, { 0 } };
        }


        /// <summary>
        /// 3D cross product of two vectors
        /// </summary>
        /// <param name="multiplied"></param>
        /// <param name="multiplier"></param>
        /// <returns></returns>
        public static CustomVector operator *(CustomVector multiplied, CustomVector multiplier)
        {
            if (multiplied.size.rows == 3)
            {
                Vector3D vec1 = new Vector3D(
                    multiplied.Get(0,0), multiplied.Get(1, 0), multiplied.Get(2, 0));
                Vector3D vec2 = new Vector3D(
                    multiplier.Get(0, 0), multiplier.Get(1, 0), multiplier.Get(2, 0));
                Vector3D res = Vector3D.CrossProduct(vec1, vec2);
                return new CustomVector(new double[3] { res.X, res.Y, res.Z }, 3);
            }
            else if (multiplied.size.rows == 4)
            {
                Vector3D vec1 = new Vector3D(
                    multiplied.Get(0, 0), multiplied.Get(1, 0), multiplied.Get(2, 0));
                Vector3D vec2 = new Vector3D(
                    multiplier.Get(0, 0), multiplier.Get(1, 0), multiplier.Get(2, 0));
                Vector3D res = Vector3D.CrossProduct(vec1, vec2);
                return new CustomVector(new double[4] { res.X, res.Y, res.Z, 0}, 4);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Dot product of two vectors
        /// </summary>
        /// <param name="multiplied"></param>
        /// <param name="multiplier"></param>
        /// <returns></returns>
        public static double operator |(CustomVector multiplied, CustomVector multiplier)
        {
            if (multiplied.size.rows == multiplier.size.rows)
            {
                double sum = 0;
                for (int i = 0; i < multiplied.size.rows; i++)
                {
                    sum += multiplied.Get(i, 0) * multiplier.Get(i, 0);
                }
                return sum;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public static CustomVector operator *(double scalar, CustomVector multiplied)
        {
            CustomMatrix newPos = scalar * (multiplied as CustomMatrix);
            return new CustomVector(newPos.AsDoubleArray(),
                new MatrixSize(multiplied.size.rows, multiplied.size.columns));
        }

        public static CustomVector operator -(CustomVector vector, CustomVector subtract)
        {
            double[] ar = new double[vector.size.rows];
            for (int i = 0; i < vector.size.rows; i++)
            {
                ar[i] = vector.Get(i,0) - subtract.Get(i,0);
            }
            return new CustomVector(ar, vector.size.rows);
        }
        public override double ElementSum(int power = 1)
        {
            double sum = 0;
                for (int j = 0; j < size.rows-1; j++)
                {
                    sum += ToPower(matrix[j, 0], power);
                }
            return sum;
        }
        public double Length()
        {
            return Math.Sqrt(ElementSum(2));
        }

        public CustomVector Normalized()
        {
            double length = Length();
            if (length != 0)
            {
                CustomVector customMatrix = new CustomVector(matrix, size);
                customMatrix = (1.0 / length) * customMatrix;
                customMatrix.Set(3,0,0);
                return customMatrix;
            }
            else
            {
                return new CustomVector(size.rows);
            }
        }

        private void FillWithArray(int[] array)
        {
            for (int i = 0; i < size.rows; i++)
            {
                matrix[i, 0] = array[i];
            }
        }
        private void FillWithArray(double[] array)
        {
            for (int i = 0; i < size.rows; i++)
            {
                matrix[i, 0] = array[i];
            }
        }
        private void FillWithArray(float[] array)
        {
            for (int i = 0; i < size.rows; i++)
            {
                matrix[i, 0] = array[i];
            }
        }

    }
}
