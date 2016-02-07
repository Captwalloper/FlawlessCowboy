using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CortanaExtension.Shared.Model
{
    public class SharedModel
    {
        public IList<UserCortanaCommand> UserCortanaCommands = new List<UserCortanaCommand>();
        public ObservableCollection<UserTask> AvailableTasks = new ObservableCollection<UserTask>();

        public SharedModel()
        {
            InitAvailableTasks();
            InitUserCortanaCommands();
        }

        private void InitAvailableTasks()
        {
            AvailableTasks.Add( new ExecuteUserTask() );
            AvailableTasks.Add( new ToggleListeningUserTask() );
        }

        private void InitUserCortanaCommands()
        {
            
        }
    }
}
