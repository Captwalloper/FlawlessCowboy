using CortanaExtension.Shared.Utility.Cortana;
using CortanaExtension.Shared.Utility;
using System.Threading.Tasks;
using System.Windows.Input;
using FlawlessCowboy.View;
using static CortanaExtension.Shared.Utility.FileHelper;
using CortanaExtension.Shared.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FlawlessCowboy.ViewModel
{
    public class MainViewModel : AppViewModel<FlawlessCowboy.View.MainPage>
    {

        private UserCortanaCommand UserCommand { get; set; }

        private ObservableCollection<UserTask> availableTasks = null;
        public ObservableCollection<UserTask> AvailableTasks
        {
            get { return availableTasks; }
            set
            {
                if (value != null && !value.Equals(availableTasks))
                {
                    availableTasks = value;
                    OnPropertyChanged("AvailableTasks");
                }
            }
        }

        public ObservableCollection<UserTask> ComponentTasks
        {
            get { return UserCommand.Task.Tasks; }
            set
            {
                if (value != null && !value.Equals(UserCommand.Task.Tasks))
                {
                    UserCommand.Task.Tasks = value;
                    OnPropertyChanged("ComponentTasks");
                }
            }
        }

        public string Name
        {
            get { return UserCommand.Name; }
            set
            {
                if (value != null && !value.Equals(UserCommand.Name))
                {
                    UserCommand.Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string ListenFor
        {
            get { return UserCommand.ListenFor; }
            set
            {
                if (value != null && !value.Equals(UserCommand.ListenFor))
                {
                    UserCommand.ListenFor = value;
                    OnPropertyChanged("ListenFor");
                }
            }
        }

        public string Feedback
        {
            get { return UserCommand.Feedback; }
            set
            {
                if (value != null && !value.Equals(UserCommand.Feedback))
                {
                    UserCommand.Feedback = value;
                    OnPropertyChanged("Feedback");
                }
            }
        }

        public UserTask Task
        {
            get { return UserCommand.Task; }
            set
            {
                if (value != null && !value.Equals(UserCommand.Task))
                {
                    UserCommand.Task = value;
                    OnPropertyChanged("Task");
                }
            }
        }

        //Commands

        private ICommand _launchCommand;
        public ICommand LaunchCommand
        {
            get
            {
                return _launchCommand ?? (_launchCommand = new Command(Test));
            }
            protected set
            {
                _launchCommand = value;
            }
        }


        public MainViewModel(MainPage page, UserCortanaCommand ucc, ObservableCollection<UserTask> availableTasks)
        {
            ViewPage = page;
            UserCommand = ucc;
            AvailableTasks = availableTasks;
        }


        public override async Task RespondToVoice(CortanaCommand command)
        {
            await command.Perform();
        }

        private static string Sanitize(string inputString)
        {
            return inputString.Trim();
        }

        private async void Test()
        {
            //await PopupHelper.ShowPopup(UserCommand.Name);
            Name = "yay";
        }

        private static async void TestFileIO()
        {
            const string filename = "sample.txt";
            await FileHelper.CreateFile(filename, StorageFolders.LocalFolder);
            await FileHelper.WriteTo(filename, StorageFolders.LocalFolder, "booyah");
            string text = await FileHelper.ReadFrom(filename, StorageFolders.LocalFolder);
            await PopupHelper.ShowPopup(text);
        }


    }
}
