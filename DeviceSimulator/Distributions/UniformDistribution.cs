using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSimulator.Distributions
{
    public class UniformDistribution : IDistribution
    {
        double _lowerBounds, _upperBounds;
        public UniformDistribution(double lowerBounds, double upperBounds)
        {
            this._lowerBounds = lowerBounds;
            this._upperBounds = upperBounds;
        }
        public double GetNumber()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            return _lowerBounds + (_upperBounds - _lowerBounds) * random.NextDouble();
        }

        public void GetNumbers(double[] values)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = _lowerBounds + (_upperBounds - _lowerBounds) * random.NextDouble();
            }
        }
    }
}
