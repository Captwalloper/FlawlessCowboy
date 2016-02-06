using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CortanaExtension.Shared.Model
{
    public abstract class UserTask
    {
        public const string execute = "Execute";
        public const string toggle_listening = "Toggle Listening";

        public string Name { get; set; }
        public ObservableCollection<UserTask> Tasks = new ObservableCollection<UserTask>();

        public UserTask(string name)
        {
            Name = name;
        }

        public abstract void Perform();
    }

    public class AggregateUserTask : UserTask
    {
        public AggregateUserTask(string name, params UserTask[] tasks) : base(name)
        {
            Tasks.Clear();
            foreach (UserTask task in tasks)
            {
                Tasks.Add(task);
            }
        }

        public override void Perform()
        {
            foreach (UserTask task in Tasks)
            {
                task.Perform();
            }
        }
    }

    public class ExecuteUserTask : UserTask
    {
        public ExecuteUserTask() : base(execute)
        {

        }

        public override void Perform()
        {
            throw new NotImplementedException();
        }
    }

    public class ToggleListeningUserTask : UserTask
    {
        public ToggleListeningUserTask() : base(toggle_listening)
        {

        }

        public override void Perform()
        {
            throw new NotImplementedException();
        }
    }

}
