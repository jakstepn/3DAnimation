using System;
using System.Threading.Tasks;

namespace GKtoolkit
{
    public class CustomMatrix
    {
        protected MatrixSize size;
        protected double[,] matrix;

        protected CustomMatrix() { }

        public CustomMatrix(int height, int width)
        {
            size = new MatrixSize(height, width);
            matrix = new double[height, width];
        }

        public CustomMatrix(int[,] array, MatrixSize matrixSize)
        {
            size = matrixSize;
            matrix = new double[size.rows, size.columns];
            FillWithArray(array);
        }

        public CustomMatrix(float[,] array, MatrixSize matrixSize)
        {
            size = matrixSize;
            matrix = new double[size.rows, size.columns];
            FillWithArray(array);
        }

        public CustomMatrix(double[,] array, MatrixSize matrixSize)
        {
            size = matrixSize;
            matrix = new double[size.rows, size.columns];
            FillWithArray(array);
        }

        public double Get(int row, int column)
        {
            return matrix[row, column];
        }

        public void Set(int row, int column, double value)
        {
            matrix[row, column] = value;
        }

        public int[,] AsIntArray()
        {
            int[,] result = new int[size.rows, size.columns];
            for (int i = 0; i < size.rows; i++)
            {
                for (int j = 0; j < size.columns; j++)
                {
                    result[i, j] = (int)matrix[i, j];
                }
            }
            return result;
        }

        public double[,] AsDoubleArray()
        {
            return matrix;
        }

        public void FillWithArray(int[,] src)
        {
            for (int i = 0; i < size.rows; i++)
            {
                for (int j = 0; j < size.columns; j++)
                {
                    matrix[i, j] = src[i, j];
                }
            }
        }

        public void FillWithArray(double[,] src)
        {
            for (int i = 0; i < size.rows; i++)
            {
                for (int j = 0; j < size.columns; j++)
                {
                    matrix[i, j] = src[i, j];
                }
            }
        }

        public void FillWithArray(float[,] src)
        {
            for (int i = 0; i < size.rows; i++)
            {
                for (int j = 0; j < size.columns; j++)
                {
                    matrix[i, j] = src[i, j];
                }
            }
        }

