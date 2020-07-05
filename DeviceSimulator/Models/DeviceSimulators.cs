using DeviceSimulator.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceSimulator.Models
{
    public class DeviceSimulators
    {

        private IDistribution _distribution;
        private ICombinationMessageGenerator _combinationMessageGenerator;


        public DeviceSimulators(IDistribution distribution, ICombinationMessageGenerator combinationMessageGenerator)
        {
            _distribution = distribution;
            _combinationMessageGenerator = combinationMessageGenerator;
        }

        public async Task ContinueSendingEventsTask(Action<MessagesModel> increaseMessageNumbers, CancellationToken cancellationToken)
        {

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    MessagesModel model = await _combinationMessageGenerator.GenerateAndSendUpdMessages();

                    increaseMessageNumbers?.Invoke(model);

                    double delayMilliseconds = _distribution.GetNumber();
                    if (delayMilliseconds > 0)
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(delayMilliseconds), cancellationToken);
                    }

                }
                catch (TaskCanceledException)
                {
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    System.Windows.MessageBox.Show(e.ToString());
                    break;
                }

            }
        }


        public Task StopingSendingEventsTask(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
            }, cancellationToken);
        }


    }
}
