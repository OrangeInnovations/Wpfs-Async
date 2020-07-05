using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSimulator.Generators
{
    public interface IMessageGenerator
    {
        //byte[] Generate(string deviceId, string packetID, DateTime dt);

        //byte[] Generate(int deviceId, int packetId, int version, DateTime dt);
        List<byte[]> GenerateMessages(List<string> deviceIds, int numOfEvt);
        //List<byte[]> GenerateMessages(List<int> deviceIds, int numOfEvt, bool include51);

        //List<byte[]> GenerateMessages(int numOfEvt, bool include51 = false);
    }
}
