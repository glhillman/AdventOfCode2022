using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day17
{
    public class Chamber
    {
        public Chamber()
        {
            Rows = new();
            Rows.Add(new ChamberRow());
            Tops = new();
        }

        public List<ChamberRow> Rows { get; set; }
        public int HighestRow
        {
            
            get { return Rows.Count; }
        }

        public Dictionary<string, List<(int, int)>> Tops { get; set; } // nth rock, chamber height

        public bool HasCollision(Shape shape, int bottomRow, int offset)
        {
            bool hasCollision = false;

            for (int shapeRow = 0, chamberRow = bottomRow; hasCollision == false && shapeRow < shape.Height; shapeRow++, chamberRow++)
            {
                if (chamberRow >= Rows.Count)
                {
                    break;
                }
                hasCollision = Rows[bottomRow++].HasCollision(shape, shapeRow, offset);
            }

            return hasCollision;
        }

        public void CombineShape(Shape shape)
        {
            while (Rows.Count < shape.BottomRow + shape.Height)
            {
                Rows.Add(new ChamberRow());
            }
            int chamberRow = shape.BottomRow;
            for (int shapeRow = 0; shapeRow < shape.Height; shapeRow++, chamberRow++)
            {
                Rows[chamberRow].CombineShapeWithRow(shape, shapeRow);
            }
        }

        public void StoreMaxColls(int nthRock, int moveIndex)
        {
            StringBuilder sb = new StringBuilder();

            byte mask = 0;
            int range = Rows.Count;
            while (range > 0 && mask != 0b1111111)
            {
                range--;
                mask |= Rows[range].Row;
            }
            if (mask == 0b1111111)
            {
                int[] tops = new int[7];
                int index = Rows.Count - 1;
                while (index >= range)
                {
                    byte bit = 0b1000000;
                    for (int col = 0; col < 7; col++)
                    {
                        if (tops[col] == 0 && (Rows[index].Row & bit) != 0)
                        {
                            tops[col] = index;
                        }
                        bit >>= 1;
                    }
                    index--;
                }
                int minTop = tops.Min();
                
                for (int i = 0; i < 7; i++)
                {
                    sb.Append(tops[i] - minTop);
                }
                sb.Append(moveIndex);
                string key = sb.ToString();
                if (Tops.ContainsKey(key) == false)
                {
                    Tops[key] = new List<(int, int)>();
                }
                Tops[key].Add((nthRock, Rows.Count - 1));
            }
        }
    }
}
