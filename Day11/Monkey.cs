using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{
    public class Monkey
    {
        public Monkey(int id, string operations, long divisor, int trueTarget, int falseTarget, string startItems)
        {
            Id = id;
            Divisor = divisor;
            TrueTarget = trueTarget;
            FalseTarget = falseTarget;
            OperationDefinition = operations.Split(' ');
            Op2 = OperationDefinition[2] == "old" ? 0 : long.Parse(OperationDefinition[2]);
            Items = new LinkedList<long>();
            string[] parts = startItems.Replace(",", "").Split(' ');
            foreach (string part in parts)
            {
                Items.AddFirst(long.Parse(part));
            }
        }

        public int Id { get; private set; }
        public long Divisor { get; private set; }
        public int TrueTarget { get; private set; }
        public int FalseTarget { get; private set; }
        public long Op2 { get; private set; }
        public static long Reducer { get; set; }
        public void AddItem(long value) { Items.AddFirst(value); }
        public bool EvaluateNextItem(ref long newValue, bool reduceByDivision)
        {
            long old = Items.Last.Value;
            
            Items.RemoveLast();

            long op1 = old; // always
            long op2 = Op2 > 0 ? Op2 : op1;

            newValue = OperationDefinition[1] == "+" ? op1 + op2 : op1 * op2;

            newValue = reduceByDivision ? newValue / Reducer : newValue % Reducer;

            return newValue % Divisor == 0;
        }

        public string[] OperationDefinition;
        public LinkedList<long> Items;
    }
}
