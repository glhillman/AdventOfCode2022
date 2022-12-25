using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day21
{
    internal class Monkey
    {
        public Monkey(string name, string monkey1Name, string op, string monkey2Name)
        {
            Name = name;
            Monkey1Name = monkey1Name;
            Op = op[0];
            Monkey2Name = monkey2Name;
            Num1 = null;
            Num2 = null;
        }

        public string Name { get; private set; }
        public string Monkey1Name { get; private set; }
        public string Monkey2Name { get; private set; }
        public double? Num1 { get; set; }
        public double? Num2 { get; set; }
        public char Op { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}({2}) {3} {4}({5})", Name, Monkey1Name, Num1.HasValue ? Num1.Value : "null", Op, Monkey2Name, Num2.HasValue ? Num2.Value : "null");
        }
    }
}
