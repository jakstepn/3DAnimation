namespace GKtoolkit
{
    public class MatrixSize
    {
        public int rows { get; }
        public int columns { get; }
        public int amountOfElements { get; }

        public MatrixSize(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            amountOfElements = rows * columns;
        }
    }
}
