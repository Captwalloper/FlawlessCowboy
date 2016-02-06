using CortanaExtension.Shared.Model;
using CortanaExtension.Shared.Utility.Cortana;
using CortanaExtension.Utility;
using FlawlessCowboy.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FlawlessCowboy.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, IAppPage
    {
        public MainViewModel ViewModel;

        //testing
        private UserCortanaCommand UCC = new UserCortanaCommand("test", new AggregateUserTask("Test", new ExecuteUserTask()));
        private ObservableCollection<UserTask> AvailableTasks = new ObservableCollection<UserTask>();

        public MainPage()
        {
            this.InitializeComponent();
            InitTest();
            ViewModel = new MainViewModel(this, UCC, AvailableTasks);
            this.DataContext = ViewModel;
        }

        async Task IAppPage.RespondToVoice(CortanaCommand command)
        {
            await ViewModel.RespondToVoice(command);
        }

        private void InitTest()
        {
            AvailableTasks.Add(new ExecuteUserTask());
            AvailableTasks.Add(new ToggleListeningUserTask());
            UCC.ListenFor = "yay";
            UCC.Feedback = "booyah!";

        }
    }
}
