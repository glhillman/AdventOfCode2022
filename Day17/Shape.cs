using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Day17
{
    public enum ShapeTypes
    {
        Bar,
        Plus,
        Comma,
        Tower,
        Block
    }
    public class Shape
    {
        public Shape(ShapeTypes shapeType)
        {
            ShapeType = shapeType;
            Offset = 2;
            Settled = false;
        }

        protected void SetShapeData(Chamber chamber, byte[] shapeData, int width)
        {
            ShapeData = shapeData;
            Height = shapeData.GetLength(0);
            Width = width;
            BottomRow = chamber.HighestRow + 3;
        }

        public byte[] ShapeData { get; private set; }
        public void Shift(Chamber chamber, char direction)
        {
            if (direction == '<')
            {
                if (Offset > 0 && chamber.HasCollision(this, BottomRow, Offset - 1) == false)
                {
                    Offset--;
                }
            }
            else
            {
                if (Offset + Width < 7 && chamber.HasCollision(this, BottomRow, Offset + 1) == false)
                {
                    Offset++;
                }
            }
        }
        public void Drop(Chamber chamber)
        {
            if (BottomRow-1 > chamber.HighestRow)
            {
                BottomRow--;
            }
            else
            {
                // here is where we look for collisions on the way down
                if (BottomRow > 1 && chamber.HasCollision(this, BottomRow - 1, Offset) == false)
                {
                    BottomRow--;
                }
                else
                {
                    chamber.CombineShape(this);
                    Settled= true;
                }
            }
        }
        public ShapeTypes ShapeType { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }
        public int Offset { get; set; }
        public int BottomRow { get; set; }
        public bool Settled { get; set; }

        public override string ToString()
        {
            return string.Format("Type: {0}, Height: {1}, Width {2}, Offset {3}, BottomRow {4}, Settled: {5}",
                                 ShapeType, Height, Width, Offset, BottomRow, Settled);
        }
    }

    public class Bar : Shape
    {
        public Bar(Chamber chamber)
            :base(ShapeTypes.Bar)
        {
            byte[] shapeData = { 0b1111000 }; 
            SetShapeData(chamber, shapeData, 4);
        }
    }

    public class Plus : Shape
    { 
        public Plus(Chamber chamber)
            :base(ShapeTypes.Plus)
        {
            byte[] shapeData =
             {
                0b0100000,
                0b1110000,
                0b0100000
            };
            SetShapeData(chamber, shapeData, 3);
        }
    }

    public class Comma : Shape
    {
        public Comma(Chamber chamber)
            :base(ShapeTypes.Comma)
        {
            byte[] shapeData =
            {
                // inverted so low to high row indexes match direction of rows in chamber
                0b1110000,
                0b0010000,
                0b0010000
            };    
            SetShapeData(chamber, shapeData, 3);
        }
    }

    public class Tower : Shape
    {
        public Tower(Chamber chamber)
            : base(ShapeTypes.Tower)
        {
            byte[] shapeData =
            {
                0b1000000,
                0b1000000,
                0b1000000,
                0b1000000
            };
            SetShapeData(chamber, shapeData, 1);
        }
    }
    public class Block : Shape
    {
        public Block(Chamber chamber)
            : base(ShapeTypes.Block)
        {
            byte[] shapeData =
            {
                0b1100000,
                0b1100000
            };
            SetShapeData(chamber, shapeData, 2);
        }
    }
}
