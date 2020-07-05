using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSimulator.Models
{
    public class ErrorMessageGenerator : IErrorMessageGenerator
    {
        public List<byte[]> GenerateErrorMessages(int numofErrorMessage)
        {
            List<byte[]> list = new List<byte[]>();
            for (int i = 0; i < numofErrorMessage; i++)
            {
                Byte[] sendBytes = Encoding.ASCII.GetBytes(Guid.NewGuid().ToString());
                list.Add(sendBytes);
            }
            return list;
        }
    }
}
