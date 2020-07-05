using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSimulator.Generators
{
    public class MessageGenerator: IMessageGenerator
    {
        public MessageGenerator(string path = "")
        {

        }
        public MessageGenerator() : this("")
        {

        }

        public List<byte[]> GenerateMessages(List<string> deviceIds, int numOfEvt)
        {
            //List<byte[]> messages = new List<byte[]>();
            ConcurrentBag<byte[]> messages = new ConcurrentBag<byte[]>();

            Parallel.ForEach(deviceIds, deviceId =>
            {
                Parallel.For(0, numOfEvt, i =>
                {
                    byte[] tp = GenerateRandomPacket(deviceId);
                    messages.Add(tp);
                });

                //for (var i = 0; i < numOfEvt; i++)
                //{
                //    byte[] tp = GenerateRandomPacket(deviceId);
                //    messages.Add(tp);
                //}
            });

            return messages.ToList();
        }

        private byte[] GenerateRandomPacket(string deviceId)
        {
            MessageFormat messageFormat = new MessageFormat() { DeviceId = deviceId };
            Random random = new Random();
            int num=random.Next(9, 100);
            messageFormat.Data = GenerateUniqueString(num);
            var json=JsonConvert.SerializeObject(messageFormat);

            Byte[] sendBytes = Encoding.ASCII.GetBytes(json);
            return sendBytes;
        }

        string GenerateUniqueString(int stringlength)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var bit_count = (stringlength * 6);
                var byte_count = ((bit_count + 7) / 8); // rounded up
                var bytes = new byte[byte_count];
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
