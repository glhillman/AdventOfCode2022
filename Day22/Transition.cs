using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day22
{
    public delegate (char newDirection, int row, int col) TransitionAtEdge(char dstDirection, int curRow, int curCol, Surface dstSurface);

    public class TransitionDefinition
    {
        public TransitionDefinition(int surfaceNum, int nextSurface, char srcDirection, char dstDirection, TransitionAtEdge transBody)
        {
            SurfaceNum = surfaceNum;
            NextSurface = nextSurface;
            SrcDirection= srcDirection;
            DstDirection= dstDirection;
            TransBody= transBody;
        }

        public int SurfaceNum { get; private set; }
        public int NextSurface { get; private set; }
        public char SrcDirection { get; private set; }
        public char DstDirection { get; private set; }
        public TransitionAtEdge TransBody { get; set; }
        public (char newDirection, int row, int col) Transition(int curRow, int curCol, Surface dstSurface)
        {
            return TransBody(DstDirection, curRow, curCol, dstSurface);
        }

        // static TransitionAtEdge logic functions

        // 1 ^ 6, 4 ^ 3 : ^ => >
        public static (char newDirection, int row, int col) RowMinToColMin(char dstDirection, int curRow, int curCol, Surface dstSurface)
        {
            int colOffset = curCol % 50;
            return (dstDirection, dstSurface.RowRange.min + colOffset, dstSurface.ColRange.min);
        }

        // 2 ^ 6, 3 ^ 1, 5 ^ 3, 6 ^ 4 : ^ => ^
        public static (char newDirection, int row, int col) RowMinToRowMax(char dstDirection, int curRow, int curCol, Surface dstSurface)
        {
            int colOffset = curCol % 50;
            return (dstDirection, dstSurface.RowRange.max, dstSurface.ColRange.min + colOffset);
        }

        // 1 v 3, 3 v 5, 4 v 6, 6 v 2 : v => v
        public static (char newDirection, int row, int col) RowMaxToRowMin(char dstDirection, int curRow, int curCol, Surface dstSurface)
        {
            int colOffset = curCol % 50;
            return (dstDirection, dstSurface.RowRange.min, dstSurface.ColRange.min + colOffset);
        }

          // 2 v 3, 5 v 6 : v => <
        public static (char newDirection, int row, int col) RowMaxToColMax(char dstDirection, int curRow, int curCol, Surface dstSurface)
        {
            int colOffset = curCol % 50;
            return (dstDirection, dstSurface.RowRange.min + colOffset, dstSurface.ColRange.max);
        }

        // 2 < 1, 5 < 4 : < => <
        public static (char newDirection, int row, int col) ColMinToColMax(char dstDirection, int curRow, int curCol, Surface dstSurface)
        {
            int rowOffset = curRow % 50;
            return (dstDirection, dstSurface.RowRange.min + rowOffset, dstSurface.ColRange.max);
        }

        // 3 < 4, 6 < 1 : < => v
        public static (char newDirection, int row, int col) ColMinToRowMin(char dstDirection, int curRow, int curCol, Surface dstSurface)
        {
            int rowOffset = curRow % 50;
            return (dstDirection, dstSurface.RowRange.min, dstSurface.ColRange.min + rowOffset);
        }

         // 1 < 4, 4 < 1 : < => >
        public static (char newDirection, int row, int col) ColMinToColMin(char dstDirection, int curRow, int curCol, Surface dstSurface)
        {
            int rowOffset = curRow % 50;
            return (dstDirection, dstSurface.RowRange.max - rowOffset, dstSurface.ColRange.max);
        }

        // 2 > 5, 5 > 2 : > => <
        public static (char newDirection, int row, int col) ColMaxToColMax(char dstDirection, int curRow, int curCol, Surface dstSurface)
        {
            int rowOffset = curRow % 50;
            return (dstDirection, dstSurface.RowRange.max - rowOffset, dstSurface.ColRange.max);
        }

        // 1 > 2, 4 > 5 : > => >
        public static (char newDirection, int row, int col) ColMaxToColMin(char dstDirection, int curRow, int curCol, Surface dstSurface)
        {
            int rowOffset = curRow % 50;
            return (dstDirection, dstSurface.RowRange.min + rowOffset, dstSurface.ColRange.min);
        }

        // 3 > 2, 6 > 5 : > => ^
        public static (char newDirection, int row, int col) ColMaxToRowMax(char dstDirection, int curRow, int curCol, Surface dstSurface)
        {
            int rowOffset = curRow % 50;
            return (dstDirection, dstSurface.RowRange.max, dstSurface.ColRange.min + rowOffset);
        }


    }
}
