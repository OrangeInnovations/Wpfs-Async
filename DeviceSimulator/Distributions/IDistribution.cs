using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSimulator.Distributions
{
    public interface IDistribution
    {
        /// <summary>
        /// get a number obey a specified distribution
        /// </summary>
        /// <param name="mean"></param>
        /// <param name="range"></param>
        /// <param name="probability">from 1 to 100</param>
        /// <returns></returns>
        double GetNumber();

        /// <summary>
        /// get numbers obj a specified distribution
        /// </summary>
        /// <param name="values"></param>
        /// <param name="mean"></param>
        /// <param name="range"></param>
        /// <param name="probability">from 0 to 100</param>
        void GetNumbers(double[] values);
    }

}
