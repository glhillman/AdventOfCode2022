using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day22
{
    public class Surface
    {
        public Surface(int surfaceNum, (int min, int max) rowRange, (int min, int max) colRange)
        {
            SurfaceNum = surfaceNum;
            RowRange = rowRange;
            ColRange = colRange;
            Transitions = new();
        }
        
        public int SurfaceNum { get; private set; }
        public (int min, int max) RowRange { get; private set; }
        public (int min, int max) ColRange { get; private set; }
        public bool Contains(int row, int col)
        {
            return (row >= RowRange.min && row <= RowRange.max && col >= ColRange.min && col <= ColRange.max);
        }
        public Dictionary<char, TransitionDefinition> Transitions { get; set; }
    }
}
