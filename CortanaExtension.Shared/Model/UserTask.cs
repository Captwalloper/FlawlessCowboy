using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CortanaExtension.Shared.Model
{
    public abstract class UserTask
    {
        protected IList<UserTask> Tasks = new List<UserTask>();

        public abstract void Perform();
    }

    public class AggregateUserTask : UserTask
    {
        public AggregateUserTask(params UserTask[] tasks)
        {
            Tasks = tasks.ToList();
        }

        public override void Perform()
        {
            foreach (UserTask task in Tasks)
            {
                task.Perform();
            }
        }
    }

}
