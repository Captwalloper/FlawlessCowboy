using CortanaExtension.Shared.Utility.Cortana;
using CortanaExtension.Shared.Utility;
using System.Threading.Tasks;
using System.Windows.Input;
using FlawlessCowboy.View;
using static CortanaExtension.Shared.Utility.FileHelper;
using CortanaExtension.Shared.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Storage;
using System;
using Windows.UI.Xaml;

namespace FlawlessCowboy.ViewModel
{
    public class MainViewModel : AppViewModel<FlawlessCowboy.View.MainPage>
    {

        private UserCortanaCommand UserCommand { get; set; }

        private ObservableCollection<CortanaCommand> availableTasks = null;
        public ObservableCollection<CortanaCommand> AvailableTasks
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

        public ObservableCollection<CortanaCommand> ComponentTasks
        {
            get { return UserCommand.Tasks; }
            set
            {
                if (value != null && !value.Equals(UserCommand.Tasks))
                {
                    UserCommand.Tasks = value;
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

        //Commands

        private ICommand _scriptEditorCommand;
        public ICommand ScriptEditorCommand
        {
            get
            {
                return _scriptEditorCommand ?? (_scriptEditorCommand = new Command(OpenFileExplorer));
            }
            protected set
            {
                _scriptEditorCommand = value;
            }
        }

        private ICommand _saveScriptChanges;
        public ICommand SaveScriptChanges
        {
            get
            {
                return _saveScriptChanges ?? (_saveScriptChanges = new Command(SaveChanges));
            }
            protected set
            {
                _saveScriptChanges = value;
            }
        }


        private ICommand _vcdEditorCommand;
        public ICommand VCDEditorCommand
        {
            get
            {
                return _vcdEditorCommand ?? (_vcdEditorCommand = new Command(EditVCD));
            }
            protected set
            {
                _vcdEditorCommand = value;
            }
        }

        private ICommand _installVCD;
        public ICommand InstallVCD
        {
            get
            {
                return _installVCD ?? (_installVCD = new Command(InstallTheVCD));
            }
            protected set
            {
                _installVCD = value;
            }
        }

        private ICommand _demoCommand;
        public ICommand DemoCommand
        {
            get
            {
                return _demoCommand ?? (_demoCommand = new Command(Demo));
            }
            protected set
            {
                _demoCommand = value;
            }
        }

        private ICommand _confirmCommand;
        public ICommand ConfirmCommand
        {
            get
            {
                return _confirmCommand ?? (_confirmCommand = new Command(Confirm));
            }
            protected set
            {
                _confirmCommand = value;
            }
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                return _deleteCommand ?? (_deleteCommand = new Command(Delete));
            }
            protected set
            {
                _deleteCommand = value;
            }
        }


        public MainViewModel(MainPage page, UserCortanaCommand ucc, ObservableCollection<CortanaCommand> availableTasks)
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

        public async void Confirm()
        {
            SharedModel model = (Application.Current as App).Model;
            if (!model.UserCortanaCommands.Contains(UserCommand))
            {
                model.UserCortanaCommands.Add(UserCommand);
            }
            GoToMasterPage();
        }

        private async void Delete()
        {
            SharedModel model = (Application.Current as App).Model;
            model.UserCortanaCommands.Remove(UserCommand);
            GoToMasterPage();
        }

        private async void GoToMasterPage()
        {
            ViewPage.Frame.Navigate(typeof(MasterPage));
        }

        private async void OpenFileExplorer()
        {
            await FileHelper.OpenFileExplorer();
        }

        private async void SaveChanges()
        {
            await App.StoreLocalFolderInResourceFiles();
        }

        private async void EditVCD()
        {
            await FileHelper.EditVCD();
        }

        private async void InstallTheVCD()
        {
            await Cortana.InstallVoiceCommands();
        }

        private async void Demo()
        {
            await VCDEditor.DoThing("dummy");
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
