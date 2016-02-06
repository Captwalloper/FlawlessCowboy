using CortanaExtension.Shared.Utility.Cortana;
using CortanaExtension.Shared.Utility;
using System.Threading.Tasks;
using System.Windows.Input;
using FlawlessCowboy.View;

namespace FlawlessCowboy.ViewModel
{
    public class MainViewModel : AppViewModel<FlawlessCowboy.View.MainPage>
    {
        private string rawText = "";
        public string RawText
        {
            get { return rawText; }
            set
            {
                if (value != rawText) {
                    rawText = Sanitize(value);
                    OnPropertyChanged("RawText");
                }
            }
        }

        private string commandName = "";
        public string CommandName
        {
            get { return commandName; }
            set
            {
                if (value != commandName) {
                    commandName = Sanitize(value);
                    OnPropertyChanged("CommandName");
                }
            }
        }

        private string commandArg = "";
        public string CommandArg
        {
            get { return commandArg; }
            set
            {
                if (value != commandArg) {
                    commandArg = Sanitize(value);
                    OnPropertyChanged("CommandArg");
                }
            }
        }

        private string commandMode = "";
        public string CommandMode
        {
            get { return commandMode; }
            set
            {
                if (value != commandMode) {
                    commandMode = Sanitize(value);
                    OnPropertyChanged("CommandMode");
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


        public MainViewModel(MainPage page)
        {
            ViewPage = page;
        }


        public override async Task RespondToVoice(CortanaCommand command)
        {
            await command.Perform();
        }

        private static string Sanitize(string inputString)
        {
            return inputString.Trim();
        }

        private static async void Test()
        {
            const string filename = "sample.txt";
            //await FileHelper.CreateFile(filename);
            //await FileHelper.WriteTo(filename, "booyah");
            string text = await FileHelper.ReadFrom(filename);
            await PopupHelper.ShowPopup(text);
        }


    }
}
