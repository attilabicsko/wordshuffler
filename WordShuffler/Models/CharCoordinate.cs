using System;
using System.Collections.Generic;

namespace WordShuffler.Models
{
    public class CharCoordinate
    {
        public CharCoordinate(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public int Row { get; set; }
        public int Column { get; set; }

        public bool IsSiblingOf(CharCoordinate other)
        {
            return Math.Abs(Row - other.Row) <= 1 && Math.Abs(Column - other.Column) <= 1 && !this.Equals(other);
        }

        public IEnumerable<CharCoordinate> FindSiblings(int matrixSize)
        {
            var siblings = new List<CharCoordinate>();

            if (Row > 0)
                siblings.Add(new CharCoordinate(Row - 1, Column));

            if (Column > 0)
                siblings.Add(new CharCoordinate(Row, Column - 1));

            if (Row > 0 && Column > 0)
                siblings.Add(new CharCoordinate(Row - 1, Column - 1));

            if (Row < matrixSize - 1)
                siblings.Add(new CharCoordinate(Row + 1, Column));

            if (Column < matrixSize - 1)
                siblings.Add(new CharCoordinate(Row, Column + 1));

            if (Column < matrixSize - 1 && Row < matrixSize - 1)
                siblings.Add(new CharCoordinate(Row + 1, Column + 1));

            if (Column < matrixSize - 1 && Row > 0)
                siblings.Add(new CharCoordinate(Row - 1, Column + 1));

            if (Row < matrixSize - 1 && Column > 0)
                siblings.Add(new CharCoordinate(Row + 1, Column - 1));

            return siblings;
        }

        #region overrides

        public override string ToString()
        {
            return string.Format("[{0}, {1}]", Row, Column);
        }

        public override bool Equals(object obj)
        {
            var other = obj as CharCoordinate;
            if (other == null) return false;
            return (other.Row == this.Row && other.Column == this.Column);
        }

        public override int GetHashCode()
        {
            return Row * 1000 + Column;
        }

        #endregion
    }
}
