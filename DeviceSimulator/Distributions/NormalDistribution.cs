using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSimulator.Distributions
{
    public class NormalDistribution : IDistribution
    {
        double _lowerBounds, _upperBounds, _probability;

        public NormalDistribution(double lowerBounds, double upperBounds, double probability)
        {
            this._lowerBounds = lowerBounds;
            this._upperBounds = upperBounds;
            if (probability <= 0)
                this._probability = 0.0000000001;
            else if (probability >= 100)
                this._probability = 99.9999999999;
            else
                this._probability = probability;
        }
        public double GetNumber()
        {
            double mean = (_lowerBounds + _upperBounds) / 2;
            double stddev = ((_upperBounds - _lowerBounds) / 2) / MathNet.Numerics.Distributions.Normal.InvCDF(0, 1, ((1 + _probability / 100) / 2));
            return MathNet.Numerics.Distributions.Normal.Sample(mean, stddev);
        }

        public void GetNumbers(double[] values)
        {
            double mean = (_lowerBounds + _upperBounds) / 2;
            double stddev = ((_upperBounds - _lowerBounds) / 2) / MathNet.Numerics.Distributions.Normal.InvCDF(0, 1, ((1 + _probability / 100) / 2));
            MathNet.Numerics.Distributions.Normal.Samples(values, mean, stddev);
        }
    }
}
