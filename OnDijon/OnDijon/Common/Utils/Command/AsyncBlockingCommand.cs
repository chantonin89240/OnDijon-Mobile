using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OnDijon.Common.Utils.Command
{
    //Command qui permet de désactiver le clique jusqu'à la fin d'execution de la méthode asynchrone _toExecute
    public class AsyncBlockingCommand : ICommand
    {
        bool _canExecute = true;
        Func<Task> _toExecute;

        public AsyncBlockingCommand(Func<Task> toExecute)
        {
            _toExecute = toExecute;
        }

        public event EventHandler CanExecuteChanged;

        public async void Execute(object data)
        {
            _canExecute = false;
            RaiseCanExecuteChanged();
            await _toExecute();
            _canExecute = true;
            RaiseCanExecuteChanged();
        }

        public bool CanExecute(object data) => _canExecute;

        private void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
