using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSimulator.Generators
{
    public interface IErrorMessageGenerator
    {
        List<byte[]> GenerateErrorMessages(int numofErrorMessage);
    }
}