        public void Print()
        {
            for (int i = 0; i < size.rows; i++)
            {
                for (int j = 0; j < size.columns; j++)
                {
                    Console.Write("{0} ", matrix[i, j]);
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }

        public static CustomMatrix operator *(CustomMatrix multiplied, CustomMatrix multiplier)
        {
            if (multiplied.size.columns == multiplier.size.rows)
            {
                CustomMatrix tmp = Multiply(multiplied, multiplier);
                return tmp;
            }
            else
            {
                throw new TaskCanceledException();
            }
        }

        public static CustomMatrix operator *(CustomVector vector, CustomMatrix matrix)
        {
            if (vector.size.columns == matrix.size.rows)
            {
                CustomMatrix tmp = Multiply(vector, matrix);
                return tmp;
            }
            else
            {
                throw new TaskCanceledException();
            }
        }

        public static CustomVector operator *(CustomMatrix matrix, CustomVector vector)
        {
            if (matrix.size.columns == vector.size.rows)
            {
                CustomMatrix tmp = Multiply(matrix, vector);
                double[] resarray = new double[tmp.size.rows];
                for (int i = 0; i < tmp.size.rows; i++)
                {
                    resarray[i] = tmp.Get(i, 0);
                }
                CustomVector res = new CustomVector(resarray, tmp.size.rows);
                return res;
            }
            else
            {
                throw new TaskCanceledException();
            }
        }

        public static CustomMatrix operator *(double scalar, CustomMatrix multiplied)
        {
            return Multiply(multiplied, scalar);
        }

        public static CustomMatrix operator *(CustomMatrix multiplied, double scalar)
        {
            return Multiply(multiplied, scalar);
        }

        public static CustomMatrix operator +(CustomMatrix multiplied, CustomMatrix multiplier)
        {
            if (multiplied.size.columns == multiplier.size.columns && multiplied.size.rows == multiplier.size.rows)
            {
                CustomMatrix tmp = Add(multiplied, multiplier);
                return tmp;
            }
            else
            {
                throw new TaskCanceledException();
            }
        }

        public static CustomMatrix operator +(double scalar, CustomMatrix multiplied)
        {
            return Add(multiplied, scalar);
        }

        public static CustomMatrix operator +(CustomMatrix multiplied, double scalar)
        {
            return Add(multiplied, scalar);
        }

        public static CustomMatrix operator -(CustomMatrix multiplied, CustomMatrix multiplier)
        {
            if (multiplied.size.columns == multiplier.size.columns && multiplied.size.rows == multiplier.size.rows)
            {
                CustomMatrix tmp = Subtract(multiplied, multiplier);
                return tmp;
            }
            else
            {
                throw new TaskCanceledException();
            }
        }

        public static CustomMatrix operator -(double scalar, CustomMatrix multiplied)
        {
            return Subtract(multiplied, scalar);
        }

        public static CustomMatrix operator -(CustomMatrix multiplied, double scalar)
        {
            return Subtract(multiplied, scalar);
        }

        public virtual double ElementSum(int power = 1)
        {
            double sum = 0;
            for (int i = 0; i < size.columns; i++)
            {
                for (int j = 0; j < size.rows; j++)
                {
                    sum += ToPower(matrix[j, i], power);
                }
            }
            return sum;
        }

        protected double ToPower(double element, int power)
        {
            for (int i = 1; i < power; i++)
            {
                element *= element;
            }
            return element;
        }

        protected static CustomMatrix Multiply(CustomMatrix multiplied, CustomMatrix multiplier)
        {
            CustomMatrix result = new CustomMatrix(multiplied.size.rows, multiplier.size.columns);
            Parallel.For(
                0, multiplied.size.rows,
                (k) =>
                {
                    double sum;
                    for (int p = 0; p < multiplier.size.columns; p++)
                    {
                        sum = 0;
                        for (int i = 0; i < multiplied.size.columns; i++)
                        {
                            sum += multiplied.matrix[k, i] * multiplier.matrix[i, p];
                        }
                        result.matrix[k, p] = sum;
                    }
                });
            return result;
        }

        protected static CustomMatrix Multiply(CustomMatrix multiplied, double scalar)
        {
            CustomMatrix result = new CustomMatrix(multiplied.size.rows, multiplied.size.columns);
            Parallel.For(
                0, multiplied.size.rows,
                (k) =>
                {
                    for (int i = 0; i < multiplied.size.columns; i++)
                    {
                        result.matrix[k, i] = multiplied.matrix[k, i] * scalar;
                    }
                });
            return result;
        }

        protected static CustomMatrix Add(CustomMatrix multiplied, CustomMatrix multiplier)
        {
            CustomMatrix result = new CustomMatrix(multiplied.size.rows, multiplier.size.columns);
            Parallel.For(
                0, multiplied.size.rows,
                (k) =>
                {
                        for (int i = 0; i < multiplied.size.columns; i++)
                        {
                            result.matrix[k, i] = multiplied.matrix[k, i] + multiplier.matrix[k, i];
                        }
                });
            return result;
        }

        protected static CustomMatrix Add(CustomMatrix multiplied, double scalar)
        {
            CustomMatrix result = new CustomMatrix(multiplied.size.rows, multiplied.size.columns);
            Parallel.For(
                0, multiplied.size.rows,
                (k) =>
                {
                    for (int i = 0; i < multiplied.size.columns; i++)
                    {
                        result.matrix[k, i] = multiplied.matrix[k, i] + scalar;
                    }
                });
            return result;
        }

        protected static CustomMatrix Subtract(CustomMatrix multiplied, CustomMatrix multiplier)
        {
            CustomMatrix result = new CustomMatrix(multiplied.size.rows, multiplier.size.columns);
            Parallel.For(
                0, multiplied.size.rows,
                (k) =>
                {
                    for (int i = 0; i < multiplied.size.columns; i++)
                    {
                        result.matrix[k, i] = multiplied.matrix[k, i] - multiplier.matrix[k, i];
                    }
                });
            return result;
        }

        protected static CustomMatrix Subtract(CustomMatrix multiplied, double scalar)
        {
            CustomMatrix result = new CustomMatrix(multiplied.size.rows, multiplied.size.columns);
            Parallel.For(
                0, multiplied.size.rows,
                (k) =>
                {
                    for (int i = 0; i < multiplied.size.columns; i++)
                    {
                        result.matrix[k, i] = multiplied.matrix[k, i] - scalar;
                    }
                });
            return result;
        }
    }
}
