using System.Threading.Tasks;
using System.Windows.Input;

namespace CloudConnection.Simulator.Commands
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}