using CortanaExtension.Shared.Model;
using CortanaExtension.Shared.Utility.Cortana;
using CortanaExtension.Utility;
using FlawlessCowboy.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
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
        private UserCortanaCommand UCC; // new UserCortanaCommand("test", new ExecuteCortanaCommand(""));
        private ObservableCollection<CortanaCommand> AvailableTasks;

        public MainPage()
        {
            this.InitializeComponent();
            InitAvailableTasks();
            SharedModel model = (Application.Current as App).Model;
            UCC = model.selected;
            ViewModel = new MainViewModel(this, UCC, AvailableTasks);
            this.DataContext = ViewModel;
        }

        private void InitAvailableTasks()
        {
            SharedModel model = (Application.Current as App).Model;
            if (model == null) {
                model = new SharedModel();
            }
            AvailableTasks = model.AvailableTasks;
        }

        async Task IAppPage.RespondToVoice(CortanaCommand command)
        {
            await ViewModel.RespondToVoice(command);
        }

    }
}
