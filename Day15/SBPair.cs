using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day15
{
    // Sensor/Beacon pair
    public class SBPair
    {
        public SBPair(Point sensor, Point beacon)
        {
            Sensor = sensor;
            Beacon = beacon;
            Distance = Math.Abs(sensor.X - beacon.X) + Math.Abs(sensor.Y - beacon.Y);
        }

        public Point Sensor { get; private set; }
        public Point Beacon { get; private set; }
        public long Distance { get; private set; }
        public (long minX, long maxX, bool isValid) MinMaxForY(long y)
        {
            long minx = 0;
            long maxx = 0;
            bool isValid = false;

            if (y >= Sensor.Y && y <= Sensor.Y + Distance)
            {
                // y is below Sensor, but within sensor coverage
                long temp = Distance - (y - Sensor.Y);
                minx = Sensor.X - temp;
                maxx = Sensor.X + temp;
                isValid = true;
            }
            else if (y <= Sensor.Y && y >= Sensor.Y - Distance)
            {
                // y is above Sensor, but within its coverage
                long temp = Distance - (Sensor.Y - y);
                minx = Sensor.X - temp;
                maxx = Sensor.X + temp;
                isValid = true;
            }
            return (minx, maxx, isValid);
        }

        public override string ToString()
        {
            return string.Format("Sensor: {0}, Beacon: {1}, Distance: {2}", Sensor, Beacon, Distance);
        }
    }
}
