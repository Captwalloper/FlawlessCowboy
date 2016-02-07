using CortanaExtension.Shared.Utility.Cortana;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace CortanaExtension.Shared.Model
{
    public class SharedModel
    {
        public IList<UserCortanaCommand> UserCortanaCommands = new List<UserCortanaCommand>();
        public ObservableCollection<CortanaCommand> AvailableTasks = new ObservableCollection<CortanaCommand>();

        public UserCortanaCommand selected = null;

        public SharedModel()
        {
            InitAvailableTasks();
        }

        private void InitAvailableTasks()
        {
            AvailableTasks = InitPrebuiltCortanaCommands();
        }

        public static ObservableCollection<CortanaCommand> InitPrebuiltCortanaCommands()
        {
            ObservableCollection<CortanaCommand> prebuiltCommands = new ObservableCollection<CortanaCommand>();
            prebuiltCommands.Add(new ExecuteCortanaCommand(""));
            prebuiltCommands.Add(new ToggleListeningCortanaCommand(null));
            prebuiltCommands.Add(new NotepadCortanaCommand(""));
            prebuiltCommands.Add(new FeedMeCortanaCommand(null));
            prebuiltCommands.Add(new YoutubeCortanaCommand(""));
            prebuiltCommands.Add(new CalibrateCortanaCommand(null));
            prebuiltCommands.Add(new BriefMeCortanaCommand(null));
            return prebuiltCommands;
        }

        public static DataContractJsonSerializerSettings GetSerializerSettings()
        {
            ObservableCollection<CortanaCommand> prebuiltCommands = InitPrebuiltCortanaCommands();
            DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();
            IList<Type> knownTypes = new List<Type>();
            foreach (CortanaCommand task in prebuiltCommands)
            {
                knownTypes.Add(task.GetType());
            }
            settings.KnownTypes = knownTypes;
            return settings;
        }
    }
}
