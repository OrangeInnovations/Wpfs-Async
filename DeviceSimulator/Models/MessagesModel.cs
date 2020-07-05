using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSimulator.Models
{
    public class MessagesModel
    {
        public MessagesModel(int numofCorrect, int numOfWrong)
        {
            NumofCorrectMessages = numofCorrect;
            NumofWrongMessages = numOfWrong;
        }
        public MessagesModel()
        {

        }
        public int NumofCorrectMessages { get; set; }
        public int NumofWrongMessages { get; set; }
        public int TotalMessages => NumofCorrectMessages + NumofWrongMessages;
    }
}
