using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKtoolkit
{
    class CubeDimensions
    {
        public int amountOfFaces
        {
            get
            {
                return 6;
            }
        }
        public FaceDimensions faceDimensions { get; }
        public double rowLength
        {
            get
            {
                return faceDimensions.rowLength;
            }
        }
        public double columnLength
        {
            get
            {
                return faceDimensions.columnLength;
            }
        }
        public double depth
        {
            get
            {
                return rowLength;
            }
        }
        public CubeDimensions(double rowLength, double columnLength,
            double distanceBetweenVerticesRow, double distanceBetweenVerticesColumn)
        {
            faceDimensions = new FaceDimensions(rowLength, columnLength,
                distanceBetweenVerticesRow, distanceBetweenVerticesColumn);
        }
        public CubeDimensions(FaceDimensions faceDimensions)
        {
            this.faceDimensions = faceDimensions;
        }
    }

    class FaceDimensions
    { 
        public double rowLength { get; }
        public double columnLength { get; }
        public double distanceBetweenVerticesRows { get; }
        public double distanceBetweenVerticesColumns { get; }
        public int amountOfVerticesRow { get
            {
                return (int)(rowLength / distanceBetweenVerticesRows) + 1;
            } 
        }
        public int amountOfVerticesColumn
        {
            get
            {
                return (int)(columnLength / distanceBetweenVerticesColumns) + 1;
            }
        }
        public double amountOfVertices
        {
            get
            {
                return amountOfVerticesColumn * amountOfVerticesRow;
            }
        }
        public FaceDimensions(double rowLength, double columnLength,
            double distanceBetweenVerticesRows, double distanceBetweenVerticesColumns)
        {
            this.distanceBetweenVerticesRows = distanceBetweenVerticesRows;
            this.distanceBetweenVerticesColumns = distanceBetweenVerticesColumns;
            this.rowLength = rowLength;
            this.columnLength = columnLength;
        }
    }
}
