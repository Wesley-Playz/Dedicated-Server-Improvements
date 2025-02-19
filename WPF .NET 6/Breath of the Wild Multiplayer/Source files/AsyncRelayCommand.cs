using Breath_of_the_Wild_Multiplayer.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Breath_of_the_Wild_Multiplayer.Source_files
{
    public class AsyncRelayCommand : ICommand
    {
        private Func<object, Task> _execute;
        private Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public AsyncRelayCommand(Func<object, Task> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public async void Execute(object parameter)
        {
            try
            {
                await ExecuteAsync(parameter);
            }
            catch (Exception ex)
            {
                SharedData.LoadErrorMessage(ex.Message);
            }
        }

        public async Task ExecuteAsync(object parameter)
        {
            await _execute(parameter);
        }
    }
}
