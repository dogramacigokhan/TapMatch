using System;

namespace TapMatch.GridSystem
{
    public readonly struct GridIndex : IEquatable<GridIndex>
    {
        public readonly int Row;
        public readonly int Column;

        public GridIndex(int row, int column)
        {
            this.Row = row;
            this.Column = column;
        }

        public bool Equals(GridIndex other)
        {
            return this.Row == other.Row && this.Column == other.Column;
        }

        public override bool Equals(object obj)
        {
            return obj is GridIndex other && this.Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Row, this.Column);
        }
    }
}