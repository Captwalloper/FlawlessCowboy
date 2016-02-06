using CortanaExtension.Shared.Utility.Cortana;
using FlawlessCowboy.ViewModel;
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
        public MainViewModel viewModel;

        public MainPage()
        {
            this.InitializeComponent();
            viewModel = new MainViewModel(this);
            this.DataContext = viewModel;
        }

        async Task IAppPage.RespondToVoice(CortanaCommand command)
        {
            await viewModel.RespondToVoice(command);
        }
    }
}
