using DeviceSimulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSimulator.Generators
{
    public interface ICombinationMessageGenerator
    {
        Task<MessagesModel> GenerateAndSendUpdMessages();
    }
}
