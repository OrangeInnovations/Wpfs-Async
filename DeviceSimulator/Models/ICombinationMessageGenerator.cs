using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSimulator.Models
{
    public interface ICombinationMessageGenerator
    {
        Task<MessagesModel> GenerateAndSendUpdMessages();
    }
}
