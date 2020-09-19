using HTMLBuilderUI.ViewModels.Commands.Parents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLBuilderUI.ViewModels.Commands.Template
{
    class ParameterlessCommand : CommandBase
    {
        private Action _function;

        public ParameterlessCommand(Action function)
        {
            this._function = function;
        }

        public override void Execute(object parameter)
        {
            this._function.Invoke();
        }
    }
}
