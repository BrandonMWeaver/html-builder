﻿using HTMLBuilderUI.ViewModels.Commands.Parents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLBuilderUI.ViewModels.Commands
{
    class ParameterlessCommand : CommandBase
    {
        private readonly Action _function;

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
