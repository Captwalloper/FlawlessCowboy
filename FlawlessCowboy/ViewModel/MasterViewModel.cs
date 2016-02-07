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
    public class MasterViewModel : AppViewModel<FlawlessCowboy.View.MasterPage>
    {

        private ObservableCollection<UserCortanaCommand> userCortanaCommands = new ObservableCollection<UserCortanaCommand>();
        public ObservableCollection<UserCortanaCommand> UserCortanaCommands
        {
            get { return userCortanaCommands; }
            set
            {
                if (value != null && !value.Equals(userCortanaCommands))
                {
                    userCortanaCommands = value;
                    OnPropertyChanged("UserCortanaCommands");
                }
            }
        }

        //Commands

        private ICommand _addCommand;
        public ICommand AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand = new Command(AddTheCommand));
            }
            protected set
            {
                _addCommand = value;
            }
        }


        public MasterViewModel(MasterPage page)
        {
            ViewPage = page;
            //UserCortanaCommand test = new UserCortanaCommand("test");
            //UserCortanaCommands.Add(test);

            if (!firstFocus)
            {
                SharedModel model = (Application.Current as App).Model;
                foreach (UserCortanaCommand command in model.UserCortanaCommands)
                {
                    UserCortanaCommands.Add(command);
                }
            }
        }

        private static bool firstFocus = true;

        public async void OnGotFocus()
        {
            if (firstFocus)
            {
                App app = Application.Current as App;
                app.Model = await app.Load();
                SharedModel model = (Application.Current as App).Model;
                foreach (UserCortanaCommand command in model.UserCortanaCommands)
                {
                    UserCortanaCommands.Add(command);
                }
            }
            firstFocus = false;
            
        }

        private async void AddTheCommand()
        {
            UserCortanaCommand userCortanaCommand = new UserCortanaCommand("placeholder");
            SharedModel model = (Application.Current as App).Model;
            model.selected = userCortanaCommand;
            GotoMainPage();
        }

        public async void EditTheCommand(UserCortanaCommand command)
        {
            SharedModel model = (Application.Current as App).Model;
            model.selected = command;
            GotoMainPage();
        }

        public async void GotoMainPage()
        {
            ViewPage.Frame.Navigate(typeof(MainPage));
        }


        public override async Task RespondToVoice(CortanaCommand command)
        {
            await command.Perform();
        }

        private static string Sanitize(string inputString)
        {
            return inputString.Trim();
        }

        private async void OpenFileExplorer()
        {
            await FileHelper.OpenFileExplorer();
        }

    }
}
