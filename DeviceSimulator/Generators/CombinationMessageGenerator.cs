using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Deployment.Application;
using DeviceSimulator.Models;

namespace DeviceSimulator.Generators
{
    public class CombinationMessageGenerator : ICombinationMessageGenerator
    {
        private IPEndPoint _clusterServerEndPoint;
        private IErrorMessageGenerator _errorMessageGenerator;
        private IMessageGenerator _messageGenerator;

        private int _numOfCorrectMsg;
        private int _numOfErrorMsg;

        private List<string> _seedDeviceIdList;
        public CombinationMessageGenerator(IMessageGenerator messageGenerator, IErrorMessageGenerator errorMessageGenerator,
            IPEndPoint clusterServerEndPoint, int numOfCorrectMsg, int numOfErrorMsg)
        {
            _messageGenerator = messageGenerator;
            _errorMessageGenerator = errorMessageGenerator;
            _clusterServerEndPoint = clusterServerEndPoint;
            _numOfCorrectMsg = numOfCorrectMsg;
            _numOfErrorMsg = numOfErrorMsg;

            _seedDeviceIdList = GenerateDeviceIdList();
        }

        public Task<MessagesModel> GenerateAndSendUpdMessages()
        {
            return Task.Run(() =>
            {
                List<byte[]> bytesList = GenerateMessageBytesList(_numOfCorrectMsg, _numOfErrorMsg);
                using (UdpClient udp = new UdpClient())
                {
                    Parallel.ForEach(bytesList, evtData =>
                    {
                        udp.Send(evtData, evtData.Length, _clusterServerEndPoint);
                    });
                }
                return new MessagesModel(_numOfCorrectMsg, _numOfErrorMsg);
            });

        }

        private List<byte[]> GenerateMessageBytesList(int numOfCorrectMsg, int numOfErrorMsg)
        {
            List<byte[]> list = new List<byte[]>();
            if (numOfCorrectMsg > 0)
            {
                List<byte[]> correctList = GenerateCorrectMessageBytesList(_seedDeviceIdList, numOfCorrectMsg);
                list.AddRange(correctList);
            }

            if (numOfErrorMsg > 0)
            {
                List<byte[]> errList = _errorMessageGenerator.GenerateErrorMessages(numOfErrorMsg);
                list.AddRange(errList);
            }

            return list;
        }

        private List<byte[]> GenerateCorrectMessageBytesList(List<string> pickDeviceList, int totalCorrectMsg)
        {
            int deviceCount = pickDeviceList.Count;

            int messagesPerDevice = (totalCorrectMsg + deviceCount - 1) / deviceCount;

            List<byte[]> correctList = _messageGenerator.GenerateMessages(pickDeviceList, messagesPerDevice);

            Random rnd = new Random();

            var orderedList = correctList.OrderBy(c => rnd.Next());

            return orderedList.Take(totalCorrectMsg).ToList();
        }

        private List<string> GenerateDeviceIdList(int numOfIds = 1000)
        {
            try
            {
                string rootPath;
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    rootPath = ApplicationDeployment.CurrentDeployment.DataDirectory;
                    rootPath = Path.Combine(rootPath, "Data");
                }
                else
                {
                    rootPath = Path.Combine(Environment.CurrentDirectory, "Data");
                }
                string path = Path.Combine(rootPath, "DeviceIds.json");
                string json;
                using (StreamReader r = new StreamReader(path))
                {
                    json = r.ReadToEnd();
                }
                var jObject = JObject.Parse(json);
                IEnumerable<string> q = jObject.GetValue("DeviceIds").Values<string>();
                HashSet<string> hash = new HashSet<string>(q);
                //172500147
                Random rand = new Random();
                for (var j = 0; j < numOfIds; j++)
                {
                    int deviceId = rand.Next(000000000, 99999999);
                    while (hash.Contains(deviceId.ToString()))
                    {
                        deviceId = rand.Next(000000000, 999999999);
                    }
                    hash.Add(deviceId.ToString());
                }

                return hash.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}
