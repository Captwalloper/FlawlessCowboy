using CortanaExtension.Shared.Utility.Cortana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.SpeechRecognition;

namespace CortanaExtension.Shared.Model
{
    public class UserCortanaCommand : CortanaCommand
    {
        private UserTask Task { get; set; }

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
    }
}
