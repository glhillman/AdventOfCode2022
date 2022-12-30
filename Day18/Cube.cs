using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Day18
{
    public class Cube
    {
        public Cube(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
            TouchCount = 0;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public (int, int, int) Coords { get { return (X, Y, Z); } }
        public int TouchCount { get; set; }
        public int UntouchedCount { get { return 6 - TouchCount; } }
        public override string ToString()
        {
            return string.Format("x: {0}, y: {1}, z: {2}, TouchCount: {3}", X, Y, Z, TouchCount);
        }

    }
}
