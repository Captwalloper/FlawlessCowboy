using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CortanaExtension.Shared.Model
{
    public class UserTask
    {
        public const string execute = "Execute";
        public const string toggle_listening = "Toggle Listening";

        public string Name { get; set; }
        public ObservableCollection<UserTask> Tasks = new ObservableCollection<UserTask>();

        public UserTask()
        {

        }

        public UserTask(string name)
        {
            Name = name;
        }

        public virtual void Perform()
        {
            throw new NotImplementedException();
        }

        public static Type GetType(string name)
        {
            switch(name)
            {
                case execute:
                    return typeof(ExecuteUserTask);
                case toggle_listening:
                    return typeof(ToggleListeningUserTask);
                default:
                    return typeof(AggregateUserTask);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is UserTask))
            {
                return false;
            }

            UserTask ut = obj as UserTask;
            bool sameName = this.Name.Equals(ut.Name);
            return sameName;
        }
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
