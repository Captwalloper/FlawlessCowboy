using CortanaExtension.Shared.Utility.Cortana;
using System.Threading.Tasks;
using Windows.Media.SpeechRecognition;

namespace CortanaExtension.Shared.Model
{
    public class UserCortanaCommand : CortanaCommand
    {
        public string ListenFor { get; set; }
        public string Feedback { get; set; }
        public UserTask Task { get; set; }

        public UserCortanaCommand(string name, UserTask task) : base(name)
        {
            Task = task;
        }

        public override async Task Perform()
        {
            Task.Perform();
        }

        public UserCortanaCommand Spawn(SpeechRecognitionResult speechRecognitionResult)
        {
            //TODO: use argument
            UserCortanaCommand clone = new UserCortanaCommand(Name, Task);
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
