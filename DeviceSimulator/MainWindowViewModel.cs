using DeviceSimulator.Commands;
using DeviceSimulator.Distributions;
using DeviceSimulator.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Deployment.Application;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceSimulator
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {

            StartSendingEvents = AsyncCommand.Create(token => StartGenerateAndSendingEvents(token));
        }

        public IAsyncCommand StartSendingEvents { get; set; }


        public int DeviceNumber { get; set; } = 1000;
        public double LowBoundsMilliseconds { get; set; } = 0;
        public double UpperBoundsMilliseconds { get; set; } = 6000;
        public double NormalDistributionPercentage { get; set; } = 99;
        public double CorrectRatePercentage { get; set; } = 98;
        public string IpAddress { get; set; } = "127.0.0.1";
        public int UdpPort { get; set; } = 23400;



        public long NumberOfTotalMessages
        {
            get { return _numberOfTotalMessages; }
            set { _numberOfTotalMessages = value; OnPropertyChanged(); }
        }

        public long NumberOfCorrectMessages
        {
            get { return _numberOfCorrectMessages; }
            set { _numberOfCorrectMessages = value; OnPropertyChanged(); }
        }

        public long NumberOfErrorMessages
        {
            get { return _numberOfErrorMessages; }
            set { _numberOfErrorMessages = value; OnPropertyChanged(); }
        }


        private long _numberOfTotalMessages = 0;
        private long _numberOfCorrectMessages = 0;
        private long _numberOfErrorMessages = 0;

        public string LabelText
        {
            get { return _labelText; }
            set
            {
                _labelText = value;
                OnPropertyChanged("LabelText");
            }
        }

        private string _labelText = "";
        private DateTime _currentTime;

        public DateTime StartDateTime { get; set; }

        public DateTime CurrentTime
        {
            get { return _currentTime; }
            set
            {
                _currentTime = value;
                CurrentTimeDisplay = _currentTime.ToString("G");

                TimeSpan diff = _currentTime.Subtract(StartDateTime);

                TotalSendingTimeDisplay = diff.ToString();

                OnPropertyChanged("CurrentTimeDisplay");
                OnPropertyChanged("TotalSendingTimeDisplay");
            }
        }

        public string CurrentTimeDisplay { get; set; }
        private void ResetMessageNumbers()
        {
            NumberOfCorrectMessages = 0;
            NumberOfErrorMessages = 0;
            NumberOfTotalMessages = 0;

            StartDateTime = DateTime.Now;

            LabelText = $"Start sending messages from  { StartDateTime.ToString("G")} ...";
        }


        public string TotalSendingTimeDisplay { get; set; }
        private void IncreaseMessageNumbers(MessagesModel model)
        {
            NumberOfCorrectMessages += model.NumofCorrectMessages;
            NumberOfErrorMessages += model.NumofWrongMessages;
            NumberOfTotalMessages += model.TotalMessages;

            CurrentTime = DateTime.Now;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //private async Task StartGenerateAndSendingEvents(CancellationToken cancellationToken)
        //{
        //    ResetMessageNumbers();


        //    while (!cancellationToken.IsCancellationRequested)
        //    {
        //        try
        //        {
        //            await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);
        //            NumberOfTotalMessages += 3;
        //        }
        //        catch (TaskCanceledException)
        //        {
        //            return;
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //        }
        //    }
        //}

        private async Task<bool> StartGenerateAndSendingEvents(CancellationToken cancellationToken)
        {
            ResetMessageNumbers();
            try
            {
                DeviceSimulators simulators = CreateDeviceSimulators();
                if (simulators == null)
                {
                    return false;
                }
                await simulators.ContinueSendingEventsTask(IncreaseMessageNumbers, cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                System.Windows.MessageBox.Show(e.Message);
            }

            return false;
        }

        private DeviceSimulators CreateDeviceSimulators()
        {
            IDistribution distribution = new NormalDistribution(LowBoundsMilliseconds, UpperBoundsMilliseconds, NormalDistributionPercentage);

            IPAddress ipAddress = null;
            try
            {
                ipAddress = IPAddress.Parse(IpAddress);
            }
            catch (Exception exception)
            {
                try
                {
                    IPHostEntry hostEntry = Dns.GetHostEntry(IpAddress);
                    ipAddress = hostEntry.AddressList[0];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    System.Windows.MessageBox.Show(e.Message);
                }
            }

            try
            {
                IPEndPoint clusterServerEndPoint = new IPEndPoint(ipAddress, UdpPort);

                int numOfCorrectMsg = (int)(DeviceNumber * CorrectRatePercentage / 100 + 0.5);
                int numOfErrorMsg = DeviceNumber - numOfCorrectMsg;
                IMessageGenerator messageGenerator = CreateMessageGenerator();
                IErrorMessageGenerator errorMessageGenerator = new ErrorMessageGenerator();

                ICombinationMessageGenerator combinationMessageGenerator =
                    new CombinationMessageGenerator(messageGenerator, errorMessageGenerator, clusterServerEndPoint, numOfCorrectMsg, numOfErrorMsg);

                DeviceSimulators simulators = new DeviceSimulators(distribution, combinationMessageGenerator);

                return simulators;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                System.Windows.MessageBox.Show(e.ToString());
            }
            return null;
        }

        private IMessageGenerator CreateMessageGenerator()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                string path = ApplicationDeployment.CurrentDeployment.DataDirectory;
                return new MessageGenerator(path);
            }
            else
            {
                return new MessageGenerator();
            }

        }

        #region Methods

        private void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }


}
