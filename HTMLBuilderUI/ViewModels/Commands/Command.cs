using HTMLBuilderUI.ViewModels.Commands.Parents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLBuilderUI.ViewModels.Commands
{
    class Command<T> : CommandBase where T : class
    {
        private readonly Action<T> _function;

        public Command(Action<T> function)
        {
            this._function = function;
        }

        public override void Execute(object parameter)
        {
            this._function.Invoke(parameter as T);
        }
    }
}
