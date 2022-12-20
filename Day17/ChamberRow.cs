using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day17
{
    public class ChamberRow
    {
        public ChamberRow()
        {
            Row = 0;
        }

        public bool HasCollision(Shape shape, int shapeRow, int offset)
        {
            bool hasCollision = false;
            byte shiftedShape = shape.ShapeData[shapeRow];
            shiftedShape >>= offset;

            hasCollision = (shiftedShape & Row) != 0;

            return hasCollision;
        }

        public void CombineShapeWithRow(Shape shape, int shapeRow)
        {
            byte shiftedShape = (byte)(shape.ShapeData[shapeRow] >> shape.Offset);
            Row |= shiftedShape;
        }

        public byte Row { get; set; }
    }
}
