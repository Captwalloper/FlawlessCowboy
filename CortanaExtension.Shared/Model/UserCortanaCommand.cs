using CortanaExtension.Shared.Utility.Cortana;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Media.SpeechRecognition;

namespace CortanaExtension.Shared.Model
{
    public class UserCortanaCommand : CortanaCommand
    {
        public string ListenFor { get; set; }
        public string Feedback { get; set; }
        public ObservableCollection<CortanaCommand> Tasks = new ObservableCollection<CortanaCommand>();

        public string TasksList
        {
            get {
                string display = "";
                foreach(CortanaCommand task in Tasks)
                {
                    display += task.ToString() + "; ";
                }
                return display;
            }
        }

        public UserCortanaCommand(string name, params CortanaCommand[] tasks) : base(name)
        {
            foreach (CortanaCommand task in tasks)
            {
                Tasks.Add(task);
            }
        }

        public override async Task Perform()
        {
            foreach (CortanaCommand task in Tasks)
            {
                await task.Perform();
            }
        }

        public UserCortanaCommand Spawn(SpeechRecognitionResult speechRecognitionResult)
        {
            //TODO: use argument
            UserCortanaCommand clone = new UserCortanaCommand(Name, Tasks.ToArray());
            return clone;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is UserCortanaCommand)) {
                return false;
            }

            UserCortanaCommand ucc = obj as UserCortanaCommand;
            bool sameName = this.Name.Equals(ucc.Name);
            return sameName;
        }
    }

}
