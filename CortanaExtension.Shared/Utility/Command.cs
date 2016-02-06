using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CortanaExtension.Shared.Utility
{
    public class Command : ICommand
    {
        public Action Act { get; set; }

        /// <summary> Occurs when the target of the Command should reevaluate whether or not the Command can be executed. </summary>
        public event EventHandler CanExecuteChanged;

        public Command(Action act)
        {
            Act = act;
        }

        /// <summary> Returns a bool indicating if the Command can be exectued with the given parameter </summary>
        public bool CanExecute(object obj)
        {
            return true;
        }

        /// <summary> Send a ICommand.CanExecuteChanged </summary>
        public void ChangeCanExecute()
        {
            object sender = this;
            EventArgs eventArgs = null;
            CanExecuteChanged(sender, eventArgs);
        }

        /// <summary> Invokes the execute Action </summary>
        public void Execute(object obj)
        {
            Act();
        }
    }
}
