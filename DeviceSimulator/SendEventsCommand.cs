using DeviceSimulator.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSimulator
{
    class SendEventsCommand : IAsyncCommand
    {
        private MainWindowViewModel _simulatorViewModel;

        public SendEventsCommand(MainWindowViewModel simulatorViewModel)
        {
            _simulatorViewModel = simulatorViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteAsync(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
